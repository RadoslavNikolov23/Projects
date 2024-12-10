using HangmanGame.Engine.Contracts;
using HangmanGame.IO;
using HangmanGame.IO.Contracts;
using HangmanGame.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Engine
{
    public class Engine : IEngine
    {
        private IReader reader;
        private IWriter writer;
        private IGame gameOn;

        public Engine()
        {
            this.reader = new Reader();
            this.writer = new Writer();
            this.gameOn = new Game();
        }
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            writer.WriteLine("Let's play Hangman!\n");
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                gameOn.GameOn();

                writer.WriteLine("Do you want to play again: yes[y] or no[n]?");
                string input = reader.ReadLine().ToLower();

                if (input == "y" || input == "yes") 
                    continue;
                else if (input == "n" || input == "no")
                {
                    writer.WriteLine("GoodBye!");
                    break;
                }
                else
                {
                    writer.WriteLine("Wrong command. GoodBye!");
                    break;
                }
            }
        }
    }
}
