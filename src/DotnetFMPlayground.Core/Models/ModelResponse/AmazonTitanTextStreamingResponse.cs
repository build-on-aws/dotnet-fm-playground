using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class AmazonTitanTextStreamingResponse : IFoundationModelResponse
    {
        public string? OutputText { get; set; }
        public int TotalOutputTextTokenCount { get; set; }
        public int Index { get; set; }
        public string? CompletionReason { get; set; }
        public int InputTextTokenCount { get; set; }

        public string? GetResponse()
        {
            return OutputText;
        }

        public string? GetStopReason()
        {
            return CompletionReason;
        }
    }
}
