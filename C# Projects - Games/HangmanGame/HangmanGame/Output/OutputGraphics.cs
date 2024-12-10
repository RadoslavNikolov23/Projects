using HangmanGame.IO.Contracts;
using HangmanGame.IO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangmanGame.Output;

namespace HangmanGame
{
    public class OutputGraphics: IOutputGraphics
    {
        private IWriter writer;

        public OutputGraphics()
        {
            this.writer = new Writer();
        }
        public void MaxOut()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
           // writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|     ( )     |");
            writer.WriteLine(@"|     /|\     |");
            writer.WriteLine(@"|    / | \    |");
            writer.WriteLine(@"|     /|\     |");
            writer.WriteLine(@"|    /   \    |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }

        public void FifthTry()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|     ( )     |");
            writer.WriteLine(@"|     /|\     |");
            writer.WriteLine(@"|    / | \    |");
            writer.WriteLine(@"|     /|      |");
            writer.WriteLine(@"|    /        |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void ForthTry()
        {
            Console.ForegroundColor = ConsoleColor.Red;
           // writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|     ( )     |");
            writer.WriteLine(@"|     /|\     |");
            writer.WriteLine(@"|    / | \    |");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void ThirdTry()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
           // writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|     ( )     |");
            writer.WriteLine(@"|     /|      |");
            writer.WriteLine(@"|    / |      |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void SecondTry()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
           // writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|     ( )     |");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void FirstTry()
        {
            Console.ForegroundColor = ConsoleColor.White;
           // writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|      |      |");
            writer.WriteLine(@"|     ( )     |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }

        public void StartView()
        {
            Console.ForegroundColor = ConsoleColor.White;
            //writer.WriteLine();
            writer.WriteLine(@"_______________");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|             |");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine(@"|_____________|");
            writer.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }


    }
}
