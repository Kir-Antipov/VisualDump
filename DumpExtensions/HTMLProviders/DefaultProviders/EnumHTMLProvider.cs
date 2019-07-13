using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class EnumHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<Enum>(Obj, CallStack, (x, s) => $"<div><span class='enum'>{x.GetType().Name}</span><span class='text'>.{x}</span> (<span class='number'>{x.GetType().GetMembers().OfType<FieldInfo>().FirstOrDefault(y => y.Name == "value__")?.GetValue(x) ?? 0}</span>)</div>");
    }
}
