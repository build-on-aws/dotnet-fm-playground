using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.AmazonTitanText;

public class TextGenerationConfig
{
    [Range(0, 1)]
    [DefaultValue(0)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(1)]
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }
    
    [Range(0, 1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DefaultValue(1)]
    [JsonPropertyOrder(2)]
    [JsonPropertyName("topP")]
    public float TopP { get; set; }
    
    [Range(0, 8000)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DefaultValue(512)]
    [JsonPropertyOrder(3)]
    [JsonPropertyName("maxTokenCount")]
    public int MaxTokenCount { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(5)]
    [JsonPropertyName("stopSequences")]
    public IEnumerable<string>? StopSequences { get; set; }
}

