namespace Tennis.Tests.Tennis.TestUnitaire;

public class TennisPlayerRepositoryTests
{
    [Fact]
    public async Task GetTennisJoueurs_ApresInitialisation_DoitRetournerLesJoueursChargesDepuisLeJson()
    {
        // Arrange
        var repository = new TennisPlayerRepository();
        await repository.StartAsync(CancellationToken.None);

        // Act
        var joueurs = (await repository.GetTennisJoueurs()).ToList();

        // Assert
        Assert.NotEmpty(joueurs);
        Assert.Contains(joueurs, joueur =>
            joueur.Id == 52 &&
            joueur.Firstname == "Novak" &&
            joueur.Lastname == "Djokovic");

        await repository.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task AjouterTennisJoueur_DoitAjouterLeJoueurDansLaCollectionEnMemoire()
    {
        // Arrange
        var repository = new TennisPlayerRepository();
        await repository.StartAsync(CancellationToken.None);
        var joueur = TestData.CreerJoueur(
            id: 999_100,
            firstname: "Nouveau",
            lastname: "Joueur");

        // Act
        var joueurAjoute = await repository.AjouterTennisJoueur(joueur);
        var joueurs = (await repository.GetTennisJoueurs()).ToList();

        // Assert
        Assert.Same(joueur, joueurAjoute);
        Assert.Contains(joueurs, item => item.Id == joueur.Id);
        Assert.Equal(1, joueurs.Count(item => item.Id == joueur.Id));

        await repository.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task AjouterTennisJoueur_AvecUnIdExistant_DoitLeverUneInvalidOperationException()
    {
        // Arrange
        var repository = new TennisPlayerRepository();
        await repository.StartAsync(CancellationToken.None);
        var joueurExistant = (await repository.GetTennisJoueurs()).First();
        var doublon = TestData.CreerJoueur(id: joueurExistant.Id);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AjouterTennisJoueur(doublon));

        // Assert
        Assert.Contains(joueurExistant.Id.ToString(), exception.Message);

        await repository.StopAsync(CancellationToken.None);
    }
}
