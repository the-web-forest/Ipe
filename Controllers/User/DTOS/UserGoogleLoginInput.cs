using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.User;

public class UserGoogleLoginInput
{
    [Required]
    public string Token { get; set; }
}


