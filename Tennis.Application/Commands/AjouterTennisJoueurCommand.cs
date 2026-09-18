namespace Tennis.Application.Commands;

public sealed record AjouterTennisJoueurCommand(JoueurTennisDto Joueur) : ICommand<JoueurTennisDto>;
