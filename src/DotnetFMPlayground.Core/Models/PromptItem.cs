using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models
{
    public class PromptItem
    {
        public PromptItemType Type { get; }
        
        public string Prompt { get; set; }

        public PromptItem(PromptItemType type, string prompt)
        {
            Type = type;
            Prompt = prompt;
        }
    }
}
