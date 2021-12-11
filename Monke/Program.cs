using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;

namespace Monke
{
    class Program
    {
        static void Main()
        {
            // This is used to store information about the games played by the user. It is only useful for the
            // stats menu.
            List<Game> games = new();
            // The randomizer will be used for selecting a text randomly from the user-chosen category
            // (subject and difficulty level).
            Random randomObject = new();
            bool quitGame = false;
            do
            {
                // The code will always return to this line if the user wishes to go back to the main menu.
                // Putting this in a do-while loop makes it easy to do by simply calling 'continue;' anywhere
                // down the line.
                Console.Clear();
                WriteCentered(Art.computer, ConsoleColor.Blue);
                WriteCentered(Art.mainMenu);
                ConsoleKey menuKey = Console.ReadKey(true).Key;
                while (!(menuKey is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.Backspace)) menuKey = Console.ReadKey(true).Key;
                if (menuKey == ConsoleKey.D1)
                {
                    string subject = "";
                    string difficulty = "";
                    string gameArt = "";
                    string gameHeader = "";
                    ConsoleColor artColor = ConsoleColor.White;
                    // This is for the subjects menu screen.
                    Console.Clear();
                    WriteCentered(Art.subjectsHeader, ConsoleColor.Blue);
                    WriteCentered(Art.subjectsMenu);
                    ConsoleKey subjectKey = Console.ReadKey(true).Key;
                    while (!(subjectKey is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3 or ConsoleKey.D4 or ConsoleKey.Backspace)) subjectKey = Console.ReadKey(true).Key;
                    switch (subjectKey)
                    {
                        case ConsoleKey.D1:
                            subject = "Filipino";
                            artColor = ConsoleColor.DarkRed;
                            gameArt = Art.philippineMap;
                            gameHeader = Art.filipino;
                            break;
                        case ConsoleKey.D2:
                            subject = "Natural Science";
                            artColor = ConsoleColor.DarkGreen;
                            gameArt = Art.beaker;
                            gameHeader = Art.naturalScience;
                            break;
                        case ConsoleKey.D3:
                            subject = "English";
                            artColor = ConsoleColor.DarkBlue;
                            gameArt = Art.paperAndQuill;
                            gameHeader = Art.english;
                            break;
                        case ConsoleKey.D4:
                            subject = "Social Sciences";
                            artColor = ConsoleColor.DarkYellow;
                            gameArt = Art.worldMap;
                            gameHeader = Art.socialSciences;
                            break;
                        case ConsoleKey.Backspace:
                            continue;
                    }
                    // This is for the difficulty menu screen.
                    Console.Clear();
                    WriteCentered(Art.difficultyHeader, ConsoleColor.Blue);
                    WriteCentered(Art.difficultyMenu);
                    ConsoleKey difficultyKey = Console.ReadKey(true).Key;
                    while (!(difficultyKey is ConsoleKey.D1 or ConsoleKey.D2 or ConsoleKey.D3 or ConsoleKey.Backspace)) difficultyKey = Console.ReadKey(true).Key;
                    switch (difficultyKey)
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
                            continue;
                    }
                    while (true)
                    {
                        // The variable 'cursorPosition' will be a multi-purpose variable for saving useful
                        // coordinates of the console cursor.
                        (int, int) cursorPosition = new();
                        // This is for the loading screen.
                        Console.Clear();
                        int lineLength = WriteCentered(Art.getReady);
                        Console.WriteLine();
                        Console.CursorLeft = (Console.WindowWidth - lineLength) / 2;
                        Console.Write("  Loading... ");
                        cursorPosition = Console.GetCursorPosition();
                        Console.WriteLine();
                        WriteCentered(gameArt, artColor);
                        Console.SetCursorPosition(cursorPosition.Item1, cursorPosition.Item2);
                        for (int t = 0; t < lineLength - 13; t++)
                        {
                            Console.Write("█");
                            Thread.Sleep((int)(4000.0 / (lineLength - 13)));
                        }
                        Thread.Sleep(250);
                        // This is for displaying the game proper screen. The variable 'words' contains the individual
                        // words of the text that the user will type.
                        Console.Clear();
                        WriteCentered(gameHeader, artColor);
                        Console.WriteLine();
                        Game game = new(subject, difficulty, randomObject);
                        string[] words = game.text.Split(' ');
                        for (int i = 0; i < words.Length; i++) if (i < words.Length - 1) words[i] += " ";
                        WriteCentered("█============█ Type this text! █====================================================================█");
                        string borders = "█                                                                                                   █";
                        WriteCentered(borders, newLine: false);
                        bool[] newLineFlags = WrapText(words, borders);
                        Console.WriteLine();
                        WriteCentered(borders);
                        WriteCentered("█===================================================================================================█");
                        Console.WriteLine();
                        // The following code is for how the game proper screen works, including live tracking of user
                        // input. The text box displayed also acts as a progress bar; the displayed text becomes colored
                        // progressively as the user input correctly matches the text.
                        int[] typeTracker = new int[2] { (Console.WindowWidth - borders.Length) / 2 + 5, 6 };
                        Stopwatch time = new();
                        time.Start();
                        for (int i = 0; i < words.Length; i++)
                        {
                            Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2;
                            Console.Write("Output: ");
                            string inputWord = "";
                            while (inputWord != words[i])
                            {
                                ConsoleKeyInfo key = Console.ReadKey(true);
                                if (Char.IsLetterOrDigit(key.KeyChar) || Char.IsPunctuation(key.KeyChar) || Char.IsWhiteSpace(key.KeyChar))
                                {
                                    string inputCharacter = key.KeyChar.ToString();
                                    inputWord += inputCharacter;
                                    Console.Write(inputCharacter);
                                    if (words[i].StartsWith(inputWord))
                                    {
                                        cursorPosition = Console.GetCursorPosition();
                                        Console.SetCursorPosition(typeTracker[0], typeTracker[1]);
                                        Console.ForegroundColor = artColor;
                                        Console.Write(key.KeyChar);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        typeTracker[0]++;
                                        Console.SetCursorPosition(cursorPosition.Item1, cursorPosition.Item2);
                                    }
                                }
                                else if (key.Key == ConsoleKey.Backspace && inputWord.Length != 0)
                                {
                                    Console.Write("\b \b");
                                    if (words[i].StartsWith(inputWord))
                                    {
                                        cursorPosition = Console.GetCursorPosition();
                                        Console.SetCursorPosition(typeTracker[0] - 1, typeTracker[1]);
                                        Console.Write(inputWord.Last());
                                        typeTracker[0]--;
                                        Console.SetCursorPosition(cursorPosition.Item1, cursorPosition.Item2);
                                    }
                                    inputWord = inputWord.Remove(inputWord.Length - 1, 1);
                                }
                            }
                            Console.Write(new string('\b', inputWord.Length) + new string(' ', inputWord.Length));
                            if (i < words.Length - 1)
                            {
                                if (newLineFlags[i + 1])
                                {
                                    typeTracker[0] = (Console.WindowWidth - borders.Length) / 2 + 5;
                                    typeTracker[1]++;
                                }
                            }
                        }
                        time.Stop();
                        // This is for displaying the WPM score.
                        game.wpm = (int)(words.Length / (time.ElapsedMilliseconds / 60000.0));
                        Console.Clear();
                        WriteCentered(Art.wpmScoreHeader, artColor);
                        Console.WriteLine();
                        Console.CursorLeft = (Console.WindowWidth - lineLength) / 2;
                        Console.Write("  WPM: ");
                        Console.WriteLine(game.wpm);
                        Thread.Sleep(1500);
                        // This is for displaying the bonus quiz question.
                        Console.WriteLine();
                        WriteCentered("█============█ Bonus question! █====================================================================█");
                        Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2;
                        Console.Write(borders);
                        string[] questionWords = game.quizQuestion.Split(' ');
                        for (int i = 0; i < questionWords.Length; i++) if (i < questionWords.Length - 1) questionWords[i] += " ";
                        WrapText(questionWords, borders);
                        Console.WriteLine();
                        WriteCentered(borders);
                        string[] choiceButtons = new string[4] { " (Press A)", " (Press B)", " (Press C)", " (Press D)" };
                        for (int i = 0; i < 4; i++)
                        {
                            cursorPosition = ((Console.WindowWidth - borders.Length) / 2 + 9, Console.CursorTop);
                            WriteCentered(borders);
                            Console.SetCursorPosition(cursorPosition.Item1, cursorPosition.Item2);
                            Console.WriteLine("█ " + game.choices[i] + choiceButtons[i]);
                        }
                        WriteCentered(borders);
                        WriteCentered("█===================================================================================================█");
                        // This is for checking whether the user correctly answered the question.
                        char choiceKey = Console.ReadKey(true).KeyChar;
                        while (!(choiceKey is 'a' or 'b' or 'c' or 'd')) choiceKey = Console.ReadKey(true).KeyChar;
                        game.result = choiceKey.ToString() == game.quizAnswer ? 1 : 0;
                        // The user had just completed a game! The following is for the play again menu.
                        games.Add(game);
                        Console.Clear();
                        if (game.result == 1)
                        {
                            WriteCentered(Art.quizCorrectHeader, artColor);
                            WriteCentered(Art.quizCorrectText);
                        }
                        else
                        {
                            WriteCentered(Art.quizWrongHeader, artColor);
                            WriteCentered(Art.quizWrongText);
                        }
                        Thread.Sleep(1500);
                        WriteCentered(Art.playAgainMenu);
                        ConsoleKey playAgainKey = Console.ReadKey(true).Key;
                        while (!(playAgainKey is ConsoleKey.D1 or ConsoleKey.Backspace)) playAgainKey = Console.ReadKey(true).Key;
                        if (playAgainKey == ConsoleKey.Backspace) break;
                    }
                }
                else if (menuKey == ConsoleKey.D2)
                {
                    // This is for displaying the stats screen.
                    Console.Clear();
                    WriteCentered(Art.statsMenu);
                    Console.WriteLine();
                    string borders = "█                                                                                                   █";
                    if (games.Count == 0)
                    {
                        Thread.Sleep(2000);
                        Console.CursorLeft =  (Console.WindowWidth - borders.Length) / 2;
                        Console.WriteLine("...Uhh, well this is awkward. Seems like you haven't played a game yet!");
                    }
                    else
                    {
                        WriteCentered("█============█ Summary statistics █=================================================================█");
                        WriteCentered(borders);
                        string[] strings = new string[3] {
                            "█ Number of games played: " + games.Count,
                            "█ Average WPM: " + Math.Round(games.Average(item => item.wpm), 2),
                            "█ Correctly answered quizzes: " + games.Sum(item => item.result) + " / " + games.Count
                        };
                        for (int i = 0; i < 3; i++)
                        {
                            Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2;
                            Console.Write(borders);
                            Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2 + 5;
                            Console.WriteLine(strings[i]);
                        }
                        WriteCentered(borders);
                        WriteCentered("█===================================================================================================█");
                        Console.WriteLine();
                        Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2;
                        Console.WriteLine("Game #\tSubject\t\t\tDifficulty level\tWPM\t\tBonus quiz result");
                        Console.WriteLine();
                        for (int i = 0; i < games.Count; i++)
                        {
                            string space = games[i].subject.Length >= 8 ? "\t\t" : "\t\t\t";
                            string quiz = games[i].result == 1 ? "Correct" : "Wrong";
                            Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2;
                            Console.WriteLine(i + 1 + "\t\t" + games[i].subject + space + games[i].difficulty + "\t\t\t" + games[i].wpm + "\t\t" + quiz);
                        }
                    }
                    Console.WriteLine();
                    Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2;
                    Console.WriteLine("<< Back to Main Menu (Press Backspace)");
                    while (Console.ReadKey(true).Key != ConsoleKey.Backspace) ;
                }
                else if (menuKey == ConsoleKey.Backspace) quitGame = true;
            } while (!quitGame);
            Console.Clear();
        }
        // This is for centering strings in the console, even accounting for multiple lines of text within a
        // single string.
        static int WriteCentered(string str, ConsoleColor color = ConsoleColor.White, bool newLine = true)
        {
            StringReader reader = new(str);
            string line = reader.ReadLine();
            int lastLineLength = 0;
            Console.ForegroundColor = color;
            while (line != null)
            {
                lastLineLength = line.Length;
                Console.SetCursorPosition((Console.WindowWidth - lastLineLength) / 2, Console.CursorTop);
                Console.Write(line);
                line = reader.ReadLine();
                if (line != null || newLine) Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            return lastLineLength;
        }
        // This is for wrapping long text strings within the visible text box of the game proper UI. Notice that
        // it returns an array of boolean values. Each word in the sample text is assigned a flag in this boolean
        // array, which indicates whether or not it is the beginning of a new line.
        static bool[] WrapText(string[] words, string borders)
        {
            bool[] newLineFlags = new bool[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                newLineFlags[i] = false;
                if (words[i].Length > (Console.WindowWidth + borders.Length) / 2 - Console.CursorLeft - 5)
                {
                    newLineFlags[i] = true;
                    Console.WriteLine();
                    WriteCentered(borders, newLine: false);
                    Console.CursorLeft = (Console.WindowWidth - borders.Length) / 2 + 5;
                }
                Console.Write(words[i]);
            }
            return newLineFlags;
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
        public Game(string subj, string diff, Random randomObject)
        {
            // This is for randomly choosing what text will be typed by the user, according to difficulty
            // and subject settings.
            subject = subj;
            difficulty = diff;
            int textIndex = randomObject.Next(1, 6);
            string[] tsvLines = File.ReadAllLines("Dataset.tsv");
            for (int i = 1; i < tsvLines.Length; i++)
            {
                string[] dataRow = tsvLines[i].Split('\t');
                if (dataRow[0] == subject && dataRow[1] == difficulty)
                {
                    textIndex--;
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
    }
    // This is a static class where all the Console art is organized.
    public static class Art
    {
        public const string computer = @"
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
                                                                                         ███░░░░░░███ ";
        public const string philippineMap = @"                                              
                                ▒▒████                                              
                                ▓▓██████▓▓                                          
                                ██████████                                          
                                ██████████                                          
                                ████████████                                        
                                ██████████▒▒                                        
                                ██████████                                          
                            ▒▒  ██████▓▓                                            
                              ████████░░                                            
                              ████████                                              
                              ████████  ▒▒                                          
                                  ░░██  ░░                                          
                                ░░██████  ▒▒▓▓                                      
                                  ▒▒██  ████  ██  ██  ██                            
                                ░░░░    ██  ▒▒  ▓▓██                                
                                  ░░░░  ░░      ░░██▒▒                              
                                  ░░░░          ░░  ▒▒                              
                                    ░░  ░░      ▒▒▒▒  ░░▓▓▓▓                        
                              ░░                ▒▒  ▓▓  ▓▓▓▓▓▓                      
                                        ░░                ▓▓▓▓                      
                                          ░░░░          ░░▒▒▓▓                      
                                          ░░░░░░        ▓▓▒▒▓▓                      
                          ░░            ░░░░░░  ░░        ▓▓                        
                          ░░                  ░░░░  ▓▓    ▓▓                        
                        ░░                  ░░░░  ▓▓  ▓▓      ▒▒░░                  
                      ░░                    ░░░░              ▒▒                    
                    ░░                        ░░░░░░    ░░    ▓▓▓▓                  
                ░░░░                                      ▓▓▓▓▓▓▓▓                  
                                                ▓▓▓▓  ░░▓▓▓▓▓▓▓▓▓▓░░                
                                            ▒▒▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓                
                                          ▓▓  ▓▓▓▓  ▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░              
                                        ░░░░          ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒              
                                        ▒▒            ▓▓▓▓▓▓▓▓░░▓▓▒▒                
                                        ▓▓            ▓▓▓▓▓▓▓▓                      
                                                      ▒▒▓▓▓▓▓▓░░                    
                                                            ▓▓                      ";
        public const string beaker = @"
                    ░░░░░░            ░░                                        
                ░░░░░░░░░░        ░░░░░░░░                ░░░░                  
                    ░░░░░░            ░░░░                ░░░░░░░░              
                    ░░░░░░            ░░                  ░░░░░░░░              
                    ░░                                ░░░░░░░░░░░░              
                                                        ░░░░░░░░░░░░            
        ████████████████████████████████████████████████████▓▓▒▒████████        
        ██  ░░░░░░░░░░  ░░░░░░░░░░░░░░▒▒▒▒▒▒░░░░░░░░░░░░░░▒▒▒▒░░░░░░░░██        
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
        public const string paperAndQuill = @"
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
        public const string worldMap = @"
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
        public const string mainMenu = @"
            
  █░█░█ █▀▀ █░░ █▀▀ █▀█ █▀▄▀█ █▀▀   ▀█▀ █▀█   ▀█▀ █▄█ █▀█ ▄▄ █▀▀ █▀▄ █                                
  ▀▄▀▄▀ ██▄ █▄▄ █▄▄ █▄█ █░▀░█ ██▄   ░█░ █▄█   ░█░ ░█░ █▀▀ ░░ ██▄ █▄▀ ▄                                

  This game puts your typing skills to the test! To play the game, you must type the text that will   
  be shown on the screen. The text contains educational facts from subjects like Filipino, English,   
  Natural Science, and the Social Sciences which you can choose from as well. Be sure to pay close    
  attention to the text, so you can ace the bonus quiz question that comes after every game!          

█============█ Main Menu █==========================================================================█
█                                                                                                   █
█        █ Start (Press 1)                                                                          █
█        █ Stats (Press 2)                                                                          █
█        █ Quit Game (Press Backspace)                                                              █
█                                                                                                   █
█===================================================================================================█";
        public const string subjectsMenu = @"
█===================================================================================================█
█                                                                                                   █
█        █ Filipino (Press 1)                                                                       █
█        █ Natural Science (Press 2)                                                                █
█        █ English (Press 3)                                                                        █
█        █ Social Sciences (Press 4)                                                                █
█        █ Back to Main Menu (Press Backspace)                                                      █
█                                                                                                   █
█===================================================================================================█";
        public const string difficultyMenu = @"
█===================================================================================================█
█                                                                                                   █
█        █ Easy (Press 1)                                                                           █
█        █ Medium (Press 2)                                                                         █
█        █ Hard (Press 3)                                                                           █
█        █ Back to Main Menu (Press Backspace)                                                      █
█                                                                                                   █
█===================================================================================================█";
        public const string getReady = @"
  █▀▀ █▀▀ ▀█▀   █▀█ █▀▀ ▄▀█ █▀▄ █▄█ █                                                                 
  █▄█ ██▄ ░█░   █▀▄ ██▄ █▀█ █▄▀ ░█░ ▄                                                                 ";
        public const string filipino = @"
  █▀▀ █ █░░ █ █▀█ █ █▄░█ █▀█                                                                          
  █▀░ █ █▄▄ █ █▀▀ █ █░▀█ █▄█                                                                          ";
        public const string naturalScience = @"
  █▄░█ ▄▀█ ▀█▀ █░█ █▀█ ▄▀█ █░░   █▀ █▀▀ █ █▀▀ █▄░█ █▀▀ █▀▀                                            
  █░▀█ █▀█ ░█░ █▄█ █▀▄ █▀█ █▄▄   ▄█ █▄▄ █ ██▄ █░▀█ █▄▄ ██▄                                            ";
        public const string english = @"
  █▀▀ █▄░█ █▀▀ █░░ █ █▀ █░█                                                                           
  ██▄ █░▀█ █▄█ █▄▄ █ ▄█ █▀█                                                                           ";
        public const string socialSciences = @"
  █▀ █▀█ █▀▀ █ ▄▀█ █░░   █▀ █▀▀ █ █▀▀ █▄░█ █▀▀ █▀▀ █▀                                                 
  ▄█ █▄█ █▄▄ █ █▀█ █▄▄   ▄█ █▄▄ █ ██▄ █░▀█ █▄▄ ██▄ ▄█                                                 ";
        public const string subjectsHeader = @"
  █▀▀ █░█ █▀█ █▀█ █▀ █▀▀   ▄▀█   █▀ █░█ █▄▄ ░░█ █▀▀ █▀▀ ▀█▀ ▀                                         
  █▄▄ █▀█ █▄█ █▄█ ▄█ ██▄   █▀█   ▄█ █▄█ █▄█ █▄█ ██▄ █▄▄ ░█░ ▄                                         ";
        public const string difficultyHeader = @"
  █▀▀ █░█ █▀█ █▀█ █▀ █▀▀   ▄▀█   █▀▄ █ █▀▀ █▀▀ █ █▀▀ █░█ █░░ ▀█▀ █▄█   █░░ █▀▀ █░█ █▀▀ █░░ ▀          
  █▄▄ █▀█ █▄█ █▄█ ▄█ ██▄   █▀█   █▄▀ █ █▀░ █▀░ █ █▄▄ █▄█ █▄▄ ░█░ ░█░   █▄▄ ██▄ ▀▄▀ ██▄ █▄▄ ▄          ";
        public const string wpmScoreHeader = @"
  █▄█ █▀█ █░█ █▀█   █▀ █▀█ █▀▀ █▀▀ █▀▄   █░█░█ ▄▀█ █▀                                                 
  ░█░ █▄█ █▄█ █▀▄   ▄█ █▀▀ ██▄ ██▄ █▄▀   ▀▄▀▄▀ █▀█ ▄█ ▄ ▄ ▄                                           ";
        public const string quizCorrectHeader = @"
  █▀▀ █▀█ █▄░█ █▀▀ █▀█ ▄▀█ ▀█▀ █░█ █░░ ▄▀█ ▀█▀ █ █▀█ █▄░█ █▀ █                                        
  █▄▄ █▄█ █░▀█ █▄█ █▀▄ █▀█ ░█░ █▄█ █▄▄ █▀█ ░█░ █ █▄█ █░▀█ ▄█ ▄                                        ";
        public const string quizCorrectText = @"

  You aced the bonus question! Great job!                                                             ";
        public const string quizWrongHeader = @"
  ▄▀█ █░█░█ █░█░█ █░█░█ █░█░█   ▀ ▄▀                                                                  
  █▀█ ▀▄▀▄▀ ▀▄▀▄▀ ▀▄▀▄▀ ▀▄▀▄▀   ▄ ▀▄                                                                  ";
        public const string quizWrongText = @"

  Looks like you didn't get it right this time. That's alright! You can always try again.             ";
        public const string playAgainMenu = @"

█============█ Would you like to play again? █======================================================█
█                                                                                                   █
█        █ Yes (Press 1)                                                                            █
█        █ Back to Main Menu (Press Backspace)                                                      █
█                                                                                                   █
█===================================================================================================█";
        public const string statsMenu = @"
  █▀ ▀█▀ ▄▀█ ▀█▀ █▀                                                                                   
  ▄█ ░█░ █▀█ ░█░ ▄█                                                                                   

  Here's a summary of all the games you've played during this session!                                ";
    }
}