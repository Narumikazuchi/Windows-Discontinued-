using System;
using System.Diagnostics;
using System.IO.Pipes;

namespace Narumikazuchi.Windows.Pipes
{
    partial class NamedPipeClient<T>
    {
        private sealed class ClientPipe : NamedPipe
        {
            #region Constructor

            internal ClientPipe(String serverNameOrIp, String pipeName)
            {
                if (serverNameOrIp is null)
                {
                    throw new ArgumentNullException(nameof(serverNameOrIp));
                }
                if (pipeName is null)
                {
                    throw new ArgumentNullException(nameof(pipeName));
                }

                this._stream = new NamedPipeClientStream(serverNameOrIp, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                this.Pipe = this._stream;
            }

            #endregion

            #region Connection

            internal void Connect()
            {
                this._stream.Connect();
                this.OnConnected();
                this.StartReaderAsync();
            }

            #endregion

            #region Fields

            [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
            private NamedPipeClientStream _stream;

            #endregion
        }
    }
}
