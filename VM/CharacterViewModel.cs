using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManagerMobile.M;
using DnDPartyManagerMobile.Services;

namespace DnDPartyManagerMobile.VM;

public partial class CharacterViewModel : ObservableObject
{
    [ObservableProperty]
    private PlayerCharacter playerCharacter;
    
    public CharacterViewModel(PlayerCharacter character)
    {
        PlayerCharacter = character;
    }
    
    [RelayCommand]
    private async Task Save()
    {
        await CharacterService.SaveCharacterAsync(PlayerCharacter);
    }
    
    [RelayCommand]
    private async Task Delete()
    {
        await CharacterService.DeleteCharacterAsync(PlayerCharacter.Id);
    }
}