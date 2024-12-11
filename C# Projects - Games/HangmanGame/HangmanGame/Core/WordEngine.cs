using HangmanGame.EnumWords;
using HangmanGame.IO;
using HangmanGame.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Engine
{
    public class WordEngine
    {
        public static string GetRandomWord()
        {
            int numberTMethod = Random.Shared.Next(0, 2);
            FruitWords[] fruitsWord = Enum.GetValues<FruitWords>();
            CarBrandWords[] carBrandsWord = Enum.GetValues<CarBrandWords>();

            return numberTMethod == 0 ? RandomWord(fruitsWord) : RandomWord(carBrandsWord);
        }
        private static string RandomWord<T>(T[] words)
        {
            int randomIndex = Random.Shared.Next(words.Length);
            string wordForGame = words[randomIndex].ToString()!;
            return wordForGame!.ToLower();
        } 

    }
}
