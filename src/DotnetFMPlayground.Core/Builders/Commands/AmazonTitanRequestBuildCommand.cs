using Amazon.BedrockRuntime.Model;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Builders.Commands
{
    internal class AmazonTitanRequestBuildCommand : InvokeModelRequestBuildCommand
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
                        inputText = new StringBuilder().AppendJoin(' ', prompt.Select(x => x.Prompt)).ToString()
                    })
                )
            );

            return request;
        }

        internal override InvokeModelWithResponseStreamRequest ExecuteWithResponseStream(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            if(modelId == "amazon.titan-embed-text-v1")
            {
                throw new NotSupportedException($"ModelId {modelId} doesn't support streaming");
            }

            InvokeModelWithResponseStreamRequest request = new()
            {
                ModelId = modelId,
                ContentType = "application/json"
            };

            request.Accept = "application/json";
            request.Body = new MemoryStream(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(new
                    {
                        inputText = new StringBuilder().AppendJoin(' ', prompt.Select(x => x.Prompt)).ToString()
                    })
                )
            );

            return request;
        }
    }
}
