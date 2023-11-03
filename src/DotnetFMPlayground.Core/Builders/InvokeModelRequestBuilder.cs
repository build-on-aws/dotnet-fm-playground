﻿using Amazon.BedrockRuntime.Model;
using DotnetFMPlayground.Core.Builders.Commands;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DotnetFMPlayground.Core.Builders
{
    internal class InvokeModelRequestBuilder
    {
        private static readonly Dictionary<string, InvokeModelRequestBuildCommand> invokeModelRequestBuilderRegistry = new();

        static InvokeModelRequestBuilder()
        {

            AnthropicClaudeRequestBuildCommand claudeRequestBuildCommand = new();
            invokeModelRequestBuilderRegistry.Add("anthropic.claude-instant-v1", claudeRequestBuildCommand);
            invokeModelRequestBuilderRegistry.Add("anthropic.claude-v1", claudeRequestBuildCommand);
            invokeModelRequestBuilderRegistry.Add("anthropic.claude-v2", claudeRequestBuildCommand);

            StabilityAIStableDiffusionRequestBuildCommand stableDiffusionRequestBuildCommand = new();
            invokeModelRequestBuilderRegistry.Add("stability.stable-diffusion-xl-v0", stableDiffusionRequestBuildCommand);
            
            AmazonTitanRequestBuildCommand amazonTitanRequestBuildCommand = new();
            invokeModelRequestBuilderRegistry.Add("amazon.titan-text-lite-v1", amazonTitanRequestBuildCommand);
            invokeModelRequestBuilderRegistry.Add("amazon.titan-text-express-v1", amazonTitanRequestBuildCommand);
            invokeModelRequestBuilderRegistry.Add("amazon.titan-text-agile-v1", amazonTitanRequestBuildCommand);
            invokeModelRequestBuilderRegistry.Add("amazon.titan-embed-text-v1", amazonTitanRequestBuildCommand);

            CohereCommandRequestBuildCommand cohereCommandRequestBuildCommand = new();
            invokeModelRequestBuilderRegistry.Add("cohere.command-text-v14", cohereCommandRequestBuildCommand);

            AI21LabsJurassic2RequestBuildCommand aI21LabsJurassic2RequestBuildCommand = new();
            invokeModelRequestBuilderRegistry.Add("ai21.j2-mid-v1", aI21LabsJurassic2RequestBuildCommand);
            invokeModelRequestBuilderRegistry.Add("ai21.j2-ultra-v1", aI21LabsJurassic2RequestBuildCommand);
        }

        internal static InvokeModelRequest Build(
            string modelId,
            Prompt prompt,
            BaseInferenceParameters inferenceParameters)
        {
            InvokeModelRequest request; 

            if (invokeModelRequestBuilderRegistry.ContainsKey(modelId))
            {
                request = invokeModelRequestBuilderRegistry[modelId].Execute(modelId, prompt, inferenceParameters);
            }
            else
            {
                throw new NotSupportedException($"ModelId {modelId} not supported");
            }
            return request;
        }

        internal static InvokeModelWithResponseStreamRequest BuildWithResponseStream(
            string modelId,
            Prompt prompt,
            BaseInferenceParameters inferenceParameters)
        {
            InvokeModelWithResponseStreamRequest request;

            if (invokeModelRequestBuilderRegistry.ContainsKey(modelId))
            {
                request = invokeModelRequestBuilderRegistry[modelId].ExecuteWithResponseStream(modelId, prompt, inferenceParameters);
            }
            else
            {
                throw new NotSupportedException($"ModelId {modelId} not supported");
            }
            return request;
        }


        protected virtual InvokeModelRequest Create(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            throw new NotSupportedException("Create Method from InvokeModelRequestBuilder class should not be called directly. Only inherited methods should be called");
        }
    }
}


