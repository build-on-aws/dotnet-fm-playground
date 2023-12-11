using Amazon;
using Amazon.Bedrock;
using Amazon.BedrockRuntime;
using Amazon.BedrockAgent;
using Amazon.BedrockAgentRuntime;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

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

            builder.Services.AddSingleton<AmazonBedrockAgentRuntimeClient>(
                new AmazonBedrockAgentRuntimeClient()
                );
            
            builder.Services.AddSingleton<AmazonBedrockAgentClient>();

            return builder.Build();
        }
    }
}