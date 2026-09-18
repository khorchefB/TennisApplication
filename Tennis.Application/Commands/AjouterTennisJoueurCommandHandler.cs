using Tennis.Domain.Models;

namespace Tennis.Application.Commands;

public sealed class AjouterTennisJoueurCommandHandler(ITennisPlayerRepository tennisPlayerRepository)
    : ICommandHandler<AjouterTennisJoueurCommand, TennisPlayerDto>
{
    public async Task<TennisPlayerDto> Handle(
        AjouterTennisJoueurCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Joueur);

        var joueur = request.Joueur.Adapt<TennisJoueur>();
        var joueurAjoute = await tennisPlayerRepository.AjouterTennisJoueur(joueur, cancellationToken);

        return joueurAjoute.Adapt<TennisPlayerDto>();
    }
}
