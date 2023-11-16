using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.EventStreams;
using Amazon.Runtime.EventStreams.Internal;
using AwsSignatureVersion4;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DotnetFMPlayground.Agent
{
    public class AmazonBedrockAgentRuntimeClient
    {
        public class ChunkMessage
        {
            public string? Bytes { get; init; }
        }

        private HttpClient httpClient;

        private bool isProcessing = false;

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

        public async IAsyncEnumerable<string> InvokeAgent(string agentId, string agentAliasId, string sessionId, string inputText, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var content = JsonContent.Create( new { inputText });
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/agents/{agentId}/agentAliases/{agentAliasId}/sessions/{sessionId}/text")
            {
                Content = content
            };
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(continueOnCapturedContext: false); 
           
            response.EnsureSuccessStatusCode();

            var streamCancellationTokenSource = new CancellationTokenSource();
            var streamTaskCompletionSource = new TaskCompletionSource<string>();

            EventStreamDecoder decoder = new EventStreamDecoder();
            decoder.MessageReceived += HandleMessageReceived;

            Stream responseStream = await response.Content.ReadAsStreamAsync();

            isProcessing = true;
            _ = Task.Run(() => Process(decoder, responseStream));

            while (!streamCancellationTokenSource.IsCancellationRequested && isProcessing)
            {
                yield return await streamTaskCompletionSource.Task.WaitAsync(cancellationToken).ConfigureAwait(false);
                streamTaskCompletionSource = new TaskCompletionSource<string>();
            }

            async void  HandleMessageReceived(object? sender, EventStreamMessageReceivedEventArgs e)
            {
                if (e.Message.Headers[":event-type"].AsString() != "end")
                {
                    var chunk = JsonSerializer.Deserialize<ChunkMessage>(new MemoryStream(e.Message.Payload), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    string read = "";
                    if (chunk!.Bytes is not null)
                    {
                        var internalObject = Convert.FromBase64String(chunk.Bytes!);
                        var reader = new StreamReader(new MemoryStream(internalObject));
                        read = await reader.ReadToEndAsync();

                    }
                    streamTaskCompletionSource.SetResult(read);
                }
                else
                {
                    decoder.MessageReceived -= HandleMessageReceived;
                    streamCancellationTokenSource.Cancel();
                }
            }

            decoder.MessageReceived -= HandleMessageReceived;

        }
        private void Process(EventStreamDecoder decoder, Stream stream)
        {
            byte[] buffer = new byte[8192];
            isProcessing = true;
            while (isProcessing)
            {
                ReadFromStream(decoder, stream, buffer);
            }
        }

        private void ReadFromStream(EventStreamDecoder decoder, Stream stream, byte[] buffer)
        {
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                // Decoder raises MessageReceived for every message it encounters.
                decoder.ProcessData(buffer, 0, bytesRead);
            }
            else
            {
                isProcessing = false;
            }
        }

    }
}