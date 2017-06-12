using Newtonsoft.Json;
using System.Collections.Generic;

namespace DevPodcasts.ServiceLayer.Email
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
