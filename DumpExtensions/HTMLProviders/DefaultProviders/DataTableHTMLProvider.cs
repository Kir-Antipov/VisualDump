using System;
using System.Data;
using System.Text;
using VisualDump.Helpers;
using System.Collections.Generic;
using VisualDump.HTMLProviderArgs;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class DataTableHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => ToHTML<DataTable, DataTableArgs>(Obj, CallStack, Args, (table, stack, a) => 
        {
            Stack<object> newCallStack = stack.CloneAndPush(table);
            Type t = table.GetType();
            bool showRowIndex = a.Style.HasFlag(DataTableDumpStyle.ShowRowIndices);
            bool showColName = a.Style.HasFlag(DataTableDumpStyle.ShowColumnNames);
            StringBuilder builder = new StringBuilder();
            StringBuilder Append(string data) => builder.Append(data);
            Append("<div class='table-wrap'>")
                .Append("<div class='title-box'>")
                    .Append("<h4 class='title-box__headline'>")
                        .Append(a.Style.HasFlag(DataTableDumpStyle.TableName) ? string.IsNullOrEmpty(table.TableName) ? t.GetName() : table.TableName : t.GetName());
            if (a.Style.HasFlag(DataTableDumpStyle.CountRows))
                        Append(" (").Append(table.Rows.Count).Append(" items)");
            else if (a.Style.HasFlag(DataTableDumpStyle.CountColumns))
                        Append(" (").Append(table.Columns.Count).Append(" items)");
            else if (a.Style.HasFlag(DataTableDumpStyle.CountFields))
                        Append(" (").Append(table.Rows.Count * table.Columns.Count).Append(" items)");
                    Append("</h4>")
                    .Append("<a class='title-box__collapse'>&#9660;</a>")
                .Append("</div>")
                .Append("<table class='table'>")
                    .Append("<tbody>");
            if (a.Style.HasFlag(DataTableDumpStyle.FullType))
                            Append("<tr>")
                                .Append("<th class='type' colspan='100'>")
                                    .Append(string.IsNullOrEmpty(a.CustomFullType) ? t.FullName : a.CustomFullType)
                                .Append("</th>")
                            .Append("</tr>");
            if (showColName)
            {
                            Append("<tr class='row-header'>");
                if (showRowIndex)
                                Append("<th class='name'></th>");
                for (int i = 0; i < table.Columns.Count; ++i)
                                Append("<th class='name'>").Append(table.Columns[i].ColumnName).Append("</th>");
                            Append("</tr>");
            }
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                        Append("<tr class='row'>");
                if (showRowIndex)
                            Append("<td class='name'>").Append(i).Append("</td>");
                DataRow row = table.Rows[i];
                for (int j = 0; j < table.Columns.Count; ++j)
                            Append("<td class='value'>").Append(GetProvider(row[j]).ToHTML(row[j], newCallStack)).Append("</td>");
                        Append("</tr>");
            }
                    Append("</tbody>")
                .Append("</table>")
            .Append("</div>");
            return builder.ToString();
        });
    }
}
