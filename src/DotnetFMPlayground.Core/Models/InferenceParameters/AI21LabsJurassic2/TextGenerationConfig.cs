using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.AI21LabsJurassic2;

public class TextGenerationConfig
{
    [Range(0, 1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [Range(0, 1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("topP")]
    public float TopP { get; set; }

    [Range(0, 8191)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("maxTokens")]
    public int MaxTokens { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("stopSequences")]
    public IEnumerable<string>? StopSequences { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("countPenalty")]
    public CPenalty? CountPenalty { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("presencePenalty")]
    public PPenalty? PresencePenalty { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("frequencyPenalty")]
    public FPenalty? FrequencyPenalty { get; set; }

    public class Penalty
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("applyToWhiteSpaces")]
        public bool ApplyToWhitespaces { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("applyToPunctuations")]
        public bool ApplyToPunctuations { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("applyToNumbers")]
        public bool ApplyToNumbers { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("applyToStopWords")]
        public bool ApplyToStopWords { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("applyToEmojis")]
        public bool ApplyToEmojis { get; set; }
    }

    public class CPenalty : Penalty
    {
        [Range(0, 1)]
        [Required]
        [JsonPropertyName("scale")]
        [JsonRequired]
        public float Scale { get; set; }
    }

    public class PPenalty : Penalty
    {
        [Range(0, 5)]
        [Required]
        [JsonPropertyName("scale")]
        [JsonRequired]
        public float Scale { get; set; }
    }

    public class FPenalty : Penalty
    {
        [Range(0, 500)]
        [Required]
        [JsonPropertyName("scale")]
        [JsonRequired]
        public float Scale { get; set; }
    }
}