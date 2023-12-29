using DotnetFMPlayground.Core.Builders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models
{
    public static class ModelIds
    {
        private static readonly List<string> modelIds = new()
        {
            "anthropic.claude-instant-v1",
            "anthropic.claude-v1",
            "anthropic.claude-v2",
            "stability.stable-diffusion-xl-v0",
            "amazon.titan-text-lite-v1",
            "amazon.titan-text-express-v1",
            "amazon.titan-text-agile-v1",
            "amazon.titan-image-generator-v1",
            "cohere.command-text-v14",
            "ai21.j2-mid-v1",
            "ai21.j2-ultra-v1"
        };

        private static readonly List<string> streamingSupported = new()
        {
            "anthropic.claude-instant-v1",
            "anthropic.claude-v1",
            "anthropic.claude-v2",
            "amazon.titan-text-lite-v1",
            "amazon.titan-text-express-v1",
            "amazon.titan-text-agile-v1"
        };

        public static string ANTHROPIC_CLAUDE_INSTANT_V1
        {
            get
            {
                return modelIds[0];
            }
        } 
        
        public static string ANTHROPIC_CLAUDE_V1
        {
            get
            {
                return modelIds[1];
            }
        } 
        
        public static string ANTHROPIC_CLAUDE_V2
        {
            get
            {
                return modelIds[2];
            }
        }

        public static string STABILITY_AI_STABLE_DIFFUSION_XL_V0
        {
            get
            {
                return modelIds[3];
            }
        }

        public static string AMAZON_TITAN_TEXT_LITE_V1
        {
            get
            {
                return modelIds[4];
            }
        }

        public static string AMAZON_TITAN_TEXT_EXPRESS_V1
        {
            get
            {
                return modelIds[5];
            }
        }

        public static string AMAZON_TITAN_TEXT_AGILE_V1
        {
            get
            {
                return modelIds[6];
            }
        }

        public static string COHERE_COMMAND_TEXT_V14
        {
            get
            {
                return modelIds[8];
            }
        }

        public static string AI21_LABS_JURASSIC_V2_MID_V1
        {
            get
            {
                return modelIds[9];
            }
        }

        public static string AI21_LABS_JURASSIC_V2_ULTRA_V1
        {
            get
            {
                return modelIds[10];
            }
        }
        
        public static string AMAZON_TITAN_IMAGE_GENERATOR_v1
        {
            get
            {
                return modelIds[7];
            }
        }

        public static bool IsSupported(string modelId)
        {
            return modelIds.Contains(modelId);
        }

        public static bool IsStreamingSupported(string modelId)
        {
            return streamingSupported.Contains(modelId);
        }


    }
}
