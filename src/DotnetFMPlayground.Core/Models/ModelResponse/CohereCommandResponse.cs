using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class CohereCommandResponse : IFoundationModelResponse
    {
        public class CohereCommandGeneration
        {
            public int TokenCount { get; init; }

            public string? Text { get; init; }

            public string? Id { get; init; }

            public string? Finish_Reason { get; init; }
        }

        public IEnumerable<CohereCommandGeneration>? generations { get; set; }

        public string? Id { get; init; }

        public string? Prompt { get; init; }

        public string? GetResponse()
        {
            return generations?.FirstOrDefault()?.Text;
        }

        public string? GetStopReason()
        {
            return generations?.FirstOrDefault()?.Finish_Reason;
        }
    }
}
