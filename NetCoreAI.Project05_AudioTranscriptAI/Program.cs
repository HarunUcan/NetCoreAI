using NetCoreAI.Project05_AudioTranscriptAI.Config;
using NetCoreAI.Project05_AudioTranscriptAI.Services;

namespace NetCoreAI.Project05_AudioTranscriptAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = AppConfig.Load();
            var selectedProvider = config["AIProvider"];

            IAudioTranscriptService audioTranscriptService = selectedProvider switch
            {
                "Gemini" => new GeminiAudioTranscriptionService(
                    apiKey: config["Gemini:ApiKey"],
                    model: config["Gemini:Model"]),

                _ => throw new NotSupportedException($"The AI provider '{selectedProvider}' is not supported.")
            };

            while (true)
            {
                Console.Write("Enter the path to the audio file (or 'exit' to quit): ");
                string inputPath = Console.ReadLine();

                if (inputPath.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                
                try
                {
                    string transcription = await audioTranscriptService.TranscribeAsync(inputPath);
                    Console.WriteLine("Transcribed Text:");
                    Console.WriteLine(transcription);
                    Console.WriteLine();
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
