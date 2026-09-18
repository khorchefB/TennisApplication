namespace Tennis.Application.Queries;

public class GetTennisPlayerQuery : IQuery<TennisPlayerDto>
{
    public GetTennisPlayerQuery(int idPlayer)
    {
        IdPlayer = idPlayer;
    }
    public int IdPlayer { get; set; }
}
