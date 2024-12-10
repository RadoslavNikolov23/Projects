using HangmanGame.EnumWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame
{
    public class GameClass
    {

        public static void GameOn()
        {
            string word = WordEngine.GetRandomWord();
            int maxTries = 6;
            int countTries = 0;
            bool guestTheWord = false;

            ChechWordType(word, maxTries);

            char[] charWords = word.ToArray();
            char[] emptyCharArray = new char[charWords.Length];

            List<char> guestLettersList = new List<char>();

            for (int i = 0; i < emptyCharArray.Length; i++) emptyCharArray[i] = '*';

            int attempsToWin = charWords.Length;
            int countToWin = 0;

            Console.WriteLine("Let's begin!");
            OutputGraphics.OutputGraphics.StartView();

            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Your word is: {string.Join("", emptyCharArray)}\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Enter a letter: ");

                    char guestSymbol = char.Parse(Console.ReadLine());


                    if (charWords.Contains(guestSymbol) && char.IsLetter(guestSymbol))
                    {
                        int indexAt = charWords.ToList().IndexOf(guestSymbol);
                        emptyCharArray[indexAt] = guestSymbol;
                        charWords[indexAt] = '*';
                        Console.WriteLine($"Congratulation you have found the letter {guestSymbol}");
                        countToWin++;

                    }
                    else
                    {
                        countTries++;
                        Console.WriteLine("Wrong letter!");
                        Console.WriteLine($"You have {maxTries - countTries} wrong tries left!");
                        ChechOutPut(countTries);
                        Console.WriteLine($"Try again!");
                    }

                    if (maxTries == countTries)
                    {
                        OutputGraphics.OutputGraphics.MaxOut();
                        Console.WriteLine($"The word was - {word}");
                        break;
                    }

                    if (attempsToWin == countToWin)
                    {
                        guestTheWord = true;
                        Console.WriteLine($"\nCongratulations you have fount the word: {word}");
                        break;
                    }

                    if (char.IsLetter(guestSymbol))
                    {
                        guestLettersList.Add(guestSymbol);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You have tried the following letters:");
                        Console.WriteLine(string.Join(", ", guestLettersList));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor= ConsoleColor.Magenta;
                        Console.WriteLine("Enter a letter!\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wrong input!");
                    Console.WriteLine($"Enter ONLY one letter!\n");
                }
            }
        }

        private static void ChechWordType(string word, int maxTries)
        {
            FruitWords[] wordToMatch = Enum.GetValues<FruitWords>();
            bool isFruit = false;
            foreach(var wordWord in wordToMatch)
            {
                if (wordWord.ToString().ToLower() == word)
                {
                    isFruit = true;
                    break;
                }
            }

            string typeWord=String.Empty;
            typeWord = isFruit ? "Fruit" : "Car Brand";

            Console.WriteLine($"You have to guess a {typeWord}, with the length of {word.Length} letters and have only {maxTries} tries!\n");
        }

        public static void ChechOutPut(int number)
        {
            switch (number)
            {
                case 1:
                    OutputGraphics.OutputGraphics.FirstTry();
                    break;
                case 2:
                    OutputGraphics.OutputGraphics.SecondTry();
                    break;
                case 3:
                    OutputGraphics.OutputGraphics.ThirdTry();
                    break;
                case 4:
                    OutputGraphics.OutputGraphics.ForthTry();
                    break;
                case 5:
                    OutputGraphics.OutputGraphics.FifthTry();
                    break;
            }
        }
    }
}
