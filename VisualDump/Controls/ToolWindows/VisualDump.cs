using System;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;

namespace VisualDump.Controls
{
    [Guid("f0302c73-5c0e-4ac6-9134-c2db53be30aa")]
    public class VisualDump : ToolWindowPane
    {
        #region Init
        public VisualDump() : base(VSPackage.Instance)
        {
            Caption = "Visual Dump";
            Content = VisualDumpContainer.DumpWindow = new DumpWindow();
            VisualDumpContainer.DumpWindow.Clear();
        }
        #endregion
    }
}
