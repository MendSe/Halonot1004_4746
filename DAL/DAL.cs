using DAL.Entities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

namespace DAL
{
    public class DAL : IDAL
    {
        private readonly DBContext _context;
        public DAL() { 
            _context = new DBContext();
            //clear db
            /*using (var context = new DBContext())
            {
                context.EmptyLocalDb();
            }*/
        }
        public DAL(DBContext dBContext) {
            _context=dBContext;
        }

        #region ADD
        /// <summary>
        /// This function adds a list of game to the database
        /// </summary>
        /// <param name="games">the list of games</param>
        /// <returns></returns>
        public async Task AddGamesAsync(List<Games> games)
        {
           // _context.Games.AddRange(games)
            foreach (var game in games)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
            }
            //await _context.SaveChangesAsync();
        }
        /// <summary>
        /// This function adds one game to the database
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task AddGameAsync(Games game)
        {
            // _context.Games.AddRange(games)
            var existingObject = _context.Games.Find(game.Name);
            if (existingObject == null)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
            
            //await _context.SaveChangesAsync();
            }
            
        }

        /// <summary>
        /// This functions add a server to the database
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task AddServerAsync(Servers server)
        {
            var existingObject = _context.Servers.Find(server.GameName);
            if (existingObject != null)
            {
                // Update the existing object with new values
                existingObject.PlayersCount = server.PlayersCount;
                // ... update other properties as needed

                // Save the changes
                _context.SaveChanges();
            }
            else
            {
                _context.Servers.Add(server);
                await _context.SaveChangesAsync();
            }

        }
        
        #endregion ADD

        #region DELETE
        /// <summary>
        /// This function delete a game from the database
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task DeleteGameAsync(Games game)
        {
            var gameToDelete = _context.Games.SingleOrDefault(g => g.Name == game.Name);
            if (gameToDelete != null)
            {
                _context.Games.Remove(gameToDelete);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// This function delete a server from the database
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task DeleteServer(Servers server)
        {
            if (_context.Servers.Contains(server)) 
                _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
        }

        #endregion DELETE

        #region Get all

        public IEnumerable<Games> ListOfGames()
        {
            try
            {
                List<Games> games = _context.Games.ToList<Games>();
                return games;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<Servers> ListOfServers()
        {
            try
            {
                List<Servers> servers = _context.Servers.ToList<Servers>();
                return servers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Catalogue>> ListOfCatalogues()
        {
            try
            {
                List<Catalogue> cata = _context.Catalogue.ToList<Catalogue>();
                return cata;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion Get all

        #region Not Used
        public async Task AddGameToCatalogue(Catalogue catalogue, Games game)
        {
            _context.Catalogue.Find(catalogue)?.ListGames.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task AddCatalogue(Catalogue catalogue)
        {
            if (_context.Catalogue.Contains(catalogue))
                return;
            _context.Catalogue.Add(catalogue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGameFromCatalogue(Catalogue catalogue, Games game)
        {
            _context.Catalogue.Find(catalogue)?.ListGames?.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCatalogue(Catalogue catalogue)
        {
            if (_context.Catalogue.Contains(catalogue))
                _context.Catalogue.Remove(catalogue);
            await _context.SaveChangesAsync();
        }
        #endregion Not Used

        public async Task testtest()
        {
            Games temp;
            Servers orary;
            foreach (var game in _context.Games)
            {
                Console.WriteLine(game);
            }
            foreach (var serv in _context.Servers)
            {
                Console.WriteLine(serv);
            }
        }

    }
}
