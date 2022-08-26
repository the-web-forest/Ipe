using System;
using Ipe.UseCases;
using Ipe.UseCases.Interfaces.Services;
using System.Text;
using System.Security.Policy;
using System.Web;
using Ipe.External.Services.DTOs;
using System.Net;
using System.Text.Json;

namespace Ipe.External.Services
{
    public class GoogleService: IGoogleService
    {
        private readonly string GoogleAuthUrl;
        private readonly IHttpClientFactory _httpClientFactory;


        public GoogleService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            GoogleAuthUrl = configuration["Google:Login:Url"];
        }

        public async Task<GoogleUserResponse> GetUserInfoByGoogleToken(string GoogleToken)
        {
            var HttpClient = _httpClientFactory.CreateClient();
            var UriGoogle = new UriBuilder(GoogleAuthUrl);
            var Query = HttpUtility.ParseQueryString(UriGoogle.Query);
            Query["id_token"] = GoogleToken;
            UriGoogle.Query = Query.ToString();

            var HttpRequest = new HttpRequestMessage(HttpMethod.Get, UriGoogle.ToString());
            var HttpResponse = await HttpClient.SendAsync(HttpRequest);

            var GoogleUser = new GoogleUserResponse();

            var User = JsonSerializer.Deserialize<GoogleUserResponse>(HttpResponse.Content.ReadAsStringAsync().Result);

            if (User is not null)
            {
                GoogleUser = User;
            }

            return GoogleUser;
        }



    }
}

