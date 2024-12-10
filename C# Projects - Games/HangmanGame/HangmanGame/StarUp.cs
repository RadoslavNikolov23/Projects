namespace HangmanGame;

public class StarUp
{
    public static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Let's play Hangman!\n");
        Console.ForegroundColor = ConsoleColor.White;

        while (true)
        {
            GameClass.GameOn();

            Console.WriteLine("Do you want to play again: yes[y] or no[n]?");
            string input = Console.ReadLine().ToLower();

            if (input == "y" || input == "yes") continue;
            else if (input == "n" || input == "no")
            {
                Console.WriteLine("GoodBye!");
                break;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Wrong command. GoodBye!");
                Console.ForegroundColor = ConsoleColor.White;
                break;

            }
        }
    }
}
