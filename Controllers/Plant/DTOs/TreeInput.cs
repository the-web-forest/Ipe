using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.Plant.DTOs
{
    public class TreeInput
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Range(0, 999)]
        public int Quantity { get; set; }

    }
}

