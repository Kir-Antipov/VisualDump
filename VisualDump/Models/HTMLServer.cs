using System;
using System.Threading;
using System.Threading.Tasks;

namespace VisualDump.Models
{
    public abstract class HTMLServer : IDisposable
    {
        #region Var
        public string Name { get; }

        public event Action<string> OnDataReceived;
        #endregion

        #region Init
        public HTMLServer(string Name) => this.Name = Name;
        #endregion

        #region Functions
        protected virtual void InvokeDataReceived(string Data) => OnDataReceived?.Invoke(Data);

        public abstract Task WaitForConnectionAsync(CancellationToken Token);
        public Task WaitForConnectionAsync() => WaitForConnectionAsync(CancellationToken.None);
        public Task WaitForConnectionAsync(int TimeoutMS) => WaitForConnectionAsync(TimeoutMS < 1 ? new CancellationTokenSource().Token : new CancellationTokenSource(TimeoutMS).Token);

        public abstract void BeginRead();
        public abstract void EndRead();

        public virtual void Dispose() => EndRead();

        public override string ToString() => Name;
        #endregion
    }
}
