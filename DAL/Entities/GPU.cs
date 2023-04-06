using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class GPU
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string Generation { get; set; }

        public double Temperature { get; set; }
        public GPU() { }

    }
}
