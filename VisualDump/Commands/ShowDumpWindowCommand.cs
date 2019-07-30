using KE.VSIX.Commands;
using Microsoft.VisualStudio.Shell;

namespace VisualDump.Commands
{
    [CommandID("e3161eb3-4aec-49de-bfe6-2eb5a01a0c34", 0x0100)]
    public sealed class ShowDumpWindowCommand : BaseCommand<ShowDumpWindowCommand>
    {
        protected override void Execute(OleMenuCommand Button) => VSPackage.ShowToolWindow(typeof(Controls.VisualDump));
    }
}
