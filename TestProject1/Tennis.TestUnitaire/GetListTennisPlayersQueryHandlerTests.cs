namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetListTennisPlayersQueryHandlerTests
{
    [Fact]
    public async Task Handle_DoitRetournerLesJoueursTriesParRankDecroissant()
    {
        // Arrange
        IEnumerable<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, rank: 1, firstname: "Novak"),
            TestData.CreerJoueur(id: 2, rank: 50, firstname: "Venus"),
            TestData.CreerJoueur(id: 3, rank: 20, firstname: "Stan")
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs())
            .ReturnsAsync(joueurs);

        var handler = new GetListTennisPlayersQueryHandler(repositoryMock.Object);

        // Act
        var resultat = (await handler.Handle(
            new GetListTennisPlayersQuery(),
            CancellationToken.None))!.ToList();

        // Assert
        Assert.Equal(new[] { 2, 3, 1 }, resultat.Select(joueur => joueur.Id));
        repositoryMock.Verify(repository => repository.GetTennisJoueurs(), Times.Once);
    }
}
