using System.Web;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class StringHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<string>(Obj, x => $"<div class='string'>\"{HttpUtility.HtmlEncode(x)}\"</div>");
    }
}
