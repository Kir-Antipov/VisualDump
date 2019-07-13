using System.Web;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class CharHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<char>(Obj, CallStack, (x, s) => $"<div class='string'>'{HttpUtility.HtmlEncode(x)}'</div>");
    }
}
