using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.Trees.DTOs
{
    public class TreeSearchInput
    {
        [MaxLength(15)]
        public string? Biome { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? RequiredTotal { get; set; }
    }
}
