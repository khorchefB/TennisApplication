using Tennis.Domain.Models;

namespace Tennis.Application.Commands;

public sealed class AjouterTennisJoueurCommandHandler(ITennisPlayerRepository tennisPlayerRepository)
    : ICommandHandler<AjouterTennisJoueurCommand, JoueurTennisDto>
{
    public async Task<JoueurTennisDto> Handle(
        AjouterTennisJoueurCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Joueur);

        var joueur = request.Joueur.Adapt<TennisJoueur>();
        var joueurAjoute = await tennisPlayerRepository.AjouterTennisJoueur(joueur, cancellationToken);

        return joueurAjoute.Adapt<JoueurTennisDto>();
    }
}
