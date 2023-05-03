using DAL.Entities;

namespace BL
{
    public interface IBL
    {
        IEnumerable<Games> GetGames();
        Task<Games> RetrieveGameFromApiAsync(string searchTerm);
        Task<List<Games>> RetrieveGamesFromApiAsync(string searchTerm);
        Task<List<PlayersTime>> RetrieveNumberOfPlayersTime(int numplayers, DateTime start, DateTime end);
        Task<Servers> RetrieveServerFromApiAsync(string gameName);
        Task StoreGameAsync(string searchTerm);
        Task SaveGameAsync(Games game);
        Task DeleteGame(Games game);
        Task StoreGamesAsync(string searchTerm);
        Task StoreServerAsync(string searchTerm);
        Task testtest();
    }
}
