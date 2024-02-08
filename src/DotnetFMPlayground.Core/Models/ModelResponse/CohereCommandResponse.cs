using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class CohereCommandResponse : IFoundationModelResponse
    { 
        [JsonPropertyName("generations")] public IEnumerable<Generation>? Generations { get; set; }

        [JsonPropertyName("id")] public string? Id { get; init; }

        [JsonPropertyName("prompt")] public string? Prompt { get; init; }
        
        public class Generation
        {
            [JsonPropertyName("finish_reason")] public FinishReason? FinishReason { get; init; }
            
            [JsonPropertyName("id")] public string? Id { get; init; }
            
            [JsonPropertyName("text")] public string? Text { get; init; }
            
            [JsonPropertyName("prompt")] public string? Prompt { get; init; }
            
            [JsonPropertyName("likelihood")] public float? Likelihood { get; init; }
            
            [JsonPropertyName("token_likelihoods")] public IEnumerable<JsonObject>? TokenLikelihoods { get; init; }
            
            [JsonPropertyName("is_finished")] public bool? IsFinished { get; init; }
            
            [JsonPropertyName("index")] public int? Index { get; init; }
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum FinishReason
        {
            COMPLETE,
            MAX_TOKENS,
            ERROR,
            ERROR_TOXIC
        }

        public string? GetResponse()
        {
            return Generations?.FirstOrDefault()?.Text;
        }

        public string? GetStopReason()
        {
            return Generations?.FirstOrDefault()?.FinishReason.ToString();
        }
    }
}
