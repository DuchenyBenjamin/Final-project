using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;
using System.Globalization;
using System.IO;
using System.Linq;

namespace arcadeGame
{
    //************************************
    //Title: ArcadeGame
    //Application Type: A few fun arcade games
    //Description: To function as a quick fun time
    //Author: Benjamin Ducheny
    //Date Created: 11/20/2020
    //Last Modified:12/3/2020
    //*************************************
    class Program
    {
        /// <summary>
        /// *****************************************************************
        /// *                           Main                                *
        /// *****************************************************************
        /// </summary>
        static void Main()
        {
            WindowHeight = 20;
            WindowWidth = 80;
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayWelcomeScreen();
            displayMenuScreen();
        }
        /// <summary>
        /// *****************************************************************
        /// *                         Main Menu                             *
        /// *****************************************************************
        /// </summary>
        static void displayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\t--------------------------------");
                Console.WriteLine("\ta) Change Theme");
                Console.WriteLine("\tb) Arcade");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine("\t--------------------------------");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplaySetTheme();
                        break;

                    case "b":
                        displayArcadeMenuScreen();
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitApplication);
        }

        /// <summary>
        /// *****************************************************************
        /// *                     arcadeGame Menu                          *
        /// *****************************************************************
        /// </summary>
        #region arcadeMenu
        static void displayArcadeMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Arcade Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\t--------------------------------");
                Console.WriteLine("\ta) snake Game");
                Console.WriteLine("\tb) Word Game");
                Console.WriteLine("\tq) Go back");
                Console.WriteLine("\t--------------------------------");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        snakeGameMenu();
                        break;

                    case "b":
                        displayWordMenuScreen();
                        break;

                    case "q":
                        DisplayContinuePrompt();
                        displayMenuScreen();
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitApplication);
        }
        #endregion

        /// <summary>
        /// *****************************************************************
        /// *                       theme editor                            *
        /// *****************************************************************
        /// </summary>
        #region Theme

        static void DisplaySetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            //
            // set theme from data
            //
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");

            Console.Write("\tWould you like to change the current theme [ yes | no ]?");
            if (Console.ReadLine().ToLower() == "yes")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");

                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    //
                    // set new theme
                    //
                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\ttNew background color: {Console.BackgroundColor}");

                    Console.WriteLine();
                    Console.Write("\tIs this the theme you would like?");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                    }

                } while (!themeChosen);

            }
            DisplayContinuePrompt();
        }
        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}:");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t It appears you did not provide a valid console color. Please Try again");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;
        }

        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData()
        {
            string dataPath = @"Data/Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);
        }

        static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }
        #endregion

        /// <summary>
        /// *****************************************************************
        /// *                     Snake game                                *
        /// *****************************************************************
        /// </summary>
        #region snakeGame
        /// <summary>
        /// *****************************************************************
        /// *                     Snake game Menu                          *
        /// *****************************************************************
        /// </summary>
        static void snakeGameMenu()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;
            do
            {
                string dataPath = @"Data/HighScoreSnake.txt";
                string storeScore = File.ReadAllText(dataPath);
                int highScore = Int32.Parse(storeScore);
                DisplayScreenHeader("Snake Game Menu");
                //
                // get user menu choice
                //
                Console.WriteLine("\t--------------------------------");
                Console.WriteLine("\ta) Snake game controls and rules(Read this first): ");
                Console.WriteLine("\tb) Play snake Game");
                Console.WriteLine("\tc) Show High Score");
                Console.WriteLine("\tq) Go back to menu");
                Console.WriteLine("\t--------------------------------");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();
                switch (menuChoice)
                {
                    case "a":
                        Console.Clear();
                        SnakeGameRules();
                        break;
                    case "b":
                        DisplayContinuePrompt();
                        snakeGame();
                        break;

                    case "c":
                        Console.Clear();
                        displayHighScore(highScore);
                        break;

                    case "q":
                        DisplayContinuePrompt();
                        displayArcadeMenuScreen();
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitApplication);
        }
        static void snakeGame()
        {
            WindowHeight = 16;
            WindowWidth = 32;
            //
            //A var is data type that can be used to declare local variable that tells the compiler to figure out what to do with it.
            //
            var rand = new Random();//this generate random numbers 
            int score = 5;
            var head = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red); //this declares a new pixel for the head of the snake
            var apple = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Yellow);//this also declares a new pixel but for the berry item that is in the snake game
            var body = new List<Pixel>();//I needed to use a list for the body as it is made up of multiple pixels.
            var currentMovement = Direction.Right; //this sets a starting positiomn 
            bool gameover = false;// this will be a bool 
            //
            //While loop 
            //
            while (true)
            {
                Console.Clear();

                gameover |= (head.XPos == WindowWidth - 1 || head.XPos == 0 || head.YPos == WindowHeight - 1 || head.YPos == 0);

                if (apple.XPos == head.XPos && apple.YPos == head.YPos)
                {
                    score++;
                    apple = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Yellow);
                }

                for (int i = 0; i < body.Count; i++)//adds to the body for every new apple in body list
                {
                    DrawPixel(body[i]);
                    gameover |= (body[i].XPos == head.XPos && body[i].YPos == head.YPos);// it was easyer to make it so that you die when you hit the head rather than any thing more complex
                }

                if (gameover)
                {
                    break; //sends you out of the while
                }

                DrawPixel(head);
                DrawPixel(apple);

                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds <= 200)//This is the time in which it takes the screen to refresh, I thought it looked pretty cool
                {
                    currentMovement = ReadKeyMovement(currentMovement);// This is the new key press
                }

                body.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));

                switch (currentMovement)//I Shouldn't have used var's, I don't yet know enough about them.
                {
                    case Direction.Up:
                        head.YPos--;
                        break;
                    case Direction.Down:
                        head.YPos++;
                        break;
                    case Direction.Left:
                        head.XPos--;
                        break;
                    case Direction.Right:
                        head.XPos++;
                        break;
                }

                if (body.Count > score)// Count is used in lists to count the number of items that said list holds. in this case body
                {
                    body.RemoveAt(0);
                }
            }
            // display end screen
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            WriteLine($"Game over, Score: {score - 5}");
            writeScore(score);
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            ReadKey();
            WindowHeight = 20;
            WindowWidth = 80;
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
        }
        private static void SnakeGameRules()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t--------------------------------");
            Console.WriteLine();
            Console.WriteLine("\tControls and Rules:");
            Console.WriteLine();
            Console.WriteLine("\t1. Use the arrow keys to move around the screen.");
            Console.WriteLine("\t2. Eat the golden apples to grow.");
            Console.WriteLine("\t3. The goal of the game is to eat as many golden apples");
            Console.WriteLine("\t   as you can before you hit a wall, or your self.");
            Console.WriteLine("\t--------------------------------");
            DisplayContinuePrompt();
        }//Rules
        static Direction ReadKeyMovement(Direction movement)//This calls my movment Enum
        {
            if (KeyAvailable)//"Gets a value indicating whether a key press is available in the input stream." Thank you google::::: https://docs.microsoft.com/en-us/dotnet/api/system.console.keyavailable?view=net-5.0 
            {
                var key = ReadKey(true).Key;//reads the key of direction


                //I like switchs more but couldent figure out how to use it here.
                if (key == ConsoleKey.UpArrow && movement != Direction.Down)
                {
                    movement = Direction.Up;
                }
                else if (key == ConsoleKey.DownArrow && movement != Direction.Up)
                {
                    movement = Direction.Down;
                }
                else if (key == ConsoleKey.LeftArrow && movement != Direction.Right)
                {
                    movement = Direction.Left;
                }
                else if (key == ConsoleKey.RightArrow && movement != Direction.Left)
                {
                    movement = Direction.Right;
                }
            }
            //returns the key press.
            return movement;
        }

        static void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Write("■");
            SetCursorPosition(0, 0);
        }

        struct Pixel //My first time useing a struct, Google help a lot with this
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                ScreenColor = color;
            }
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ScreenColor { get; set; }
        }

        enum Direction//More enums
        {
            Up,
            Down,
            Right,
            Left
        }

        static void writeScore(int score)
        {
            string dataPath = @"Data/HighScoreSnake.txt";
            string storeScore = File.ReadAllText(dataPath);
            int highScore = Int32.Parse(storeScore);
            if (score > highScore)
            {
                string scoreString = score.ToString($"{score - 5}");
                File.WriteAllText(dataPath, scoreString);
            }
        }
        static int displayHighScore(int highScore)
        {
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            WriteLine($"\tThe Current High Score Is: {highScore}");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            DisplayContinuePrompt();
            return highScore;
        }
        #endregion

        /// <summary>
        /// *****************************************************************
        /// *                        Word game                              *
        /// *****************************************************************
        /// </summary>
        #region WordGame
        /// <summary>
        /// *****************************************************************
        /// *                     Word game Menu                          *
        /// *****************************************************************
        /// </summary>
        static void displayWordMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;
            string[] args = null;
            do
            {
                DisplayScreenHeader("Arcade Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\t--------------------------------");
                Console.WriteLine("\ta) Rules(Read this first)");
                Console.WriteLine("\tb) Hang-man WordGame.");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine("\t--------------------------------");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        WordGameRules();
                        break;

                    case "b":
                        WordGameStart(args);
                        break;

                    case "q":
                        DisplayContinuePrompt();
                        displayMenuScreen();
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitApplication);
        }//Display menu

        private static void WordGameRules()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t--------------------------------");
            Console.WriteLine();
            Console.WriteLine("\tThere are a few Rules you should know.");
            Console.WriteLine();
            Console.WriteLine("\t1. This is a 2 player game, so please find");
            Console.WriteLine("\t   someone that you can play with.(or at least start)");
            Console.WriteLine("\t2. A word must contain only letters.");
            Console.WriteLine("\t3. have fun, you only have 5 lives so use them wisely.");
            Console.WriteLine("\t--------------------------------");
            DisplayContinuePrompt();
        }//Rules
        public static string WordGameStart(string[] args)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tLet's play Hangman!");
            //Call for user input
            Console.Write("\tPlease enter you hidden word: ");
            string hiddenWord = Console.ReadLine();

            bool wordTest = hiddenWord.All(Char.IsLetter);//Checks To make sure The word is only letters.

            while (wordTest == false || hiddenWord.Length == 0)//This runs only if there are non letter objects in the Hidden word.
            {
                Console.WriteLine("\tA word must contain (only) letters");
                Console.Write("\tPlease enter you hidden word: ");
                hiddenWord = Console.ReadLine();
                wordTest = hiddenWord.All(Char.IsLetter);
            }
            Console.Clear();//Clears the screen
            hiddenWord = hiddenWord.ToUpper();
            WordGameMain(args, hiddenWord);
            return hiddenWord;
        }//Start the game
        static string DrawHangman(int livesLeft)//simple function to print out the hangman
        {

            string drawHangman = "";

            if (livesLeft < 5)
            {
                Console.WriteLine();
                drawHangman += "\t--------\n";
            }

            if (livesLeft < 4)
            {
                drawHangman += "\t       |\n";
            }

            if (livesLeft < 3)
            {
                drawHangman += "\t       O\n";
            }

            if (livesLeft < 2)
            {
                drawHangman += "\t      /|\\ \n";
            }

            if (livesLeft == 0)
            {
                drawHangman += "\t      / \\ \n";
            }

            return drawHangman;

        }
        public static void WordGameMain(string[] args, string hiddenWord)
        {
            int lives = 5;//The lives you have
            int counter = -1;//count
            int wordLength = hiddenWord.Length;//get length
            char[] secretWord = hiddenWord.ToCharArray();//A character array
            char[] Print = new char[wordLength];//word length
            char[] guessedLetters = new char[26];// letters guessed 
            int storeNumber = 0;
            bool victory = false;//Victory bool

            foreach (char letter in Print)
            {
                counter++;
                Print[counter] = '-';//prints blanks for the entire length of the word.
            }

            while (lives > 0)// while lives are greater than 0 play this
            {
                counter = -1;
                string printProgress = String.Concat(Print);
                bool letterFound = false;
                int multiples = 0;

                if (printProgress == hiddenWord)
                {
                    victory = true;
                    break;
                }

                if (lives > 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("\tYou have {0} lives!", lives);
                }//prints remaining lives

                else//prints only if you have 1 life
                {
                    Console.WriteLine();
                    Console.WriteLine("\tYou only have {0} life left!!", lives);
                }

                Console.WriteLine();
                Console.WriteLine("\tcurrent progress: " + printProgress);
                Console.Write("\n");
                Console.Write("\tGuess a letter: ");
                string playerGuess = Console.ReadLine();//player input

                bool guessTest = playerGuess.All(Char.IsLetter);//this checks that only 1 letter is chosen  

                while (guessTest == false || playerGuess.Length != 1)
                {
                    Console.WriteLine("\tPlease enter only a single letter!");
                    Console.Write("\tGuess a letter: ");
                    playerGuess = Console.ReadLine();
                    guessTest = playerGuess.All(Char.IsLetter);
                }//this runs only if guessTest comes back false
                Console.Clear();
                playerGuess = playerGuess.ToUpper();
                char playerChar = Convert.ToChar(playerGuess);

                if (guessedLetters.Contains(playerChar) == false)//this checkes if the guessed letter is in hidden word.
                {

                    guessedLetters[storeNumber] = playerChar;
                    storeNumber++;

                    foreach (char letter in secretWord)
                    {
                        counter++;
                        if (letter == playerChar)
                        {
                            Print[counter] = playerChar;
                            letterFound = true;
                            multiples++;
                        }

                    }

                    if (letterFound)//displayes output if lette ris found
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tFound {0} letter {1}!", multiples, playerChar);
                    }

                    else//displayes out put if there is no letter in hiddenword.
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tNo letter {0}!", playerChar);
                        lives--;
                    }
                    Console.WriteLine(DrawHangman(lives));//draws the hangman based on numbers of lives remaining.
                }
                else//If player allready guessed inputed letter
                {
                    Console.WriteLine();
                    Console.WriteLine("\tYou already guessed {0}!!", playerChar);
                }
            }
            Endscreen(victory, hiddenWord);//End screen
        }//messy code, This is the main function.
        static void Endscreen(bool victory, string hiddenWord)
        {
            if (victory)
            {
                Console.WriteLine();
                Console.WriteLine("\tThe word was: {0}", hiddenWord);
                Console.WriteLine("\tYOU WIN!!");
                DisplayContinuePrompt();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tThe word was: {0}", hiddenWord);
                Console.WriteLine("\tYOU LOSE!");
                DisplayContinuePrompt();
            }

        }//Less fancy end screen, But it still looks great.
        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tArcade Game");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for playing some of my arcade games!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
