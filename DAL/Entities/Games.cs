using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Games
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        //public double? Rating { get; set; }

        //public int TotalRatingCount { get; set; }

        public DateTime ReleaseDate { get; set; }

        public override string ToString()
        {
            return $"Game ID: {Id}\nGame Name: {Name}\nSummary: {Summary}\nRelease_Date: {ReleaseDate}";
        }


    }
}
