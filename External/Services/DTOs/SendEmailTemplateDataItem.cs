using Newtonsoft.Json;

namespace Ipe.External.Services.DTOs
{
    public class SendEmailTemplateDataItem
    {
        [JsonProperty("quantity")]
        public string? Quantity { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}

