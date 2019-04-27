using System;
using System.Linq;
using System.Reflection;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class EnumHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<Enum>(Obj, x => $"<div><span class='enum'>{x.GetType().Name}</span><span class='text'>.{x}</span> (<span class='number'>{x.GetType().GetMembers().OfType<FieldInfo>().FirstOrDefault(y => y.Name == "value__")?.GetValue(x) ?? 0}</span>)</div>");
    }
}
