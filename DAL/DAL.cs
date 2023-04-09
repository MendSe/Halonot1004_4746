using DAL.Entities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _context.Games.AddRange(games);
            await _context.SaveChangesAsync();
        }

    }
}
