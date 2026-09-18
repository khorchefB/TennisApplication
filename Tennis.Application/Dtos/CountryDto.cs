using System.ComponentModel.DataAnnotations;

namespace Tennis.Application.Dtos;

public class CountryDto
{
    public string Picture { get; set; } = string.Empty;

    [Required]
    [StringLength(3, MinimumLength = 2)]
    public string Code { get; set; } = string.Empty;
}
