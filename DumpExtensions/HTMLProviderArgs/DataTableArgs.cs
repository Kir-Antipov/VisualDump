using VisualDump.HTMLProviders;

namespace VisualDump.HTMLProviderArgs
{
    public class DataTableArgs
    {
        #region Var
        public string CustomFullType { get; }
        public DataTableDumpStyle Style { get; }
        #endregion

        #region Init
        public DataTableArgs() : this(DataTableDumpStyle.Default, null) { }
        public DataTableArgs(DataTableDumpStyle Style) : this(Style, null) { }
        public DataTableArgs(string CustomFullType) : this(DataTableDumpStyle.Default, CustomFullType) { }
        public DataTableArgs(string CustomFullType, DataTableDumpStyle Style) : this(Style, CustomFullType) { }

        public DataTableArgs(DataTableDumpStyle Style, string CustomFullType)
        {
            this.Style = Style;
            this.CustomFullType = CustomFullType;
        }
        #endregion
    }
}
