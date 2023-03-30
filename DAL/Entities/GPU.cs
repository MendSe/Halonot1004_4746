using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    internal class GPU
    {
        public string Name { get; set; }
        public string Generation { get; set; }

        public double Temperature { get; set; }
        public GPU() { }

    }
}
