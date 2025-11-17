using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using NetCoreAI.Project09_TranslateAI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAI.Project09_TranslateAI.Services.TranslateProviders
{
    public class GoogleCloudTranslateService : ITranslateService
    {
        private readonly string _projectId;
        private readonly string _secretJsonFileName;

        public GoogleCloudTranslateService(string projectId, string secretJsonFileName)
        {
            _projectId = projectId;
            _secretJsonFileName = secretJsonFileName;
        }

        public string ProviderName => "Google Cloud";

        public async Task<string> TranslateAsync(string content, string targetLang)
        {
            try
            {
                string credentialsPath = Path.Combine(AppContext.BaseDirectory, "Secrets", _secretJsonFileName);
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

                TranslationServiceClient client = await TranslationServiceClient.CreateAsync();

                var parent = new LocationName(_projectId, "global");

                var request = new TranslateTextRequest
                {
                    Contents = { content },
                    TargetLanguageCode = targetLang,
                    ParentAsLocationName = parent
                };

                TranslateTextResponse response = await client.TranslateTextAsync(request);

                var translated = response.Translations
                                        .FirstOrDefault()
                                        ?.TranslatedText;

                // Güvenli çıkış
                if (string.IsNullOrWhiteSpace(translated))
                    return "Çeviri bulunamadı.";

                // Gereksiz boşlukları temizle
                translated = translated.Trim();

                return translated;
            }
            catch (Exception ex)
            {
                return $"Google Cloud Translate hatası: {ex.Message}";
            }
        }
    }
}
