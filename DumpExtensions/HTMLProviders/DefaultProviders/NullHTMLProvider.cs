using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class NullHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => "<div class='keyword'>null</div>";
    }
}
