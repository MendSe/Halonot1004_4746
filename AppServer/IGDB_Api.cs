using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class IGDB_Api
    {
        private readonly HttpClient _client;

        public IGDB_Api(string clientId, string secretId)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.igdb.com/v4/");
            _client.DefaultRequestHeaders.Add("Client-ID", clientId);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {secretId}");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<string> SearchGamesAsync(string searchTerm)
        {
            var requestContent = new StringContent($"search \"{searchTerm}\"; fields *;", Encoding.UTF8, "text/plain");
            var response = await _client.PostAsync("games", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        public async Task<string> GetGameAsync(int id)
        {
            var requestContent = new StringContent($"fields name,summary,cover.image_id; where id = {id};", Encoding.UTF8, "text/plain");
            var response = await _client.PostAsync("games", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
