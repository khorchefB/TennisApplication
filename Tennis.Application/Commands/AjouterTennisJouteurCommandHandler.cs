using Tennis.Domain.Models;

namespace Tennis.Application.Commands;

public sealed class AjouterTennisJouteurCommandHandler(ITennisPlayerRepository tennisPlayerRepository)
    : ICommandHandler<AjouterTennisJouteurCommand, TennisPlayerDto>
{
    public async Task<TennisPlayerDto> Handle(
        AjouterTennisJouteurCommand request,
        CancellationToken cancellationToken)
    {
        var joueur = request.Joueur.Adapt<TennisJoueur>();
        var joueurAjoute = await tennisPlayerRepository.AjouterTennisJoueur(joueur);

        return joueurAjoute.Adapt<TennisPlayerDto>();
    }
}
