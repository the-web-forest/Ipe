using System;
using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.User.DTOS
{
    public class UserUpdateInput
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        public string City { get; set; }

        [Required]
        [MaxLength(2)]
        public string State { get; set; }
    }
}

