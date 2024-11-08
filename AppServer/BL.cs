﻿using DAL.Entities;
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
        /// <summary>
        /// This function retrieves a list of game from another function and send them to the dal
        /// </summary>
        /// <param name="searchTerm">the name of the games</param> 
        /// <returns></returns>
        public async Task StoreGamesAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            List<Games> games = await RetrieveGamesFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGamesAsync(games);
            await myDal.testtest();       
        }
        /// <summary>
        /// This function retrieves a game from another function and send it to the dal
        /// </summary>
        /// <param name="searchTerm">the name of the game</param> 
        /// <returns></returns>
        public async Task StoreGameAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            Games game = await RetrieveGameFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGameAsync(game);
            await myDal.testtest();
        }
        /// <summary>
        /// this function send a game to the dal to be saved
        /// </summary>
        /// <param name="game">the game</param> 
        /// <returns></returns>
        public async Task SaveGameAsync(Games game)
        {
            await myDal.AddGameAsync(game);
        }
        /// <summary>
        /// this function send a game to the dal to be deleted
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task DeleteGame(Games game)
        {
            await myDal.DeleteGameAsync(game);
        }
        /// <summary>
        /// This function retrieve a list of games from the api
        /// </summary>
        /// <param name="searchTerm">the games we want to retrieve</param> 
        /// <returns></returns>
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

        /// <summary>
        /// This function retrieve a single game from the api
        /// </summary>
        /// <param name="searchTerm">the game we want to retrieve</param> 
        /// <returns></returns>
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

        /// <summary>
        /// This function retrieve a server from api and send it to the DAL
        /// </summary>
        /// <param name="searchTerm">the server name</param> 
        /// <returns></returns>
        public async Task StoreServerAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            Servers server = await RetrieveServerFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddServerAsync(server);
            await myDal.testtest();


        }

        /// <summary>
        /// This function retrieve a server from the emulator
        /// </summary>
        /// <param name="gameName">the server name</param> 
        /// <returns></returns>
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
        /// <summary>
        /// This fonction calculate and predivt the number of players for a time interval and it's using a sinus function 
        /// which is modified to more accuracy on a reflexion of the reality
        /// </summary>
        /// <param name="numplayers">the current number of player</param>
        /// <param name="start">start date/hour</param>
        /// <param name="end">end date/hour</param>
        /// <returns></returns>
        public async Task<List<PlayersTime>> RetrieveNumberOfPlayersTime(int numplayers, DateTime start, DateTime end)
        {
            
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            double coef = now.Hour-12;
            double x = (coef/12)*Math.PI;

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
        /// <summary>
        /// This fonction returns the a factor depending of the day and the month for the prediction
        /// </summary>
        /// <param name="time">the current day</param> 
        /// <returns></returns>
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
