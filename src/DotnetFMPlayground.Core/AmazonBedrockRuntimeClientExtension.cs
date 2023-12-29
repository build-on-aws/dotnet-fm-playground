using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Util;
using DotnetFMPlayground.Core.Builders;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using DotnetFMPlayground.Core.Models.ModelResponse;

namespace DotnetFMPlayground.Core
{
    public static class AmazonBedrockRuntimeClientExtension
    {
        public static async Task<IFoundationModelResponse?> InvokeModelAsync(this AmazonBedrockRuntimeClient client,
            string modelId,
            Prompt prompt,
            BaseInferenceParameters inferenceParameters,
            CancellationToken cancellationToken = default
            )
        {
            var invokeModelResponse = await client.InvokeModelAsync(
                InvokeModelRequestBuilder.Build(modelId, prompt, inferenceParameters),
                cancellationToken
            );

            return await FoundationModelResponseBuilder.Build(modelId, invokeModelResponse.Body);
        }

        public static async Task InvokeModelWithResponseStreamAsync(
            this AmazonBedrockRuntimeClient client,
            string modelId,
            Prompt prompt, 
            BaseInferenceParameters inferenceParameters,
            Func<string?, Task> chunckReceived,
            Func<string?, Task> exceptionReceived, 
            CancellationToken cancellationToken = default)
        {
            var response = await client.InvokeModelWithResponseStreamAsync(
                InvokeModelRequestBuilder.BuildWithResponseStream(modelId, prompt, inferenceParameters),
                cancellationToken
            );
            response.Body.ChunkReceived += async (sender, e) =>
            {
                var chunk = await FoundationModelResponseBuilder.Build(modelId, e.EventStreamEvent.Bytes, true);
                await chunckReceived(chunk?.GetResponse());
            };
            response.Body.ExceptionReceived += async (sender, e) =>
            {
                await exceptionReceived(e.EventStreamException.Message);
            };

            response.Body.StartProcessing();
        }
        
        /// <summary>
        /// Invoke Titan Image Generator G1 for text to image generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="textToImageParams">The text to image prompt definition</param>
        /// <param name="imageGenerationConfig">The image configuration definition. If null, default values will be used</param>
        /// <returns>The Titan Image Generator G1 response</returns>
        public static async Task<AmazonTitanImageGeneratorG1Response?> InvokeTitanImageGeneratorG1ForTextToImageAsync(
            this AmazonBedrockRuntimeClient client, Models.InferenceParameters.AmazonTitanImageGeneratorG1.TextToImageParams textToImageParams, Models.InferenceParameters.AmazonTitanImageGeneratorG1.ImageGenerationConfig? imageGenerationConfig = null)
        {
            ArgumentNullException.ThrowIfNull(textToImageParams);
            JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter<Models.InferenceParameters.AmazonTitanImageGeneratorG1.ImageGenerationConfig.ImageQuality>(JsonNamingPolicy.CamelCase)
                }
            };

            JsonObject payload = new JsonObject()
            {
                ["taskType"] = "TEXT_IMAGE",
                ["textToImageParams"] = JsonSerializer.SerializeToNode(textToImageParams, jsonSerializerOptions)
            };

            if (imageGenerationConfig is not null)
            {
                payload.Add("imageGenerationConfig", JsonSerializer.SerializeToNode(imageGenerationConfig, jsonSerializerOptions));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                 ModelId = ModelIds.AMAZON_TITAN_IMAGE_GENERATOR_v1,
                 ContentType = "application/json",
                 Accept = "application/json",
                 Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });
            
            return JsonSerializer.Deserialize<AmazonTitanImageGeneratorG1Response>(response.Body, new JsonSerializerOptions());
        }

        /// <summary>
        /// Invoke Stability AI Stable Diffusion XL v1 for text to image generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="textToImageParams">The text to image prompt definition</param>
        /// <returns>The Stability AI Stable Diffusion XL response</returns> 
        public static async Task<StableDiffusionXLResponse?> InvokeStabilityAIStableDiffusionXLv1ForTextToImageAsync(this AmazonBedrockRuntimeClient client, Models.InferenceParameters.StabilityAIStableDiffusionXL.TextToImageParams textToImageParams)
        {
            ArgumentNullException.ThrowIfNull(textToImageParams);
            Validator.ValidateObject(textToImageParams, new ValidationContext(textToImageParams), true);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = ModelIds.STABILITY_AI_STABLE_DIFFUSION_XL_V0,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(textToImageParams))
            });
            
            return JsonSerializer.Deserialize<StableDiffusionXLResponse>(response.Body, new JsonSerializerOptions());
        }
    }
    
    
        
}
