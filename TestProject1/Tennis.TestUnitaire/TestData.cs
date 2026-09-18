using Tennis.Application.Dtos;

namespace Tennis.Tests.Tennis.TestUnitaire;

internal static class TestData
{
    public static TennisJoueur CreerJoueur(
        int id,
        int rank = 1,
        int weight = 80_000,
        int height = 180,
        int age = 25,
        IEnumerable<int>? last = null,
        string firstname = "Test",
        string lastname = "Player") => new()
        {
            Id = id,
            Firstname = firstname,
            Lastname = lastname,
            Shortname = $"{firstname[0]}.{lastname.ToUpperInvariant()}",
            Sex = "M",
            Picture = "https://example.test/player.png",
            Country = new Country
            {
                Code = "FRA",
                Picture = "https://example.test/france.png"
            },
            Data = new PlayerData
            {
                Rank = rank,
                Points = 1_000,
                Weight = weight,
                Height = height,
                Age = age,
                Last = (last ?? [1, 1, 0, 1, 1]).ToList()
            }
        };

    public static TennisPlayerDto CreerJoueurDto(
        int id,
        int rank = 1,
        int weight = 80_000,
        int height = 180,
        string firstname = "Test",
        string lastname = "Player") => new()
        {
            Id = id,
            Firstname = firstname,
            Lastname = lastname,
            Shortname = $"{firstname[0]}.{lastname.ToUpperInvariant()}",
            Sex = "M",
            Picture = "https://example.test/player.png",
            Country = new CountryDto
            {
                Code = "FRA",
                Picture = "https://example.test/france.png"
            },
            Data = new PlayerDataDto
            {
                Rank = rank,
                Points = 1_000,
                Weight = weight,
                Height = height,
                Age = 25,
                Last = [1, 1, 0, 1, 1]
            }
        };
}
