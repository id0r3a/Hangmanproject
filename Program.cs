using Figgle;
using Spectre.Console;
using System;
using System.Text;

namespace Hangman
{
    class Hangman
    {
        static void Main(string[] args)
        {
            // Kör spelet i en loop tills användaren vill avsluta
            bool playAgain = true;
            bool firstRound = true;  // Flagga för att kolla om det är första omgången

            while (playAgain)
            {
                if (firstRound)
                {
                    // Skapa en logga
                    var welcomeText = FiggleFonts.Small.Render("Welcome to");
                    var gameText = FiggleFonts.Small.Render("Hangman Game");

                    // Visa logotypen
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[bold yellow]{welcomeText}[/]"); // Visa "Welcome to" i gul
                    AnsiConsole.MarkupLine($"[bold red]{gameText}[/]"); // Visa "Hangman Game" i röd

                    // Vänta lite så användaren hinner se logotypen
                    AnsiConsole.MarkupLine("[green]Press any key to start the game...[/]");
                    Console.ReadKey(true); // Vänta på att användaren trycker på en tangent för att starta spelet

                    firstRound = false;  // Efter första omgången, sätt flaggan till false
                }

                // Ordlistan med möjliga ord att gissa
                string[] words = { "coding", "csharp", "hangman", "game", "application" };

                // Välj ett slumpmässigt ord från listan
                Random random = new Random();
                string secretWord = words[random.Next(words.Length)];

                // Skapa en array för att hålla koll på de gissade bokstäverna
                char[] guessedWord = new string('_', secretWord.Length).ToCharArray();

                // Variabler för försök och max antal försök
                int maxTries = 6;
                int incorrectGuesses = 0;

                // Lista med tidigare gissade bokstäver
                StringBuilder guessedLetters = new StringBuilder();

                // Spelloop
                while (incorrectGuesses < maxTries)
                {
                    Console.Clear(); // Rensa skärmen varje gång vi går vidare i spelet

                    // Visa ordet med de gissade bokstäverna
                    AnsiConsole.MarkupLine($"[bold blue]The word:[/] {new string(guessedWord)}");

                    // Visa försök kvar
                    AnsiConsole.MarkupLine($"[bold yellow]Left attempts:[/] {maxTries - incorrectGuesses}");

                    // Visa gissade bokstäver
                    AnsiConsole.MarkupLine($"[bold cyan]Guessed letters:[/] {guessedLetters}");

                    // Visa hangman-bilden
                    DisplayHangman(incorrectGuesses);

                    // Läs in spelarens gissning (en hel rad med en bokstav)
                    AnsiConsole.MarkupLine("[bold green]Enter a letter:[/]");
                    string input = Console.ReadLine()!; // Spelaren skriver en bokstav och trycker Enter

                    // Kontrollera om användaren angav en giltig input
                    if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                    {
                        AnsiConsole.MarkupLine("[red]Invalid input. Please enter a single letter.[/]");
                        continue;
                    }

                    char guess = Char.ToLower(input[0]);

                    // Kolla om bokstaven redan är gissad
                    if (guessedLetters.ToString().Contains(guess))
                    {
                        // Visa kort meddelande om bokstaven redan är gissad
                        AnsiConsole.MarkupLine("[yellow]You've already guessed this letter![/]");
                        System.Threading.Thread.Sleep(1000); // Vänta en sekund innan nästa input
                        continue;
                    }

                    // Lägg till bokstaven till listan med gissade bokstäver
                    guessedLetters.Append(guess);

                    // Kontrollera om gissningen finns i ordet
                    bool correctGuess = false;
                    for (int i = 0; i < secretWord.Length; i++)
                    {
                        if (secretWord[i] == guess)
                        {
                            guessedWord[i] = guess;
                            correctGuess = true;
                        }
                    }

                    // Om gissningen var felaktig, öka antalet misslyckade gissningar
                    if (!correctGuess)
                    {
                        incorrectGuesses++;
                    }

                    // Kontrollera om spelaren har vunnit
                    if (!guessedWord.Contains('_'))
                    {
                        // Visa vinstanimation
                        AnsiConsole.MarkupLine("[bold green]Congratulations! You guessed the word:[/] " + secretWord);
                        PlayWinAnimation();
                        break;
                    }
                }

                // Om spelaren förlorade
                if (incorrectGuesses == maxTries)
                {
                    // Visa förlustanimation
                    Console.Clear();
                    AnsiConsole.MarkupLine("[bold red]Sorry, you've lost. The word was:[/] " + secretWord);
                    PlayLossAnimation();
                }

                // Fråga om användaren vill spela igen
                playAgain = AskPlayAgain();
            }

            AnsiConsole.MarkupLine("[bold blue]Thanks for playing![/]");
        }

        // Metod för att visa hangman-figur beroende på antalet misslyckade gissningar
        static void DisplayHangman(int incorrectGuesses)
        {
            var hangmanArt = new string[]
            {
                @" ______ 
 |      | 
 |      
 |      
 |      
 |______ ",
                @" ______ 
 |      | 
 |      O 
 |      
 |      
 |______ ",
                @" ______ 
 |      | 
 |      O 
 |      | 
 |      
 |______ ",
                @" ______ 
 |      | 
 |      O 
 |     /| 
 |      
 |______ ",
                @" ______ 
 |      | 
 |      O 
 |     /|\ 
 |      
 |______ ",
                @" ______ 
 |      | 
 |      O 
 |     /|\ 
 |     / 
 |______ ",
                @" ______ 
 |      | 
 |      O 
 |     /|\ 
 |     / \ 
 |______ "
            };

            Console.WriteLine(hangmanArt[incorrectGuesses]);
        }

        // Metod för att spela vinstanimation
        static void PlayWinAnimation()
        {
            var winMessage = FiggleFonts.Big.Render("You Win!");
            AnsiConsole.MarkupLine($"[bold green]{winMessage}[/]");
            System.Threading.Thread.Sleep(1000); // Vänta en sekund för att låta meddelandet sjunka in
        }

        // Metod för att spela förlustanimation
        static void PlayLossAnimation()
        {
            var lossMessage = FiggleFonts.Big.Render("Game Over");
            AnsiConsole.MarkupLine($"[bold red]{lossMessage}[/]");
            System.Threading.Thread.Sleep(1000); // Vänta en sekund för att låta meddelandet sjunka in
        }

        // Fråga om användaren vill spela igen
        static bool AskPlayAgain()
        {
            var menu = new SelectionPrompt<string>()
                .Title("Do you want to play again?")
                .AddChoices("Yes", "No");

            var result = AnsiConsole.Prompt(menu);
            return result == "Yes";
        }
    }
}
