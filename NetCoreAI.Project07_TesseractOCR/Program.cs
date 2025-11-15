using Tesseract;

namespace NetCoreAI.Project07_TesseractOCR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Okuma yapılacak resmi buraya sürükleyip bırakın: ");
                string imagePath = Console.ReadLine();
                imagePath = imagePath.Trim('"');

                if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Geçersiz dosya yolu.");
                    Console.ResetColor();
                    continue;
                }

                string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

                try
                {
                    using (var engine = new TesseractEngine(tessDataPath, "eng+tur", EngineMode.Default))
                    using (var img = Pix.LoadFromFile(imagePath))
                    using (var page = engine.Process(img))
                    {
                        string text = page.GetText();

                        Console.WriteLine();
                        Console.WriteLine("────────── OCR RESULT ──────────");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(text);
                        Console.ResetColor();

                        Console.WriteLine("────────────────────────────────");
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hata oluştu: " + ex.Message);
                    Console.ResetColor();
                }
            }

        }
    }
}
