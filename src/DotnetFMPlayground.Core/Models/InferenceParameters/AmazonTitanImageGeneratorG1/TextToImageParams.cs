using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.AmazonTitanImageGeneratorG1;

public class TextToImageParams
{
    public required string Text { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NegativeText { get; set; }
    
}