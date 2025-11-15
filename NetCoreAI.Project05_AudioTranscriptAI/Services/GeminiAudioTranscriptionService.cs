using Google.GenAI;
using Google.GenAI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project05_AudioTranscriptAI.Services
{
    public class GeminiAudioTranscriptionService : IAudioTranscriptService
    {
        private readonly Client _client;
        private readonly string _model;

        public GeminiAudioTranscriptionService(string apiKey, string model)
        {
            _client = new Client(apiKey: apiKey);
            _model = model;
        }

        public async Task<string> TranscribeAsync(string audioFilePath)
        {
            audioFilePath = CleanPath(audioFilePath);

            if (!File.Exists(audioFilePath))
                return "Ses dosyası bulunamadı.";

            var audioBytes = await File.ReadAllBytesAsync(audioFilePath);

            var parts = new List<Part>
            {
                new Part { Text = "Bu sesi metne çevir." },
                new Part
                {
                    InlineData = new Blob
                    {
                        MimeType = GetMimeType(audioFilePath),
                        Data = audioBytes
                    }
                }
            };

            var content = new Content
            {
                Parts = parts
            };

            var response = await _client.Models.GenerateContentAsync(
                model: _model,
                contents: new List<Content> { content }
            );

            return response.Candidates[0].Content.Parts[0].Text;
        }

        private string GetMimeType(string file)
        {
            string ext = Path.GetExtension(file).ToLower();

            return ext switch
            {
                ".wav" => "audio/wav",
                ".mp3" => "audio/mpeg",
                ".m4a" => "audio/mp4",
                ".aac" => "audio/aac",
                ".ogg" => "audio/ogg",
                ".webm" => "audio/webm",
                _ => "audio/wav"
            };
        }

        private string CleanPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            // Trim surrounding quotes
            path = path.Trim('"');

            // Trim normal whitespace
            path = path.Trim();

            // Trim BOM (UTF-8 Byte Order Mark)
            path = path.Trim('\uFEFF');

            // Trim zero-width spaces, NBSP, control chars, '?', and all non-printable chars
            path = new string(path.Where(ch => !char.IsControl(ch) && ch != '?' && ch != '\u200B' && ch != '\u00A0').ToArray());

            return path;
        }
    }
}
