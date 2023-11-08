using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using AwsSignatureVersion4;
using Amazon.Runtime.Internal.Util;

namespace DotnetFMPlayground.Agent
{
    public class AmazonBedrockAgentRuntimeClient
    {
        public class ChunkMessage
        {
            public string? Bytes { get; init; }
        }

        private HttpClient httpClient;

        public AmazonBedrockAgentRuntimeClient()
        {
            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials("default", out awsCredentials))
            {
                var signatureHandler = new AwsSignatureHandler(new AwsSignatureHandlerSettings("us-east-1", "bedrock", awsCredentials))
                {
                    InnerHandler = new HttpClientHandler()
                };

                var httpClient = new HttpClient(signatureHandler)
                {
                    BaseAddress = new Uri("https://bedrock-agent-runtime.us-east-1.amazonaws.com")
                };
                this.httpClient = httpClient;
            }
            else
            {
                throw new Exception("missing aws credentials");
            }
        }

        public async Task<string> InvokeAgent(string agentId, string agentAliasId, string inputText)
        {
            Guid sessionId = new Guid();
            var content = JsonContent.Create( new { inputText });
            var response = await httpClient.PostAsync($"/agents/{agentId}/agentAliases/{agentAliasId}/sessions/{sessionId.ToString()}/text", content);

            response.EnsureSuccessStatusCode();


            var buffer = await response.Content.ReadAsByteArrayAsync();
            var message = Amazon.Runtime.EventStreams.EventStreamMessage.FromBuffer(buffer, 0, buffer.Length);

            var chunk = JsonSerializer.Deserialize<ChunkMessage>(new MemoryStream(message.Payload), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (chunk.Bytes is not null)
            {
                var internalObject = Convert.FromBase64String(chunk.Bytes!);
                var reader = new StreamReader(new MemoryStream(internalObject));
                var read = await reader.ReadToEndAsync();
                return read;
            }
            else
            {
                return "Failure";
            }
        }

    }
}