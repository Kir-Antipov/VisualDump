using System;
using System.Linq;
using VisualDump.Models;
using System.Diagnostics;
using System.Windows.Forms;

namespace VisualDump.Controls
{
    public partial class OptionControl : UserControl
    {
        #region Init
        public OptionControl()
        {
            InitializeComponent();
            checkClear.Checked = OptionContainer.AutoClear;
            ReloadThemes();
        }
        #endregion

        #region Functions
        private void ReloadThemes()
        {
            LoadThemes();
            comboThemes.SelectedIndex = comboThemes.Items.OfType<Theme>().Select((x, i) => new { x, i }).FirstOrDefault(x => x.x.Path == OptionContainer.SelectedTheme.Path)?.i ?? 0;
        }

        private void LoadThemes()
        {
            comboThemes.Items.Clear();
            comboThemes.Items.AddRange(OptionContainer.LoadThemes());
        }

        private void CheckClear_CheckedChanged(object sender, EventArgs e)
        {
            OptionContainer.Settings.AutoClear = checkClear.Checked;
            OptionContainer.Settings.Save(OptionContainer.SettingsPath);
        }
        private void ComboThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                OptionContainer.Settings.SelectedTheme = (Theme)comboThemes.Items[comboThemes.SelectedIndex];
                OptionContainer.Settings.Save(OptionContainer.SettingsPath);
            }
            catch { }
        }
        private void OptionControl_VisibleChanged(object sender, EventArgs e) => ReloadThemes();
        private void ButtOpen_Click(object sender, EventArgs e) => Process.Start("explorer.exe", OptionContainer.ThemesPath);
        #endregion
    }
}
