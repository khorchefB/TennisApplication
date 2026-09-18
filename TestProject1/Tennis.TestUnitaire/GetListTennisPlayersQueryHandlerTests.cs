namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetListTennisPlayersQueryHandlerTests
{
    [Fact]
    public async Task Handle_DoitRetournerLesJoueursDuMeilleurAuMoinsBonRang()
    {
        IReadOnlyCollection<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, rank: 1, firstname: "Novak"),
            TestData.CreerJoueur(id: 2, rank: 50, firstname: "Venus"),
            TestData.CreerJoueur(id: 3, rank: 20, firstname: "Stan")
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()))
            .ReturnsAsync(joueurs);

        var handler = new GetListTennisPlayersQueryHandler(repositoryMock.Object);

        var resultat = await handler.Handle(
            new GetListTennisPlayersQuery(),
            CancellationToken.None);

        Assert.Equal(new[] { 1, 3, 2 }, resultat.Select(joueur => joueur.Id));
        repositoryMock.Verify(
            repository => repository.GetTennisJoueurs(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
