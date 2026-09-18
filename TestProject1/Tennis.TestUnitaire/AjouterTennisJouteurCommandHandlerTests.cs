namespace Tennis.Tests.Tennis.TestUnitaire;

public class AjouterTennisJouteurCommandHandlerTests
{
    [Fact]
    public async Task Handle_DoitAjouterEtRetournerLeJoueur()
    {
        // Arrange
        var repositoryMock = new Mock<ITennisPlayerRepository>();
        repositoryMock
            .Setup(repository => repository.AjouterTennisJoueur(It.IsAny<TennisJoueur>()))
            .ReturnsAsync((TennisJoueur joueur) => joueur);

        var handler = new AjouterTennisJouteurCommandHandler(repositoryMock.Object);
        var joueur = TestData.CreerJoueurDto(id: 999_001);
        var command = new AjouterTennisJouteurCommand(joueur);

        // Act
        var resultat = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(joueur.Id, resultat.Id);
        Assert.Equal(joueur.Firstname, resultat.Firstname);
        Assert.Equal(joueur.Lastname, resultat.Lastname);

        repositoryMock.Verify(
            repository => repository.AjouterTennisJoueur(
                It.Is<TennisJoueur>(joueurAjoute =>
                    joueurAjoute.Id == joueur.Id &&
                    joueurAjoute.Firstname == joueur.Firstname &&
                    joueurAjoute.Lastname == joueur.Lastname)),
            Times.Once);
    }
}
