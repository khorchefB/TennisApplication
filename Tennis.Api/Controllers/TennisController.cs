using MediatR;

namespace Tennis.Api.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class TennisController(ITennisPlayerRepository tennisPlayerRepository,
                              ISender sender) : ControllerBase
{

    [HttpGet]
    [Route("players")]
    public async Task<IEnumerable<TennisPlayerDto>> GetTennisJoueurs()
    {
        var query = new GetListTennisPlayersQuery();
        return await sender.Send(query);
    }

    [HttpGet]
    [Route("players/{idPlayer}")]
    public async Task<TennisPlayerDto> GetTennisJoueur([FromRoute] int idPlayer)
    {
        var query = new GetTennisPlayerQuery(idPlayer);
        return await sender.Send(query);
    }

    [HttpGet]
    [Route("players/statistiques")]
    public async Task<StatistiquesDto> GetIMCMoyenTennisJoueurs([FromRoute] int idPlayer)
    {
        var query = new GetIMCJoueursQuery();
        var imcMoyen = await sender.Send(query);

        var getGrandRatioPartiesGagneesQuery = new GetGrandRatioPartiesGagneesQuery();
        var ratio = await sender.Send(query);

        var GetMedianneTailleJoueursQuery = new GetMedianneTailleJoueursQuery();
        var medianne = await sender.Send(query);

        return new StatistiquesDto { Ratio = ratio, IMC = imcMoyen, Medianne = medianne };
    }

    [HttpPost]
    [Route("players/ajouter")]
    public async Task AjouterJoueur(TennisJoueur joueur)
    {

    }
}
