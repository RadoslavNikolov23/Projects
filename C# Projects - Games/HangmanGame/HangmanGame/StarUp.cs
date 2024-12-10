using HangmanGame;
using HangmanGame.Engine.Contracts;

namespace HangmanGame.Engine;

public class StarUp
{
    public static void Main()
    {
        IEngine engine = new Engine();
        engine.Run();
    }
}
