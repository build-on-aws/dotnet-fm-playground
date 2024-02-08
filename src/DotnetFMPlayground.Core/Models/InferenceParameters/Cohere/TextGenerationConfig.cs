using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.Cohere;

public class TextGenerationConfig
{
    [Range(0f, 5f)]
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Temperature { get; init; }
    
    [Range(0f, 1f)]
    [JsonPropertyName("p")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? P { get; init; }
    
    [Range(0f, 500f)]
    [JsonPropertyName("k")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? K { get; init; }
    
    [Range(1, 4096)]
    [JsonPropertyName("max_tokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxTokens { get; init; }
    
    [JsonPropertyName("stop_sequences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? StopSequences { get; init; }
    
    [JsonPropertyName("return_likelihoods")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReturnLikelihoodsEnum? ReturnLikelihoods { get; init; }
    
    [JsonPropertyName("stream")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Stream { get; init; }
    
    [Range(1, 5)]
    [JsonPropertyName("num_generations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NumGenerations { get; init; }
    
    [JsonPropertyName("truncate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TruncateEnum? Truncate { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReturnLikelihoodsEnum
    {
        GENERATION,
        ALL,
        NONE
    }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TruncateEnum
    {
        NONE,
        START,
        END
    }
}