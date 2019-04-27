using System;
using System.Data;
using System.Collections;
using VisualDump.HTMLProviderArgs;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class ArrayHTMLProvider : HTMLProvider
    {
        public override string ToHTML(object Obj, params object[] Args) => ToHTML<Array>(Obj, arr =>
        {
            Type arrType = Obj.GetType();
            Type elType = arr.GetType().GetElementType();
            if (elType?.IsArray ?? false && !elType.GetElementType().IsArray && arr.Length > 0)
            {
                int rows = arr.Length;
                int columns = ((Array)arr.GetValue(0)).Length;
                bool isMatrix = true;
                for (int i = 1; i < rows && isMatrix; ++i)
                    if (((Array)arr.GetValue(i)).Length != columns)
                        isMatrix = false;
                if (isMatrix)
                {
                    elType = elType.GetElementType();
                    Array tmp = Array.CreateInstance(elType, rows, columns);
                    for (int row = 0; row < rows; ++row)
                        for (int column = 0; column < columns; ++column)
                            tmp.SetValue(((Array)arr.GetValue(row)).GetValue(column), row, column);
                    arr = tmp;
                }
            }
            if (arr.Rank == 2)
            {
                DataTable table = new DataTable();
                int rows = arr.GetLength(0);
                int columns = arr.GetLength(1);
                for (int column = 0; column < columns; ++column)
                    table.Columns.Add(column.ToString(), elType);
                for (int row = 0; row < rows; ++row)
                {
                    table.Rows.Add();
                    for (int column = 0; column < columns; ++column)
                        table.Rows[row][column] = arr.GetValue(row, column);
                }
                table.TableName = arrType.GetName();
                string fullType = arrType.FullName ;
                DataTableDumpStyle style = DataTableDumpStyle.TableName |
                                           DataTableDumpStyle.CountFields |
                                           DataTableDumpStyle.ShowRowIndices |
                                           DataTableDumpStyle.ShowColumnNames;
                return GetProvider<DataTable>().ToHTML(table, arrType.Namespace.StartsWith("System") || elType.IsAnonymous() ? new DataTableArgs(style) : new DataTableArgs(style, fullType));
            }
            return GetProvider<IEnumerable>().ToHTML(arr, Args);
        });
    }
}
