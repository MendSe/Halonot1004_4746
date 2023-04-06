using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class RAM
    {
        [Key]
        public int id { get; set; }
        public int Speed { get; set; }
        public int Capacity { get; set; }
        public RAM() { }
    }
}
