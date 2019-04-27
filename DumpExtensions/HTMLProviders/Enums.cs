using System;

namespace VisualDump.HTMLProviders
{
    [Flags]
    public enum DataTableDumpStyle
    {
        TableName         = 0b0000001,
        CountRows         = 0b0000010,
        CountColumns      = 0b0000100,
        CountFields       = 0b0001000,
        FullType          = 0b0010000,
        ShowColumnNames   = 0b0100000,
        ShowRowIndices    = 0b1000000,

        Default = TableName | CountRows | FullType | ShowColumnNames,
        NumberingDefault = Default | ShowRowIndices
    }
}
