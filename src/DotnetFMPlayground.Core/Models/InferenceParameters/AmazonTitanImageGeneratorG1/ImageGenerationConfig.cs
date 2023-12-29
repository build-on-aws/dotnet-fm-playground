using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotnetFMPlayground.Core.Models.InferenceParameters.AmazonTitanImageGeneratorG1
{
    public class ImageGenerationConfig
    {
        [Range(1, 5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(1)]
        public int NumberOfImages { get; set; }

        [JsonPropertyOrder(2)] public ImageQuality Quality { get; set; }

        [Range(1, 1024)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(4)]
        public int Height { get; set; }

        [Range(1, 1024)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(5)]
        public int Width { get; set; }

        [Range(1.0, 10.0)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(3)]
        public float CfgScale { get; set; }

        [Range(0, 214783647)]
        [JsonPropertyOrder(6)]
        public int Seed { get; set; }

        public enum ImageQuality
        {
            Standard,
            Premium
        }
    }
}