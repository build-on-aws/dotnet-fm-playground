using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Builders
{
    internal static class FoundationModelResponseBuilder
    {
        internal static async Task<IFoundationModelResponse?> Build(string modelId, MemoryStream stream, bool streaming = false)
        {
            IFoundationModelResponse? response;
            switch(modelId)
            {
                case "anthropic.claude-instant-v1":
                case "anthropic.claude-v1":
                case "anthropic.claude-v2":
                    response = await JsonSerializer.DeserializeAsync<AnthropicClaudeResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    break;
                case "stability.stable-diffusion-xl-v0":
                    response = await JsonSerializer.DeserializeAsync<StableDiffusionXLResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    break;
                case "amazon.titan-text-lite-v1":
                case "amazon.titan-text-express-v1":
                case "amazon.titan-text-agile-v1":
                    if (streaming)
                    {
                        response = await JsonSerializer.DeserializeAsync<AmazonTitanTextStreamingResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        response = await JsonSerializer.DeserializeAsync<AmazonTitanTextResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    }
                    break;
                case "cohere.command-text-v14":
                    string temp = new StreamReader(stream).ReadToEnd();
                    response = null;
                    break;
                default:
                    throw new NotSupportedException($"ModelId {modelId} not supported");
            };
            return response;
        }
    }
}
