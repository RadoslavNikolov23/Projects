using HangmanGame.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame.Output
{
    public interface IOutputGraphics
    {
        public void MaxOut();

        public void FifthTry();

        public void ForthTry();

        public void ThirdTry();
        
        public void SecondTry();

        public void FirstTry();

        public void StartView();
    }
}
