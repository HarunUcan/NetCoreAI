using System.Speech.Synthesis;

namespace NetCoreAI.Project10_TextToSpeech
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.Volume = 100;
            speechSynthesizer.Rate = 0;

            Console.WriteLine("Enter the text you want to convert to speech:");
            string text = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(text))
                speechSynthesizer.Speak(text);
        }
    }
}
