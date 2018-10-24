using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CasellaTest.Models
{
    public class Document
    {
        private List<WordItem> Words { get; set; }
        private string JsonString { get; set; }
        private int WhitespaceCount { get; set; }
        private int PunctuationCount { get; set; }
        private int WordCount { get; set; }
        private string SourceText { get; set; }

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
        /// 
        /// </summary>
        /// <param name="argText"></param>
        /// <returns></returns>
        public void Process()
        {
            char[] punctuation = SourceText.Where(char.IsPunctuation).Distinct().ToArray();
            List<string> tmpWords = SourceText.Split().Select(x => x.Trim(punctuation)).ToList();
            SourceText = SourceText.Replace("\r\n", "\n");      //fix for proper character count

            WhitespaceCount = SourceText.Count(f => char.IsWhiteSpace(f));
            PunctuationCount = SourceText.Count(f => punctuation.Contains(f));
            
            
            for (int i = 0; i < tmpWords.Count(); i++)
            {
                WordItem currentWord = new WordItem() { Word = tmpWords.ElementAt(i).ToLower(), Frequency = 1 };

                for (int j = i + 1; j < tmpWords.Count(); j++)
                {
                    if (tmpWords.ElementAt(j).ToLower().Equals(currentWord.Word.ToLower()))
                    {
                        currentWord.Frequency += 1;
                        tmpWords.RemoveAt(j);
                        j--;
                    }
                }

                if (!string.IsNullOrWhiteSpace(currentWord.Word))
                {
                    Words.Add(currentWord);
                    WordCount += currentWord.Frequency;
                }
            }
        }

        public string GetJsonForTop(int iNumberOfResponses)
        {

            string sWords = JsonConvert.SerializeObject(Words.OrderByDescending(x => x.Frequency).ThenBy(x => x.Word));

            decimal whitespacePercentage = (decimal)WhitespaceCount / SourceText.Length;
            decimal punctuationPercentage = (decimal)PunctuationCount / SourceText.Length;
            
            string sFooter = string.Format("\"WordCount\": {0}, \"WhitespacePercentage\": {1}, \"PunctuationPercentage\":{2}", 
                WordCount, whitespacePercentage, punctuationPercentage);

            string sOut = string.Format("{{ \"Frequency\": {0} , {1} }}", sWords, sFooter);
            
            return sOut;
        }

    }
}