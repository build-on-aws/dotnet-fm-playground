using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.Internal;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Builders.Commands
{
    internal class StableDiffusionRequestBuildCommand : InvokeModelRequestBuildCommand
    {
        internal override InvokeModelRequest Execute(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            InvokeModelRequest request = new()
            {
                ModelId = modelId,
                ContentType = "application/json"
            };

            request.Accept = "application/json";
            request.Body = new MemoryStream(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(new
                    {
                        text_prompts = prompt.Select(x => new { text = x.Prompt }).ToArray()
                    })
                )
            );

            return request;
        }

        internal override InvokeModelWithResponseStreamRequest ExecuteWithResponseStream(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            throw new NotSupportedException($"ModelId {modelId} doesn't support streaming");
        }
    }
}
