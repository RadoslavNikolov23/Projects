using HangmanGame.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.IO
{
    public class Reader : IReader
    {
        public string ReadLine() => Console.ReadLine();
    }
}
