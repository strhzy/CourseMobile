using DnDPartyManagerMobile.M;

namespace DnDPartyManagerMobile.S;

public interface ICharacterService
{
    Task<IEnumerable<PlayerCharacter>> GetCharactersAsync();
    Task<PlayerCharacter> GetCharacterAsync(int id);
    Task SaveCharacterAsync(PlayerCharacter character);
    Task DeleteCharacterAsync(int id);
}