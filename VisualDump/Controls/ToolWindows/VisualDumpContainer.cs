using EnvDTE;
using System;
using VisualDump.Models;
using VisualDump.VSHelpers;

namespace VisualDump.Controls
{
    public class VisualDumpContainer
    {
        #region Var
        public static DumpWindow DumpWindow { get; set; }
        public static HTMLServer Server { get; private set; }
        #endregion

        #region Init
        public static void Initialize()
        {
            ThemeListener.ThemeChanged += ThemeListener_ThemeChanged;
            DebugListener.OnProjectDebuggingStart += DebugListener_OnProjectDebugging;
            Clear();
            Server = new PipeHTMLServer($"VisualDump-{System.Diagnostics.Process.GetCurrentProcess().Id}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            Server.OnDataReceived += Server_HTMLReceived;
            Server.BeginRead();
        }
        #endregion

        #region Functions
        private static void Clear() => DumpWindow?.Clear();

        private static void Server_HTMLReceived(string HTML) => DumpWindow?.Render(HTML);
        private static void DebugListener_OnProjectDebugging(Project Project)
        {
            if (OptionContainer.AutoClear)
                Clear();
        }

        private static void ThemeListener_ThemeChanged(Themes Theme) => DumpWindow?.ChangeTheme(Theme == Themes.Dark);
        #endregion
    }
}
