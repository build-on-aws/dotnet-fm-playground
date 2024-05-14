using Amazon.BedrockRuntime;
using DotnetFMPlayground.Core.Builders;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using DotnetFMPlayground.Core.Models.ModelResponse;

namespace DotnetFMPlayground.Core
{
    public static class AmazonBedrockRuntimeClientExtension
    {
        public static async Task<IFoundationModelResponse?> InvokeModelAsync(this AmazonBedrockRuntimeClient client, string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters, CancellationToken cancellationToken = default)
        {
            var invokeModelResponse = await client.InvokeModelAsync(InvokeModelRequestBuilder.Build(modelId, prompt, inferenceParameters), cancellationToken);
            return await FoundationModelResponseBuilder.Build(modelId, invokeModelResponse.Body);
        }

        public static async Task InvokeModelWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters, Func<string?, Task> chunkReceived, Func<string?, Task> exceptionReceived, CancellationToken cancellationToken = default)
        {
            var response = await client.InvokeModelWithResponseStreamAsync(InvokeModelRequestBuilder.BuildWithResponseStream(modelId, prompt, inferenceParameters), cancellationToken);
            
            response.Body.ChunkReceived += async (sender, e) =>
            {
                var chunk = await FoundationModelResponseBuilder.Build(modelId, e.EventStreamEvent.Bytes, true);
                await chunkReceived(chunk?.GetResponse());
            };
            
            response.Body.ExceptionReceived += async (sender, e) =>
            {
                await exceptionReceived(e.EventStreamException.Message);
            };

            response.Body.StartProcessing();
        }
    }
}
