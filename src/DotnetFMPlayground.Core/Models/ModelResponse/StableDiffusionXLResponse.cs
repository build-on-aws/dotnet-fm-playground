using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class StableDiffusionXLResponse : IFoundationModelResponse
    {
        public class StableDiffusionXLOutput
        {
            public string? Base64 { get; set; }

            public string? FinishReason { get; set; }

            public long? Seed { get; set; }
        }

        public IEnumerable<StableDiffusionXLOutput>? Artifacts { get; set; }

        public string? GetResponse()
        {
            return Artifacts?.FirstOrDefault()?.Base64;
        }

        public string? GetStopReason()
        {
            return Artifacts?.FirstOrDefault()?.FinishReason;
        }
    }
}
