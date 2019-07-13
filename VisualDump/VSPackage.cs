using EnvDTE;
using System;
using System.Threading;
using NuGet.VisualStudio;
using VisualDump.Controls;
using VisualDump.VSHelpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.ComponentModelHost;

namespace VisualDump
{
    [ProvideBindingPath]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideOptionPage(typeof(OptionPageGrid), "Visual Dump", "General", 0, 0, true, 0)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.CodeWindow_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideToolWindow(typeof(Controls.VisualDump), Orientation = ToolWindowOrientation.Right, Window = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}", Style = VsDockStyle.Tabbed)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed partial class VSPackage : AsyncPackage
    {
        #region Var
        public static string Path { get; }
        public static string AssemblyPath { get; }
        public static DTE DTE { get; private set; }
        public static VSPackage Instance { get; private set; }
        public static IVsPackageInstaller PackageInstaller { get; private set; }
        public static Guid Guid { get; } = new Guid(PackageGuidString);
        public const string PackageGuidString = "bf22b5f8-9ec7-4810-880d-8d2bec2b68af";
        public static OptionControl OptionPage => (Instance.GetDialogPage(typeof(OptionPageGrid)) as OptionPageGrid)?.OptionControl;
        #endregion

        #region Init
        static VSPackage()
        {
            Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VisualDump\\");
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);
            AssemblyPath = System.IO.Path.GetDirectoryName(new Uri(typeof(VSPackage).Assembly.CodeBase, UriKind.Absolute).LocalPath);
        }

        protected override async Task InitializeAsync(CancellationToken CancellationToken, IProgress<ServiceProgressData> Progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(CancellationToken);

            Instance = this;
            DTE = GetGlobalService(typeof(DTE)) as DTE;

            KnownUIContexts.ShellInitializedContext.WhenActivated(() => {
                ThreadHelper.ThrowIfNotOnUIThread();
                DebugListener.Initialize();
                ThemeListener.Initialize(GetService(typeof(SVsShell)) as IVsShell);
                ProjectListener.Initialize();
                NuGetListener.Initialize();
                PackageInstaller = (GetService(typeof(SComponentModel)) as IComponentModel2)?.GetService<IVsPackageInstaller>();
            });

            await Commands.ShowDumpWindowCommand.InitializeAsync(this);
            await OptionContainer.InitializeAsync();
            VisualDumpContainer.Initialize();          
        }
        #endregion

        #region Functions
        public static Project GetActiveProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return DTE.ActiveDocument.ProjectItem.ContainingProject;
        }

        public static void ShowToolWindow(Type ToolWindow)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IVsWindowFrame frame = (IVsWindowFrame)Instance.FindToolWindow(ToolWindow, 0, true)?.Frame;
            if (frame == null)
                throw new NotSupportedException($"Can't create {ToolWindow.Name}'s tool window");
            ErrorHandler.ThrowOnFailure(frame.Show());
        }
        #endregion
    }

}
