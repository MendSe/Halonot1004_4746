using DAL.Entities;
using System;
using Newtonsoft.Json.Linq;
using DAL;

namespace BL
{
    public class BL
    {
        private IDAL myDal;
        public BL()
        {
            myDal = new DAL.DAL();
        }
        public async Task StoreGamesAsync(string searchTerm)
        {
            // Retrieve games info from IGDB API
            List<Games> games = await RetrieveGamesFromApiAsync(searchTerm);

            // Store games in database
            await myDal.AddGamesAsync(games);
        }


        public async Task<List<Games>> RetrieveGamesFromApiAsync(string searchTerm)
        {
            // Replace with your IGDB API key.
            string clientID = "gqmx72k19ulx4zgvik4os0tshig4x5";
            string secretID = "ze9zicpgudjhw4dzjxxqs50o0hmvrp";
            IGDB_Api igdbClient = new IGDB_Api(clientID, secretID);

            // Search for games.
            string searchResult = await igdbClient.SearchGamesAsync(searchTerm);

            // Deserialize the search result.
            JArray jsonArray = JArray.Parse(searchResult);

            // Create a list to store the games.
            List<Games> games = new List<Games>();

            // Loop through the search result and retrieve more details for each game.
            foreach (var game in jsonArray)
            {
                // Get the game details.
                string gameResult = await igdbClient.GetGameAsync(Convert.ToInt32(game["id"]));


                // Deserialize the game details.
                JArray gameArray = JArray.Parse(gameResult);

                // Get the first (and only) element of the array.
                JObject gameObj = (JObject)gameArray[0];

                // Create a new Games object and add it to the list.
                Games newGame = new Games
                {
                    Name = gameObj["name"].ToString(),
                    Summary = gameObj["summary"]?.ToString(),
                    ReleaseDate = DateTimeOffset.FromUnixTimeSeconds((long)gameObj["first_release_date"]).UtcDateTime,
                    Rating = (double?)gameObj["total_rating"],
                    CoverImageUrl = gameObj["cover"]?["url"]?.ToString(),
                };
                games.Add(newGame);
            }


            // Return the list of games.
            return games;
        }

    }
}
