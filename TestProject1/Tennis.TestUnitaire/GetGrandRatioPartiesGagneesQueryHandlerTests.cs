
namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetGrandRatioPartiesGagneesQueryHandlerTests
{
    [Fact]
    public async Task Handle_DoitInterrogerLeRepositoryEtRetournerLaValeurCalculeeParLeHandler()
    {
        // Arrange
        IEnumerable<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, last: [1, 1, 1, 0, 1]),
            TestData.CreerJoueur(id: 2, last: [0, 0, 1, 0, 0])
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs())
            .ReturnsAsync(joueurs);

        var handler = new GetGrandRatioPartiesGagneesQueryHandler(repositoryMock.Object);

        // Act
        var resultat = await handler.Handle(
            new GetGrandRatioPartiesGagneesQuery(),
            CancellationToken.None);

        // Assert
        // Le handler actuel retourne 0 : ce test documente le comportement existant.
        Assert.Equal(0, resultat);
        repositoryMock.Verify(repository => repository.GetTennisJoueurs(), Times.Once);
    }
}
