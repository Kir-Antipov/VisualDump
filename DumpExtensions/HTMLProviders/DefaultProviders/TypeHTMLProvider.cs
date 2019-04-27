using System;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class TypeHTMLProvider : HTMLProvider
    {
        #region Functions
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<Type>(Obj, t => $"<div class='keyword'>{t.FullName}</div>");
        #endregion
    }
}
