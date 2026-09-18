using Tennis.Domain.Dtos;

namespace Tennis.Domain.Models;

public class TennisJoueur
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Shortname { get; set; }
    public string Sex { get; set; }
    public Country Country { get; set; }
    public string Picture { get; set; }
    public PlayerData Data { get; set; }
}
