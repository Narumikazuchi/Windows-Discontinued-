namespace Narumikazuchi.Windows.Pipes;

internal sealed class __ClientPipe : NamedPipe
{
    public __ClientPipe(String serverNameOrIp,
                        String pipeName) :
        base(new NamedPipeClientStream(serverNameOrIp,
                                       pipeName,
                                       PipeDirection.InOut,
                                       PipeOptions.Asynchronous)) =>
            this._stream = (NamedPipeClientStream)this.Pipe;

    public void Connect()
    {
        this._stream.Connect();
        this._connected = true;
        this.OnConnected();
        this.StartReaderAsync();
    }

    private NamedPipeClientStream _stream;
}