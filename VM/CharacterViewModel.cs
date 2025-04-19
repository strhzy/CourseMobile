using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManagerMobile.M;

namespace DnDPartyManagerMobile.ViewModels;

public partial class CharacterViewModel : ObservableObject
{
    private readonly PlayerCharacter _character;

    public PlayerCharacter Character => _character;

    public CharacterViewModel(PlayerCharacter character)
    {
        _character = character;
    }
}