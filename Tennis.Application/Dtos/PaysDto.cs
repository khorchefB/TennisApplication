using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tennis.Application.Dtos;

public class PaysDto
{
    [JsonPropertyName("picture")]
    public string Photo { get; set; } = string.Empty;

    [Required]
    [StringLength(3, MinimumLength = 2)]
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
}
