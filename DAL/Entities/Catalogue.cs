using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Catalogue
    {
        public Games[] Games { get; set; }
        public string Type { get; set; }// Action game, Simulators, etc
        public Catalogue() { }

    }
}
