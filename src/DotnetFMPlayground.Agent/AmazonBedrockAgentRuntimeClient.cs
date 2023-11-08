using System.Net.Http.Json;

namespace DotnetFMPlayground.Agent
{
    public class AmazonBedrockAgentRuntimeClient
    {

        private HttpClient httpClient;

        public AmazonBedrockAgentRuntimeClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async string InvokeAgent(string agentId, string agentAliasId, string inputText)
        {
            JsonContent.Create();
            httpClient.PostAsync($"",)
        }

    }
}