using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class StableDiffusionXLResponse : IFoundationModelResponse
    {
        public class StableDiffusionXLGeneratedImage
        {
            [JsonPropertyName("base64")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]    
            public string? Base64 { get; set; }

            [JsonPropertyName("finishReason")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? FinishReason { get; set; }

            [JsonPropertyName("seed")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public long? Seed { get; set; }
        }

        [JsonPropertyName("artifacts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<StableDiffusionXLGeneratedImage>? Artifacts { get; set; }

        public string? GetResponse()
        {
            return Artifacts?.FirstOrDefault()?.Base64;
        }

        public string? GetStopReason()
        {
            return Artifacts?.FirstOrDefault()?.FinishReason;
        }
    }
}
