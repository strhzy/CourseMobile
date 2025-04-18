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
        var enemyList = await DBHelper.GetEnemiesAsync(CurrentPage, Search);
        
        foreach (var enem in enemyList)
        {
            Enemies.Add(enem);
            Console.WriteLine(Enemies.Count);
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