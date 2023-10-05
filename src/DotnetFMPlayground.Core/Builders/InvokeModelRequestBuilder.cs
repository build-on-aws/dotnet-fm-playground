using Amazon.BedrockRuntime.Model;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DotnetFMPlayground.Core.Builders
{
    internal static class InvokeModelRequestBuilder
    {
        internal static InvokeModelRequest Build(
            string modelId,
            Prompt prompt,
            BaseInferenceParameters inferenceParameters)
        {
            InvokeModelRequest request = new()
            {
                ModelId = modelId,
                ContentType = "application/json"
            };
            
            switch (modelId)
            {
                case "stability.stable-diffusion-xl":
                case "stability.stable-diffusion-xl-v0":
                    request.Accept = "application/json";
                    request.Body = new MemoryStream(
                        Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(new
                            {
                                text_prompts = prompt.Select(x => new { text = x.Prompt }).ToArray()
                            })
                        )
                    );
                    break;
                case "anthropic.claude-instant-v1":
                case "anthropic.claude-v1":
                case "anthropic.claude-v2":
                    StringBuilder promtValueBuilder = new();
                    PromptItemType? lastPromptType = null;
                    foreach (var item in prompt)
                    {
                        lastPromptType = item.Type;
                        string label = item.Type == PromptItemType.User ? "Human" : "Assistant";
                        promtValueBuilder.Append(label);
                        promtValueBuilder.Append(": ");
                        promtValueBuilder.Append(item.Prompt);
                        promtValueBuilder.AppendLine();
                    }
                    if (lastPromptType != PromptItemType.FMAnswer)
                    {
                        if (!promtValueBuilder.ToString().EndsWith('.'))
                            promtValueBuilder.Append('.');
                        promtValueBuilder.AppendLine();
                        promtValueBuilder.AppendLine("Assistant: ");
                    }

                    request.Accept = "application/json";
                    request.Body = new MemoryStream(
                        Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(new
                            {
                                prompt = promtValueBuilder.ToString(),
                                max_tokens_to_sample = inferenceParameters["max_tokens_to_sample"]
                            })
                        )
                    );
                    break;
                default:
                    throw new NotSupportedException($"ModelId {modelId} not supported");
            }
            return request;
        }

        internal static InvokeModelWithResponseStreamRequest BuildWithResponseStream(
            string modelId,
            Prompt prompt,
            BaseInferenceParameters inferenceParameters)
        {
            InvokeModelWithResponseStreamRequest request = new()
            {
                ModelId = modelId,
                ContentType = "application/json"
            };

            switch (modelId)
            {

                case "anthropic.claude-instant-v1":
                case "anthropic.claude-v1":
                case "anthropic.claude-v2":
                    StringBuilder promtValueBuilder = new();
                    PromptItemType? lastPromptType = null;
                    foreach (var item in prompt)
                    {
                        lastPromptType = item.Type;
                        string label = item.Type == PromptItemType.User ? "Human" : "Assistant";
                        promtValueBuilder.Append(label);
                        promtValueBuilder.Append(": ");
                        promtValueBuilder.Append(item.Prompt);
                        promtValueBuilder.AppendLine();
                    }
                    if (lastPromptType != PromptItemType.FMAnswer)
                    {
                        if (!promtValueBuilder.ToString().EndsWith('.'))
                            promtValueBuilder.Append('.');
                        promtValueBuilder.AppendLine();
                        promtValueBuilder.AppendLine("Assistant: ");
                    }

                    request.Accept = "application/json";
                    request.Body = new MemoryStream(
                        Encoding.UTF8.GetBytes(
                            JsonSerializer.Serialize(new
                            {
                                prompt = promtValueBuilder.ToString(),
                                max_tokens_to_sample = inferenceParameters["max_tokens_to_sample"]
                            })
                        )
                    );
                    break;
                default:
                    throw new NotSupportedException($"ModelId {modelId} not supported");
            }
            return request;
        }
    }
}


