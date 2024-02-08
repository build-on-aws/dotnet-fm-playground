using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Util;
using DotnetFMPlayground.Core.Builders;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using DotnetFMPlayground.Core.Models.InferenceParameters.AnthropicClaude;
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
        
        /// <summary>
        /// Invoke Jurassic 2 model
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The Jurassic 2 model Id</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Jurassic 2 model response</returns>
        private static async Task<AI21LabsJurassic2Response?> InvokeJurassic2Async(this AmazonBedrockRuntimeClient client, string modelId, string prompt, Models.InferenceParameters.AI21LabsJurassic2.TextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.AI21_LABS_JURASSIC_V2_MID_V1 && modelId != ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.AI21_LABS_JURASSIC_V2_MID_V1)} or {nameof(ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1)}");
            }
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload ??= new();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<AI21LabsJurassic2Response>(response.Body);
        }
        
        /// <summary>
        /// Invoke Jurassic 2 Mid for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Jurassic 2 Mid model response</returns>
        public static async Task<AI21LabsJurassic2Response?> InvokeJurassic2MidAsync(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.AI21LabsJurassic2.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeJurassic2Async(ModelIds.AI21_LABS_JURASSIC_V2_MID_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Jurassic 2 Ultra for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Jurassic 2 Mid model response</returns>
        public static async Task<AI21LabsJurassic2Response?> InvokeJurassic2UltraAsync(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.AI21LabsJurassic2.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeJurassic2Async(ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1, prompt, textGenerationConfig);
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
                 ModelId = ModelIds.AMAZON_TITAN_IMAGE_GENERATOR_G1_V1,
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

        /// <summary>
        /// Invoke Titan Text G1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Titan Text G1 model</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Titan Text G1 model response</returns>
        private static async Task<AmazonTitanTextResponse?> InvokeTitanTextG1Async(this AmazonBedrockRuntimeClient client, string modelId, string inputText, Models.InferenceParameters.AmazonTitanText.TextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1 && modelId != ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1)} or {nameof(ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1)}");
            }
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload.Add("imageGenerationConfig", JsonSerializer.SerializeToNode(textGenerationConfig));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
            });
            
            return JsonSerializer.Deserialize<AmazonTitanTextResponse>(response.Body);   
        }
        
        /// <summary>
        /// Invoke Titan Text G1 Express model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Titan Text G1 Express model response</returns>
        public static async Task<AmazonTitanTextResponse?> InvokeTitanTextG1Express(this AmazonBedrockRuntimeClient client, string inputText, Models.InferenceParameters.AmazonTitanText.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeTitanTextG1Async(ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1, inputText, textGenerationConfig);
        }

        /// <summary>
        /// Invoke Titan Text G1 Lite model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Titan Text G1 Lite model response</returns>
        public static async Task<AmazonTitanTextResponse?> InvokeTitanTextG1Lite(this AmazonBedrockRuntimeClient client, string inputText, Models.InferenceParameters.AmazonTitanText.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeTitanTextG1Async(ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1, inputText, textGenerationConfig);
        }

        public static async Task<AmazonTitanEmbeddingsResponse?> InvokeTitanEmbeddingsG1Text(
            this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }

        public static async Task<AmazonTitanMultimodalEmbeddingsResponse?> InvokeTitanMultimodalEmbeddingsG1(
            this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invoke Claude model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Claude model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        private static async Task<AnthropicClaudeResponse?> InvokeClaudeAsync(this AmazonBedrockRuntimeClient client, string modelId, string prompt, Models.InferenceParameters.AnthropicClaude.TextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.ANTHROPIC_CLAUDE_V1 && modelId != ModelIds.ANTHROPIC_CLAUDE_V2 && modelId != ModelIds.ANTHROPIC_CLAUDE_V2_1 && modelId != ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1 )
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.ANTHROPIC_CLAUDE_V1)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_V2)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_V2_1)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1)}");
            }
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
            }
            else
            {
                textGenerationConfig = new TextGenerationConfig();
            }

            JsonObject payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject() ?? new ();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<AnthropicClaudeResponse>(response.Body);
        }

        /// <summary>
        /// Invoke Claude V1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<AnthropicClaudeResponse?> InvokeClaudeV1Async(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.AnthropicClaude.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Claude V2 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<AnthropicClaudeResponse?> InvokeClaudeV2Async(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.AnthropicClaude.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_V2, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Claude V2.1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<AnthropicClaudeResponse?> InvokeClaudeV2_1Async(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.AnthropicClaude.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_V2_1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Claude Instant V1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<AnthropicClaudeResponse?> InvokeClaudeInstantV1Async(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.AnthropicClaude.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Command v14 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Command model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Command model response</returns>
        private static async Task<CohereCommandResponse?> InvokeCommandV14(this AmazonBedrockRuntimeClient client, string modelId, string prompt, Models.InferenceParameters.Cohere.TextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.COHERE_COMMAND_TEXT_V14 && modelId != ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.COHERE_COMMAND_TEXT_V14)} or {nameof(ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14)}");
            }
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload??= new JsonObject();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<CohereCommandResponse>(response.Body);
        }
        
        /// <summary>
        /// Invoke Command v14 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Command model response</returns>
        public static async Task<CohereCommandResponse?> InvokeCommandV14(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.Cohere.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeCommandV14(ModelIds.COHERE_COMMAND_TEXT_V14, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Command Light v14 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Command model response</returns>
        public static async Task<CohereCommandResponse?> InvokeCommandLightV14(this AmazonBedrockRuntimeClient client, string prompt, Models.InferenceParameters.Cohere.TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeCommandV14(ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14, prompt, textGenerationConfig);
        }
        
        public static async Task<CohereEmbedEnglishResponse?> InvokeEmbedEnglishV3(this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }
        
        public static async Task<CohereEmbedMultilingualResponse?> InvokeEmbedMultilingualV3(this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }

        public static async Task<MetaLlama2ChatResponse?> InvokeLlama2Chat13Bv1(this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }
        
        public static async Task<MetaLlama2ChatResponse?> InvokeLlama2Chat70Bv1(this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }
        
        public static async Task<MetaLlama2Response?> InvokeLlama213Bv1(this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }
        
        public static async Task<MetaLlama2Response?> InvokeLlama270Bv1(this AmazonBedrockRuntimeClient client)
        {
            throw new NotImplementedException();
        }
    }
}
