using EnvDTE;
using System;
using System.Drawing;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VisualDump.VSHelpers
{
    public class ThemeListener : IVsBroadcastMessageEvents
    {
        #region Var
        public static event Action<Themes> ThemeChanged;

        /*private static Dictionary<string, Themes> DefaultThemes { get; } = new Dictionary<string, Themes> {
            { "de3dbbcd-f642-433c-8353-8f1df4370aba", Themes.Light },
            { "a4d6a176-b948-4b29-8c66-53c97a1ed7d0", Themes.Light },
            { "1ded0138-47ce-435e-84ef-9ec1f439b749", Themes.Dark }
        };*/
        #endregion

        #region Init
        public static bool Initialize(IVsShell Shell)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return Shell.AdviseBroadcastMessages(new ThemeListener(), out _) == VSConstants.S_OK;
            } catch
            {
                return false;
            }
        }

        private ThemeListener() { }
        #endregion

        #region Functions
        public static Themes GetCurrentTheme()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Color color = ColorTranslator.FromOle(Convert.ToInt32(((FontsAndColorsItems)VSPackage.DTE.Properties["FontsAndColors", "TextEditor"].Item("FontsAndColorsItems").Object).Item("Plain Text").Background));
            int r = color.R;
            int g = color.G;
            int b = color.B;
            // Lightness < 0.5 ? Dark : Light
            return Math.Max(r, Math.Max(g, b)) + Math.Min(r, Math.Min(g, b)) < 250 ? Themes.Dark : Themes.Light;
        }

        int IVsBroadcastMessageEvents.OnBroadcastMessage(uint Message, IntPtr wParam, IntPtr lParam)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (Message == 0x15)
            {
                Themes currentTheme = GetCurrentTheme();
                ThemeChanged?.Invoke(currentTheme == Themes.Undefined ? Themes.Dark : currentTheme);
            }
            return 0;
        }
        #endregion
    }
}
