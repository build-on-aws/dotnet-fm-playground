using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models
{
    public record PromptItem(PromptItemType Type, string Prompt);
}
