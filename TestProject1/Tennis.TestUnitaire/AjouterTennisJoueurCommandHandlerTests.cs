namespace Tennis.Tests.Tennis.TestUnitaire;

public class AjouterTennisJoueurCommandHandlerTests
{
    [Fact]
    public async Task Handle_DoitAjouterEtRetournerLeJoueur()
    {
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.AjouterTennisJoueur(
                It.IsAny<TennisJoueur>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((TennisJoueur joueur, CancellationToken _) => joueur);

        var handler = new AjouterTennisJoueurCommandHandler(repositoryMock.Object);
        var joueur = TestData.CreerJoueurDto(id: 999_001);

        var resultat = await handler.Handle(
            new AjouterTennisJoueurCommand(joueur),
            CancellationToken.None);

        Assert.Equal(joueur.Id, resultat.Id);
        Assert.Equal(joueur.Prenom, resultat.Prenom);
        Assert.Equal(joueur.Nom, resultat.Nom);

        repositoryMock.Verify(
            repository => repository.AjouterTennisJoueur(
                It.Is<TennisJoueur>(joueurAjoute =>
                    joueurAjoute.Id == joueur.Id &&
                    joueurAjoute.Prenom == joueur.Prenom &&
                    joueurAjoute.Nom == joueur.Nom),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
