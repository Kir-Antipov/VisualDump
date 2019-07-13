using System;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class DateTimeHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<DateTime>(Obj, CallStack, (x, s) => $"<div class='date'>{x}</div>");
    }
}
