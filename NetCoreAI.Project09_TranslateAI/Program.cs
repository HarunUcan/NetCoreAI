using NetCoreAI.Project09_TranslateAI.Config;
using NetCoreAI.Project09_TranslateAI.Interfaces;
using NetCoreAI.Project09_TranslateAI.Services.TranslateProviders;

namespace NetCoreAI.Project09_TranslateAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = AppConfig.Load();
            var selectedProvider = config["AIProvider"];

            ITranslateService translateService = selectedProvider switch
            {
                "Gemini" => new GeminiTranslateService(
                    apiKey: config["Gemini:ApiKey"],
                    model: config["Gemini:Model"]),

                "GoogleCloud" => new GoogleCloudTranslateService(
                    projectId: config["GoogleCloud:ProjectId"],
                    secretJsonFileName: config["GoogleCloud:SecretJsonFileName"]),

                "Ollama" => new OllamaTranslateService(
                    endpoint: config["Ollama:UrlEndpoint"],
                    model: config["Ollama:Model"]),

                _ => throw new NotSupportedException($"The AI provider '{selectedProvider}' is not supported.")
            };

            Console.WriteLine($"Selected provider is {translateService.ProviderName}\n");

            Console.WriteLine("────────── METİN ÇEVİRİ SERVİSİ ──────────");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Çevrilecek metni yazın: ");
                string text = Console.ReadLine();

                Console.Write("Hedef dil kodu (ör: tr, en, de): ");
                string targetLanguage = Console.ReadLine();

                var response = await translateService.TranslateAsync(text, targetLanguage);
                Console.WriteLine("\n────────── ÇEVİRİ SONUCU ──────────\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(response);
                Console.ResetColor();
                Console.WriteLine("\n-------------------------------------------\n");
            }
        }
    }
}
