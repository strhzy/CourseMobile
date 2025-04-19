using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace DnDPartyManagerMobile.Views;

public partial class CharacterCard : ContentView
{
    public static readonly BindableProperty CardTappedCommandProperty =
        BindableProperty.Create(nameof(CardTappedCommand), typeof(ICommand), typeof(CharacterCard));

    public ICommand CardTappedCommand
    {
        get => (ICommand)GetValue(CardTappedCommandProperty);
        set => SetValue(CardTappedCommandProperty, value);
    }

    public CharacterCard()
    {
        InitializeComponent();
    }
}