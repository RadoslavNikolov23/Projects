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
            string word = WordsClass.GetRandomWord();
            int maxTries = 6;
            int countTries = 0;
            bool guestTheWord = false;

            char[] charWords = word.ToArray();
            char[] emptyCharArray = new char[charWords.Length];

            for (int i = 0; i < emptyCharArray.Length; i++) emptyCharArray[i] = '*';

            int attempsToWin = charWords.Length;
            int countToWin = 0;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"You have to guess a fruit, with the length of {word.Length} letters and have only 6 tries!\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Let's begin!");

            Console.ForegroundColor = ConsoleColor.White;
            Output.StartView();

            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Your word is: {string.Join("", emptyCharArray)}\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Enter a letter: ");

                    char guestSymbol = char.Parse(Console.ReadLine());

                    if (charWords.Contains(guestSymbol))
                    {
                        int indexAt = charWords.ToList().IndexOf(guestSymbol);
                        emptyCharArray[indexAt] = guestSymbol;
                        charWords[indexAt] = '*';
                        Console.WriteLine();
                        Console.WriteLine($"Congratulation you have found the letter {guestSymbol}");
                        countToWin++;

                    }
                    else
                    {
                        countTries++;
                        Console.WriteLine();
                        Console.WriteLine("Wrong letter!");
                        Console.WriteLine($"You have {6 - countTries} wrong tries left!");
                        ChechOutPut(countTries);
                        Console.WriteLine();
                        Console.WriteLine($"Try again!");
                    }

                    if (maxTries == countTries)
                    {
                        Output.MaxOut();
                        break;
                    }


                    if (attempsToWin == countToWin)
                    {
                        guestTheWord = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor= ConsoleColor.Cyan;
                    
                    Console.WriteLine($"\nWrong input!");
                    Console.WriteLine($"Enter ONLY one letter!\n");
                    Console.ForegroundColor = ConsoleColor.White;

                }


            }
        }

        public static void ChechOutPut(int number)
        {
            switch (number)
            {
                case 1:
                    Output.FirstTry();
                    break;
                case 2:
                    Output.SecondTry();
                    break;
                case 3:
                    Output.ThirdTry();
                    break;
                case 4:
                    Output.ForthTry();
                    break;
                case 5:
                    Output.FifthTry();
                    break;
            }
        }
    }
}
