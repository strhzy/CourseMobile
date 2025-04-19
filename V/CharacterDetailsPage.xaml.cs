using DnDPartyManagerMobile.ViewModels;

namespace DnDPartyManagerMobile.Views;

[QueryProperty(nameof(Character), "Character")]
public partial class CharacterDetailsPage : ContentPage
{
    private CharacterViewModel _character;

    public CharacterViewModel Character
    {
        get => _character;
        set
        {
            _character = value;
            BindingContext = _character;
            OnPropertyChanged();
        }
    }

    public CharacterDetailsPage()
    {
        InitializeComponent();
    }
}