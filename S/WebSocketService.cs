using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DnDPartyManagerMobile.S;

public class WebSocketService
{
    private ClientWebSocket _webSocket;
    private string _uri;
    private CancellationTokenSource _cancellationTokenSource;

    public event EventHandler<string> OnMessageReceived;
    public event EventHandler<string> OnError;

    public WebSocketService(string uri)
    {
        _uri = uri;
        _webSocket = new ClientWebSocket();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void UpdateUri(string newUri)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            throw new InvalidOperationException("Нельзя изменить URI во время активного подключения.");
        }
        _uri = newUri;
    }

    public async Task ConnectAsync()
    {
        try
        {
            if (_webSocket.State != WebSocketState.Open)
            {
                await _webSocket.ConnectAsync(new Uri(_uri), _cancellationTokenSource.Token);
                _ = ReceiveMessagesAsync();
            }
        }
        catch (Exception ex)
        {
            OnError?.Invoke(this, $"Ошибка подключения: {ex.Message}");
            throw;
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancellationTokenSource.Token);
            }
        }
        catch (Exception ex)
        {
            OnError?.Invoke(this, $"Ошибка отключения: {ex.Message}");
        }
        finally
        {
            _webSocket.Dispose();
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            if (_webSocket.State == WebSocketState.Open)
                return true;

            await ConnectAsync();
            return _webSocket.State == WebSocketState.Open;
        }
        catch
        {
            return false;
        }
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
            }
            else
            {
                OnError?.Invoke(this, "WebSocket не подключен.");
            }
        }
        catch (Exception ex)
        {
            OnError?.Invoke(this, $"Ошибка отправки: {ex.Message}");
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024 * 4];
        while (_webSocket.State == WebSocketState.Open && !_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    OnMessageReceived?.Invoke(this, message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await DisconnectAsync();
                    break;
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, $"Ошибка получения сообщения: {ex.Message}");
                break;
            }
        }
    }
}