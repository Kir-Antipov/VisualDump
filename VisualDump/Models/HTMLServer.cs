using System;

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
        protected void InvokeDataReceived(string Data) => OnDataReceived?.Invoke(Data);

        public abstract void BeginRead();
        public abstract void EndRead();

        public virtual void Dispose() => EndRead();

        public override string ToString() => Name;
        #endregion
    }
}
