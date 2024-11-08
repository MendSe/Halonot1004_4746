﻿using Newtonsoft.Json;
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
        public string Name { get; set; }
        public int Game_Id { get; set; }
        public string Summary { get; set; }

        public string CoverPath { get; set; }

        public string CoverUrl { get; set; }

        public DateTime ReleaseDate { get; set; }

        public override string ToString()
        {
            return $"Game ID: {Game_Id}\nGame Name: {Name}\nSummary: {Summary}\nRelease_Date: {ReleaseDate}\nCoverUrl: {CoverUrl}";
        }


    }
}
