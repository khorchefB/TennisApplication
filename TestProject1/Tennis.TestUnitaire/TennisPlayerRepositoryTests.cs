using Tennis.Application.Exceptions;

namespace Tennis.Tests.Tennis.TestUnitaire;

public class TennisPlayerRepositoryTests
{
    [Fact]
    public async Task GetTennisJoueurs_ApresInitialisation_DoitRetournerLesJoueursChargesDepuisLeJson()
    {
        var repository = new TennisPlayerRepository();
        await repository.StartAsync(CancellationToken.None);

        var joueurs = await repository.GetTennisJoueurs(CancellationToken.None);

        Assert.NotEmpty(joueurs);
        Assert.Contains(joueurs, joueur =>
            joueur.Id == 52 &&
            joueur.Prenom == "Novak" &&
            joueur.Nom == "Djokovic");
    }

    [Fact]
    public async Task AjouterTennisJoueur_DoitAjouterLeJoueurDansLaCollectionEnMemoire()
    {
        var repository = new TennisPlayerRepository();
        await repository.StartAsync(CancellationToken.None);
        var joueur = TestData.CreerJoueur(id: 999_100, firstname: "Nouveau", lastname: "Joueur");

        var joueurAjoute = await repository.AjouterTennisJoueur(joueur, CancellationToken.None);
        var joueurs = await repository.GetTennisJoueurs(CancellationToken.None);

        Assert.Same(joueur, joueurAjoute);
        Assert.Contains(joueurs, item => item.Id == joueur.Id);
        Assert.Single(joueurs.Where(item => item.Id == joueur.Id));
    }

    [Fact]
    public async Task AjouterTennisJoueur_AvecUnIdExistant_DoitLeverTennisPlayerAlreadyExistsException()
    {
        var repository = new TennisPlayerRepository();
        await repository.StartAsync(CancellationToken.None);
        var joueurExistant = (await repository.GetTennisJoueurs(CancellationToken.None)).First();
        var doublon = TestData.CreerJoueur(id: joueurExistant.Id);

        var exception = await Assert.ThrowsAsync<TennisPlayerAlreadyExistsException>(
            () => repository.AjouterTennisJoueur(doublon, CancellationToken.None));

        Assert.Equal(joueurExistant.Id, exception.IdPlayer);
    }
}
