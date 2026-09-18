using Tennis.Application.Queries;
using Tennis.Domain.Models;

namespace Tennis.Tests.Tennis.TestUnitaire;

public class GetIMCJoueursQueryHandlerTests
{
    [Fact]
    public async Task Handle_DoitRetournerLaMoyenneDesImcDesJoueurs()
    {
        // Arrange
        // Joueur 1 : 100 / 10² = 1
        // Joueur 2 : 800 / 20² = 2
        // Moyenne attendue : 1,5
        IEnumerable<TennisJoueur> joueurs =
        [
            TestData.CreerJoueur(id: 1, weight: 100, height: 10),
            TestData.CreerJoueur(id: 2, weight: 800, height: 20)
        ];

        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.GetTennisJoueurs())
            .ReturnsAsync(joueurs);

        var handler = new GetIMCJoueursQueryHandler(repositoryMock.Object);

        // Act
        var resultat = await handler.Handle(
            new GetIMCJoueursQuery(),
            CancellationToken.None);

        // Assert
        Assert.Equal(1.5, resultat, 6);
        repositoryMock.Verify(repository => repository.GetTennisJoueurs(), Times.Once);
    }
}
