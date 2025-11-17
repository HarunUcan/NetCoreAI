using Google.Cloud.Vision.V1;

namespace NetCoreAI.Project08_GoogleCloudVisionOCR
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
                Console.WriteLine();

                //string credentialsPath = "C:\\Users\\harun\\Desktop\\Folders\\Visual Studio Projects\\NetCoreAI\\NetCoreAI.Project08_GoogleCloudVisionOCR\\SecretJson\\involuted-tuner-478422-h5-417f4a3bfb21.json";
                string credentialsPath = Path.Combine(AppContext.BaseDirectory, "Secrets", "involuted-tuner-478422-h5-417f4a3bfb21.json");
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

                try
                {
                    var client = ImageAnnotatorClient.Create();

                    var image = Image.FromFile(imagePath);
                    var response = client.DetectText(image);

                    Console.WriteLine("────────── OCR RESULT ──────────");
                    Console.WriteLine();

                    if(response != null && response.Count > 0)
                    {
                        // İlk eleman genellikle tüm metni içerir
                        var fullTextAnnotation = response[0];
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(fullTextAnnotation.Description);
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Resimden metin algılanamadı.");
                        Console.ResetColor();
                    }

                    //foreach (var annotation in response)
                    //{
                    //    if (!string.IsNullOrEmpty(annotation.Description))
                    //    {
                    //        Console.ForegroundColor = ConsoleColor.Cyan;
                    //        Console.WriteLine(annotation.Description);
                    //        Console.ResetColor();
                    //    }
                    //}
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
