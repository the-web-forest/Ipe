using System;
using System.Text.Json.Serialization;

namespace Ipe.External.Services.DTOs
{
    public class GoogleUserResponse
    {
        [JsonPropertyName("iss")]
        public string Iss { get; set; }

        [JsonPropertyName("azp")]
        public string Azp { get; set; }

        [JsonPropertyName("aud")]
        public string Aud { get; set; }

        [JsonPropertyName("sub")]
        public string Sub { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("email_verified")]
        public string EmailVerified { get; set; }

        [JsonPropertyName("at_hash")]
        public string AtHash { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; }

        [JsonPropertyName("given_name")]
        public string GiveName { get; set; }

        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("iat")]
        public string Iat { get; set; }

        [JsonPropertyName("exp")]
        public string Exp { get; set; }

        [JsonPropertyName("jti")]
        public string Jwt { get; set; }

        [JsonPropertyName("alg")]
        public string Alg { get; set; }

        [JsonPropertyName("kid")]
        public string Kid { get; set; }

        [JsonPropertyName("typ")]
        public string Typ { get; set; }
    }
}

