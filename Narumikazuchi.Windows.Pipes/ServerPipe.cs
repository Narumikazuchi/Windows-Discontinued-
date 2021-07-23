using System;
using System.Diagnostics;
using System.IO.Pipes;

namespace Narumikazuchi.Windows.Pipes
{
    partial class NamedPipeServer<TMessage>
    {
        private sealed class ServerPipe : NamedPipe
        {
            #region Constructor

            internal ServerPipe(String pipeName)
            {
                if (pipeName is null)
                {
                    throw new ArgumentNullException(nameof(pipeName));
                }

                this._stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                this.Pipe = this._stream;
                this._stream.BeginWaitForConnection(this.OnConnected, null);
            }

            #endregion

            #region Connection

            private void OnConnected(IAsyncResult result)
            {
                this._stream.EndWaitForConnection(result);
                this.OnConnected();
                this.StartReaderAsync();
            }

            #endregion

            #region Fields

            [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
            private NamedPipeServerStream _stream;

            #endregion
        }
    }
}
