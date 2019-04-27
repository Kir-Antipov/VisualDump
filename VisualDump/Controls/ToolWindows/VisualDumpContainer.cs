using EnvDTE;
using System.Threading;
using VisualDump.Models;
using VisualDump.VSHelpers;

namespace VisualDump.Controls
{
    public class VisualDumpContainer
    {
        #region Var
        public static DumpWindow DumpWindow { get; set; }
        public static HTMLServer Server { get; private set; }

        private const int MaxConnectionWaitTime = 15000;
        private static CancellationTokenSource PreviousConnectionSource { get; set; }
        #endregion

        #region Init
        public static void Initialize()
        {
            ThemeListener.ThemeChanged += ThemeListener_ThemeChanged;
            DebugListener.OnProjectDebuggingStart += DebugListener_OnProjectDebugging;
            DebugListener.OnProjectDebuggingStop += DebugListener_OnProjectDebuggingStop;
            Clear();
        }
        #endregion

        #region Functions
        private static void Clear() => DumpWindow?.Clear();

        private static void Server_HTMLReceived(string HTML) => DumpWindow?.Render(HTML);
#pragma warning disable 
        private static async void DebugListener_OnProjectDebugging(Project Project)
        {
            ProjectExplorer explorer = new ProjectExplorer(Project);
            if (explorer.Language != Models.Languages.Undefined)
            {
                if (OptionContainer.AutoClear)
                    Clear();
                PreviousConnectionSource?.Cancel();
                Server?.Dispose();
                Server = new PipeHTMLServer(explorer.AssemblyName);
                Server.OnDataReceived += Server_HTMLReceived;
                PreviousConnectionSource = new CancellationTokenSource(MaxConnectionWaitTime);
                await Server.WaitForConnectionAsync(PreviousConnectionSource.Token);
                Server.BeginRead();
            }
        }

        private static void DebugListener_OnProjectDebuggingStop(Project Project) => Server?.EndRead();
#pragma warning restore

        private static void ThemeListener_ThemeChanged(Themes Theme) => DumpWindow?.ChangeTheme(Theme == Themes.Dark);
        #endregion
    }
}
