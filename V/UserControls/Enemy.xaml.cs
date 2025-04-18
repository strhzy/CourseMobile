using DnDPartyManagerMobile.VM;
using Microsoft.Maui.Controls;

namespace DnDPartyManagerMobile.V.UserControls;

public partial class Enemy : ContentView
{
    public Enemy()
    {
        InitializeComponent();
    }

    private void OnToggleButtonClicked(object sender, EventArgs e)
    {
        detailsPanel.IsVisible = !detailsPanel.IsVisible;
        toggleButton.Text = detailsPanel.IsVisible ? "Свернуть" : "Развернуть";
    }
}