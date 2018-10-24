using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CasellaTest.Models
{
    /// <summary>
    /// Class to maintain unique Word items along with the frequency they appear.
    /// </summary>
    public class WordItem
    {
        /// <summary>
        /// the word itself.
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// frequency of appearance.
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WordItem()
        {
            Word = null;
            Frequency = 0;
        }
    }
}