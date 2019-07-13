using System.Reflection;
using VisualDump.Helpers;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class AssemblyHTMLProvider : HTMLProvider
    {
        #region Functions
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<Assembly>(Obj, CallStack, (a, s) => GetProvider<string>().ToHTML(a.FullName, s.CloneAndPush(a)));
        #endregion
    }
}
