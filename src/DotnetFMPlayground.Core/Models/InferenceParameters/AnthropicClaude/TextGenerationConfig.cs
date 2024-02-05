using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.AnthropicClaude;

public class TextGenerationConfig
{

    [Range(0f, 1f)]
    [DefaultValue(0.5f)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("temperature")]
    public float Temperature { get; init; } = 0.5f;

    [Range(0f, 1f)]
    [DefaultValue(1f)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("top_p")]
    public float TopP { get; init; } = 1f;

    [Range(0, 500)]
    [DefaultValue(250)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("top_k")] 
    public int TopK { get; init; } = 250;

    [Range(0, 4096)]
    [DefaultValue(200)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("max_tokens_to_sample")]
    private int MaxTokensToSample { get; init; } = 200;

    [JsonPropertyName("stop_sequences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IEnumerable<string>? StopSequences { get; init; }
}