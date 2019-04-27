using System.Web;
using System.Text;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class StringBuilderHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<StringBuilder>(Obj, x => $"<div class='stringbuilder'><span class='string'>\"{HttpUtility.HtmlEncode(x)}\"</span> <span class='text'>({x.Length}/{x.Capacity})</span></div>");
    }
}
