using Tennis.Application.Exceptions;

namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetTennisPlayerQueryHandlerTests
{
    [Fact]
    public async Task Handle_QuandLeJoueurExiste_DoitRetournerLeJoueurDemande()
    {
        var joueur = TestData.CreerJoueur(id: 95, firstname: "Venus", lastname: "Williams");
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisPlayer(95, It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueur);

        var handler = new GetTennisPlayerQueryHandler(repositoryMock.Object);

        var resultat = await handler.Handle(new GetTennisPlayerQuery(95), CancellationToken.None);

        Assert.Equal(95, resultat.Id);
        Assert.Equal("Venus", resultat.Prenom);
        repositoryMock.Verify(
            repository => repository.GetTennisPlayer(95, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_QuandLeJoueurNExistePas_DoitLeverTennisPlayerNotFoundException()
    {
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisPlayer(404, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TennisJoueur?)null);

        var handler = new GetTennisPlayerQueryHandler(repositoryMock.Object);

        var exception = await Assert.ThrowsAsync<TennisPlayerNotFoundException>(
            () => handler.Handle(new GetTennisPlayerQuery(404), CancellationToken.None));

        Assert.Equal(404, exception.IdPlayer);
        repositoryMock.Verify(
            repository => repository.GetTennisPlayer(404, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
