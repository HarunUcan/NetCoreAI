using NetCoreAI.Project04_ChatAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreAI.Project04_ChatAI.Services.ChatProviders
{
    public class OllamaChatService : IChatService
    {
        private readonly string _urlEndpoint;
        private readonly string _model;

        private readonly HttpClient _httpClient;
        public OllamaChatService(string model, string apiUrl)
        {
            _model = model;
            _urlEndpoint = apiUrl;

            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(300)
            };
        }
        public string ProviderName => $"Ollama[{_model}]";


        public async Task<string> GenerateResponseAsync(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                prompt = prompt,
                stream = false
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_urlEndpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<JsonElement>(responseString);

                    if (result.TryGetProperty("response", out var textProp))
                    {
                        return textProp.GetString() ?? "No response from Ollama.";
                    }

                    return "Invalid response from Ollama.";
                }

                return $"Error: {response.StatusCode}, Details: {responseString}";
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error generating response from Ollama API", ex);
            }
        }
    }
}
