using NetCoreAI.Project06_ImageGenerationAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetCoreAI.Project06_ImageGenerationAI.Services
{
    public class HuggingFaceImageGenerationService : IImageGenerationService
    {
        private readonly string _apiUrl;
        private readonly string _model;
        private readonly string _apiKey;

        

        public HuggingFaceImageGenerationService(string apiUrl, string model, string apiKey)
        {
            _apiUrl = apiUrl;
            _model = model;
            _apiKey = apiKey;
        }

        public async Task<string> GenerateImageAsync(string prompt)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var requestBody = new
            {
                prompt = prompt,
                model = _model,
                response_format = "base64"
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(_apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(responseJson);
                    var imageBase64Data = document.RootElement.GetProperty("data")[0].GetProperty("b64_json").GetString();
                    return imageBase64Data ?? "No image data received.";
                }
                return $"Error: {response.StatusCode}, Details: {await response.Content.ReadAsStringAsync()}";
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex.Message}";
            }
        }
    }
}
