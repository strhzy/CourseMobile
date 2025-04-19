using DnDPartyManagerMobile.Views;

namespace DnDPartyManagerMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("CharacterDetailsPage", typeof(CharacterDetailsPage));
    }
}