using System;
using System.Threading.Tasks;

namespace BL
{
    public class Tests
    {
        static async Task Main(string[] args)
        {
            var bl = new BL();
            await bl.StoreGamesAsync("Pokemon");
            Console.ReadLine();
        }
    }
}
