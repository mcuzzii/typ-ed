using System;
using System.Collections.Generic;
using System.Linq;

namespace Monke
{
    class Program
    {
        static void Main()
        {
            List<Game> games = new List<Game>();
            bool quitGame = false;
            do
            {
                Console.Clear();
                printCenter(Art.comp, ConsoleColor.Blue);
                printCenter(Art.startscreen);
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        bool backToMainMenu = false;
                        ConsoleKeyInfo menuKey = Console.ReadKey(true);
                        if (menuKey.Key == ConsoleKey.D1)
                        {
                            string subject = "";
                            string difficulty = "";
                            Console.Clear();
                            printCenter(Art.subjheader, ConsoleColor.Blue);
                            printCenter(Art.subjectscreen);
                            while (true)
                            {
                                if (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo subjectKey = Console.ReadKey(true);
                                    switch (subjectKey.Key)
                                    {
                                        case ConsoleKey.D1:
                                            subject = "Filipino";
                                            break;
                                        case ConsoleKey.D2:
                                            subject = "Natural Science";
                                            break;
                                        case ConsoleKey.D3:
                                            subject = "English";
                                            break;
                                        case ConsoleKey.D4:
                                            subject = "Social Sciences";
                                            break;
                                        case ConsoleKey.Backspace:
                                            backToMainMenu = true;
                                            break;
                                        default:
                                            continue;
                                    }
                                    break;
                                }
                            }
                            if (backToMainMenu) break;
                            Console.Clear();
                            printCenter(Art.diffheader, ConsoleColor.Blue);
                            printCenter(Art.difficultyscreen);
                            while (true)
                            {
                                if (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo difficultyKey = Console.ReadKey(true);
                                    switch (difficultyKey.Key)
                                    {
                                        case ConsoleKey.D1:
                                            difficulty = "Easy";
                                            break;
                                        case ConsoleKey.D2:
                                            difficulty = "Medium";
                                            break;
                                        case ConsoleKey.D3:
                                            difficulty = "Hard";
                                            break;
                                        case ConsoleKey.Backspace:
                                            backToMainMenu = true;
                                            break;
                                        default:
                                            continue;
                                    }
                                    break;
                                }
                            }
                            if (backToMainMenu) break;
                            ConsoleColor artColor = ConsoleColor.White;
                            string gameArt = "";
                            string gameHeader = "";
                            switch (subject)
                            {
                                case "Filipino":
                                    artColor = ConsoleColor.DarkRed;
                                    gameArt = Art.phflag;
                                    gameHeader = Art.filheader;
                                    break;
                                case "Natural Science":
                                    artColor = ConsoleColor.DarkGreen;
                                    gameArt = Art.natsci;
                                    gameHeader = Art.natsciheader;
                                    break;
                                case "English":
                                    artColor = ConsoleColor.DarkBlue;
                                    gameArt = Art.eng;
                                    gameHeader = Art.engheader;
                                    break;
                                case "Social Sciences":
                                    artColor = ConsoleColor.DarkYellow;
                                    gameArt = Art.socsci;
                                    gameHeader = Art.socsciheader;
                                    break;
                            }
                            while (true)
                            {
                                Console.Clear();
                                printCenter(Art.loadingscreen);
                                string loadingBar = "  Loading...                                                                                          ";
                                Console.WriteLine();
                                (int, int) cursorPos = ((Console.WindowWidth - loadingBar.Length) / 2 + 13, Console.CursorTop );
                                printCenter(loadingBar);
                                Console.WriteLine();
                                printCenter(gameArt, artColor);
                                Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);
                                for (int t = 0; t < loadingBar.Length - 13; t++)
                                {
                                    Console.Write("█");
                                    System.Threading.Thread.Sleep((int)(4000.0 / (loadingBar.Length - 13)));
                                }
                                System.Threading.Thread.Sleep(250);
                                Console.Clear();
                                printCenter(gameHeader, artColor);
                                Console.WriteLine();
                                Game game = new Game(subject, difficulty);
                                string text = game.text;
                                string[] words = text.Split(' ');
                                for (int i = 0; i < words.Length; i++) if (i < words.Length - 1) words[i] += " ";
                                printCenter("█============█ Type this text! █====================================================================█");
                                string borders = "█                                                                                                   █";
                                Console.SetCursorPosition((Console.WindowWidth - borders.Length) / 2, Console.CursorTop);
                                Console.Write(borders);
                                int[] newlineFlags = WrapText(words, borders);
                                Console.WriteLine();
                                printCenter(borders);
                                printCenter("█===================================================================================================█");
                                Console.WriteLine();
                                int[] typeTracker = new int[2] { (Console.WindowWidth - borders.Length) / 2 + 5, Console.CursorTop - newlineFlags.Sum() - 3 };
                                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                                time.Start();
                                for (int i = 0; i < words.Length; i++)
                                {
                                    Console.SetCursorPosition((Console.WindowWidth - borders.Length) / 2, Console.CursorTop);
                                    Console.Write("Output: ");
                                    string input = "";
                                    while (true)
                                    {
                                        if (Console.KeyAvailable)
                                        {
                                            ConsoleKeyInfo key = Console.ReadKey(true);
                                            if (Char.IsLetterOrDigit(key.KeyChar) || Char.IsPunctuation(key.KeyChar) || Char.IsWhiteSpace(key.KeyChar))
                                            {
                                                string character = key.KeyChar.ToString();
                                                input += character;
                                                Console.Write(character);
                                                if (words[i].StartsWith(input))
                                                {
                                                    cursorPos = Console.GetCursorPosition();
                                                    Console.SetCursorPosition(typeTracker[0], typeTracker[1]);
                                                    Console.ForegroundColor = artColor;
                                                    Console.Write(key.KeyChar);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    typeTracker[0]++;
                                                    Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);
                                                }
                                            }
                                            else if (key.Key == ConsoleKey.Backspace && input.Length != 0)
                                            {
                                                Console.Write("\b \b");
                                                if (words[i].StartsWith(input))
                                                {
                                                    cursorPos = Console.GetCursorPosition();
                                                    Console.SetCursorPosition(typeTracker[0] - 1, typeTracker[1]);
                                                    Console.Write(input.Last());
                                                    typeTracker[0]--;
                                                    Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);
                                                }
                                                input = input.Remove(input.Length - 1, 1);
                                            }
                                            if (input == words[i])
                                            {
                                                Console.Write(new string ('\b', input.Length) + new string(' ', input.Length));
                                                if (i < words.Length - 1)
                                                {
                                                    if (newlineFlags[i + 1] == 1)
                                                    {
                                                        typeTracker[0] = (Console.WindowWidth - borders.Length) / 2 + 5;
                                                        typeTracker[1]++;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                time.Stop();
                                game.wpm = (int)(words.Length / (time.ElapsedMilliseconds / 60000.0));
                                Console.Clear();
                                printCenter(Art.quizheader, artColor);
                                Console.WriteLine();
                                string wpmLine = "  WPM:                                                                                                ";
                                cursorPos = ((Console.WindowWidth - wpmLine.Length) / 2 + 7, Console.CursorTop);
                                printCenter(wpmLine);
                                Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);
                                Console.Write(game.wpm);
                                System.Threading.Thread.Sleep(1500);
                                Console.SetCursorPosition(0, Console.CursorTop + 1);
                                Console.WriteLine();
                                printCenter("█============█ Bonus question! █====================================================================█");
                                Console.SetCursorPosition((Console.WindowWidth - borders.Length) / 2, Console.CursorTop);
                                Console.Write(borders);
                                string[] questionWords = game.quizQuestion.Split(' ');
                                for (int i = 0; i < questionWords.Length; i++) if (i < questionWords.Length - 1) questionWords[i] += " ";
                                WrapText(questionWords, borders);
                                Console.WriteLine();
                                printCenter(borders);
                                string[] choiceButtons = new string[4] { " (Press A)", " (Press B)", " (Press C)", " (Press D)" };
                                for (int i = 0; i < 4; i++)
                                {
                                    cursorPos = ((Console.WindowWidth - borders.Length) / 2 + 9, Console.CursorTop);
                                    printCenter(borders);
                                    Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);
                                    Console.WriteLine("█ " + game.choices[i] + choiceButtons[i]);
                                }
                                printCenter(borders);
                                printCenter("█===================================================================================================█");
                                while (true)
                                {
                                    if (Console.KeyAvailable)
                                    {
                                        string choiceKey = Console.ReadKey(true).KeyChar.ToString();
                                        if (choiceKey != "a" && choiceKey != "b" && choiceKey != "c" && choiceKey != "d") continue;
                                        game.result = choiceKey == game.quizAnswer? 1 : 0;
                                        break;
                                    }
                                }
                                games.Add(game);
                                Console.Clear();
                                if (game.result == 1)
                                {
                                    printCenter(Art.congratulations1, artColor);
                                    printCenter(Art.congratulations2);
                                }
                                else
                                {
                                    printCenter(Art.answerwrong1, artColor);
                                    printCenter(Art.answerwrong2);
                                }
                                System.Threading.Thread.Sleep(1500);
                                printCenter(Art.playagainmenu);
                                bool playAgain = false;
                                while (true)
                                {
                                    if (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo playAgainKey = Console.ReadKey(true);
                                        if (playAgainKey.Key == ConsoleKey.D1) playAgain = true;
                                        else if (playAgainKey.Key == ConsoleKey.Backspace) backToMainMenu = true;
                                        else continue;
                                        break;
                                    }
                                }
                                if (!playAgain) break;
                            }
                            if (backToMainMenu) break;
                        }
                        else if (menuKey.Key == ConsoleKey.D2)
                        {
                            Console.Clear();
                            printCenter(Art.statsmenu);
                            Console.WriteLine();
                            string topEdge = "█============█ Summary statistics █=================================================================█";
                            string borders = "█                                                                                                   █";
                            if (games.Count == 0)
                            {
                                System.Threading.Thread.Sleep(2000);
                                Console.SetCursorPosition((Console.WindowWidth - topEdge.Length) / 2, Console.CursorTop);
                                Console.WriteLine("...Uhh, well this is awkward. Seems like you haven't played a game yet!");
                            }
                            else
                            {
                                printCenter(topEdge);
                                printCenter(borders);
                                string[] strings = new string[3] {
                                    "█ Number of games played: " + games.Count,
                                    "█ Average WPM: " + Math.Round(games.Average(item => item.wpm), 2),
                                    "█ Correctly answered quizzes: " + games.Sum(item => item.result) + " / " + games.Count
                                };
                                for (int i = 0; i < 3; i++)
                                {
                                    Console.SetCursorPosition((Console.WindowWidth - topEdge.Length) / 2, Console.CursorTop);
                                    Console.Write(borders);
                                    Console.SetCursorPosition((Console.WindowWidth - topEdge.Length) / 2 + 5, Console.CursorTop);
                                    Console.WriteLine(strings[i]);
                                }
                                printCenter(borders);
                                printCenter("█===================================================================================================█");
                                Console.WriteLine();
                                Console.SetCursorPosition((Console.WindowWidth - topEdge.Length) / 2, Console.CursorTop);
                                Console.WriteLine("Game #\tSubject\t\t\tDifficulty level\tWPM\t\tBonus quiz result");
                                Console.WriteLine();
                                for(int i = 0; i < games.Count; i++)
                                {
                                    string space = games[i].subject.Length >= 8 ? "\t\t" : "\t\t\t";
                                    string quiz = games[i].result == 1 ? "Correct" : "Wrong";
                                    Console.SetCursorPosition((Console.WindowWidth - topEdge.Length) / 2, Console.CursorTop);
                                    Console.WriteLine(i + 1 + "\t\t" + games[i].subject + space + games[i].difficulty + "\t\t\t" + games[i].wpm + "\t\t" + quiz);
                                }
                            }
                            Console.WriteLine();
                            Console.SetCursorPosition((Console.WindowWidth - topEdge.Length) / 2, Console.CursorTop);
                            Console.WriteLine("<< Back to Main Menu (Press Backspace)");
                            while (true) if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Backspace) break;
                            break;
                        }
                        else if (menuKey.Key == ConsoleKey.Backspace)
                        {
                            quitGame = true;
                            break;
                        }
                    }
                }
            } while (!quitGame);
            Console.Clear();
        }
        static void printCenter(string str, ConsoleColor color = ConsoleColor.White)
        {
            System.IO.StringReader reader = new System.IO.StringReader(str);
            string line;
            Console.ForegroundColor = color;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                    Console.WriteLine(line);
                }
            } while (line != null);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static int[] WrapText(string[] words, string borders)
        {
            int[] newlineFlags = new int[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                newlineFlags[i] = 0;
                if (words[i].Length > (Console.WindowWidth + borders.Length) / 2 - Console.CursorLeft - 5 )
                {
                    newlineFlags[i] = 1;
                    Console.SetCursorPosition((Console.WindowWidth - borders.Length) / 2, Console.CursorTop + 1);
                    Console.Write(borders);
                    Console.SetCursorPosition((Console.WindowWidth - borders.Length) / 2 + 5, Console.CursorTop);
                }
                Console.Write(words[i]);
            }
            return newlineFlags;
        }
    }
    public class Game
    {
        public string subject;
        public string difficulty;
        public string text;
        public int wpm;
        public string quizQuestion;
        public string[] choices = new string[4];
        public string quizAnswer;
        public int result;
        public Game(string subj, string diff)
        {
            subject = subj;
            difficulty = diff;
            Random random = new Random();
            int textIndex = random.Next(1, 3);
            string[] tsvLines = System.IO.File.ReadAllLines("Dataset.tsv");
            for (int i = 1; i < tsvLines.Length; i++)
            {
                string[] dataRow = tsvLines[i].Split('\t');
                if (dataRow[0] == subject && dataRow[1] == difficulty) textIndex--;
                if (textIndex == 0)
                {
                    text = dataRow[2];
                    quizQuestion = dataRow[3];
                    for (int j = 0; j < 4; j++) choices[j] = dataRow[4 + j];
                    quizAnswer = dataRow[8];
                    break;
                }
            }
        }
    }
    public static class Art
    {
        public static string comp = @"
            ████████████████████████████████████████████████████████                                  
            ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██                                  
            ██▒▒██████████████████▓▓▓▓██████████████████████████▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██                                  
            ██▒▒██▒        ▒  ▒▒▒  ▒    ▒▒▒▒▒▒▒     ▒     ▒▒▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒  ▒▒▒▒▒  ▒  ▒▒  ▒▒  ▒▒▒▒▒  ▒▒▒▒  ▒▒▒  ▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒  ▒▒▒▒▒▒   ▒▒▒    ▒▒▒   ▒     ▒  ▒▒▒  ▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒  ▒▒▒▒▒▒   ▒▒▒  ▒▒▒▒▒▒▒▒▒  ▒▒▒▒  ▒▒▒  ▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒  ▒▒▒▒▒▒   ▒▒▒  ▒▒▒▒▒▒▒▒▒     ▒     ▒▒▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██                                  
            ██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██                                  
            ██▒▒████████████████████████████████████████████████▒▒██                                  
            ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██                                  
            ████████████████████████████████████████████████████▓▓▓▓                                  
          ████▒▒░░░░░░░░░░░░░░░░░░▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██▒▒                                
        ████░░░░██░░██░░██░░██░░▓▓░░██░░██░░██░░██░░██░░██░░██░░▓▓░░████████████████████████          
      ████▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░████                ████        
    ████▒▒░░░░██░░██░░██░░██░░██░░██░░██░░██░░██░░██░░██░░██░░██░░██░░░░████                ████      
  ████▒▒░░░░░░░░░░░░░░░░░░▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░░░░░▒▒████                ██      
████░░░░░░██▓▓▓▓░░██░░██▒▒░░▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓████▓▓██░░▓▓▓▓░░██░░██▓▓▓▓░░░░████              ██      
██▒▒░░░░░░▒▒▒▒▒▒░░░░░░░░▒▒░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒░░░░▒▒▒▒░░▒▒░░▒▒▒▒▒▒░░░░▒▒██              ██      
██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓██          ██▓▓██▓▓██  
                                                                                        ██▒▒▒▒██▒▒▒▒██
                                                                                        ██▒▒██████▒▒██
                                                                                        ██████░░██████
                                                                                        ██░░░░░░░░░░██
                                                                                        ██░░░░░░░░░░██
                                                                                        ████░░░░░░████
                                                                                        ██████▓▓██  ";
        public static string phflag = @"                                              
                                                                                          
                                            ░░                                          
                                                                                          
                                ▓▓▒▒▒▒                                                
                                ▓▓▓▓▒▒▓▓    ▓▓                                        
                                ▒▒▒▒▒▒▓▓▒▒▓▓▓▓░░                                        
                                ▒▒▒▒▒▒▓▓▓▓▓▓▓▓                                          
                                ▓▓▓▓▓▓    ▓▓▓▓                                          
                                ▒▒▓▓░░▒▒▒▒▓▓▓▓▒▒                                        
                                ▓▓▓▓░░    ▓▓▓▓▓▓░░                                      
                                ▓▓▒▒░░▒▒▒▒▓▓▓▓▓▓░░                                      
                                ▓▓░░░░░░▓▓▓▓▓▓▓▓                                        
                                ▒▒  ▓▓▓▓▓▓▓▓▒▒░░                                        
                        ▓▓  ░░    ▓▓▓▓▓▓▓▓▒▒░░                                        
                        ▓▓▓▓░░▓▓▓▓▒▒▒▒▓▓▒▒                                            
                            ▒▒▓▓▓▓▓▓▒▒▒▒▒▒                                              
                            ▒▒▒▒░░▒▒▒▒▒▒▒▒                                              
                            ▒▒░░░░▒▒▒▒▒▒░░                                              
                            ▒▒░░░░▒▒▒▒▒▒                                                
                            ▒▒▒▒░░▒▒▒▒▒▒                                                
                            ░░▒▒░░░░▒▒▒▒▒▒  ▒▒                                          
                            ▒▒▒▒  ▒▒▒▒▒▒  ▒▒                                          
                                ▒▒  ▒▒░░▒▒                                              
                                ▒▒▒▒  ▒▒░░    ▒▒▒▒                                    
                                ▒▒  ░░░░▒▒  ▒▒▒▒▒▒▒▒                                  
                                    ▒▒▒▒▒▒  ▒▒▒▒▒▒▓▓▓▓  ▓▓▓▓  ▒▒▒▒                      
                                    ▒▒      ▒▒▒▒  ▓▓▒▒▓▓    ▓▓▒▒                      
                                ▒▒▒▒▒▒      ▒▒  ░░    ▓▓▓▓▓▓                            
                                ▒▒▒▒▒▒░░              ▓▓▓▓░░                          
                                ░░▒▒▒▒░░            ░░  ▓▓▒▒▒▒                        
                                    ▒▒▒▒░░                  ▒▒▒▒                        
                                    ▒▒▒▒▒▒                ▓▓░░                          
                                    ▒▒▒▒                ▒▒      ▒▒░░░░▒▒                
                            ▒▒            ░░░░░░░░░░    ▒▒▒▒▒▒    ▓▓▒▒▓▓▓▓              
                            ░░                      ▒▒    ▒▒      ▓▓▓▓▓▓              
                            ▒▒              ░░  ░░                  ▒▒▓▓▓▓              
                                            ░░░░░░                ░░▓▓▓▓              
                                            ░░▒▒▒▒▒▒          ▓▓  ▓▓▓▓▓▓▓▓            
                    ░░            ░░▒▒░░▒▒░░▒▒▓▓▓▓▓▓          ░░▒▒▒▒  ░░░░            
                                                ▓▓▓▓  ▒▒▓▓  ▒▒      ▓▓                  
            ▒▒░░░░░░▒▒▒▒░░                      ░░▓▓▓▓▓▓  ██      ▓▓▓▓                
                    ░░▒▒                            ▓▓▓▓  ▒▒██      ▓▓▓▓                
                ░░▒▒                              ▓▓▓▓  ██        ▓▓░░                
                ▒▒▒▒                              ▓▓██▒▒    ░░░░                      
                ▒▒                              ▓▓▓▓██  ██  ░░░░░░    ░░░░▒▒▒▒▒▒▒▒      
            ▒▒                                  ▓▓██▒▒▒▒                ▒▒▒▒          
        ▒▒▒▒                                      ████  ▒▒              ▒▒▓▓▒▒▒▒▒▒▒▒▒▒
        ▒▒▒▒                                          ░░                  ░░▓▓▒▒        
    ░░▒▒                                                          ▒▒░░▓▓▒▒██▒▒▒▒      
    ▒▒                                                ▓▓▓▓    ▒▒  ▓▓▓▓██████▓▓        
                                ▒▒░░░░░░░░  ░░░░    ▒▒▓▓▓▓    ▓▓▓▓████████████▒▒      
                                                    ▓▓▓▓▓▓    ▓▓▓▓████▓▓██████▓▓      
░░                                            ▓▓▓▓▓▓▒▒░░▓▓▓▓▓▓▓▓████████████████      
░░                                          ▓▓▓▓▒▒▓▓░░░░░░▓▓▒▒▓▓▒▒██▓▓▓▓▓▓▓▓▓▓██      
                                            ▓▓▒▒  ▒▒░░      ▓▓▒▒▓▓▓▓██▓▓██████████    
                                            ▒▒    ░░          ░░▒▒▒▒▒▒██▓▓▒▒██████    
                                            ░░                ░░░░▓▓▓▓▓▓▓▓██░░▓▓██▒▒    
    ░░                                                ░░░░░░▒▒▒▒▒▒▒▒▓▓▓▓██              
░░░░                                    ░░▒▒▒▒              ▓▓▓▓▓▓▒▒▓▓██              
░░░░░░░░                                                      ▓▓▒▒▒▒▓▓▒▒████            
░░░░░░░░                          ░░                            ▒▒▓▓▓▓▓▓██▓▓            
░░░░░░░░░░░░                        ░░                              ██  ██▓▓            
░░░░░░░░░░░░░░░░                                                        ▓▓              
░░░░░░░░░░░░░░░░░░  ░░                                                                  
░░░░░░░░░░░░░░░░░░░░░░      ░░                                                          
░░░░░░░░░░░░░░░░░░░░    ░░▒▒░░                                                          
░░░░░░░░░░░░░░                                                                          ";
        public static string natsci = @"                      ░░                                                          
                    ░░░░░░            ░░                                          
                ░░░░░░░░░░        ░░░░░░░░                ░░░░                  
                    ░░░░░░            ░░░░                ░░░░░░░░                
                    ░░░░░░            ░░                  ░░░░░░░░                
                    ░░                                ░░░░░░░░░░░░              
                                                        ░░░░░░░░░░░░              
        ████████████████████████████████████████████████████▓▓▒▒████████        
        ██  ░░░░░░░░░░  ░░░░░░░░░░░░░░▒▒▒▒▒▒░░░░░░░░░░░░░░▒▒▒▒░░░░░░░░██        
        ██                          ░░░░░░░░░░            ░░░░        ██        
        ██          ░░              ░░░░░░░░░░                        ██        
        ██        ░░░░            ░░░░░░░░░░░░░░                      ██        
        ██          ░░          ░░░░░░░░░░░░░░░░░░                    ██        
        ████▓▓▒▒                  ░░░░░░░░░░░░░░          ░░░░    ▓▓▓▓██        
            ▓▓▓▓                    ░░░░░░░░░░            ░░░░░░  ██            
            ████▓▓██                  ░░░░░░      ░░      ░░░░▓▓████            
                    ██                    ░░      ░░░░░░        ██                
                    ██                          ░░░░░░░░░░      ██                
                    ██          ░░░░              ░░░░          ██                
                    ██▒▒    ░░░░░░░░░░              ░░  ▒▒    ▒▒██                
                    ██▒▒▒▒░░░░░░░░░░░░░░        ▒▒▒▒  ▒▒▒▒▒▒▒▒▒▒██                
                ░░████▒▒▒▒░░░░░░░░░░░░░░▒▒    ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████              
                ░░██▒▒▒▒▒▒▒▒░░░░░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██              
            ▓▓▓▓██▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒██▓▓            
            ▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██            
            ████▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒████          
            ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▒▒██          
        ████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒████        
        ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██        
        ▓▓██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██▓▓      
        ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██      
    ▓▓██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒██▓▓    
    ██▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▓▓██    
    ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▒▒██    
    ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██    
    ████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒████  
    ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██  
▓▓██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒██▒▒
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒████
██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▒▒██
██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▒▒▒▒██
██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░▒▒▒▒▒▒▒▒▒▒██
    ██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██  
    ░░▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓░░  
    ░░▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓██▓▓▓▓▓▓░░    ";
        public static string eng = @"
    ██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓            
██████████████████████████████████████████████████████████████████████          
██████▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓████████░░░░░░░░▒▒██████        
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░░░██▒▒██████      
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░░░██▒▒██░░██████    
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░░░██▒▒██▒▒░░░░██████  
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░░░██▒▒██▒▒░░░░░░▒▒████  
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░██████
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░████
████▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░██░░░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░░░████
████▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░██░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░░░░░████
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░██░░░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░░░██████
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░░░░░████  
████▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░██░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░░░░░██████  
████▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░██░░░░░░░░░░██░░██▓▓░░░░░░░░░░░░░░░░░░██████    
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░████████      
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░░░██▒▒██░░░░░░░░░░░░░░░░████████        
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░██▒▒██▓▓░░░░░░░░░░░░████████            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░░░██▒▒██░░░░░░░░░░░░████████              
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░██░░██░░░░░░░░░░░░██████                  
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▓▓░░░░██▒▒██░░░░░░░░░░░░██████                    
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░░░██▒▒██░░░░░░░░░░██████                      
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░██░░██░░░░░░░░░░██████                        
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░██▒▒██░░░░░░░░████████                          
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░░░██▒▒██░░░░░░██▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░██▒▒██░░░░████▒▒▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██░░██▒▒██░░██▒▒▒▒▒▒▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████▒▒██░░██▒▒▒▒▒▒▒▒▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████▒▒████▒▒▒▒▒▒▒▒▒▒▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████                            
████▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒██▒▒██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████                            
████▒▒▒▒████████████████████▒▒████████████████████████                            
████▒▒██                  ██▒▒██              ██▓▓████                            
████▒▒██                ████▒▒██      ▒▒▒▒▒▒▒▒██▒▒████                            
████▒▒██                ██████                ██▒▒████                            
████▒▒██                ████              ▒▒▒▒██▒▒████                            
████▒▒██                                      ██▒▒████                            
████▒▒▒▒████████████████████████████████████████▒▒████                            
██████▒▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████                            
████████████████████████████████████████████████████                            
    ██████████████████████████████████████████████████                            
                                                                                ";
        public static string socsci = @"
                            ░░░░▒▒░░▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░▒▒▒▒░░▒▒░░░░▒▒░░                            
                      ░░░░░░░░▒▒░░▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓▓▒▒▒▒▒▒▓▓░░░░░░▒▒▒▒░░░░░░▒▒░░░░▒▒▒▒▒▒░░▒▒▒▒░░▒▒░░░░░░░░                        
                  ░░▒▒░░░░░░░░▒▒▓▓▒▒▒▒▒▒▓▓▒▒░░░░▒▒██▓▓▓▓░░░░░░░░░░░░░░░░░░▒▒░░▒▒▒▒▓▓▓▓▓▓▒▒▒▒▒▒▓▓▒▒░░░░░░░░▒▒                    
                ▒▒▓▓▓▓▒▒▓▓▓▓▒▒▓▓▓▓▒▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░▓▓▓▓▓▓▒▒▓▓▒▒▒▒▓▓▓▓▒▒▒▒▒▒▒▒▒▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒▓▓▒▒░░                
            ░░▒▒▓▓▓▓▒▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒▒▒░░▒▒▒▒░░░░▒▒▒▒░░▒▒▓▓▒▒▓▓▓▓▒▒▒▒▓▓▓▓▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▓▓▒▒▓▓▒▒░░▒▒              
          ░░▒▒▒▒░░░░░░▓▓▒▒▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░██▒▒▒▒▒▒▓▓▓▓▒▒▒▒▒▒▓▓▒▒▓▓▓▓▒▒▓▓▓▓▓▓▒▒▓▓▒▒▒▒▒▒░░░░▒▒░░░░░░░░            
        ░░░░░░░░░░░░░░▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▒▒▒▒▒▒░░░░░░░░░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒░░░░░░░░░░░░░░░░        
      ░░░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░░░▓▓▓▓▓▓▓▓██▒▒▓▓▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▒▒▒▒▒▒░░░░░░░░░░░░        
    ░░░░░░░░░░░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░▒▒▒▒▓▓▒▒▒▒▒▒▓▓▓▓▓▓▓▓██▒▒▓▓▓▓██▓▓▓▓██▓▓▓▓▓▓▓▓▒▒▓▓▒▒▓▓░░░░░░░░░░░░░░░░      
    ░░░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▒▒░░▒▒▒▒██▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓██▓▓▓▓▒▒▓▓▓▓░░░░░░░░░░░░░░░░░░    
  ░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░░▒▒▒▒▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░    
  ░░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒░░░░▒▒▒▒▒▒░░░░░░░░░░░░░░░░▓▓▒▒▓▓▒▒▓▓▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▒▒▒▒▓▓▒▒▓▓██▓▓▒▒▒▒▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░░░  
  ░░▒▒░░░░░░░░░░░░░░░░░░██▓▓▒▒░░▒▒▒▒▓▓▒▒░░░░░░░░░░░░▓▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▓▓▓▓▒▒░░▒▒▓▓▓▓░░██▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░  
░░░░░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▓▓▒▒░░▓▓░░░░░░░░░░░░░░▓▓▓▓▓▓▓▓██▓▓▓▓▓▓▓▓██▓▓▓▓▓▓░░░░░░░░▓▓░░░░▒▒▓▓██▒▒▒▒▓▓▒▒▒▒░░░░░░░░░░░░░░░░░░  
░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒▓▓▒▒▓▓▒▒▒▒▓▓▒▒▒▒░░░░▒▒▒▒▒▒▓▓▓▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░▒▒░░░░░░▒▒▓▓▒▒▒▒▓▓░░░░░░░░░░░░░░▒▒░░░░░░  
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░▒▒▒▒▒▒▓▓▓▓████▓▓▓▓▓▓▓▓▓▓░░░░▒▒▒▒░░▓▓▒▒░░▓▓░░▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░
░░░░░░░░░░░░▒▒░░░░░░░░░░░░░░▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░░░░░░░▒▒▒▒▓▓▓▓▓▓▓▓▓▓██▓▓░░░░░░▒▒░░░░░░▒▒▒▒▓▓▒▒▓▓▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒░░░░░░░░░░
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▓▓▓▓▒▒▒▒▓▓▓▓▒▒░░░░░░░░░░░░░░▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░░░░░░░░░░░░░░░▓▓▒▒▓▓▒▒▒▒▒▒▓▓▓▓▓▓░░▒▒░░░░░░░░░░
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▒▒██▓▓▒▒▒▒░░░░░░░░░░░░░░▒▒▓▓▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒░░░░░░░░▒▒░░░░░░░░░░░░  
░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░▒▒▒▒▓▓▓▓██▒▒▓▓░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░  
  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▓▓▓▓▒▒▒▒░░░░░░░░░░░░░░░░░░▓▓▓▓▓▓██▒▒▒▒▒▒░░░░░░░░░░▒▒░░░░░░░░░░▒▒▓▓▒▒▒▒▒▒▒▒░░░░▒▒░░░░░░░░  
  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▒▒▓▓▒▒░░░░░░░░░░░░░░░░░░░░▒▒▓▓▒▒▓▓░░░░░░░░░░░░░░░░▒▒░░░░░░░░▒▒▓▓████████▓▓░░░░░░░░░░░░░░  
  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▒▒▓▓▓▓░░░░░░░░░░░░░░░░░░░░░░██▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░░░░░    
    ▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓▓▓▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░▒▒▒▒▓▓░░░░▒▒░░░░░░░░    
    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░░░▒▒▒▒░░░░      
      ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░░░        
        ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▓▓░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░          
          ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░            
            ░░▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒              
                ▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  ░░░░░░░░░░░░░░░░░░░░░░                
                  ░░░░░░░░░░░░▒▒░░░░▒▒░░░░▒▒░░▒▒░░░░░░░░░░      ░░  ░░                              ░░░░░░▒▒                    
                      ░░░░░░░░            ░░░░░░  ░░░░░░                                        ░░░░░░░░                        
                            ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░                            
          ";
        public static string startscreen = @"
            
  █░█░█ █▀▀ █░░ █▀▀ █▀█ █▀▄▀█ █▀▀   ▀█▀ █▀█   ▀█▀ █▄█ █▀█ ▄▄ █▀▀ █▀▄ █                                
  ▀▄▀▄▀ ██▄ █▄▄ █▄▄ █▄█ █░▀░█ ██▄   ░█░ █▄█   ░█░ ░█░ █▀▀ ░░ ██▄ █▄▀ ▄                                

  This game puts your typing skills to the test! To play the game, you must type the text that will   
  be shown on the screen. The text contains educational facts from subjects like Filipino, English,   
  Natural Science, and the Social Sciences which you can choose from as well. Be sure to pay close    
  attention to the text, so you can ace the bonus quiz question that comes after1 game!           

  The game records your WPM (words per minute) score for each game. The Stats window (press 2) shows  
  a summary of your scores so far, so you will be able to track down your progress!                   

  Good luck, typemaster!                                                                              

█============█ Main Menu █==========================================================================█
█                                                                                                   █
█        █ Start (Press 1)                                                                          █
█        █ Stats (Press 2)                                                                          █
█        █ Quit Game (Press Backspace)                                                              █
█                                                                                                   █
█===================================================================================================█";
        public static string subjectscreen = @"
█===================================================================================================█
█                                                                                                   █
█        █ Filipino (Press 1)                                                                       █
█        █ Natural Science (Press 2)                                                                █
█        █ English (Press 3)                                                                        █
█        █ Social Sciences (Press 4)                                                                █
█        █ Back to Main Menu (Press Backspace)                                                      █
█                                                                                                   █
█===================================================================================================█";
        public static string difficultyscreen = @"
█===================================================================================================█
█                                                                                                   █
█        █ Easy (Press 1)                                                                           █
█        █ Medium (Press 2)                                                                         █
█        █ Hard (Press 3)                                                                           █
█        █ Back to Main Menu (Press Backspace)                                                      █
█                                                                                                   █
█===================================================================================================█";
        public static string loadingscreen = @"
  █▀▀ █▀▀ ▀█▀   █▀█ █▀▀ ▄▀█ █▀▄ █▄█ █                                                                 
  █▄█ ██▄ ░█░   █▀▄ ██▄ █▀█ █▄▀ ░█░ ▄                                                                 ";
        public static string filheader = @"
  █▀▀ █ █░░ █ █▀█ █ █▄░█ █▀█                                                                          
  █▀░ █ █▄▄ █ █▀▀ █ █░▀█ █▄█                                                                          ";
        public static string natsciheader = @"
  █▄░█ ▄▀█ ▀█▀ █░█ █▀█ ▄▀█ █░░   █▀ █▀▀ █ █▀▀ █▄░█ █▀▀ █▀▀                                            
  █░▀█ █▀█ ░█░ █▄█ █▀▄ █▀█ █▄▄   ▄█ █▄▄ █ ██▄ █░▀█ █▄▄ ██▄                                            ";
        public static string engheader = @"
  █▀▀ █▄░█ █▀▀ █░░ █ █▀ █░█                                                                           
  ██▄ █░▀█ █▄█ █▄▄ █ ▄█ █▀█                                                                           ";
        public static string socsciheader = @"
  █▀ █▀█ █▀▀ █ ▄▀█ █░░   █▀ █▀▀ █ █▀▀ █▄░█ █▀▀ █▀▀ █▀                                                 
  ▄█ █▄█ █▄▄ █ █▀█ █▄▄   ▄█ █▄▄ █ ██▄ █░▀█ █▄▄ ██▄ ▄█                                                 ";
        public static string subjheader = @"
  █▀▀ █░█ █▀█ █▀█ █▀ █▀▀   ▄▀█   █▀ █░█ █▄▄ ░░█ █▀▀ █▀▀ ▀█▀ ▀                                         
  █▄▄ █▀█ █▄█ █▄█ ▄█ ██▄   █▀█   ▄█ █▄█ █▄█ █▄█ ██▄ █▄▄ ░█░ ▄                                         ";
        public static string diffheader = @"
  █▀▀ █░█ █▀█ █▀█ █▀ █▀▀   ▄▀█   █▀▄ █ █▀▀ █▀▀ █ █▀▀ █░█ █░░ ▀█▀ █▄█   █░░ █▀▀ █░█ █▀▀ █░░ ▀          
  █▄▄ █▀█ █▄█ █▄█ ▄█ ██▄   █▀█   █▄▀ █ █▀░ █▀░ █ █▄▄ █▄█ █▄▄ ░█░ ░█░   █▄▄ ██▄ ▀▄▀ ██▄ █▄▄ ▄          ";
        public static string quizheader = @"
  █▄█ █▀█ █░█ █▀█   █▀ █▀█ █▀▀ █▀▀ █▀▄   █░█░█ ▄▀█ █▀                                                 
  ░█░ █▄█ █▄█ █▀▄   ▄█ █▀▀ ██▄ ██▄ █▄▀   ▀▄▀▄▀ █▀█ ▄█ ▄ ▄ ▄                                           ";
        public static string congratulations1 = @"
  █▀▀ █▀█ █▄░█ █▀▀ █▀█ ▄▀█ ▀█▀ █░█ █░░ ▄▀█ ▀█▀ █ █▀█ █▄░█ █▀ █                                        
  █▄▄ █▄█ █░▀█ █▄█ █▀▄ █▀█ ░█░ █▄█ █▄▄ █▀█ ░█░ █ █▄█ █░▀█ ▄█ ▄                                        ";
        public static string congratulations2 = @"

  You aced the bonus question! Great job!                                                             ";
        public static string answerwrong1 = @"
  ▄▀█ █░█░█ █░█░█ █░█░█ █░█░█   ▀ ▄▀                                                                  
  █▀█ ▀▄▀▄▀ ▀▄▀▄▀ ▀▄▀▄▀ ▀▄▀▄▀   ▄ ▀▄                                                                  ";
        public static string answerwrong2 = @"

  Looks like you didn't get it right this time. That's alright! You can always try again.             ";
        public static string playagainmenu = @"

█============█ Would you like to play again? █======================================================█
█                                                                                                   █
█        █ Yes (Press 1)                                                                            █
█        █ Back to Main Menu (Press Backspace)                                                      █
█                                                                                                   █
█===================================================================================================█";
        public static string statsmenu = @"
  █▀ ▀█▀ ▄▀█ ▀█▀ █▀                                                                                   
  ▄█ ░█░ █▀█ ░█░ ▄█                                                                                   

  Here's a summary of all the games you've played during this session!                                ";
    }
}