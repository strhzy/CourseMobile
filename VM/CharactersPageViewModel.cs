using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManagerMobile.M;
using System.Collections.ObjectModel;

namespace DnDPartyManagerMobile.ViewModels;

public partial class CharactersPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CharacterViewModel> characters = new();

    [ObservableProperty]
    private bool isRefreshing;

    public CharactersPageViewModel()
    {
        LoadCharactersCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadCharacters()
    {
        try
        {
            IsRefreshing = true;
            Characters.Clear();
            // Имитация загрузки данных
            Characters.Add(new CharacterViewModel(new PlayerCharacter
            {
                Name = "Маркус",
                PlayerName = "Биба Бобский",
                ClassType = "Воин",
                Level = 3,
                Strength = 16,
                Dexterity = 12,
                Constitution = 14,
                Intelligence = 10,
                Wisdom = 11,
                Charisma = 13,
                ProficiencyBonus = 2,
                ArmorClass = 18,
                MaxHitPoints = 30,
                CurrentHitPoints = 25
            }));
            Characters.Add(new CharacterViewModel(new PlayerCharacter
            {
                Name = "Марк Аврелий",
                PlayerName = "Боба Бибский",
                ClassType = "Волшебник",
                Level = 2,
                Strength = 8,
                Dexterity = 14,
                Constitution = 12,
                Intelligence = 16,
                Wisdom = 13,
                Charisma = 10,
                ProficiencyBonus = 2,
                ArmorClass = 15,
                MaxHitPoints = 15,
                CurrentHitPoints = 12
            }));
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task CardTapped(CharacterViewModel character)
    {
        if (character == null)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Персонаж не выбран", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"/CharacterDetailsPage", new Dictionary<string, object>
        {
            { "Character", character }
        });
    }
}