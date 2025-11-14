using NetCoreAI.Project03_RapidApi.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace NetCoreAI.Project03_RapidApi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            List<ApiSeriesViewModel> seriesList = new List<ApiSeriesViewModel>();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/series/"),
                Headers =
                {
                    { "x-rapidapi-key", "c5d5302a13mshf5befc9027d507dp1b716djsn6ad4410c06d2" },
                    { "x-rapidapi-host", "imdb-top-100-movies.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                seriesList = JsonConvert.DeserializeObject<List<ApiSeriesViewModel>>(body);
                foreach (var series in seriesList)
                {
                    Console.WriteLine($"{series.rank}. {series.title} ({series.year}) - Rating: {series.rating}");
                }
            }
        }
    }
}
