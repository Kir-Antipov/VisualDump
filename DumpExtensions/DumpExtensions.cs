using System;
using System.IO;
using System.Web;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using VisualDump.Helpers;
using System.ComponentModel;
using VisualDump.ExtraTypes;
using VisualDump.HTMLClients;
using VisualDump.HTMLProviders;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using VisualDump.HTMLProviders.DefaultProviders;

public static class DumpExtensions
{
    #region Var
    public static bool Enabled
    {
        get => _enabled;
        private set
        {
            if (_enabled != value)
            {
                _enabled = value;
                CloseClient();
            }
        }
    }
    private static bool _enabled = true;

    private static HTMLClient Client
    {
        get
        {
            if (_client is null && Enabled)
            {
                if (string.IsNullOrEmpty(ServerName))
                    ServerName = GetDefaultServerName();
                _client = (HTMLClient)Activator.CreateInstance(HTMLClientType, ServerName);
                _client.WaitForConnection(MaxWaitTime);
            }
            return _client;
        }
    }
    private static HTMLClient _client;

    private static Assembly EntryAssembly
    {
        get => _entryAssembly;
        set => _entryAssembly = _entryAssembly ?? value;
    }
    private static Assembly _entryAssembly;

    private static string ServerName { get; set; }
    private static Type HTMLClientType { get; set; } = typeof(PipeHTMLClient);
    private static readonly Regex ProcessRegex = new Regex(@"(.+) - Microsoft Visual Studio", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex ProcessInActionRegex = new Regex(@"(.+) \(.+\) - Microsoft Visual Studio", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private const int MaxWaitTime = 5000;

    private static readonly object _sync = new object();

    private static ConcurrentDictionary<Type, HTMLProvider> Providers { get; }
    #endregion

    #region Init
    static DumpExtensions()
    {
        Providers = new ConcurrentDictionary<Type, HTMLProvider> {
            [typeof(object)] = new ObjectHTMLProvider(),
            [typeof(sbyte)] = new NumberProvider(),
            [typeof(short)] = new NumberProvider(),
            [typeof(int)] = new NumberProvider(),
            [typeof(long)] = new NumberProvider(),
            [typeof(byte)] = new NumberProvider(),
            [typeof(ushort)] = new NumberProvider(),
            [typeof(uint)] = new NumberProvider(),
            [typeof(ulong)] = new NumberProvider(),
            [typeof(float)] = new NumberProvider(),
            [typeof(double)] = new NumberProvider(),
            [typeof(decimal)] = new NumberProvider(),
            [typeof(BigInteger)] = new NumberProvider(),
            [typeof(Enum)] = new EnumHTMLProvider(),
            [typeof(string)] = new StringHTMLProvider(),
            [typeof(char)] = new CharHTMLProvider(),
            [typeof(StringBuilder)] = new StringBuilderHTMLProvider(),
            [typeof(bool)] = new BooleanHTMLProvider(),
            [typeof(DateTime)] = new DateTimeHTMLProvider(),
            [typeof(Array)] = new ArrayHTMLProvider(),
            [typeof(CyclicalReference)] = new CyclicalReferenceHTMLProvider(),
            [typeof(NullReference)] = new NullHTMLProvider(),
            [typeof(DataTable)] = new DataTableHTMLProvider(),
            [typeof(Assembly)] = new AssemblyHTMLProvider(),
            [typeof(Module)] = new ModuleHTMLProvider(),
            [typeof(Type)] = new TypeHTMLProvider(),
#if NETFRAMEWORK
            [typeof(Image)] = new ImageHTMLProvider(),
#endif
            [typeof(IEnumerable)] = new IEnumerableHTMLProvider()
        };
        foreach (Type p in AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => {
                    string name = x.GetName().Name;
                    return !name.StartsWith("System") && !name.StartsWith("Microsoft") && name != "mscorlib" && name != "netstandard";
                })
                .SelectMany(x => x.GetExportedTypes()
                .Where(y => typeof(HTMLProvider).IsAssignableFrom(y) && y.GetConstructor(Type.EmptyTypes) != null)))
            Register(p);
    }
    #endregion

    #region Functions
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void EnableDumping()
    {
        EntryAssembly = Assembly.GetCallingAssembly();
        Enabled = true;
    }
    public static void DisableDumping() => Enabled = false;

    private static string GetDefaultServerName()
    {
        string asmName = (EntryAssembly ?? Assembly.GetEntryAssembly()).GetName().Name;
        Process activeDevenv = Process.GetProcesses().Where(x => x.ProcessName == "devenv").FirstOrDefault(x => x.EnumerateWindows().Select(WindowsInterop.GetWindowText).Where(title => !string.IsNullOrWhiteSpace(title)).Any(title => {
            Match m = ProcessInActionRegex.Match(title);
            m = m.Success ? m : ProcessRegex.Match(title);
            return m.Success && m.Groups[1].Value == asmName;
        }));
        if (activeDevenv is null)
            return asmName;
        string searchPattern = $"VisualDump-{activeDevenv.Id}";
        return Directory.EnumerateFiles(@"\\.\pipe\")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(x => x.StartsWith(searchPattern))
            .FirstOrDefault() ?? asmName;
    }
    private static void CloseClient()
    {
        lock (_sync)
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static T Dump<T>(this T Obj) => Dump(Obj, string.Empty, new object[0], Assembly.GetCallingAssembly());
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static T Dump<T>(this T Obj, string Header) => Dump(Obj, Header, new object[0], Assembly.GetCallingAssembly());
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static T Dump<T>(this T Obj, params object[] Args) => Dump(Obj, string.Empty, Args, Assembly.GetCallingAssembly());
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static T Dump<T>(this T Obj, string Header, params object[] Args) => Dump(Obj, Header, Args, Assembly.GetCallingAssembly());
    private static T Dump<T>(T Obj, string Header, object[] Args, Assembly EntryAssembly)
    {
        if (Enabled)
        {
            string html = GetProvider(Obj?.GetType()).ToHTML(Obj, new Stack<object>(), Args);
            if (!string.IsNullOrEmpty(Header))
                html = WrapWithHeader(html, Header);
            lock (_sync)
            {
                DumpExtensions.EntryAssembly = EntryAssembly;
                Client?.Send(WrapWithWrapper(html));
            }
        }
        return Obj;
    }

    private static string WrapWithWrapper(string HTML) => $"<div class='wrapper'>{HTML}</div>";
    private static string WrapWithHeader(string HTML, string Header) => $"<div class='header-box'><div class='header'><h3>{HttpUtility.HtmlEncode(Header)}</h3></div>{HTML}</div>";

    internal static HTMLProvider GetProvider(Type T)
    {
        T = T ?? typeof(NullReference);
        if (Providers.ContainsKey(T))
            return Providers[T];
        if (T.IsGenericType && Providers.ContainsKey(T.GetGenericTypeDefinition()))
            return Providers[T.GetGenericTypeDefinition()];
        Type parent = T.BaseType ?? typeof(object);
        while (!Providers.ContainsKey(parent))
        {
            if (parent.IsGenericType && Providers.ContainsKey(parent.GetGenericTypeDefinition()))
                return Providers[parent.GetGenericTypeDefinition()];
            parent = parent.BaseType;
        }
        if (parent == typeof(object))
        {
            foreach (Type i in T.GetInterfaces())
                if (Providers.ContainsKey(i))
                    return Providers[i];
        }
        return Providers[parent];
    }
    internal static bool Register(Type HTMLProviderType)
    {
        try
        {
            Type forExport = HTMLProviderType.GetCustomAttribute<ExportHTMLProviderAttribute>()?.ExportType;
            return forExport is null ? false : Register((HTMLProvider)Activator.CreateInstance(HTMLProviderType), forExport);
        } 
        catch
        {
            return false;
        }
    }
    internal static bool Register(HTMLProvider Provider, Type T)
    {
        if (Provider != null && T != null)
        {
            Providers[T] = Provider;
            return true;
        }
        return false;
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetHTMLClient<TClient>() where TClient : HTMLClient, new()
    {
        HTMLClientType = typeof(TClient);
        CloseClient();
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    private static void SetHTMLClient(Type T)
    {
        HTMLClientType = typeof(HTMLClient).IsAssignableFrom(T) && T.GetConstructor(Type.EmptyTypes) != null ? T : throw new NotSupportedException();
        CloseClient();
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetServerName(string ServerName)
    {
        DumpExtensions.ServerName = string.IsNullOrEmpty(ServerName) ? throw new ArgumentException(nameof(ServerName)) : ServerName;
        CloseClient();
    }
    #endregion
}
