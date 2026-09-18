using System.ComponentModel.DataAnnotations;

namespace Tennis.Application.Dtos;

public class TennisPlayerDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Lastname { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Shortname { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string Sex { get; set; } = string.Empty;

    [Required]
    public CountryDto Country { get; set; } = new();

    public string Picture { get; set; } = string.Empty;

    [Required]
    public PlayerDataDto Data { get; set; } = new();
}
