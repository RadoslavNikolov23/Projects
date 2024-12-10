using HangmanGame.EnumWords;
using HangmanGame.IO.Contracts;
using HangmanGame.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangmanGame.Engine.Contracts;
using HangmanGame.Output;

namespace HangmanGame.Engine
{
    public class Game:IGame
    {
        private IReader reader;
        private IWriter writer;
        private IOutputGraphics outputGraphics;
        private const int maxTries = 6;

        public Game()
        {
            reader = new Reader();
            writer = new Writer();
            outputGraphics = new OutputGraphics();
        }

        public void GameOn()
        {
            string word = WordEngine.GetRandomWord();
            int countTries = 0;
            bool guestTheWord = false;

            ChechWordType(word, maxTries);

            char[] charWords = word.ToArray();
            char[] emptyCharArray = new char[charWords.Length];

            List<char> guestLettersList = new List<char>();

            for (int i = 0; i < emptyCharArray.Length; i++) 
                emptyCharArray[i] = '*';

            int attempsToWin = charWords.Length;
            int countToWin = 0;

            writer.WriteLine("Let's begin!");
            outputGraphics.StartView();

            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    writer.WriteLine($"Your word is: {string.Join("", emptyCharArray)}");
                    Console.ForegroundColor = ConsoleColor.White;
                    writer.Write("Enter a letter: ");

                    char guestSymbol = char.Parse(reader.ReadLine());


                    if (charWords.Contains(guestSymbol) && char.IsLetter(guestSymbol))
                    {
                        int indexAt = charWords.ToList().IndexOf(guestSymbol);
                        emptyCharArray[indexAt] = guestSymbol;
                        charWords[indexAt] = '*';
                        writer.WriteLine($"\nCongratulation you have found the letter {guestSymbol}");
                        countToWin++;

                    }
                    else
                    {
                        countTries++;
                        writer.WriteLine("\nWrong letter!");
                        writer.WriteLine($"You have {maxTries - countTries} wrong tries left!");
                        ChechOutPut(countTries);
                        writer.WriteLine($"Try again!");
                    }

                    if (maxTries == countTries)
                    {
                        outputGraphics.MaxOut();
                        writer.WriteLine($"\nThe word was - {word}");
                        break;
                    }

                    if (attempsToWin == countToWin)
                    {
                        guestTheWord = true;
                        writer.WriteLine($"\nCongratulations you have fount the word: {word}");
                        break;
                    }

                    if (char.IsLetter(guestSymbol))
                    {
                        guestLettersList.Add(guestSymbol);
                        Console.ForegroundColor = ConsoleColor.Green;
                        writer.WriteLine($"You have tried the following letters:");
                        writer.WriteLine(string.Join(", ", guestLettersList));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        writer.WriteLine("Enter a letter!\n");
                    
                }
                catch (Exception ex)
                {
                    writer.WriteLine($"Wrong input!");
                    writer.WriteLine($"Enter ONLY one letter!\n");
                }

            }
        }

        private void ChechWordType(string word, int maxTries)
        {
            FruitWords[] wordToMatch = Enum.GetValues<FruitWords>();
            bool isFruit = false;
            foreach (var wordWord in wordToMatch)
            {
                if (wordWord.ToString().ToLower() == word)
                {
                    isFruit = true;
                    break;
                }
            }

            string typeWord = isFruit ? "Fruit" : "Car Brand";

            writer.WriteLine($"You have to guess a {typeWord}, with the length of {word.Length} letters and have only {maxTries} tries!\n");
        }

        public void ChechOutPut(int number)
        {
            switch (number)
            {
                case 1:
                    outputGraphics.FirstTry();
                    break;
                case 2:
                    outputGraphics.SecondTry();
                    break;
                case 3:
                    outputGraphics.ThirdTry();
                    break;
                case 4:
                    outputGraphics.ForthTry();
                    break;
                case 5:
                    outputGraphics.FifthTry();
                    break;
            }
        }
    }
}
