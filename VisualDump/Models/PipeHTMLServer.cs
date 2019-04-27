using System;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace VisualDump.Models
{
    public class PipeHTMLServer : HTMLServer
    {
        #region Var
        private readonly NamedPipeServerStream Stream;

        private bool Reading { get; set; }
        private Thread Listener { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        private const int MaxTerminateWaitTimeMS = 5000;
        #endregion

        #region Init
        public PipeHTMLServer(string Name) : base(Name) => Stream = new NamedPipeServerStream(Name, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        #endregion

        #region Functions
        public override async Task WaitForConnectionAsync(CancellationToken Token)
        {
            try
            {
                if (!Stream.IsConnected)
                    await Stream.WaitForConnectionAsync(Token);
            } 
            catch { }
        }

        public override void BeginRead()
        {
            if (!Reading)
            {
                TokenSource = new CancellationTokenSource();
                Listener = new Thread(() => Listen(TokenSource.Token));
                Listener.Start();
            }
        }

        private void Listen(CancellationToken Token)
        {
            Reading = true;
            while (!Token.IsCancellationRequested)
            {
                try
                {
                    int[] blockSizeData = new[] { Stream.ReadByte(), Stream.ReadByte(), Stream.ReadByte(), Stream.ReadByte() };
                    if (blockSizeData.Any(x => x == -1))
                        break;
                    int blockSize = BitConverter.ToInt32(blockSizeData.Select(x => (byte)x).ToArray(), 0);
                    if (blockSize > 0)
                    {
                        byte[] buffer = new byte[blockSize];
                        Stream.Read(buffer, 0, blockSize);
                        InvokeDataReceived(Encoding.UTF8.GetString(buffer));
                    }
                } 
                catch
                {
                    break;
                }
            }
            Reading = false;
        }

        public override void EndRead()
        {
            if (Reading)
            {
                TokenSource.Cancel();
                if (Listener.IsAlive)
                    Listener.Join(MaxTerminateWaitTimeMS);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            try
            {
                Stream.Dispose();
            } catch { }
        }
        #endregion
    }
}
