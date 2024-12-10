using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.IO.Contracts
{
    public interface IWriter
    {
        void Write(string input);
        void WriteLine();
        void WriteLine(string input);
    }
}
