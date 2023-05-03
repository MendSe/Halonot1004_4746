using DAL.Entities;
using System;
using Newtonsoft.Json.Linq;
using DAL;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Connections.Features;

namespace BL
{
    public class BL : IBL
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
        }
        public async Task StoreGameAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            Games game = await RetrieveGameFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGameAsync(game);
            await myDal.testtest();
        }
        //no need api
        public async Task SaveGameAsync(Games game)
        {
            await myDal.AddGameAsync(game);
        }
        public async Task DeleteGame(Games game)
        {
            await myDal.DeleteGameAsync(game);
        }
        public async Task<List<Games>> RetrieveGamesFromApiAsync(string searchTerm)
        {

            IGDBApi igdbClient = new IGDBApi(clientID,secretID);

            // Search for games.
            string searchResult = await igdbClient.SearchGamesAsync(searchTerm,5);

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
                    Name = game["name"]?.ToString(),
                    Summary = game["summary"]?.ToString(),
                    ReleaseDate = game["first_release_date"]?.Value<long?>() != null ? DateTimeOffset.FromUnixTimeSeconds((long)game["first_release_date"]).UtcDateTime : new DateTime(1753, 1, 1),
                    CoverPath = null,
                    CoverUrl = game["cover"] != null ? "https:" + (game["cover"]["url"]?.ToString()).Replace("t_thumb", "t_cover_big") : null,
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
                    CoverPath = null,
                    //CoverUrl = firstGame["cover"]?.ToString()
                    CoverUrl = firstGame["cover"] != null ? "https:" + (firstGame["cover"]["url"]?.ToString()).Replace("t_thumb", "t_cover_big") : null,
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

        public async Task<List<PlayersTime>> RetrieveNumberOfPlayersTime(int numplayers, DateTime start, DateTime end)
        {
            
            //Servers serv = await RetrieveServerFromApiAsync(gamename);
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            double coef = now.Hour-12;
            double x = (coef/12)*Math.PI;

            //double test = Math.Sin(Math.PI);
            //int sign = coef < 0 ? 1 : -1;
            //int numOfPlayers = (int)(serv.PlayersCount * ((float)2 / 3 * Math.Sin(((coef * 15) * Math.PI) / 180) + 1));
            //double b = (double)4 / 5 * Math.Sin(((coef * 15) * Math.PI) / 180);

            //int numOfPlayers = (int)(serv.PlayersCount / ((float)(2 / 3.0) * Math.Sin(x) + 1));
            int numOfPlayers = (int)(numplayers / ((float)(2 / 3.0) * Math.Sin(x) + 1));
            start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
            List<PlayersTime> playersTimes = new List<PlayersTime>();
            Random random = new Random();
            double randomFactor = 1 + (random.NextDouble() * 0.14 - 0.07);


            while (start < end)
            {
                double coefDayMonth = CoefDayMonth(start);
                PlayersTime playersTime = new PlayersTime
                {
                    Hour = start,
                    Num = now == start ? numplayers : (int)(numOfPlayers * ((float)(2 / 3.0) * Math.Sin((((start.Hour-now.Hour) / 12.0) * Math.PI) + x) + 1) * randomFactor*coefDayMonth)
            };
                playersTimes.Add(playersTime);
                start = start.AddHours(1);
                randomFactor = 1 + (random.NextDouble() * 0.14 - 0.07);
            }
            return playersTimes;
        }

        public double CoefDayMonth(DateTime time)
        {
            double coef = 1;

            switch (time.Month)
            {
                case 8:
                case 9:
                case 12:
                    coef *= 1.2;
                    break;
                case 10:
                case 3:
                case 4:
                case 5:
                case 6:
                    coef *= 0.9;
                    break;
                default:
                    break;
            }

            switch (time.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    coef *= 1.2;
                    break;
                case DayOfWeek.Friday:
                    coef *= 1.1;
                    break;
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Thursday:
                    coef *= 0.9;
                    break;
                default:
                    break;
            }

            return coef;
        }


        #endregion Emulator

        public IEnumerable<Games> GetGames()
        {
            return myDal.ListOfGames();

        }

        //test from dal class to print things
        public async Task testtest()
        {
            await myDal.testtest();
        }


    }
}
