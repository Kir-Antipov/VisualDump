using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace VisualDump.Models.HTMLBuilders
{
    internal abstract class AppendableHTMLBuilder : HTMLBuilder
    {
        #region Var
        protected abstract string Tag { get; }
        protected List<string> Appends { get; } = new List<string>();
        #endregion

        #region Functions
        public AppendableHTMLBuilder AppendFile(string Path) => Append(File.ReadAllText(Path));
        public AppendableHTMLBuilder Append(string Style) { Appends.Add(Style); return this; }

        public override string BuildHTML() => Appends.Aggregate(new StringBuilder(), (builder, x) => builder.Append('<').Append(Tag).Append('>').Append(x).Append("</").Append(Tag).Append('>')).ToString();
        #endregion
    }
}
