using HangmanGame.EnumWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame
{
    public class WordEngine
    {
        public static string GetRandomWord()
        {
            Random random = new Random();
            int numberTMethod = random.Next(0, 2);

            return numberTMethod == 0 ? GetRandomWordFruit() : GetRandomWordCarBrand();
        }
        public static string GetRandomWordFruit()
        {
            List<FruitWords> word = Enum.GetValues(typeof(FruitWords)).Cast<FruitWords>().ToList();
            int randomIndex = Random.Shared.Next(word.Count);
            string wordForGame = word[randomIndex].ToString();

            return wordForGame.ToLower();
        }  
        
        public static string GetRandomWordCarBrand()
        {
            List<CarBrandWords> word = Enum.GetValues(typeof(CarBrandWords)).Cast<CarBrandWords>().ToList();
            int randomIndex = Random.Shared.Next(word.Count);
            string wordForGame = word[randomIndex].ToString();
            return wordForGame.ToLower();
        }
    }
}
