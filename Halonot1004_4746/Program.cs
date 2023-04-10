using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Sales
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            // Replace with your IGDB API key.
            string ClientID = "gqmx72k19ulx4zgvik4os0tshig4x5";
            string SecretID = "ze9zicpgudjhw4dzjxxqs50o0hmvrp";
            IGDBApi igdbClient = new IGDBApi(ClientID, SecretID);

            Console.Write("Enter a game to search: ");
            string searchTerm = "Pokemon";

            string result = await igdbClient.SearchGamesAsync(searchTerm);
            JArray jsonArray = JArray.Parse(result);
            int[] gameIds = new int[jsonArray.Count];
            //string imageurl = await igdbClient.GetGameCoverAsync(searchTerm);
            string[] imageurls = new string[jsonArray.Count];   
            for (int i = 0; i < jsonArray.Count; i++)
            {
                int gameId = jsonArray[i].Value<int>("id");
                gameIds[i] = gameId;
            }

            string firstimage = await igdbClient.GetGameCoverAsync(gameIds[0]);
            int count = 0;
            foreach (var game in result.Split(new string[] { "}," }, StringSplitOptions.None))
            {
                Console.WriteLine(game);
                Console.WriteLine(gameIds[count]);
                Console.WriteLine(firstimage);
                Console.WriteLine(count++);
            }
            Console.WriteLine($"Results: {result}");
            string imagePath = "C:\\Users\\Mendel\\Desktop\\Fuck\\finally.jpg";
            string directoryPath = Path.GetDirectoryName(imagePath);
            Directory.CreateDirectory(directoryPath);
            string imageurl = "http://" + firstimage.Substring(2);
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageurl.Replace("t_thumb", "t_cover_big"), imagePath);
            }

            Console.ReadLine();
            
        }
    }

}