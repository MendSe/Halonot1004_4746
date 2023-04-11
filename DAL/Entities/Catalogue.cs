using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Catalogue
    {
        [Key]
        public int id { get; set; }
        public string Type { get; set; }// Action game, Simulators, etc
        public List<Games> ListGames { get; set; }
        public Catalogue() { }

    }
}
