namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class BooleanHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<bool>(Obj, x => $"<div class='keyword'>{(x ? "true" : "false")}</div>");
    }
}
