using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManagerMobile.Services;

namespace DnDPartyManagerMobile.VM;

public partial class CharactersPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CharacterViewModel> characters = new();
    
    [ObservableProperty]
    private bool isRefreshing;
    
    [RelayCommand]
    private async Task LoadCharacters()
    {
        try 
        {
            IsRefreshing = true;
            Characters.Clear();
            
            var characters = await CharacterService.GetCharactersAsync();
            foreach (var character in characters)
            {
                Characters.Add(new CharacterViewModel(character));
            }
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}