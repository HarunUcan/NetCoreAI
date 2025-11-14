using NetCoreAI.Project04_ChatAI.Config;
using NetCoreAI.Project04_ChatAI.Interfaces;
using NetCoreAI.Project04_ChatAI.Services.ChatProviders;

namespace NetCoreAI.Project04_ChatAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = AppConfig.Load();
            var selectedProvider = config["AIProvider"];

            IChatService chatService = selectedProvider switch
            {
                "Gemini" => new GeminiChatService(
                    apiKey: config["Gemini:ApiKey"], 
                    model: config["Gemini:Model"]),

                "Ollama" => new OllamaChatService(
                    model: config["Ollama:Model"],
                    apiUrl: config["Ollama:UrlEndpoint"]),

                "OpenAI" => new OpenAIChatService(
                    apiKey: config["OpenAI:ApiKey"],
                    model: config["OpenAI:Model"],
                    urlEndpoint: config["OpenAI:UrlEndpoint"]),

                _ => throw new NotSupportedException($"The AI provider '{selectedProvider}' is not supported.")
            };
            Console.WriteLine($"Selected provider is {chatService.ProviderName}\n");

            while(true)
            {
                Console.Write("\nYou: ");
                var userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    break;
                }
                var response = await chatService.GenerateResponseAsync(userInput);
                Console.WriteLine($"\n{chatService.ProviderName}: {response}");
            }
        }
    }
}
