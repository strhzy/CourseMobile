using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManagerMobile.S;

namespace DnDPartyManagerMobile.VM;

public partial class WebSocketViewModel : ObservableObject
{
    [ObservableProperty]
    private string ip = "192.168.1.1";

    [ObservableProperty]
    private string port = "8080";

    [ObservableProperty]
    private string connectionStatus = "WebSocket не подключен";

    private readonly WebSocketService _webSocketService;

    public WebSocketViewModel(WebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
        _webSocketService.OnError += (s, error) => ConnectionStatus = $"Ошибка: {error}";
    }

    [RelayCommand]
    private async Task ConnectToWebSocket()
    {
        // Проверка валидности IP и порта
        var ipParts = Ip.Split('.');
        bool isIpValid = ipParts.Length <= 4 && ipParts.All(p => int.TryParse(p, out int num) && num >= 0 && num <= 255);
        bool isPortValid = int.TryParse(Port, out int portNum) && portNum >= 0 && portNum <= 65535;
        Debug.Print("123123123");

        if (!isIpValid || !isPortValid)
        {
            ConnectionStatus = "Некорректный IP или порт";
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Проверьте IP и порт", "OK");
            return;
        }

        try
        {
            // Используем IP как есть (с точками)
            string url = $"ws://{Ip}:{Port}/ws";
            _webSocketService.UpdateUri(url);
            bool isConnected = await _webSocketService.TestConnectionAsync();
            ConnectionStatus = isConnected ? "WebSocket подключен" : "WebSocket не подключен";
        }
        catch (Exception ex)
        {
            ConnectionStatus = $"Ошибка: {ex.Message}";
            await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }
}