using DnDPartyManagerMobile.ViewModels;

namespace DnDPartyManagerMobile.Views;

public partial class CharacterPage : ContentPage
{
    public CharacterPage()
    {
        InitializeComponent();
        BindingContext = new CharactersPageViewModel();
    }
}