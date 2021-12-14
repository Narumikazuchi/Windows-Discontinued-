namespace Narumikazuchi.Windows.Pipes;

internal sealed class __ServerPipe : NamedPipe
{
    internal __ServerPipe(String pipeName) :
        base(new NamedPipeServerStream(pipeName,
                                       PipeDirection.InOut,
                                       NamedPipeServerStream.MaxAllowedServerInstances,
                                       PipeTransmissionMode.Message,
                                       PipeOptions.Asynchronous))
    {
        this._stream = (NamedPipeServerStream)this.Pipe;
        this._stream.BeginWaitForConnection(this.OnConnected, null);
    }

    private void OnConnected(IAsyncResult result)
    {
        this._stream.EndWaitForConnection(result);
        this._connected = true;
        this.OnConnected();
        this.StartReaderAsync();
    }

    private NamedPipeServerStream _stream;
}