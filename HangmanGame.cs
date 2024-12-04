using Figgle;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangmanproject
{
    public class HangmanGame
    {
        private string secretWord;
        private StringBuilder guessedWord;
        private HashSet<char> guessedLetters;
        private int maxTries;
        private int incorrectGuesses;

        public void ShowWelcomeMessage()
        {
            var welcomeText = FiggleFonts.Small.Render("Welcome to");
            var gameText = FiggleFonts.Small.Render("Hangman Game");

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[bold yellow]{welcomeText}[/]");
            AnsiConsole.MarkupLine($"[bold red]{gameText}[/]");

            AnsiConsole.MarkupLine("[green]Press any key to start the game...[/]");
            Console.ReadKey(true); // Vänta på att användaren trycker på en tangent för att starta spelet
        }

        public void PlayGame()
        {
            InitializeGame();

            while (incorrectGuesses < maxTries)
            {
                Console.Clear();
                DisplayGameStatus();
                DisplayHangman();

                char guess = GetUserGuess();
                if (guessedLetters.Contains(guess))
                {
                    AnsiConsole.MarkupLine("[yellow]You've already guessed this letter![/]");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                guessedLetters.Add(guess);

                if (secretWord.Contains(guess))
                {
                    UpdateGuessedWord(guess);
                }
                else
                {
                    incorrectGuesses++;
                }

                if (!guessedWord.ToString().Contains('_'))
                {
                    AnsiConsole.MarkupLine($"[bold green]Congratulations! You guessed the word:[/] {secretWord}");
                    PlayWinAnimation();
                    return;
                }
            }

            AnsiConsole.MarkupLine($"[bold red]Sorry, you've lost. The word was:[/] {secretWord}");
            PlayLossAnimation();
        }

        private void InitializeGame()
        {
            secretWord = GetRandomWord();
            guessedWord = new StringBuilder(new string('_', secretWord.Length));
            guessedLetters = new HashSet<char>();
            maxTries = 6;
            incorrectGuesses = 0;
        }

        private string GetRandomWord()
        {
            string[] words = { "coding", "csharp", "hangman", "game", "application" };
            Random random = new Random();
            return words[random.Next(words.Length)];
        }

        private void DisplayGameStatus()
        {
            AnsiConsole.MarkupLine($"[bold blue]The word:[/] {guessedWord}");
            AnsiConsole.MarkupLine($"[bold yellow]Left attempts:[/] {maxTries - incorrectGuesses}");
            AnsiConsole.MarkupLine($"[bold cyan]Guessed letters:[/] {string.Join(", ", guessedLetters)}");
        }

        private void DisplayHangman()
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

        private char GetUserGuess()
        {
            while (true)
            {
                AnsiConsole.MarkupLine("[bold green]Enter a letter:[/]");
                string input = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(input) && input.Length == 1 && char.IsLetter(input[0]))
                {
                    return char.ToLower(input[0]);
                }

                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a single letter.[/]");
            }
        }

        private void UpdateGuessedWord(char guess)
        {
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (secretWord[i] == guess)
                {
                    guessedWord[i] = guess;
                }
            }
        }

        private void PlayWinAnimation()
        {
            var winMessage = FiggleFonts.Big.Render("You Win!");
            AnsiConsole.MarkupLine($"[bold green]{winMessage}[/]");
            System.Threading.Thread.Sleep(1000);
        }

        private void PlayLossAnimation()
        {
            var lossMessage = FiggleFonts.Big.Render("Game Over");
            AnsiConsole.MarkupLine($"[bold red]{lossMessage}[/]");
            System.Threading.Thread.Sleep(1000);
        }
    }
}
