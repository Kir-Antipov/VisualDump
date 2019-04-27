using System;
using System.Text;
using System.IO.Pipes;

namespace VisualDump.HTMLClients
{
    public class PipeHTMLClient : HTMLClient
    {
        #region Var
        private readonly NamedPipeClientStream Stream;
        #endregion

        #region Init
        public PipeHTMLClient(string Name) : base(Name) => Stream = new NamedPipeClientStream(".", Name, PipeDirection.Out);
        #endregion

        #region Functions
        public override void WaitForConnection(int TimeoutMS)
        {
            if (TimeoutMS > 0)
                Stream.Connect(TimeoutMS);
            else
                Stream.Connect();
        }

        public override void Send(string Data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            foreach (byte x in BitConverter.GetBytes(bytes.Length))
                Stream.WriteByte(x);
            Stream.Write(bytes, 0, bytes.Length);
        }

        public override void Disconnect() => Stream.Close();

        public override void Dispose()
        {
            base.Dispose();
            Stream.Dispose();
        }
        #endregion
    }

}
