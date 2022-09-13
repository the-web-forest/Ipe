using System.Drawing;
using System.Globalization;
using System.Web;
using Ipe.Domain.Models;
using Ipe.External.Services.DTOs;
using Ipe.External.Services.EmailDTOs;
using Ipe.UseCases.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ipe.External.Services
{
	public class SendGridService: IEmailService
	{

		private readonly SendGridClient _sendGridClient;
		private readonly IConfiguration _configuration;

		public SendGridService(
			SendGridClient sendGridClient,
			IConfiguration configuration
			)
		{
			_sendGridClient = sendGridClient;
			_configuration = configuration;
		}

		public async Task<bool> SendWelcomeEmail(string Email, string Name, string Token, string Role) {
			var WelcomeEmailTemplateId = _configuration["Email:Templates:WelcomeEmail"];
			var From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]);
			var To = new EmailAddress(Email, Name);
			var ButtonUrl = BuildWelcomeButtonUrl(Token, Role, Email);
			var TemplateData = new { buttonUrl = ButtonUrl };
			var Message = MailHelper.CreateSingleTemplateEmail(From, To, WelcomeEmailTemplateId, TemplateData);
			var Response = await _sendGridClient.SendEmailAsync(Message);
			return Response.IsSuccessStatusCode;
		}

		private string BuildWelcomeButtonUrl(string Token, string Role, string Email)
        {
			var BaseUrl = _configuration["Email:Urls:WelcomeEmail"];
            BaseUrl = BaseUrl.Replace("{{_EMAIL_}}", HttpUtility.UrlEncode(Email));
            BaseUrl = BaseUrl.Replace("{{_TOKEN_}}", HttpUtility.UrlEncode(Token));
            BaseUrl = BaseUrl.Replace("{{_ROLE_}}", HttpUtility.UrlEncode(Role));
            return BaseUrl;
		}

		public async Task<bool> SendPasswordResetEmail(string Email, string Name, string Token, string Role)
		{
			var UserFirstName = Name.Split(" ")[0];
			var WelcomeEmailTemplateId = _configuration["Email:Templates:PasswordResetEmail"];
			var From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]);
			var To = new EmailAddress(Email, Name);
			var ButtonUrl = BuildPasswordResetButtonUrl(Token, Role, Email);
			var TemplateData = new { buttonUrl = ButtonUrl, userName = UserFirstName };
			var Message = MailHelper.CreateSingleTemplateEmail(From, To, WelcomeEmailTemplateId, TemplateData);
			var Response = await _sendGridClient.SendEmailAsync(Message);
			return Response.IsSuccessStatusCode;
		}

        public async Task<bool> SendPlantSuccessEmail(string Email, string Name, Order Order, List<Tree> Trees)
        {
            var UserFirstName = Name.Split(" ")[0];
            var WelcomeEmailTemplateId = _configuration["Email:Templates:PlantSuccessEmail"];
            var From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]);
            var To = new EmailAddress(Email, UserFirstName);

			var Items = new List<SendEmailTemplateDataItem>();

			Order.Trees.ForEach(Tree =>
			{
				var CurrentTree = Trees.Find(x => x.Id == Tree.Id);

				Items.Add(new SendEmailTemplateDataItem
				{
					Quantity = Tree.Quantity.ToString(),
					Name = CurrentTree is not null ? CurrentTree.Name : "Árvore",
                });
			});

			var FormattedValue = Order.Value.ToString();
            var FirstPart = FormattedValue[..^2];
            var LastPart = FormattedValue[^2..];
            var Double = Convert.ToDouble(FirstPart + "." + LastPart);

            var TemplateData = new SendEmailTemplateData
			{
                UserName = UserFirstName,
                OrderId = Order.Id,
				OrderPrice = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Double),
				Date = string.Format("{0: dd/MM/yyyy}", Order.CreatedAt),
				Time = string.Format("{0: HH:mm:ss}", Order.CreatedAt),
				Items = Items,
				ForesUrl = _configuration["Email:Urls:MyForest"]
            };

            var Message = MailHelper.CreateSingleTemplateEmail(From, To, WelcomeEmailTemplateId, TemplateData);

			var Response = await _sendGridClient.SendEmailAsync(Message);
            return Response.IsSuccessStatusCode;
        }

        public async Task<bool> SendFirstPlantEmail(string Email, string Name)
        {
            var UserFirstName = Name.Split(" ")[0];
            var WelcomeEmailTemplateId = _configuration["Email:Templates:FirstPlantEmail"];
            var From = new EmailAddress(_configuration["Email:FromEmail"], _configuration["Email:FromName"]);
            var To = new EmailAddress(Email, Name);
            var TemplateData = new { userName = UserFirstName, userEmail = Email };
            var Message = MailHelper.CreateSingleTemplateEmail(From, To, WelcomeEmailTemplateId, TemplateData);
            var Response = await _sendGridClient.SendEmailAsync(Message);
            return Response.IsSuccessStatusCode;
        }

        private string BuildPasswordResetButtonUrl(string Token, string Role, string Email)
		{
			var BaseUrl = _configuration["Email:Urls:PasswordResetEmail"];
			BaseUrl = BaseUrl.Replace("{{_EMAIL_}}", HttpUtility.UrlEncode(Email));
			BaseUrl = BaseUrl.Replace("{{_TOKEN_}}", HttpUtility.UrlEncode(Token));
			BaseUrl = BaseUrl.Replace("{{_ROLE_}}", HttpUtility.UrlEncode(Role));
            return BaseUrl;
		}
	}
}

