using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.MVVM.Model
{
    class GameM
    {
        public List<string> gameNames { get; set; }

        public GameM() {
            gameNames = new List<string> {"a","b","c","d","e"};                          
        }

    }
}
