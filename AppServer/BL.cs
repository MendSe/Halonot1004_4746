using DAL.Entities;
using System;
using Newtonsoft.Json.Linq;
using DAL;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Connections.Features;

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
        #region API
        public async Task StoreGamesAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            List<Games> games = await RetrieveGamesFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGamesAsync(games);
            await myDal.testtest();
            List<Games> mytest = (List<Games>)await myDal.ListOfGames();           
        }
        public async Task StoreGameAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            Games game = await RetrieveGameFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGameAsync(game);
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
                    Game_Id = game["id"].Value<int>(),
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
        public async Task<Games> RetrieveGameFromApiAsync(string searchTerm)
        {

            IGDBApi igdbClient = new IGDBApi(clientID, secretID);

            // Search for games.
            string searchResult = await igdbClient.SearchGamesAsync(searchTerm,1);

            // Deserialize the search result.
            JArray jsonArray = JArray.Parse(searchResult);

            // Create a list to store the games.
            Games game = new Games();

            // Loop through the search result and retrieve more details for each game.
            if (jsonArray.Count > 0)
            {
                JObject firstGame = jsonArray.First as JObject;

                // Create a new Games object and add it to the list.
                Games newGame = new Games
                {
                    Game_Id = firstGame["id"].Value<int>(),
                    Name = firstGame["name"].ToString(),
                    Summary = firstGame["summary"]?.ToString(),
                    ReleaseDate = firstGame["first_release_date"]?.Value<long?>() != null ? DateTimeOffset.FromUnixTimeSeconds((long)firstGame["first_release_date"]).UtcDateTime : new DateTime(1753, 1, 1),
                    //Rating = (double?)game["total_rating"],
                    //CoverImageUrl = game["cover"]?["url"]?.ToString(),
                };

                // Return the list of games.
                return newGame;
            }
            return null;
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
        #endregion API

        #region Emulator

        public async Task<List<PlayersTime>> RetrieveNumberOfPlayersTime(string gamename, DateTime start, DateTime end)
        {
            Servers serv = await RetrieveServerFromApiAsync(gamename);
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            double coef = now.Hour-12;
            double x = (coef/12)*Math.PI;
            //double test = Math.Sin(Math.PI);
            //int sign = coef < 0 ? 1 : -1;
            //int numOfPlayers = (int)(serv.PlayersCount * ((float)2 / 3 * Math.Sin(((coef * 15) * Math.PI) / 180) + 1));
            //double b = (double)4 / 5 * Math.Sin(((coef * 15) * Math.PI) / 180);
            int numOfPlayers = (int)(serv.PlayersCount / ((float)(2 / 3.0) * Math.Sin(x) + 1));
            start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
            List<PlayersTime> playersTimes = new List<PlayersTime>();
            Random random = new Random();
            double randomFactor = 1 + (random.NextDouble() * 0.14 - 0.07);

            while (start < end)
            {
                PlayersTime playersTime = new PlayersTime
                {
                    Hour = start,
                    Num = now == start ? serv.PlayersCount : (int)(numOfPlayers * ((float)(2 / 3.0) * Math.Sin((((start.Hour-now.Hour) / 12.0) * Math.PI) + x) + 1) * randomFactor)
            };
                playersTimes.Add(playersTime);
                start = start.AddHours(1);
                randomFactor = 1 + (random.NextDouble() * 0.14 - 0.07);
            }
            return playersTimes;
        }


        #endregion Emulator

        //test from dal class to print things
        public async Task testtest()
        {
            myDal.testtest();
        }


    }
}
