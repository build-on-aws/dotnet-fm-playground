using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class AnthropicClaudeResponse : IFoundationModelResponse
    {
        public string? Completion { get; set; }

        public string? Stop_Reason { get; set; }

        public string? GetResponse()
        {
            return Completion;
        }

        public string? GetStopReason()
        {
            return Stop_Reason;
        }
    }
}
