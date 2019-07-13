using System;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class TypeHTMLProvider : HTMLProvider
    {
        #region Functions
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<Type>(Obj, CallStack, (t, s) => $"<div class='keyword'>{t.FullName}</div>");
        #endregion
    }
}
