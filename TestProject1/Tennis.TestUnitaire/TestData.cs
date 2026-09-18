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
            Prenom = firstname,
            Nom = lastname,
            NomCourt = $"{firstname[0]}.{lastname.ToUpperInvariant()}",
            Sexe = "M",
            Photo = "https://example.test/player.png",
            Pays = new Pays
            {
                Code = "FRA",
                Photo = "https://example.test/france.png"
            },
            Donnees = new DonneesJoueur
            {
                Rang = rank,
                Points = 1_000,
                Poids = weight,
                Taille = height,
                Age = age,
                DerniersResultats = (last ?? [1, 1, 0, 1, 1]).ToList()
            }
        };

    public static JoueurTennisDto CreerJoueurDto(
        int id,
        int rank = 1,
        int weight = 80_000,
        int height = 180,
        string firstname = "Test",
        string lastname = "Player") => new()
        {
            Id = id,
            Prenom = firstname,
            Nom = lastname,
            NomCourt = $"{firstname[0]}.{lastname.ToUpperInvariant()}",
            Sexe = "M",
            Photo = "https://example.test/player.png",
            Pays = new PaysDto
            {
                Code = "FRA",
                Photo = "https://example.test/france.png"
            },
            Donnees = new DonneesJoueurDto
            {
                Rang = rank,
                Points = 1_000,
                Poids = weight,
                Taille = height,
                Age = 25,
                DerniersResultats = [1, 1, 0, 1, 1]
            }
        };
}
