using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class AI21LabsJurassic2Response : IFoundationModelResponse
    {
        [JsonPropertyName("id")] public int? Id { get; init; }

        [JsonPropertyName("prompt")] public J2Text? Prompt { get; init; }

        [JsonPropertyName("completions")]
        public IEnumerable<J2Completion>? Completions { get; init; }

        public class J2Text
        {
            [JsonPropertyName("text")] public string? Text { get; init; }

            [JsonPropertyName("tokens")] public IEnumerable<J2Token>? Tokens { get; init; }
        }

        public class J2Token
        {
            [JsonPropertyName("generatedToken")] public J2GeneratedToken? GeneratedToken { get; init; }

            [JsonPropertyName("topTokens")] public string? TopTokens { get; init; }

            [JsonPropertyName("textRange")] public J2TextRange? TextRange { get; init; }
        }

        public class J2GeneratedToken
        {
            [JsonPropertyName("token")] public string? Token { get; init; }

            [JsonPropertyName("logprob")] public double? LogProb { get; init; }

            [JsonPropertyName("raw_logprob")] public double? RawLogProb { get; init; }
        }

        public class J2TextRange
        {
            [JsonPropertyName("start")] public int? Start { get; init; }

            [JsonPropertyName("end")] public int? End { get; init; }
        }

        public class J2Completion
        {
            [JsonPropertyName("data")] public J2Text? Data { get; init; }

            [JsonPropertyName("finishReason")] public J2FinishReason? FinishReason { get; init; }
        }

        public class J2FinishReason
        {
            [JsonPropertyName("reason")] public string? Reason { get; init; }

            [JsonPropertyName("length")] public int? Length { get; init; }
        }


        public string? GetResponse()
        {
            return Completions?.FirstOrDefault()?.Data?.Text;
        }

        public string? GetStopReason()
        {
            return Completions?.FirstOrDefault()?.FinishReason?.Reason;
        }
    }
}