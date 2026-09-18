using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tennis.Application.Dtos;

public class JoueurTennisDto
{
    [Range(1, int.MaxValue)]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("firstname")]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [JsonPropertyName("lastname")]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [JsonPropertyName("shortname")]
    public string NomCourt { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    [JsonPropertyName("sex")]
    public string Sexe { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("country")]
    public PaysDto Pays { get; set; } = new();

    [JsonPropertyName("picture")]
    public string Photo { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("data")]
    public DonneesJoueurDto Donnees { get; set; } = new();
}
