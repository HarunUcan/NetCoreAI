using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project05_AudioTranscriptAI
{
    public interface IAudioTranscriptService
    {
        Task<string> TranscribeAsync(string audioFilePath);
    }
}
