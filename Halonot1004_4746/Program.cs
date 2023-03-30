using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string searchTerm = "DICK";

            string result = await igdbClient.SearchGamesAsync(searchTerm);

            Console.WriteLine($"Results: {result}");

            Console.ReadLine();
        }
    }

}