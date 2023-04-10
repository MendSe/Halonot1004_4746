using DAL.Entities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL : IDAL
    {
        private readonly DBContext _context;
        public DAL() { 
            _context = new DBContext();
        }
        public DAL(DBContext dBContext) {
            _context=dBContext;
        }

        public async Task AddGamesAsync(List<Games> games)
        {
           // _context.Games.AddRange(games)
            foreach (var game in games)
            {

                _context.Games.Add(game);
                _context.SaveChanges();
            }
            Games temp;
            foreach (var game in _context.Games)
            {
                temp = game;
            }
        }
        public async Task testtest()
        {
            Games temp;
            foreach (var game in _context.Games)
            {
                temp = game;
            }
        }

    }
}
