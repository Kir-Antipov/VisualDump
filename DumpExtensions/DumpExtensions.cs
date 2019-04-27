using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using VisualDump.ExtraTypes;
using VisualDump.HTMLClients;
using VisualDump.HTMLProviders;
using System.Collections.Generic;
using System.Collections.Concurrent;
using VisualDump.HTMLProviders.DefaultProviders;

public static class DumpExtensions
{
    #region Var
    public static bool Enabled { get; private set; }
    private static ConcurrentDictionary<Type, HTMLProvider> Providers { get; }

    private static HTMLClient Client;
    private static string ServerName { get; set; }
    private static Type HTMLClientType { get; set; } = typeof(PipeHTMLClient);

    private const int MaxWaitTime = 5000;
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
#if NETFRAMEWORK
            [typeof(Image)] = new ImageHTMLProvider(),
#endif
            [typeof(IEnumerable)] = new IEnumerableHTMLProvider()
        };
        HashSet<string> defaultAssemblies = new HashSet<string> { "mscorlib", "System", "System.Core", "System.Drawing", "System.Windows.Forms" };
        foreach (Type p in AppDomain.CurrentDomain.GetAssemblies().Where(x => !defaultAssemblies.Contains(x.GetName().Name)).SelectMany(x => x.GetExportedTypes().Where(y => IsHTMLProvider(y) && y.GetConstructors().Where(z => z.GetParameters().Length == 0).Count() == 1)))
            Register(p);
    }
    #endregion

    #region Functions
    public static void EnableDumping()
    {
        Enabled = true;
        SetServerName(string.IsNullOrEmpty(ServerName) ? Assembly.GetCallingAssembly().GetName().Name : ServerName);
    }
    public static void DisableDumping()
    {
        Enabled = false;
        Client?.Dispose();
    }

    public static T Dump<T>(this T Obj) => Dump(Obj, string.Empty, new object[0]);
    public static T Dump<T>(this T Obj, string Header) => Dump(Obj, Header, new object[0]);
    public static T Dump<T>(this T Obj, params object[] Args) => Dump(Obj, string.Empty, Args);
    public static T Dump<T>(this T Obj, string Header, params object[] Args)
    {
        if (Enabled)
        {
            string html = GetProvider(Obj?.GetType()).ToHTML(Obj, Args);
            if (!string.IsNullOrEmpty(Header))
                html = WrapWithHeader(html, Header);
            SendHTML(WrapWithWrapper(html));
        }
        return Obj;
    }

    private static void SendHTML(string HTML) => Client?.Send(HTML);
    private static string WrapWithHeader(string HTML, string Header) => $"<div class='header-box'><div class='header'><h3>{HttpUtility.HtmlEncode(Header)}</h3></div>{HTML}</div>";
    private static string WrapWithWrapper(string HTML) => $"<div class='wrapper'>{HTML}</div>";

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
    private static bool IsHTMLProvider(Type T)
    {
        while (T.BaseType != null)
            if (T.BaseType == typeof(HTMLProvider))
                return true;
            else
                T = T.BaseType;
        return false;
    }
    private static void InitializeClient()
    {
        Client?.Dispose();
        Client = (HTMLClient)Activator.CreateInstance(HTMLClientType, ServerName);
        Client.WaitForConnection(MaxWaitTime);
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetHTMLClient(Type T)
    {
        Type tmp = T.BaseType;
        while (!(tmp is null) && tmp != typeof(HTMLClient))
            tmp = tmp.BaseType;
        HTMLClientType = tmp is null ? throw new NotSupportedException() : T;
        InitializeClient();
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetServerName(string ServerName)
    {
        DumpExtensions.ServerName = ServerName;
        InitializeClient();
    }
#endregion
}
