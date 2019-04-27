using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class IEnumerableHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<IEnumerable>(Obj, enumerable =>
        {
            Type t = Obj.GetType();
            StringBuilder builder = new StringBuilder();
            StringBuilder Append(string data) => builder.Append(data);
            Append("<div class='table-wrap'>")
                .Append("<div class='title-box'>")
                    .Append("<h4 class='title-box__headline'>")
                        .AppendTypeName(t)
                        .Append(" (").Append(enumerable.Cast<object>().Count()).Append(" items)")
                    .Append("</h4>")
                    .Append("<a class='title-box__reorganize'>&#9935;</a>")
                    .Append("<a class='title-box__collapse'>&#9660;</a>")
                .Append("</div>")
                .Append("<table class='table'>")
                    .Append("<tbody>");
            if (!(t.GetInterfaces().Where(x => x.IsGenericType && !x.IsGenericTypeDefinition && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).Select(x => x.GetGenericArguments()[0]).FirstOrDefault()?.IsAnonymous() ?? false))
                        builder.AppendFullTypeIfNotSystem(t);
            foreach (object x in enumerable)
                        Append("<tr class='row'>")
                            .Append("<td class='value'>")
                                .Append(GetProvider(x).ToHTML(x, Args))
                            .Append("</td>")
                        .Append("</tr>");

                    Append("</tbody>")
                .Append("</table>")
            .Append("</div>");
            return builder.ToString();
        });
    }
}
