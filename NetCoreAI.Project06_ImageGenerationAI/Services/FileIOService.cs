using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project06_ImageGenerationAI.Services
{
    public static class FileIOService
    {
        public static async Task<string> SaveBase64Image(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(projectDir, "Images");

            Directory.CreateDirectory(folderPath);

            string fileName = $"GeneratedImage_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string fullPath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(fullPath, imageBytes);

            return fullPath;
        }

    }
}
