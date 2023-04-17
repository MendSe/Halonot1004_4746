using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDAL
    {
        Task AddGamesAsync(List<Games> games);
        Task AddGameAsync(Games game);
        Task AddServerAsync(Servers server);
        Task testtest();
        Task AddGameToCatalogue(Catalogue catalogue, Games game);
        Task AddCatalogue(Catalogue catalogue);
        Task DeleteGameAsync(Games game);
        Task DeleteGameFromCatalogue(Catalogue catalogue, Games game);
        Task DeleteCatalogue(Catalogue catalogue);
        Task DeleteServer(Servers server);
        Task<IEnumerable<Games>> ListOfGames();
        Task<IEnumerable<Servers>> ListOfServers();
        Task<IEnumerable<Catalogue>> ListOfCatalogues();
    }
}
