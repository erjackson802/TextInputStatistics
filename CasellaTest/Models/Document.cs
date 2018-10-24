using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CasellaTest.Models
{/// <summary>
/// Document object, containing a list of word items.
/// </summary>
    public class Document
    {
        private List<WordItem> Words { get; set; }
        private string JsonString { get; set; }
        private int WhitespaceCount { get; set; }
        private int PunctuationCount { get; set; }
        private int WordCount { get; set; }
        private string SourceText { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="sText"></param>
        public Document(string sText)
        {
            SourceText = sText;
            JsonString = "";
            WhitespaceCount = 0;
            PunctuationCount = 0;
            WordCount = 0;
            Words = new List<WordItem>();
        }


        /// <summary>
        /// Separates each unique word from the SourceText into a WordItem in Words.
        /// </summary>
        public void Process()
        {
            //splits the SourceText into individual words and trims punctuation.
            char[] punctuation = SourceText.Where(char.IsPunctuation).Distinct().ToArray();
            List<string> tmpWords = SourceText.Split().Select(x => x.Trim(punctuation)).ToList();
            
            //Counts the amount of whitespace and punctuation
            SourceText = SourceText.Replace("\r\n", "\n");                      //for proper whitespace character count
            WhitespaceCount = SourceText.Count(f => char.IsWhiteSpace(f));
            PunctuationCount = SourceText.Count(f => char.IsPunctuation(f));
                    
            //iterate through the separated words
            for (int i = 0; i < tmpWords.Count(); i++)
            {
                WordItem currentWord = new WordItem() { Word = tmpWords.ElementAt(i).ToLower(), Frequency = 1 };
                
                //iterate through the remaining words 
                for (int j = i + 1; j < tmpWords.Count(); j++)
                {
                    //if a match to currentWord is found, increment frequency and remove the match.
                    //  decrement the remaining words index as the list has shortened.
                    if (tmpWords.ElementAt(j).ToLower().Equals(currentWord.Word.ToLower()))
                    {
                        currentWord.Frequency++;
                        tmpWords.RemoveAt(j);
                        j--;
                    }
                }

                //if not a whitespace WordItem add to our Words List and increment WordCount with the frequency of this new WordItem
                if (!string.IsNullOrWhiteSpace(currentWord.Word))
                {
                    Words.Add(currentWord);
                    WordCount += currentWord.Frequency;
                }
            }
        }

        /// <summary>
        ///    Creates a JSON string containing an ordered list of (iNumberOfResponses) items.  Ordered by frequency then 
        ///    by Word alphabeticall
        /// </summary>
        /// <param name="iNumberOfResponses"> The maximum number of responses expected from an ordered list of word items </param>
        /// <returns></returns>
        public string GetJsonForTop(int iNumberOfResponses)
        {
            //using OrderBy instead of Sort due to double sort parameters and possiblity for inaccurate results if a sort key has too
            // many common occurrences 
            string sWords = JsonConvert.SerializeObject(Words.OrderByDescending(x => x.Frequency).ThenBy(x => x.Word).Take(iNumberOfResponses));

            //using decimal for its greater accuracy 
            decimal whitespacePercentage = (decimal)WhitespaceCount / SourceText.Length;
            decimal punctuationPercentage = (decimal)PunctuationCount / SourceText.Length;

            string sOut = string.Format("{{ \"Frequency\": {0} , \"WordCount\": {1}, \"WhitespacePercentage\": {2}, \"PunctuationPercentage\":{3} }}", 
                sWords, WordCount, whitespacePercentage, punctuationPercentage);
            
            return sOut;
        }

    }
}