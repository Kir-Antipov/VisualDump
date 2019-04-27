using System.Reflection;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class AssemblyHTMLProvider : HTMLProvider
    {
        #region Functions
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<Assembly>(Obj, a => GetProvider<string>().ToHTML(a.FullName));
        #endregion
    }
}
