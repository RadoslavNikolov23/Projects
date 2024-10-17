using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame
{
    public class WordsClass
    {
        enum FruitWords
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

        enum CarBrandWords
        {
            Acura,
            AlfaRomeo,
            AstonMartin,
            Audi,
            Bentley,
            BMW,
            Bugatti,
            Buick,
            Cadillac,
            Chevrolet,
            Chrysler,
            Citroen,
            Dacia,
            Datsun,
            Dodge,
            Ferrari,
            Fiat,
            Ford,
            GMC,
            GreatWall,
            Haval,
            Hennessey,
            Holden,
            Honda,
            Hummer,
            Hyundai,
            Infiniti,
            JAC,
            Jaguar,
            Jeep,
            Kia,
            Koenigsegg,
            Lada,
            Lamborghini,
            Lancia,
            LandRover,
            Lexus,
            Lincoln,
            Lotus,
            Maserati,
            Maybach,
            Mazda,
            McLaren,
            MercedesBenz,
            Mercury,
            MG,
            MINI,
            Mitsubishi,
            Morgan,
            Nissan,
            Noble,
            Oldsmobile,
            Opel,
            Pagani,
            Peugeot,
            Plymouth,
            Pontiac,
            Porsche,
            Ram,
            Renault,
            Rimac,
            RollsRoyce,
            Saab,
            Saturn,
            Scion,
            SEAT,
            Skoda,
            Smart,
            Subaru,
            Suzuki,
            Tesla,
            Toyota,
            Vauxhall,
            Volkswagen,
            Volvo,
            Wiesmann
        }

        public static string GetRandomWord()
        {
            Random random = new Random();
            int numberTMethod = random.Next(0, 2);

            if (numberTMethod == 0) return GetRandomWordFruit();
            else return GetRandomWordCarBrand();


        }
        public static string GetRandomWordFruit()
        {
            Random randomWord = new Random();
            List<FruitWords> word = Enum.GetValues(typeof(FruitWords)).Cast<FruitWords>().ToList();
            int randomIndex = randomWord.Next(word.Count);
            string wordForGame = word[randomIndex].ToString();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"You have to guess a fruit, with the length of {wordForGame.Length} letters and have only 6 tries!\n");
            Console.ForegroundColor = ConsoleColor.White;

            return wordForGame.ToLower();
        }  
        
        public static string GetRandomWordCarBrand()
        {
            Random randomWord = new Random();
            List<CarBrandWords> word = Enum.GetValues(typeof(CarBrandWords)).Cast<CarBrandWords>().ToList();
            int randomIndex = randomWord.Next(word.Count);
            string wordForGame = word[randomIndex].ToString();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"You have to guess a carBrand, with the length of {wordForGame.Length} letters and have only 6 tries!\n");
            Console.ForegroundColor = ConsoleColor.White;
            return wordForGame.ToLower();
        }
    }
}
