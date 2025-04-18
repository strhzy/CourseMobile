using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManagerMobile.M;

namespace DnDPartyManagerMobile.VM;

public partial class PlayerCharactersViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<PlayerCharacter> playerCharacters;
    
    public PlayerCharactersViewModel()
    {
        
    }
}