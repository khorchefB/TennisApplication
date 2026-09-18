namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetMedianneTailleJoueursQueryHandlerTests
{
    [Fact]
    public async Task Handle_AvecUnNombreImpairDeJoueurs_DoitRetournerLaMediane()
    {
        // Arrange
        IEnumerable<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, height: 190),
            TestData.CreerJoueur(id: 2, height: 170),
            TestData.CreerJoueur(id: 3, height: 180)
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs())
            .ReturnsAsync(joueurs);

        var handler = new GetMedianneTailleJoueursQueryHandler(repositoryMock.Object);

        // Act
        var resultat = await handler.Handle(
            new GetMedianneTailleJoueursQuery(),
            CancellationToken.None);

        // Assert
        Assert.Equal(180d, resultat);
        repositoryMock.Verify(repository => repository.GetTennisJoueurs(), Times.Once);
    }

    [Fact]
    public async Task Handle_AvecUnNombrePairDeJoueurs_DoitRetournerLaMoyenneDesDeuxValeursCentrales()
    {
        // Arrange
        IEnumerable<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, height: 170),
            TestData.CreerJoueur(id: 2, height: 180),
            TestData.CreerJoueur(id: 3, height: 190),
            TestData.CreerJoueur(id: 4, height: 200)
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs())
            .ReturnsAsync(joueurs);

        var handler = new GetMedianneTailleJoueursQueryHandler(repositoryMock.Object);

        // Act
        var resultat = await handler.Handle(
            new GetMedianneTailleJoueursQuery(),
            CancellationToken.None);

        // Assert
        Assert.Equal(185d, resultat);
        repositoryMock.Verify(repository => repository.GetTennisJoueurs(), Times.Once);
    }
}
