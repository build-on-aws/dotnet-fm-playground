using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.StabilityAIStableDiffusionXL;

public class TextToImageParams
{
    [JsonPropertyName("text_prompts")]
    public required IEnumerable<TextPrompt> TextPrompts { get; init;}
    
    [JsonPropertyName("height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [AllowedValues(null, 1024, 896, 832, 768, 640, 1536, 1344, 1216, 1152)]
    public int? Height { get; set; }
    
    [JsonPropertyName("width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [AllowedValues(null, 1024, 896, 832, 768, 640, 1536, 1344, 1216, 1152)]
    public int? Width { get; set; }
    
    [JsonPropertyName("sampler")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [AllowedValues(null, "DDIM", "DDPM", "K_DPMPP_2M", "K_DPMPP_2S_ANCESTRAL", "K_DPM_2", "K_DPM_2_ANCESTRAL", "K_EULER", "K_EULER_ANCESTRAL", "K_HEUN", "K_LMS")]
    public string? Sampler { get; set; }
    
    [JsonPropertyName("seed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(0, 4294967295)]
    public int? Seed { get; set; }
    
    [JsonPropertyName("steps")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(10, 50)]
    public int? Steps { get; set; }
    
    [JsonPropertyName("style_preset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [AllowedValues(null, "3d-model", "analog-film", "anime", "cinematic", "comic-book", "digital-art", "enhance", "fantasy-art", "isometric", "line-art", "low-poly", "modelin-compound", "neon-punk", "origami", "photographic", "pixel-art", "tile-texture")]
    public string? StylePreset { get; set; }
    
    public struct TextPrompt
    {
        [JsonPropertyName("text")]
        public required string Text { get; init; }
        
        [JsonPropertyName("weight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Weight { get; set; }
    }
}