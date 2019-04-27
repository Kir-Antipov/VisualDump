using System.Linq;
using System.Collections.Generic;

namespace VisualDump.Models
{
    public class Theme
    {
        #region Var
        public string Path { get; set; } // Public setter for XML-deserialization!
        public string Name => System.IO.Path.GetFileName(Path);
        #endregion

        #region Init
        public Theme() : this(string.Empty) { }
        public Theme(string Path) => this.Path = Path.EndsWith(System.IO.Path.PathSeparator.ToString()) ? System.IO.Path.GetDirectoryName(Path) : Path;
        #endregion

        #region Functions
        public override string ToString() => Name;

        public static IEnumerable<Theme> LoadThemes(string Path) => System.IO.Directory.GetDirectories(Path).Select(x => new Theme(x));
        #endregion
    }
}
