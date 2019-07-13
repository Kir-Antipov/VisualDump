using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class BooleanHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<bool>(Obj, CallStack, (x, s) => $"<div class='keyword'>{(x ? "true" : "false")}</div>");
    }
}
