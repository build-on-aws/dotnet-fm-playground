using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class AmazonTitanTextResponse : IFoundationModelResponse
    {
        public class AmazonTitanTextOutput
        {
            public int TokenCount { get; set; }

            public string? OutputText { get; set; }

            public string? CompletionReason { get; set; }
        }
        public int InputTextTokenCount { get; set; }

        public IEnumerable<AmazonTitanTextOutput>? results { get; set; }

        public string? GetResponse()
        {
            return results?.FirstOrDefault()?.OutputText;
        }

        public string? GetStopReason()
        {
            return results?.FirstOrDefault()?.CompletionReason;
        }
    }
}
