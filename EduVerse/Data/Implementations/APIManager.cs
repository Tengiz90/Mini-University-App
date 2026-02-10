using EduVerse.Data.Interfaces;
using EduVerse.Models;
using Newtonsoft.Json;

namespace EduVerse.Data.Implementations
{
    public class APIManager : IAPIManager
    {
        private readonly HttpClient _httpClient;

        public APIManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> GetIsRegistrationOpenAsync()
        {
            var data = await GetRegistrationDataAsync();
            return data.IsOpen;
        }

        public async Task<DateTime> GetRegistrationOpenUntilDateAsync()
        {
            var data = await GetRegistrationDataAsync();
            return data.OpenUntil;
        }

        
        private async Task<RegistrationAPIInfo> GetRegistrationDataAsync()
        {
            string url = "http://raw.githubusercontent.com/Tengiz90/RegistrationAPIInfo/refs/heads/main/RegistrationAPIInfo.json";

            var json = await _httpClient.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<RegistrationAPIInfo>(json);
            return data ?? new RegistrationAPIInfo { IsOpen = false, OpenUntil = DateTime.MinValue };
        }
    }
}
