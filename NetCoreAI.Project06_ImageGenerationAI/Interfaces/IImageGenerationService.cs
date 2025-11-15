using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project06_ImageGenerationAI.Interfaces
{
    public interface IImageGenerationService
    {
        Task<string> GenerateImageAsync(string prompt);
    }
}
