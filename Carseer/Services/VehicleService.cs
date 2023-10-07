using Carseer.Api.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Carseer.Api.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly VehicleOptions _options;

        public VehicleService(HttpClient httpClient, IOptions<VehicleOptions> vehicleOptions)
        {
            _httpClient = httpClient;
            _options = vehicleOptions.Value;

            _httpClient.BaseAddress = new Uri(_options.Uri);
        }

        public async Task<List<string>> GetModelsForMakeIdYear(int makeId, int modelyear)
        {
            var requestUrl = $"makeId/{makeId}/modelyear/{modelyear}?format=json";

            var response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed API:{result}");
                }
                else
                {
                    throw new Exception("Http client(s) failed to connect to vehicles api.");
                }
            }

            var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
            var results = jsonResult.SelectToken("Results");
            if (results == null || !results.Any())
            {
                return new();
            }

            List<string> models = new();
            foreach (var result in results)
            {
                models.Add(result.Value<string>("Model_Name")!);
            }

            return models;
        }
    }
}