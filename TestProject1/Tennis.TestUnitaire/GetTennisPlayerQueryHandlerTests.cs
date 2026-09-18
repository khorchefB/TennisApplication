namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetTennisPlayerQueryHandlerTests
{
    [Fact]
    public async Task Handle_QuandLeJoueurExiste_DoitRetournerLeJoueurDemande()
    {
        // Arrange
        var joueur = TestData.CreerJoueur(
            id: 95,
            firstname: "Venus",
            lastname: "Williams");

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisPlayer(95))
            .ReturnsAsync(joueur);

        var handler = new GetTennisPlayerQueryHandler(repositoryMock.Object);

        // Act
        var resultat = await handler.Handle(
            new GetTennisPlayerQuery(95),
            CancellationToken.None);

        // Assert
        Assert.NotNull(resultat);
        Assert.Equal(95, resultat.Id);
        Assert.Equal("Venus", resultat.Firstname);
        repositoryMock.Verify(repository => repository.GetTennisPlayer(95), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandLeJoueurNExistePas_DoitRetournerNull()
    {
        // Arrange
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisPlayer(404))
            .ReturnsAsync((TennisJoueur?)null);

        var handler = new GetTennisPlayerQueryHandler(repositoryMock.Object);

        // Act
        var resultat = await handler.Handle(
            new GetTennisPlayerQuery(404),
            CancellationToken.None);

        // Assert
        Assert.Null(resultat);
        repositoryMock.Verify(repository => repository.GetTennisPlayer(404), Times.Once);
    }
}
