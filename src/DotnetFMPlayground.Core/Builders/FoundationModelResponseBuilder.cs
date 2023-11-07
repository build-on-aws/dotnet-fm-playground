using Amazon.Util.Internal.PlatformServices;
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
            switch (modelId)
            {
                case var _ when modelId == ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1:
                case var _ when modelId == ModelIds.ANTHROPIC_CLAUDE_V1:
                case var _ when modelId == ModelIds.ANTHROPIC_CLAUDE_V2:
                    response = await JsonSerializer.DeserializeAsync<AnthropicClaudeResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    break;
                case var _ when modelId == ModelIds.STABILITY_AI_STABLE_DIFFUSION_XL_V0:
                    response = await JsonSerializer.DeserializeAsync<StableDiffusionXLResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    break;
                case var _ when modelId == ModelIds.AMAZON_TITAN_TEXT_LITE_V1:
                case var _ when modelId == ModelIds.AMAZON_TITAN_TEXT_EXPRESS_V1:
                case var _ when modelId == ModelIds.AMAZON_TITAN_TEXT_AGILE_V1:
                    if (streaming)
                    {
                        response = await JsonSerializer.DeserializeAsync<AmazonTitanTextStreamingResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        response = await JsonSerializer.DeserializeAsync<AmazonTitanTextResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    }
                    break;
                case var _ when modelId == ModelIds.COHERE_COMMAND_TEXT_V14:
                    response = await JsonSerializer.DeserializeAsync<CohereCommandResponse>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    break;
                case var _ when modelId == ModelIds.AI21_LABS_JURASSIC_V2_MID_V1:
                case var _ when modelId == ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1:
                    response = await JsonSerializer.DeserializeAsync<AI21LabsJurassic2Response>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    break;
                default:
                    throw new NotSupportedException($"ModelId {modelId} not supported");
            };
            return response;
        }
    }
}
