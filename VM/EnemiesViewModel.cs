namespace DnDPartyManagerMobile.VM;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManagerMobile.M;
using DnDPartyManagerMobile.S;

public partial class EnemiesViewModel : ObservableObject
{
    [ObservableProperty] 
    private List<string> slugs;
    
    [ObservableProperty] 
    private Enemy enemy;
    
    [ObservableProperty] 
    private string search = "";
    
    [ObservableProperty] 
    private int currentPage = 1;
    
    [ObservableProperty] 
    private ObservableCollection<Enemy> enemies = new();

    [RelayCommand]
    private async Task LoadEnemies()
    {
        Enemies.Clear();
        try
        {
            foreach (var enem in await DBHelper.GetEnemiesAsync(CurrentPage, Search))
            {
                Enemies.Add(enem);
                Console.WriteLine(Enemies.Count);
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("хост неизвестен"))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Отсутствует подключение к интернету", "ОК");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка",ex.Message,"OK");
            }
        }
    }

    [RelayCommand]
    private async Task NextPage()
    {
        CurrentPage++;
        await LoadEnemies();
    }
    
    [RelayCommand]
    private async Task PrevPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadEnemies();
        }
    }

    public EnemiesViewModel()
    {
        LoadEnemiesCommand.Execute(null);
    }
}