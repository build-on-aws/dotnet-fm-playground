using System.Text.Json.Serialization;
using Amazon.Auth.AccessControlPolicy;

namespace DotnetFMPlayground.Core.Models.ModelResponse;

public class AmazonTitanImageGeneratorG1Response
{
    [JsonPropertyName("images")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? Images { get; set; }
    
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; set; }
}