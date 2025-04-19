using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DnDPartyManagerMobile.VM;

public partial class WebSocketViewModel : ObservableObject
{
    [ObservableProperty] private string ip_address;
    
    public WebSocketViewModel()
    {
        
    }

    [RelayCommand]
    private async Task PingMaster()
    {
        if (ip_address != null && ip_address != "")
        {
            
        }
    }
}