using HangmanGame.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.IO
{
    public class Writer : IWriter
    {
        public void Write(string input) => Console.Write(input);
        
        public void WriteLine() => Console.WriteLine();
        public void WriteLine(string input) => Console.WriteLine(input);
    }
}
