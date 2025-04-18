using System.Text.Json;
using DnDPartyManagerMobile.M;
using DnDPartyManagerMobile.S;
using Microsoft.Maui.Storage;

namespace DnDPartyManagerMobile.Services;

public static class CharacterService
{
    private const string CharactersCacheKey = "characters_cache";
    private const string LastUpdatedKey = "characters_last_updated";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(1);

    public static async Task<IEnumerable<PlayerCharacter>> GetCharactersAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && !IsCacheExpired())
        {
            var cachedCharacters = GetCachedCharacters();
            if (cachedCharacters.Any())
            {
                return cachedCharacters;
            }
        }
        
        var freshCharacters = await LoadFreshCharactersAsync();

        await CacheCharactersAsync(freshCharacters);
        
        return freshCharacters;
    }

    public static async Task<PlayerCharacter> GetCharacterAsync(int id)
    {
        var characters = await GetCharactersAsync();
        return characters.FirstOrDefault(c => c.Id == id);
    }

    public static async Task SaveCharacterAsync(PlayerCharacter character)
    {
        var characters = (await GetCharactersAsync()).ToList();
        
        if (character.Id == 0)
        {
            // Новый персонаж
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
        }
        else
        {
            // Обновление существующего
            var existing = characters.FirstOrDefault(c => c.Id == character.Id);
            if (existing != null)
            {
                characters.Remove(existing);
                characters.Add(character);
            }
        }

        await CacheCharactersAsync(characters);
    }

    public static async Task DeleteCharacterAsync(int id)
    {
        var characters = (await GetCharactersAsync()).ToList();
        var characterToRemove = characters.FirstOrDefault(c => c.Id == id);
        
        if (characterToRemove != null)
        {
            characters.Remove(characterToRemove);
            await CacheCharactersAsync(characters);
        }
    }

    private static async Task<IEnumerable<PlayerCharacter>> LoadFreshCharactersAsync()
    {
        // Здесь реализуйте загрузку из основного источника данных
        // Это может быть API, база данных и т.д.
        // Для примера возвращаем пустой список
        return new List<PlayerCharacter>();
    }

    private static IEnumerable<PlayerCharacter> GetCachedCharacters()
    {
        var json = Preferences.Get(CharactersCacheKey, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            return Enumerable.Empty<PlayerCharacter>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<PlayerCharacter>>(json) ?? Enumerable.Empty<PlayerCharacter>();
        }
        catch
        {
            return Enumerable.Empty<PlayerCharacter>();
        }
    }

    private static async Task CacheCharactersAsync(IEnumerable<Character> characters)
    {
        var json = JsonSerializer.Serialize(characters);
        Preferences.Set(CharactersCacheKey, json);
        Preferences.Set(LastUpdatedKey, DateTime.UtcNow.ToString("O"));
    }

    private static bool IsCacheExpired()
    {
        var lastUpdatedStr = Preferences.Get(LastUpdatedKey, string.Empty);
        if (string.IsNullOrEmpty(lastUpdatedStr))
        {
            return true;
        }

        if (DateTime.TryParse(lastUpdatedStr, out var lastUpdated))
        {
            return DateTime.UtcNow - lastUpdated > CacheExpiration;
        }

        return true;
    }

    public static void ClearCache()
    {
        Preferences.Remove(CharactersCacheKey);
        Preferences.Remove(LastUpdatedKey);
    }
}