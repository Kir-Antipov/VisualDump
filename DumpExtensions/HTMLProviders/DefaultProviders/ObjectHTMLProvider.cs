using System;
using System.Linq;
using System.Text;
using System.Reflection;
using VisualDump.Helpers;
using VisualDump.ExtraTypes;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class ObjectHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) 
        {
            Stack<object> newCallStack = CallStack.CloneAndPush(Obj);
            if (Obj is null)
                return GetProvider<NullReference>().ToHTML(Obj, newCallStack);
            if (CallStack.Any(x => ReferenceEquals(x, Obj)))
                return GetProvider<CyclicalReference>().ToHTML(new CyclicalReference(Obj), newCallStack);
            Type t = Obj.GetType();
            StringBuilder builder = new StringBuilder();
            StringBuilder Append(string data) => builder.Append(data);
            Append("<div class='table-wrap'>")
                .Append("<div class='title-box'>")
                    .Append("<h4 class='title-box__headline'>")
                        .AppendTypeName(t)
                    .Append("</h4>")
                    .Append("<a class='title-box__collapse'>&#9660;</a>")
                .Append("</div>")
                .Append("<table class='table'>")
                    .Append("<tbody>")
                        .AppendFullTypeIfNotSystem(t);

            foreach (MemberInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public).Cast<MemberInfo>().Concat(t.GetFields(BindingFlags.Instance | BindingFlags.Public)))
                Append("<tr class='row'>")
                    .Append("<td title='").Append(getMemberType(prop).FullName).Append("' class='name'>")
                        .Append(prop.Name)
                    .Append("</td>")
                    .Append("<td class='value'>")
                        .Append(inspectMemberInfo(prop))
                    .Append("</td>")
                .Append("</tr>");

                    Append("</tbody>")
                .Append("</table>")
            .Append("</div>");
            return builder.ToString();

            string inspectMemberInfo(MemberInfo member)
            {
                try
                {
                    HTMLProvider provider = GetProvider(getMemberType(member));
                    object inner = member is FieldInfo field ? field.GetValue(Obj) : ((PropertyInfo)member).GetValue(Obj);
                    if (CallStack.Any(x => ReferenceEquals(x, inner)))
                        return GetProvider<CyclicalReference>().ToHTML(new CyclicalReference(inner), newCallStack);
                    return provider.ToHTML(inner, newCallStack);
                }
                catch
                {
                    return GetProvider<NullReference>().ToHTML(null, newCallStack);
                }
            }
            Type getMemberType(MemberInfo member) => member is FieldInfo field ? field.FieldType : ((PropertyInfo)member).PropertyType;
        }
    }
}
