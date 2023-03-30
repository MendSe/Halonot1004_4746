using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    internal class Servers
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public RAM RAM { get; set; }
        public GPU GPU { get; set; }
        public Servers() { }
        public Catalogue[] Catalogues { get; set; }
        public Status Status { get; set; }
        public int NumPlayers { get; set; }
        public Players[] Players { get; set; }
        public double MaxScore { get; set; }

    }
}
