using Amazon.BedrockRuntime.Model;
using DotnetFMPlayground.Core.Models;
using DotnetFMPlayground.Core.Models.InferenceParameters;

namespace DotnetFMPlayground.Core.Builders.Commands
{
    internal abstract class InvokeModelRequestBuildCommand
    {
        internal abstract InvokeModelRequest Execute(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters);

        internal abstract InvokeModelWithResponseStreamRequest ExecuteWithResponseStream(string modelId, Prompt prompt, BaseInferenceParameters inferenceParameters);
    }
}
