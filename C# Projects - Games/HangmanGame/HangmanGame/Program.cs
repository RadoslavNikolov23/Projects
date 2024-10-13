namespace HangmanGame;

public class Program
{
    enum Words
    {
        apple, banana, cherry, watermelon
    }

    public static string GetRandomWord()
    {
        Random randomWord = new Random();
        List<Words> word = Enum.GetValues(typeof(Words)).Cast<Words>().ToList();
        int randomIndex = randomWord.Next(word.Count);
        string wordForGame = word[randomIndex].ToString();
        return wordForGame;
    }

    public static void GameOn()
    {
        string word = GetRandomWord();
        int maxTries = 6;
        int countTries = 0;
        bool guestTheWord = false;

        char[] charWords = word.ToArray();
        char[] emptyCharArray = new char[charWords.Length];
     
        for(int i=0; i< emptyCharArray.Length; i++) emptyCharArray[i]='-';

        int attempsToWin = charWords.Length;
        int countToWin = 0;

        while (true)
        {
            Console.WriteLine("Let's Play a game of Hangman:");
            Console.WriteLine("Enter a symbol:");
            char guestSymbol = char.Parse(Console.ReadLine());

            if (charWords.Contains(guestSymbol))
            {
                int indexAt = charWords.ToList().IndexOf(guestSymbol);
                emptyCharArray[indexAt] = guestSymbol;
                charWords[indexAt]='-';
                Console.WriteLine($"Congratulation you have found the letter {guestSymbol}");
                Console.WriteLine(string.Join("",emptyCharArray));
                countToWin++;

            }
            else countTries++;

            if (maxTries == countTries)
            {
                Output.MaxOut();
                break;
            }


            if (attempsToWin== countToWin)
            {
                guestTheWord = true;
                break;
            }
        }



    }
    public static void Main()
    {
        GameOn();
    }
}
