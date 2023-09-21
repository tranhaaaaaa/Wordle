using System.Runtime.InteropServices;
using System;
namespace Game
{

    class Program
    {


        static void Main(string[] args)
        {

            string Name = Path.GetFileName("C:\\Program Files\\Words.txt");
            Console.WriteLine($"File: {Name}");
            Console.WriteLine("Author: Tran Quang Ha");
            Console.WriteLine("ID: HE161834");
            Console.WriteLine("Email ID: Tranha02");
            Console.WriteLine("This is my own work as defined by the University");
            Game game = new Game();

            while (true)
            {

                game.StartGame();

                Console.Write("Would you like to play again [y|n]? ");
                string playAgain = Console.ReadLine().ToLower();

                if (playAgain != "y")
                {
                    game.DisplaySummary();
                    break;
                }
            }
        }
    }

    class Game
    {
        static List<char> usedLetters = new List<char>();
        static List<char> CorrectLetters = new List<char>();
        private int totalGames;
        private int wordlesSolved;
        private int wordlesUnsolved;
        private List<string> userEnteredWords;
        
        public Game()
        {
            totalGames = 0;
            wordlesSolved = 0;
            wordlesUnsolved = 0;
            userEnteredWords = new List<string>();
        }
        public void StartGame()
        {
            usedLetters = new List<char>();
            CorrectLetters= new List<char>();
            totalGames++;
            Console.WriteLine("---------------------------------");
            Console.WriteLine("-- My Wordle! --");
            Console.WriteLine("-- Guess the Wordle in 6 tries --");
            Console.WriteLine("---------------------------------");
            bool validInput = false;
            do
            {
                
                Console.Write("Would you like to play My Wordle [y|n]? ");
                try
                {
                    string play = Console.ReadLine().ToLower();
                    if (play == "n")
                    {
                        Console.WriteLine("No worries... another time perhaps...");
                        return;

                    }
                    else if (play != "y")
                    {
                        Console.WriteLine("Please enter 'y' for yes or 'n' for no.");
                    }
                    else
                    {
                        validInput = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input. Please enter 'y' for yes or 'n' for no.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            } while (!validInput);
            Wordle wordle = new Wordle();
            wordle.WordGen();

            List<string> Notification = new List<string>();

            Console.WriteLine($"Wordle is: {wordle.TargetWord}\n");

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("-------------");
                string userWord = GetUserWord(i);
                userEnteredWords.Add(userWord.ToUpper());

                string Notice = CheckChar(wordle.TargetWord, userWord.ToUpper());

                DisplayGuess(userWord, Notice);

                string correctLetters = new string(CorrectLetters.ToArray());
                string usedLetterstring = new string(usedLetters.ToArray());
                DisplayNoticeDetails(Notice, correctLetters.ToUpper(), usedLetterstring.ToUpper());

                if (userWord.ToUpper() == wordle.TargetWord)
                {
                    if (i == 0)
                    {
                        Console.WriteLine("Solved in 1 try! Well done!");
                    }
                    else
                    {
                        Console.WriteLine("Complete! You've guessed the Wordle!");
                    }
                    wordlesSolved++;
                    break;
                }

                if (i == 6)
                {
                    Console.WriteLine("Oh no! Better luck next time!");
                    Console.WriteLine($"The Wordle was '{wordle.TargetWord}'.");
                    wordlesUnsolved++;
                }

                Notification.Add(Notice);
            }
        }



        public void DisplaySummary()
        {
            Console.WriteLine($"My Wordle Summary");
            Console.WriteLine("=================");
            Console.WriteLine($"You played {totalGames} games:");
            Console.WriteLine($"|--> Number of wordles solved: {wordlesSolved}");
            Console.WriteLine($"|--> Number of wordles unsolved: {wordlesUnsolved}");
            Console.WriteLine("Thanks for playing!");
        }

        private string GetUserWord(int count)
        {
            string? userWord ="" ;

            do
            {
                try
                {
                    Console.Write($"Please enter your guess - attempt {count + 1}: ");
                    userWord = Console.ReadLine().ToUpper();

                    if (userWord.Length != 5)
                    {
                        throw new Exception("Five letter words only please!");
                    }

                    if (!IsWordInList(userWord))
                    {
                        throw new Exception("Word not in the list");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);

            return userWord;
        }
        //private string GetUsedLetters(string userWord)
        //{



        //    foreach (string enteredWord in userEnteredWords)
        //    {

        //        foreach (char letter in enteredWord)
        //        {
        //            if (!usedLetters.Contains(letter.ToString()))
        //            {
        //                usedLetters += letter + " ";
        //                return usedLetters;
        //            }
        //        }
        //    }

        //    return usedLetters;
        //}
        private string CheckChar(string targetWord, string userGuess)
        {
            string isCheck = "";
            List<int> list = new List<int>();
            string checkTargetWord = targetWord;
            List<char> charList = checkTargetWord.ToCharArray().ToList();
            if (targetWord == userGuess)
            {
                isCheck = "^^^^^";
                foreach (char c in checkTargetWord)
                {

                    if (!CorrectLetters.Contains(c))
                        CorrectLetters.Add(c);

                }
                return isCheck;
            }
            for (int i = 0; i < targetWord.Length; i++)
            {
                if (targetWord[i] == userGuess[i] && !CorrectLetters.Contains(targetWord[i]))
                {
                    isCheck += "^";
                    list.Add(i);
                    charList[i] = '1';
                    usedLetters.Remove(targetWord[i]);
                    CorrectLetters.Add(targetWord[i]);

                    
                }
                else if (targetWord.Contains(userGuess[i]))
                {
                    isCheck += "*";
                    if (!usedLetters.Contains(userGuess[i]) && !CorrectLetters.Contains(userGuess[i]))
                        usedLetters.Add(userGuess[i]);
                }
                else
                {
                    if(!usedLetters.Contains(userGuess[i]))
                      usedLetters.Add(userGuess[i]);
                      isCheck += "-";
                }
            }
            for (int y = 0; y < targetWord.Length; y++)
            {
                if (list.Contains(y))
                {
                    continue;
                }

                if (charList.Contains(userGuess[y]))
                {
                    charList[charList.IndexOf(userGuess[y])] = '1';

                }
                else
                {
                    isCheck = isCheck.Substring(0, y) + "-" + isCheck.Substring(y + 1);
                }
            }

            return isCheck;
        }
        private bool IsWordInList(string word)
        {
            WordLa wds = new WordLa();
            string[] wordList = wds.word;

            return Array.Exists(wordList, w => w.ToLower() == word.ToLower());
        }

        private void DisplayGuess(string userWord, string result)
        {
            Console.Write("| ");
            for (int j = 0; j < 5; j++)
            {
                char letter = userWord[j];
                Console.Write($"{letter} ");
            }
            Console.WriteLine("|");

            Console.Write("| ");
            for (int j = 0; j < 5; j++)
            {
                char symbol = result[j];
                Console.Write($"{symbol} ");
            }
            Console.WriteLine("|");
        }

        private void DisplayNoticeDetails(string Notice, string correctLetters, string usedLetters)
        {
            Console.WriteLine("| Correct spot(^): " + CountChar(Notice, '^'));
            Console.WriteLine("| Wrong spot(): " + CountChar(Notice, '*'));
            Console.WriteLine("|");

            Console.WriteLine("| Correct letters: " + correctLetters);
            Console.WriteLine("| Used letters: " + usedLetters);
        }

        private int CountChar(string input, char target)
        {
            int count = 0;
            foreach (char c in input)
            {
                if (c == target)
                {
                    count++;
                }
            }
            return count;
        }
    }

    class Wordle
    {
        public string TargetWord { get; private set; }

        public void WordGen()
        {
            WordLa wds = new WordLa();
            string[] words = wds.word;
            Random random = new Random();
            int index = random.Next(words.Length);
            TargetWord = words[index].ToUpper();
        }
    }
}