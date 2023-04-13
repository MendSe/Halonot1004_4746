using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Servers
    {
        [Key]
        public string GameName { get; set; }
        public float CPUUsage  { get; set; }
        public float MaxCPU { get; set; } 
        public int PlayersCount { get; set; }
        public int RAMSize { get; set; }
        public float RAMUsage { get; set; }
        public string Source { get; set; }
        public Servers() { }
        public Status Status { get; set; }

        public override string ToString()
        {
            return $"Game name: {GameName}\nCPU usage: {CPUUsage}\nMax CPU usage: {MaxCPU}\nPlayers count: {PlayersCount}\nRAM size: {RAMSize}\nRAM usage: {RAMUsage}\nSource: {Source}";
        }
    }
}
