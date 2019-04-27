using VisualDump.VSHelpers;
using System.Windows.Controls;
using VisualDump.Models.HTMLBuilders;

namespace VisualDump.Controls
{
    public partial class DumpWindow : UserControl
    {
        #region Init
        public DumpWindow() => InitializeComponent();
        #endregion

        #region Functions
        public void Render(string HTML) => RunScript("render", HTML);
        public void ChangeTheme(bool IsDark) => RunScript("changeTheme", IsDark);
        public void Clear() => MainBrowser.NavigateToString(new PageHTMLBuilder().SetTheme(ThemeListener.GetCurrentTheme() == Themes.Dark).LoadTheme(OptionContainer.SelectedTheme.Path).BuildHTML());

#pragma warning disable
        private void RunScript(string Name, params object[] Args)
        {
            try
            {
                Dispatcher.Invoke(() => { try { MainBrowser.InvokeScript(Name, Args); } catch { Clear(); } });
            } catch { }
        }
#pragma warning restore
        #endregion
    }
}
