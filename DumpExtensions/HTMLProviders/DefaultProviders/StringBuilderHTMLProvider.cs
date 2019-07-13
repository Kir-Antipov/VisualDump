using System.Web;
using System.Text;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class StringBuilderHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<StringBuilder>(Obj, CallStack, (x, s) => $"<div class='stringbuilder'><span class='string'>\"{HttpUtility.HtmlEncode(x)}\"</span> <span class='text'>({x.Length}/{x.Capacity})</span></div>");
    }
}
