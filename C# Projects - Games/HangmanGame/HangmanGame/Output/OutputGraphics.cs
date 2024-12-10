using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.OutputGraphics
{
    public class OutputGraphics
    {

        public static void MaxOut()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|     ( )     |");
            Console.WriteLine(@"|     /|\     |");
            Console.WriteLine(@"|    / | \    |");
            Console.WriteLine(@"|     /|\     |");
            Console.WriteLine(@"|    /   \    |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void FifthTry()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|     ( )     |");
            Console.WriteLine(@"|     /|\     |");
            Console.WriteLine(@"|    / | \    |");
            Console.WriteLine(@"|     /|      |");
            Console.WriteLine(@"|    /        |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void ForthTry()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|     ( )     |");
            Console.WriteLine(@"|     /|\     |");
            Console.WriteLine(@"|    / | \    |");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void ThirdTry()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|     ( )     |");
            Console.WriteLine(@"|     /|      |");
            Console.WriteLine(@"|    / |      |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void SecondTry()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|     ( )     |");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static void FirstTry()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|      |      |");
            Console.WriteLine(@"|     ( )     |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void StartView()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine(@"_______________");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|             |");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine(@"|_____________|");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }


    }
}
