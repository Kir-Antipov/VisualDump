using System.Web;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class CharHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<char>(Obj, x => $"<div class='string'>'{HttpUtility.HtmlEncode(x)}'</div>");
    }
}
