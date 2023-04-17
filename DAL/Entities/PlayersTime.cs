using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class PlayersTime
    {
        public int Num { get; set; }
        public DateTime Hour { get; set; }
        public override string ToString()
        {
            return $"Number of players: {Num}\nHour: {Hour}";
        }
    }
}
