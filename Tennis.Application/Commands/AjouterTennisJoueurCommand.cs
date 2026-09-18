namespace Tennis.Application.Commands;

public sealed record AjouterTennisJoueurCommand(TennisPlayerDto Joueur) : ICommand<TennisPlayerDto>;
