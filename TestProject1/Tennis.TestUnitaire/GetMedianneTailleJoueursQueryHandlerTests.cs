using Tennis.Application.Exceptions;

namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetMedianneTailleJoueursQueryHandlerTests
{
    [Fact]
    public async Task Handle_AvecUnNombreImpairDeJoueurs_DoitRetournerLaMediane()
    {
        IReadOnlyCollection<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, height: 190),
            TestData.CreerJoueur(id: 2, height: 170),
            TestData.CreerJoueur(id: 3, height: 180)
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueurs);

        var handler = new GetMedianneTailleJoueursQueryHandler(repositoryMock.Object);

        var resultat = await handler.Handle(new GetMedianneTailleJoueursQuery(), CancellationToken.None);

        Assert.Equal(180d, resultat);
        repositoryMock.Verify(
            repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_AvecUnNombrePairDeJoueurs_DoitRetournerLaMoyenneDesDeuxValeursCentrales()
    {
        IReadOnlyCollection<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, height: 170),
            TestData.CreerJoueur(id: 2, height: 180),
            TestData.CreerJoueur(id: 3, height: 190),
            TestData.CreerJoueur(id: 4, height: 200)
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueurs);

        var handler = new GetMedianneTailleJoueursQueryHandler(repositoryMock.Object);

        var resultat = await handler.Handle(new GetMedianneTailleJoueursQuery(), CancellationToken.None);

        Assert.Equal(185d, resultat);
    }

    [Fact]
    public async Task Handle_SansJoueur_DoitLeverUneExceptionMetier()
    {
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<TennisJoueur>());

        var handler = new GetMedianneTailleJoueursQueryHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<TennisStatisticsUnavailableException>(
            () => handler.Handle(new GetMedianneTailleJoueursQuery(), CancellationToken.None));
    }
}
