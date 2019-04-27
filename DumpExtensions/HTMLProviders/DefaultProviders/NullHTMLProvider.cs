namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class NullHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => "<div class='keyword'>null</div>";
    }
}
