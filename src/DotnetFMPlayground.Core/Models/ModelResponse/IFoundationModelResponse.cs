using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public interface IFoundationModelResponse
    {
        public string? GetResponse();

        public string? GetStopReason();
    }
}
