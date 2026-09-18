namespace Tennis.Application.Dtos;

public class TennisPlayerDto
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Shortname { get; set; }
    public string Sex { get; set; }
    public CountryDto Country { get; set; }
    public string Picture { get; set; }
    public PlayerDataDto Data { get; set; }
}
