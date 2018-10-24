using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CasellaTest.Models
{
    public class WordItem
    {
        public string Word { get; set; }
        public int Frequency { get; set; }

        public WordItem()
        {
            Word = null;
            Frequency = 0;
        }
    }
}