using Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Frontend
{
    public class APIService
    {
        private readonly HttpClient _httpClient;

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Reading>> GetSensorData(int roomID)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Reading>>($"Room/readings/{roomID}");
            if (response == null) { return new List<Reading>(); }
            return response;
        }
    }
}
