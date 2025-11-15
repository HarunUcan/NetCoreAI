using NetCoreAI.Project06_ImageGenerationAI.Config;
using NetCoreAI.Project06_ImageGenerationAI.Interfaces;
using NetCoreAI.Project06_ImageGenerationAI.Services;

namespace NetCoreAI.Project06_ImageGenerationAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = AppConfig.Load();
            var selectedProvider = config["AIProvider"];

            IImageGenerationService imageGenerationService = selectedProvider switch
            {
                "HuggingFace" => new HuggingFaceImageGenerationService(
                    apiUrl: config["HuggingFace:UrlEndpoint"],
                    apiKey: config["HuggingFace:ApiKey"],
                    model: config["HuggingFace:Model"]),                    

                _ => throw new NotSupportedException($"The AI provider '{selectedProvider}' is not supported.")
            };

            while (true)
            {
                Console.Write("Enter a prompt for image generation (or type 'exit' to quit): ");
                var prompt = Console.ReadLine();

                if (string.Equals(prompt, "exit", StringComparison.OrdinalIgnoreCase))
                    break;

                try
                {
                    var base64 = await imageGenerationService.GenerateImageAsync(prompt);
                    var savedImagePath = await FileIOService.SaveBase64Image(base64);

                    // Üstten boşluk
                    Console.WriteLine();
                    Console.WriteLine();

                    // Renkli path yazımı
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Your image successfully generated!");
                    Console.WriteLine($"Saved Path: {savedImagePath}");

                    // Renk sıfırlama
                    Console.ResetColor();

                    Console.WriteLine(); // alttan boşluk
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error generating image: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}
