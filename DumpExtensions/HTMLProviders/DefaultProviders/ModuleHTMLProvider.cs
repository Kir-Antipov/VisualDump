using System.Reflection;
using VisualDump.Helpers;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class ModuleHTMLProvider : HTMLProvider
    {
        #region Functions
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<Module>(Obj, CallStack, (x, s) => GetProvider<string>().ToHTML(x.FullyQualifiedName, s.CloneAndPush(x)));
        #endregion
    }
}
