using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.Internal;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using System.Text;
using System.Text.Json;

namespace DotnetFMPlayground.Core.Builders.Commands
{
    internal class ClaudeRequestBuildCommand : InvokeModelRequestBuildCommand
    {
        internal override InvokeModelRequest Execute(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            InvokeModelRequest request = new()
            {
                ModelId = modelId,
                ContentType = "application/json"
            };

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

            return request;
        }

        internal override InvokeModelWithResponseStreamRequest ExecuteWithResponseStream(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            InvokeModelWithResponseStreamRequest request = new()
            {
                ModelId = modelId,
                ContentType = "application/json"
            };

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

            return request;
        }
    }
}
