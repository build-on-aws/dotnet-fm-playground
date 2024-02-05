using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class AmazonTitanTextResponse : IFoundationModelResponse
    {
        public class AmazonTitanTextOutput
        {
            [JsonPropertyName("tokenCount")]
            public int TokenCount { get; set; }

            [JsonPropertyName("outputText")]
            public string? OutputText { get; set; }

            [JsonPropertyName("completionReason")]
            public string? CompletionReason { get; set; }
        }
        
        [JsonPropertyName("inputTextTokenCount")]
        public int InputTextTokenCount { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<AmazonTitanTextOutput>? Results { get; set; }

        public string? GetResponse()
        {
            return Results?.FirstOrDefault()?.OutputText;
        }

        public string? GetStopReason()
        {
            return Results?.FirstOrDefault()?.CompletionReason;
        }
    }
}
