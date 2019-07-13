using System.Web;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class StringHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<string>(Obj, CallStack, (x, s) => $"<div class='string'>\"{HttpUtility.HtmlEncode(x)}\"</div>");
    }
}
