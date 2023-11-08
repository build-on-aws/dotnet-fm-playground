using Amazon;
using Amazon.Bedrock;
using Amazon.BedrockRuntime;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Util;
using AwsSignatureVersion4;
using DotnetFMPlayground.Agent;
using DotnetFMPlayground.Core;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.Net.Http;

namespace DotnetFMPlayground.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddSpeechRecognitionServices();
            builder.Services.AddSpeechSynthesisServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AmazonBedrockRuntimeClient>(
                new AmazonBedrockRuntimeClient(new AmazonBedrockRuntimeConfig()
                {
                    RegionEndpoint = RegionEndpoint.USEast1
                }));
            builder.Services.AddSingleton<AmazonBedrockClient>(
                new AmazonBedrockClient(new AmazonBedrockConfig()
                {
                    RegionEndpoint = RegionEndpoint.USEast1
                }));

            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials("default", out awsCredentials))
            {
                var signatureHandler = new AwsSignatureHandler(new AwsSignatureHandlerSettings("us-east-1", "bedrock-agent-runtime", awsCredentials))
                {
                    InnerHandler = new HttpClientHandler()
                };

                var httpClient = new HttpClient(signatureHandler)
                {
                    BaseAddress = new Uri("https://bedrock-agent-runtime.us-east-1.amazonaws.com")
                };

                builder.Services.AddSingleton<AmazonBedrockAgentRuntimeClient>(
                    new AmazonBedrockAgentRuntimeClient(httpClient)
                    );
                    
            }
            return builder.Build();
        }
    }
}