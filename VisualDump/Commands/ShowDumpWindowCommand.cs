using System;
using Microsoft.VisualStudio.Shell;

namespace VisualDump.Commands
{
    public sealed class ShowDumpWindowCommand : BaseCommand<ShowDumpWindowCommand>
    {
        #region Init
        static ShowDumpWindowCommand()
        {
            CommandID = 0x0100;
            CommandSet = new Guid("e3161eb3-4aec-49de-bfe6-2eb5a01a0c34");
        }
        #endregion

        #region Functions
        protected override void Execute(OleMenuCommand Button) => VSPackage.ShowToolWindow(typeof(Controls.VisualDump));
        #endregion
    }
}
