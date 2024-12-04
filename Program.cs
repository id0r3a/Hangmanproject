using Spectre.Console;

namespace Hangmanproject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Kör spelet i en loop tills användaren vill avsluta
            bool playAgain = true;

            while (playAgain)
            {
                var game = new HangmanGame();
                game.ShowWelcomeMessage();

                // Starta en ny omgång av Hangman spelet
                game.PlayGame();

                // Fråga om användaren vill spela igen
                playAgain = AskPlayAgain();
            }

            AnsiConsole.MarkupLine("[bold blue]Thanks for playing![/]");
        }

        private static bool AskPlayAgain()
        {
            var menu = new SelectionPrompt<string>()
                .Title("Do you want to play again?")
                .AddChoices("Yes", "No");

            var result = AnsiConsole.Prompt(menu);
            return result == "Yes";
        }
    }
}
