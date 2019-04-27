using System.IO;
using System.Linq;
using VisualDump.Models;
using System.Threading.Tasks;

namespace VisualDump.Controls
{
    public class OptionContainer
    {
        #region Var
        public static Settings Settings { get; private set; }
        public static string ThemesPath { get; private set; }
        public static string SettingsPath { get; private set; }
        public static string DefaultThemesPath { get; private set; }

        public static bool AutoClear => Settings.AutoClear;
        public static Theme SelectedTheme => Settings.SelectedTheme;
        #endregion

        #region Init
        public static async Task InitializeAsync() => await Task.Run(() => {
            ThemesPath = Path.Combine(VSPackage.Path, "Themes\\");
            if (!Directory.Exists(ThemesPath))
                Directory.CreateDirectory(ThemesPath);
            DefaultThemesPath = Path.Combine(VSPackage.AssemblyPath, "DefaultThemes\\");
            Settings = Settings.Read(SettingsPath = Path.Combine(VSPackage.Path, "settings.xml"));
            LoadThemes();
        });
        #endregion

        #region Functions
        public static Theme[] LoadThemes()
        {
            Theme[] themes = Theme.LoadThemes(ThemesPath).ToArray();
            if (themes.Length == 0)
            {
                foreach (string dirPath in Directory.GetDirectories(DefaultThemesPath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(DefaultThemesPath, ThemesPath));

                foreach (string newPath in Directory.GetFiles(DefaultThemesPath, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(DefaultThemesPath, ThemesPath), true);

                themes = Theme.LoadThemes(ThemesPath).ToArray();
            }
            return themes;
        }
        #endregion
    }
}
