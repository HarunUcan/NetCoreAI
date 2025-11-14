using Google.GenAI;
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
        private readonly string _model;
        private readonly string _apiKey;

        private readonly Client _client;

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
                var response = await _client.Models.GenerateContentAsync(
                    model: _model, contents: prompt
                );
                return response.Candidates[0].Content.Parts[0].Text;
            }
            catch (Exception ex)
            {
                return "Yanıt üretilirken bir hata oluştu";
            }
        }
    }
}
