
using Ipe.Domain.Models;

namespace Ipe.UseCases.Interfaces
{
	public interface IEmailService
	{
		Task<bool> SendWelcomeEmail(string Email, string Name, string Token, string Role);
		Task<bool> SendPasswordResetEmail(string Email, string Name, string Token, string Role);
		Task<bool> SendPlantSuccessEmail(string Email, string Name, Order Order, List<Tree> Trees);
		Task<bool> SendFirstPlantEmail(string Email, string Name);
    }
}