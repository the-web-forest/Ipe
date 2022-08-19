using Ipe.Controllers.Plant.Validators;
using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.Plant.DTOs;
public class PlantCustomizeInput
{
    [Required]
    [StringLength(24)]
    public string PlantId { get; set; }

    [StringLength(50)]
    public string TreeName { get; set; }

    [StringLength(280)]
    public string TreeMessage { get; set; }

    [MaxLength(5)]
    [StringLengthList(1, 50)]
    public List<string> TreeHastags { get; set; }

    public bool IsInvalid()
    {
        return string.IsNullOrEmpty(TreeName) &&
            string.IsNullOrEmpty(TreeMessage) &&
            (TreeHastags is null || !TreeHastags.Any());
    }
}
