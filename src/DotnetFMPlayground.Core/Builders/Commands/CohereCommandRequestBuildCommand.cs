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
    internal class CohereCommandRequestBuildCommand : InvokeModelRequestBuildCommand
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
                        prompt = new StringBuilder().AppendJoin(' ', prompt.Select(x => x.Prompt)).ToString(),
                    })
                )
            );

            return request;
        }

        internal override InvokeModelWithResponseStreamRequest ExecuteWithResponseStream(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters)
        {
            throw new NotImplementedException();
        }
    }
}
