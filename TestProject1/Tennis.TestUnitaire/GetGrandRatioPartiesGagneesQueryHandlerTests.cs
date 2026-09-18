using Tennis.Application.Exceptions;

namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetGrandRatioPartiesGagneesQueryHandlerTests
{
    [Fact]
    public async Task Handle_DoitRetournerLePaysAvecLeMeilleurRatioDeVictoires()
    {
        var joueurSerbe = TestData.CreerJoueur(id: 1, last: [1, 1, 1, 1, 1]);
        joueurSerbe.Pays.Code = "SRB";
        var joueurAmericain = TestData.CreerJoueur(id: 2, last: [1, 0, 0, 0, 0]);
        joueurAmericain.Pays.Code = "USA";

        IReadOnlyCollection<TennisJoueur> joueurs = [joueurSerbe, joueurAmericain];
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueurs);

        var handler = new GetGrandRatioPartiesGagneesQueryHandler(repositoryMock.Object);

        var resultat = await handler.Handle(
            new GetGrandRatioPartiesGagneesQuery(),
            CancellationToken.None);

        Assert.Equal("SRB", resultat);
        repositoryMock.Verify(
            repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SansDonneesDeMatch_DoitLeverUneExceptionMetier()
    {
        var joueur = TestData.CreerJoueur(id: 1, last: []);
        IReadOnlyCollection<TennisJoueur> joueurs = [joueur];
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueurs);

        var handler = new GetGrandRatioPartiesGagneesQueryHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<TennisStatisticsUnavailableException>(
            () => handler.Handle(new GetGrandRatioPartiesGagneesQuery(), CancellationToken.None));
    }
}
