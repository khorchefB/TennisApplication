using System.ComponentModel.DataAnnotations;

namespace Tennis.Application.Dtos;

public class PlayerDataDto
{
    [Range(1, int.MaxValue)]
    public int Rank { get; set; }

    [Range(0, int.MaxValue)]
    public int Points { get; set; }

    [Range(1, int.MaxValue)]
    public int Weight { get; set; }

    [Range(1, int.MaxValue)]
    public int Height { get; set; }

    [Range(1, 120)]
    public int Age { get; set; }

    [Required]
    [MinLength(1)]
    public List<int> Last { get; set; } = [];
}
