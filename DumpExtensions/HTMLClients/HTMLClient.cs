using System;

namespace VisualDump.HTMLClients
{
    public abstract class HTMLClient : IDisposable
    {
        #region Var
        public string Name { get; }
        #endregion

        #region Init
        public HTMLClient(string Name) => this.Name = Name;
        #endregion

        #region Functions
        public abstract void WaitForConnection(int TimeoutMS);
        public void WaitForConnection() => WaitForConnection(0);

        public abstract void Send(string Data);

        public abstract void Disconnect();

        public override string ToString() => Name;

        public virtual void Dispose() { }
        #endregion
    }
}
