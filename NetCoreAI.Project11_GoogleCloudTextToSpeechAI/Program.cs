using Google.Cloud.TextToSpeech.V1;

namespace NetCoreAI.Project11_GoogleCloudTextToSpeechAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Google Cloud Gemini TTS";

            string secretFile = @"C:\Users\harun\Desktop\Folders\Visual Studio Projects\NetCoreAI\NetCoreAI.Project11_GoogleCloudTextToSpeechAI\Secrets\involuted-tuner-478422-h5-417f4a3bfb21.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", secretFile);

            Console.WriteLine("────────── GOOGLE CLOUD TEXT-TO-SPEECH ──────────\n");

            var client = await TextToSpeechClient.CreateAsync();

            while (true)
            {
                Console.Write("Okunacak metin: ");
                string text = Console.ReadLine();

                Console.Write("\nTonlama için prompt (örn: curious, happy, dramatic): ");
                string prompt = Console.ReadLine();

                var input = new SynthesisInput
                {
                    Text = text,                    
                    Prompt = prompt
                };

                var voice = new VoiceSelectionParams
                {
                    LanguageCode = "tr-TR",
                    Name = "Algieba",                   // Seçilebilir ses medeli
                    ModelName = "gemini-2.5-flash-tts"
                };

                var config = new AudioConfig
                {
                    AudioEncoding = AudioEncoding.Linear16
                };

                var response = await client.SynthesizeSpeechAsync(input, voice, config);

                byte[] audioBytes = response.AudioContent.ToByteArray();
                string outputPath = $"output_{DateTime.Now.Ticks}.wav";

                await File.WriteAllBytesAsync(outputPath, audioBytes);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSes dosyası oluşturuldu : {outputPath}");
                Console.ResetColor();

                Console.WriteLine("\n-----------------------------------------\n");
            }
        }
    }
}
