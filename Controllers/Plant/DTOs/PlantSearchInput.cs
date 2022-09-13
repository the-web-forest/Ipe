using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.Plant.DTOs;
public class PlantSearchInput
{
    [MaxLength(15)]
    public string? Biome { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public bool? RequiredTotal { get; set; }
}