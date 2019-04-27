using System.IO;
using VisualDump.Controls;
using System.Xml.Serialization;

namespace VisualDump.Models
{
    public class Settings
    {
        #region Var
        public const string LINQPadTheme = "LINQPad Theme";
        public const string DessaderTheme = "Dessader Theme";

        public bool AutoClear { get; set; }
        public Theme SelectedTheme { get; set; }
        #endregion

        #region Functions
        public void Save(string Path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                File.WriteAllText(Path, writer.ToString());
            }
        }
        public static Settings Read(string Path)
        {
            if (!File.Exists(Path))
                return createNew();
            try
            {
                using (StringReader reader = new StringReader(File.ReadAllText(Path)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    return (Settings)serializer.Deserialize(reader);
                }
            } catch {
                return createNew();
            }

            Settings createNew()
            {
                Settings result = new Settings { SelectedTheme = new Theme(System.IO.Path.Combine(OptionContainer.ThemesPath, DessaderTheme)), AutoClear = true };
                result.Save(Path);
                return result;
            }
        }
        #endregion
    }
}
