using DAL.Entities;
using System;
using Newtonsoft.Json.Linq;
using DAL;
using System.Net.Http;
using System.Text.Json;


namespace BL
{
    public class BL
    {
        private IDAL myDal;
        private string clientID = "gqmx72k19ulx4zgvik4os0tshig4x5";
        private string secretID = "ze9zicpgudjhw4dzjxxqs50o0hmvrp";
        public BL()
        {
            myDal = new DAL.DAL();
        }
        public BL(IDAL dal)
        {
            myDal=dal;
        }
        public async Task StoreGamesAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            List<Games> games = await RetrieveGamesFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGamesAsync(games);
            await myDal.testtest();
            List<Games> mytest = (List<Games>)await myDal.ListOfGames();

            
        }


        public async Task<List<Games>> RetrieveGamesFromApiAsync(string searchTerm)
        {

            IGDBApi igdbClient = new IGDBApi(clientID,secretID);

            // Search for games.
            string searchResult = await igdbClient.SearchGamesAsync(searchTerm);

            // Deserialize the search result.
            JArray jsonArray = JArray.Parse(searchResult);

            // Create a list to store the games.
            List<Games> games = new List<Games>();

            // Loop through the search result and retrieve more details for each game.
            foreach (var game in jsonArray)
            {

                // Create a new Games object and add it to the list.
                Games newGame = new Games
                {
                    Id = game["id"].Value<int>(),
                    Name = game["name"].ToString(),
                    Summary = game["summary"]?.ToString(),
                    ReleaseDate = game["first_release_date"]?.Value<long?>() != null ? DateTimeOffset.FromUnixTimeSeconds((long)game["first_release_date"]).UtcDateTime : new DateTime(1753, 1, 1),
                    //Rating = (double?)game["total_rating"],
                    //CoverImageUrl = game["cover"]?["url"]?.ToString(),
                };
                games.Add(newGame);
                
            }


            // Return the list of games.
            return games;
        }

        public async Task StoreServerAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            Servers server = await RetrieveServerFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddServerAsync(server);
            await myDal.testtest();
            List<Servers> mytest = (List<Servers>)await myDal.ListOfServers();


        }
        public async Task<Servers> RetrieveServerFromApiAsync(string gameName)
        {
            //var apiUrl = $"http://localhost:5000/GetData/{gameName}/{start_time}/{end_time}";
            var apiUrl = $"http://localhost:5000/GetData/{gameName}";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonResponse);
                Servers newServer = new Servers
                {
                    CPUUsage = result["CPUUsage"].GetSingle(),
                    GameName = result["GameName"].GetString(),
                    MaxCPU = result["MaxCPU"].GetSingle(),
                    PlayersCount = result["PlayersCount"].GetInt32(),
                    RAMSize = result["RAMSize"].GetInt32(),
                    RAMUsage = result["RAMUsage"].GetSingle(),
                    Source = result["Source"].GetString()
                };
                return newServer;
            }

            return null;
        }
        public async Task testtest()
        {
            myDal.testtest();
        }


    }
}
