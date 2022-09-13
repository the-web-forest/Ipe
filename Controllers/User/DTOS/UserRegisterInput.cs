using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.User.DTOS;

	public class UserRegisterInput
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(8)]
		public string Password { get; set; }
	}