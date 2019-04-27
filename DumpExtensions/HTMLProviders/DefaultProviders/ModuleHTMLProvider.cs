using System.Reflection;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class ModuleHTMLProvider : HTMLProvider
    {
        #region Functions
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<Module>(Obj, x => GetProvider<string>().ToHTML(x.FullyQualifiedName));
        #endregion
    }
}
