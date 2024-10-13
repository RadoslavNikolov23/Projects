using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame
{
    public class WordsClass
    {
        enum Words
        {
            apple, banana, cherry, watermelon
        }

        public static string GetRandomWord()
        {
            Random randomWord = new Random();
            List<Words> word = Enum.GetValues(typeof(Words)).Cast<Words>().ToList();
            int randomIndex = randomWord.Next(word.Count);
            string wordForGame = word[randomIndex].ToString();
            return wordForGame;
        }
    }
}
