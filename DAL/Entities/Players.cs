using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Players
    {
        [Key]
        public int id { get; set; }
        public Players() { }
        public string Name { get; set; }
        public int Id { get; set; }
        public double Score { get; set; }
    }
}
