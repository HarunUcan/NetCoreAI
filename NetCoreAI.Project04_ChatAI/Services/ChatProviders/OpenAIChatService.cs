using NetCoreAI.Project04_ChatAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreAI.Project04_ChatAI.Services.ChatProviders
{
    public class OpenAIChatService : IChatService
    {
        public string ProviderName => "OpenAI";
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _urlEndpoint;

        public OpenAIChatService(string model, string apiKey, string urlEndpoint)
        {
            _model = model;
            _apiKey = apiKey;
            _urlEndpoint = urlEndpoint;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                input = prompt,
                max_tokens = 500,
                store = true
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(_urlEndpoint, content);
                var responseStr = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.StatusCode}, Details: {responseStr}";

                // JSON parse
                var doc = JsonDocument.Parse(responseStr);
                var msg = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return msg ?? "OpenAI returned empty response.";
            }
            catch (Exception ex)
            {
                return $"OpenAI Error: {ex.Message}";
            }
        }
    }
}
