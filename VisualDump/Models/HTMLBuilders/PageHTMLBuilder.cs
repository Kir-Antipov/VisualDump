using System.IO;

namespace VisualDump.Models.HTMLBuilders
{
    internal class PageHTMLBuilder : HTMLBuilder
    {
        #region Var
        public StyleHTMLBuilder StyleBuilder => HeadBuilder.StyleBuilder;
        public BodyHTMLBuilder BodyBuilder { get; } = new BodyHTMLBuilder();
        public HeadHTMLBuilder HeadBuilder { get; } = new HeadHTMLBuilder();
        public ScriptHTMLBuilder ScriptBuilder { get; } = new ScriptHTMLBuilder();
        public DefaultScriptHTMLBuilder DefaultScriptBuilder { get; } = new DefaultScriptHTMLBuilder();
        #endregion

        #region Functions
        public PageHTMLBuilder LoadTheme(string Path)
        {
            if (Directory.Exists(Path))
                foreach (string file in Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories))
                    switch (System.IO.Path.GetExtension(file).ToLower())
                    {
                        case ".css":
                            StyleBuilder.AppendFile(file);
                            break;
                        case ".js":
                            ScriptBuilder.AppendFile(file);
                            break;
                        default:
                            break;
                    }
            return this;
        }
        public PageHTMLBuilder SetTheme(bool IsDark)
        {
            BodyBuilder.IsDarkTheme = IsDark;
            return this;
        }

        public override string BuildHTML() => $"<!DOCTYPE html><html>{HeadBuilder}{BodyBuilder}{DefaultScriptBuilder}{ScriptBuilder}</html>";
        #endregion
    }
}
