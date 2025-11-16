using Google.GenAI;
using Google.GenAI.Types;
using NetCoreAI.Project04_ChatAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreAI.Project04_ChatAI.Services.ChatProviders
{
    public class GeminiChatService : IChatService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly Client _client;

        // Konuşma geçmişi tutuluyor
        private readonly List<Content> _history = new();

        public GeminiChatService(string apiKey, string model)
        {
            _apiKey = apiKey;
            _model = model;
            _client = new Client(apiKey: _apiKey);
        }

        public string ProviderName => "Gemini";

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            try
            {
                // Kullanıcı mesajını ekle
                var userMsg = new Content
                {
                    Role = "user",
                    Parts = new List<Part> { new Part { Text = prompt } }
                };

                _history.Add(userMsg);

                var response = await _client.Models.GenerateContentAsync(
                    model: _model,
                    contents: _history
                );

                var answer = response.Candidates[0].Content.Parts[0].Text;

                // Model yanıtını da hafızaya ekle
                _history.Add(new Content
                {
                    Role = "model",
                    Parts = new List<Part> { new Part { Text = answer } }
                });

                return answer;
            }
            catch
            {
                return "Yanıt üretilirken bir hata oluştu.";
            }
        }
    }

}
