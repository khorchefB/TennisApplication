using Tennis.Application.Exceptions;

namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetIMCJoueursQueryHandlerTests
{
    [Fact]
    public async Task Handle_DoitRetournerLaMoyenneDesImcEnKgParMetreCarre()
    {
        IReadOnlyCollection<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, weight: 80_000, height: 200), // 20
            TestData.CreerJoueur(id: 2, weight: 75_000, height: 200)  // 18,75
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueurs);

        var handler = new GetIMCJoueursQueryHandler(repositoryMock.Object);

        var resultat = await handler.Handle(new GetIMCJoueursQuery(), CancellationToken.None);

        Assert.Equal(19.375, resultat, 6);
        repositoryMock.Verify(
            repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SansJoueur_DoitLeverUneExceptionMetier()
    {
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<TennisJoueur>());

        var handler = new GetIMCJoueursQueryHandler(repositoryMock.Object);

        await Assert.ThrowsAsync<TennisStatisticsUnavailableException>(
            () => handler.Handle(new GetIMCJoueursQuery(), CancellationToken.None));
    }
}
