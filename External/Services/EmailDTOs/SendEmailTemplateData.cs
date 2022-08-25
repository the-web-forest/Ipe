using Ipe.External.Services.DTOs;
using Newtonsoft.Json;

namespace Ipe.External.Services.EmailDTOs
{
    public class SendEmailTemplateData
    {
        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("orderId")]
        public string? OrderId { get; set; }

        [JsonProperty("orderPrice")]
        public string? OrderPrice { get; set; }

        [JsonProperty("items")]
        public List<SendEmailTemplateDataItem>? Items { get; set; }

        [JsonProperty("date")]
        public string? Date { get; set; }

        [JsonProperty("time")]
        public string? Time { get; set; }

        [JsonProperty("forestUrl")]
        public string? ForesUrl { get; set; }
    }
}

