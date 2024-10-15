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
            Apple,
            Apricot,
            Avocado,
            Banana,
            Blackberry,
            Blueberry,
            Cherry,
            Coconut,
            Cucumber,
            Durian,
            Dragonfruit,
            Fig,
            Gooseberry,
            Grape,
            Guava,
            Jackfruit,
            Plum,
            Kiwifruit,
            Kumquat,
            Lemon,
            Lime,
            Mango,
            Watermelon,
            Mulberry,
            Orange,
            Papaya,
            Passionfruit,
            Peach,
            Pear,
            Persimmon,
            Pineapple,
            Pineberry,
            Quince,
            Raspberry,
            Soursop,
            Strawberry,
            Tamarind,
            Yuzu
        }

        public static string GetRandomWord()
        {
            Random randomWord = new Random();
            List<Words> word = Enum.GetValues(typeof(Words)).Cast<Words>().ToList();
            int randomIndex = randomWord.Next(word.Count);
            string wordForGame = word[randomIndex].ToString();
            return wordForGame.ToLower();
        }
    }
}
