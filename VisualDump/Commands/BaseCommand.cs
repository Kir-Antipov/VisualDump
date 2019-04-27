using System;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace VisualDump.Commands
{
    public abstract class BaseCommand<T> where T : BaseCommand<T>, new()
    {
        #region Var
        public string Text
        {
            get => Command.Text;
            set => ChangeText(value);
        }
        public static T Instance { get; private set; }
        public AsyncPackage Package { get; private set; }
        public static int CommandID { get; internal set; }
        public static Guid CommandSet { get; internal set; }
        protected IServiceProvider ServiceProvider => Package;
        protected OleMenuCommand Command { get; private set; }
        protected OleMenuCommandService Service { get; private set; }
        protected IAsyncServiceProvider AsyncServiceProvider => Package;
        #endregion

        #region Init
        public static async Task InitializeAsync(AsyncPackage Package)
        {
            Instance = new T();
            Instance.Init(Package, await Package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService);
        }
        protected virtual void BeforeInit() { }
        private void Init(AsyncPackage Package, OleMenuCommandService CommandService)
        {
            BeforeInit();
            this.Package = Package ?? throw new ArgumentNullException(nameof(Package));
            Service = CommandService ?? throw new ArgumentNullException(nameof(CommandService));
            Command = new OleMenuCommand((sender, e) => { ThreadHelper.ThrowIfNotOnUIThread(); Execute((OleMenuCommand)sender); }, new CommandID(CommandSet, CommandID));
            Service.AddCommand(Command);
            AfterInit();
        }
        protected virtual void AfterInit() { }
        #endregion

        #region Functions
        protected abstract void Execute(OleMenuCommand Button);

        private void ChangeText(string NewText)
        {
            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                commandService.RemoveCommand(Command);
                void change(object sender, EventArgs e)
                {
                    OleMenuCommand command = (OleMenuCommand)sender;
                    command.Text = NewText;
                    command.BeforeQueryStatus -= change;
                }
                Command.BeforeQueryStatus += change;
                commandService.AddCommand(Command);
            }
        }
        #endregion
    }
}
