using System;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class DateTimeHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<DateTime>(Obj, x => $"<div class='date'>{x}</div>");
    }
}
