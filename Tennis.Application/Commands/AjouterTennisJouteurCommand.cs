namespace Tennis.Application.Commands;

public sealed record AjouterTennisJouteurCommand(TennisPlayerDto Joueur) : ICommand<TennisPlayerDto>;
