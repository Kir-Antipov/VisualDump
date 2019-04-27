using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;

namespace VisualDump.Controls
{
    [Guid(VSPackage.PackageGuidString)]
    public class OptionPageGrid : DialogPage
    {
        #region Var
        public OptionControl OptionControl { get; private set; }
        protected override IWin32Window Window => OptionControl = new OptionControl();
        #endregion
    }
}
