using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Timers;

namespace connect4
{
    /// <summary>
    /// The game Connect Four. 
    /// </summary>
    class Program
    {
        static int CurrentFrame = 0;
        static int FramesPerSecond = 30;
        static string GameScreen = "MainMenu";
        static int CursorPosition = 1;
        public static Player P1 = new HumanPlayer();
        public static Player P2 = new HumanPlayer();
        static Player currentPlayer = P1;
        static List<string> msg = new List<string>();

        /// <summary>
        /// The <see cref="Board"/> class keeps track of all token positions, displays <see cref="Player"/> scores, names and colours. A <see cref="Menu"/> is also available for the user to pause, reset, exit.
        /// </summary>
        static class Board
        {
            public static int[,] Arr = new int[6, 7];
            public static int[,] colourArr = new int[6, 7];


            // draw board
            // draw scoreboard
            // add a timer? (optional) - perhaps in the windowRefresh/Redraw function. 
            // cursor position (See if mouse control is doable)
            // menu button
            // 
        }

        /// <summary>
        /// <see cref="Menu"/> pauses the game when prompted. The following options are available:
        /// <list type="bullet">
        /// <item>Reset</item>
        /// <item>Exit</item>
        /// <item>Options</item>
        /// </list>
        /// 
        /// </summary>
        static class Menu
        {
            // add Reset (current game &/or score)
            // add Exit (confirmation required - exit game)
            // add options (perhaps sound, token, show and alter keybinds)
        }

        static class KeyHandler
        {
            // ToDo
        }
        /// <summary>
        /// <para><see cref="Player"/> creates an object to represent each active player.</para>
        /// <para>Paramaters are <paramref name="Name"/>, <paramref name="Score"/>, <paramref name="TokenType"/> and <paramref name="Colour"/></para>
        /// <para>By default, the game has two human players.</para>
        /// One computer player may be implemented in the future.
        /// </summary>
        /// <param name="Name"/>
        /// <param name="Score"/>
        /// <param name="TokenType"/>
        /// <param name="Colour"/>
        public abstract class Player
        {
            public static int PlayerCount;
            public abstract string Name { get; protected set; }
            public abstract int Score { get; protected set; }
            public abstract int TokenType { get; set; }
            public abstract ConsoleColor Colour { get; set; }

            public override string ToString() {
                return Name;
            }

            // ToDo
            protected void AddPlayerCount()
            {
                PlayerCount++;
            }

            /// <summary>
            /// The tokens that <see cref="Player"/> drops into the <see cref="Board"/> columns. Connect 4 to win.
            /// </summary>
          /*protected class PlayerToken //REMOVED FOR NOW -- LET'S SEE IF I'LL STILL NEED IT
            {
                // instantiates the tokens that occupy the board's fields.
                // add design
                // add animation
            }*/
        }

        /// <summary>
        /// A human <see cref="Player"/>.
        /// </summary>
        public class HumanPlayer : Player
        {
            public override string Name { get; protected set; } = $"Player_{PlayerCount+1}";
            public override int Score { get; protected set; } = 0;
            public override int TokenType { get; set; } = PlayerCount+1;
            public override ConsoleColor Colour { get; set; } = ConsoleColor.Cyan;

            public HumanPlayer()
            {
                AddPlayerCount();
            }
        }
        /// <summary>
        /// A computer <see cref="Player"/>.
        /// </summary>
        public class ComputerPlayer : Player
        {
            public override string Name { get; protected set; } = $"Computer_{PlayerCount+1}";
            public override int Score { get; protected set; } = 0;
            public override int TokenType { get; set; } = PlayerCount + 1;
            public override ConsoleColor Colour { get; set; } = ConsoleColor.Red;
            public ComputerPlayer()
            {
                AddPlayerCount();
            }
        }


        public static void Main(string[] args)
        {
            bool Exit = false;
            Console.CursorVisible = false;
            P2.Colour = ConsoleColor.Red;
            // ToDo
            // Start Game - initial screen. Choose game type (vs. player or computer)
            // Enter player(s) name(s), choose token type, choose colour. Tokens and colours cannot be the same.

            // Create a seven-column, six-row grid.
            // 'play()'. Player1 starts. Show cursor position.
            // Control with A or LeftArrow(left), D or RightArrow(right)
            // Space or Enter drops the Token
            // The pieces fall top-to-bottom, occupying the lowest available space.
            // Instantiate a new Token, animate it moving down [optional]
            // Log position, check for non-manual game-ending condition

            // Game-ending conditions:
            // Player manually exits
            // Player manually resets
            // Winning condition is met.
            // Four consecutive pieces - Horizontal 
            // Four consecutive pieces - Vertical 
            // Four consecutive pieces - Diagonal 

            // Change current player
            // repeat play() for current player;


            // Controls the frames per second and console drawing frequency.
            System.Timers.Timer RefreshRate = new()
            {
                Interval = 1000 / FramesPerSecond,
                AutoReset = true,
                Enabled = true,
            };
            RefreshRate.Elapsed += OnTimedEvent;

            while (Exit != true)
            {
                InputHandler();
            }

        }

        // Handles keyboard inputs.
        public static void InputHandler()
        {
            
            // To avoid extremely fast key repetition, current delay = (every 8 frames[16.6ms each])
            if (CurrentFrame % 8 == 0) { 
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Main Menu commands
                if (GameScreen == "MainMenu") 
                {
                    // Navigate through options:
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        if (CursorPosition == 3)
                            CursorPosition = 1;
                        else CursorPosition++;
                    }
                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        if (CursorPosition == 1)
                            CursorPosition = 3;
                        else CursorPosition--;
                    }

                    // Select the option:
                    if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
                    {
                        if (CursorPosition == 1)
                            StartGame();
                        else if (CursorPosition == 2)
                            OpenOptions();
                        else if (CursorPosition == 3) { 
                            Console.Clear();
                            PromptExit();
                        }
                    }
                }
                else if (GameScreen == "Game")
                {
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        if (CursorPosition == 1)
                            CursorPosition = 7;
                        else CursorPosition--;
                    }
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if (CursorPosition == 7)
                            CursorPosition = 1;
                        else CursorPosition++;
                    }
                    if (key.Key == ConsoleKey.Enter ||  key.Key == ConsoleKey.Spacebar)
                    {
                        if (Board.Arr[0, CursorPosition-1] == 0) { 
                        msg.Add($"{currentPlayer} dropped at column {CursorPosition}.");
                        DropToken(CursorPosition-1);
                        }
                        else
                        msg.Add($"Player{currentPlayer} please chose another column. Column {CursorPosition} is full!");
                    }
                    if (key.Key == ConsoleKey.Escape) {
                        CursorPosition= 1;
                        ClearScreen();
                        DisplayMainMenu();
                    }
                }
            }
        }

        public static void DropToken(int pos)
        {
                for (int i=5; i>=0; i--)
                {
                    if (Board.Arr[i, pos] == 0)
                    { 
                        Board.Arr[i, pos] = currentPlayer.TokenType;
                        break;
                    }
                }
            ChangePlayer();
        }

        public static void ChangePlayer()
        {
            if (currentPlayer == P1)
                currentPlayer = P2;
            else currentPlayer = P1;
        }

        // Main Menu 
        public static void DisplayMainMenu()
        {
            GameScreen = "MainMenu";
            // Local variables to store long strings.
            string gameTitle = "- - - - CONNECT FOUR - - - -";
            string choice1 = "NEW GAME";
            string choice2 = "OPTIONS";
            string choice3 = "EXIT";
            string credits1 = "2023 - Willian P. Munhoz";
            string credits2 = "github.com/theWillPM";

            //Reset the console cursor to the top-left corner.
            Console.SetCursorPosition(0, 0);

            // Center content horizontaly
            int c1_pos = (Console.WindowWidth - choice1.Length) / 2 -3;
            int c2_pos = (Console.WindowWidth - choice2.Length) / 2 -3;
            int c3_pos = (Console.WindowWidth - choice3.Length) / 2 -4;
            int credits1_pos = (Console.WindowWidth - credits1.Length) / 2;
            int credits2_pos = (Console.WindowWidth - credits2.Length) / 2;

            // Display game title on line#3
            Console.SetCursorPosition(0, 2);
            Console.SetCursorPosition((Console.WindowWidth - gameTitle.Length) / 2, Console.CursorTop);
            Console.WriteLine(gameTitle);

            // Go to line#5
            Console.SetCursorPosition(0, 4);


            // Draw the indicator arrow besides "New Game" (option 1)
            if (CursorPosition == 1)
            {
                // Blink the indicator arrow in a subtle frequency.
                if (CurrentFrame % 20 > 5) {
                    choice1 = "-> " + choice1;
                    Console.SetCursorPosition(c1_pos, Console.CursorTop);
                    Console.WriteLine($"{choice1}\n\n");
                }
                else
                {
                    Console.SetCursorPosition(c1_pos, Console.CursorTop);
                    Console.WriteLine($"   {choice1}\n\n");
                }
                Console.SetCursorPosition(c2_pos, Console.CursorTop);
                Console.WriteLine($"   {choice2}\n\n");
                Console.SetCursorPosition(c3_pos, Console.CursorTop);
                Console.WriteLine($"   {choice3}\n\n");
            }
            // Draw the indicator arrow besides "Options" (option 1)
            else if (CursorPosition == 2)
            {
                Console.SetCursorPosition(c1_pos, Console.CursorTop);
                Console.WriteLine($"   {choice1}\n\n");
                if (CurrentFrame % 20 > 5)
                {
                    choice2 = "-> " + choice2;
                    Console.SetCursorPosition(c2_pos, Console.CursorTop);
                    Console.WriteLine($"{choice2}\n\n");
                }
                else
                {
                    Console.SetCursorPosition(c2_pos, Console.CursorTop);
                    Console.WriteLine($"   {choice2}\n\n");
                }
                Console.SetCursorPosition(c3_pos, Console.CursorTop);
                Console.WriteLine($"   {choice3}\n\n");
            }
            else if (CursorPosition == 3)
            {
                Console.SetCursorPosition(c1_pos, Console.CursorTop);
                Console.WriteLine($"   {choice1}\n\n");
                Console.SetCursorPosition(c2_pos, Console.CursorTop);
                Console.WriteLine($"   {choice2}\n\n");
                if (CurrentFrame % 20 > 5)
                {
                    choice3 = "-> " + choice3;
                    Console.SetCursorPosition(c3_pos, Console.CursorTop);
                    Console.WriteLine($"{choice3}\n\n");
                }
                else
                {
                Console.SetCursorPosition(c3_pos, Console.CursorTop);
                Console.WriteLine($"   {choice3}\n\n");
                }
            }

            // Draw Credits, in dark grey, centered
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(credits1_pos, Console.CursorTop);
            Console.WriteLine(credits1);
            Console.SetCursorPosition(credits2_pos, Console.CursorTop);
            Console.WriteLine(credits2);
            Console.ForegroundColor = ConsoleColor.White;

        }

        // Changes game screen to GAME
        public static void StartGame()
        {
            GameScreen = "Game";
            Console.Clear();
        }

        // Visually represents the board's interface - score, playernames and board state
        public static void DisplayGame()
        {
            // Reset Console Cursor to top-left corner.
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"{P1.Name,10}{" ",10}{P2.Name,10}");
            Console.WriteLine($"{P1.Score,10}{" ",10}{P2.Score,10}");

            // Blink the indicator arrow in a subtle frequency.
            if (CurrentFrame % 30 > 5)
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 3);
                Console.SetCursorPosition(CursorPosition*4-2, 3);
                if (currentPlayer == P1) { 
                    Console.ForegroundColor = P1.Colour;
                    Console.WriteLine($"{P1.TokenType}");
                }
                else if (currentPlayer == P2)
                {
                    Console.ForegroundColor = P2.Colour;
                    Console.WriteLine($"{P2.TokenType}");
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
            else 
            {
                Console.SetCursorPosition(0, 4);
            }
            DrawBoard();
            ShowLog();
        }

        // keeps a log of last moves played
        public static void ShowLog()
        {
            for(int i = msg.Count-1; i >= msg.Count-5; i--)
            { 
                if(i!=msg.Count-1)
                Console.ForegroundColor = ConsoleColor.DarkGray;
                if(i>=0)
                Console.WriteLine(msg[i]);
            }
        }

        // Draws the board with the tokens in their corresponding position.
        public static void DrawBoard()
        {
            for (int i = 0; i<=5; i++) {
                Console.WriteLine("_____________________________");
                Console.Write("|");
            for (int j = 0; j<=5; j++)
                {

                    if (Board.Arr[i, j] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{Board.Arr[i, j],2} ");
                    }
                    else if (Board.Arr[i, j] == 1)
                    { 
                        Console.ForegroundColor = P1.Colour;
                        Console.Write($"{Board.Arr[i, j],2} ");
                    } else if (Board.Arr[i, j] == 2) {
                        Console.ForegroundColor = P2.Colour;
                        Console.Write($"{Board.Arr[i, j],2} ");
                    }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("|");
                }
                if (Board.Arr[i, 6] == 0)
                    Console.ForegroundColor = ConsoleColor.Black;
                else if (Board.Arr[i, 6] == 1)
                    Console.ForegroundColor = P1.Colour;
                else if(Board.Arr[i, 6] == 2)
                    Console.ForegroundColor = P2.Colour;

                Console.Write($"{Board.Arr[i, 6],2}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" |");

            }
                Console.WriteLine("-----------------------------");
        }

        public static void OpenOptions()
        {

        }

        // Handles the redrawing of the console screen, every frame.
        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Draw();
        }

        // Draws our current screen on the console app.
        public static void Draw()
        {
            // Normal frames
            if (CurrentFrame <= FramesPerSecond)
            {
                CurrentFrame++;
                if (GameScreen == "MainMenu") DisplayMainMenu();
                if (GameScreen == "Game") DisplayGame();

                // resets counter
                if (CurrentFrame >= FramesPerSecond)
                {
                    CurrentFrame = 0;
                }
            }
        }
        public static void PromptExit()
        {

            GameScreen = "Exit";
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Are you sure you want to exit?");
            Console.WriteLine("Press ESC to cancel.");
            Console.WriteLine("Press ENTER to exit.");

            ConsoleKeyInfo keyPressed;

            while (GameScreen == "Exit") {

                while(Console.KeyAvailable) 
                Console.ReadKey(false);

                 keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPress any key to exit...");

                    Console.ForegroundColor = ConsoleColor.Black;
                    Environment.Exit(0);
                }
                else if (keyPressed.Key == ConsoleKey.Escape) { 
                Console.Clear();
                DisplayMainMenu();
                    i++;
                }
            }
        }

        public static void ClearScreen()
        {
            for (int i = 1; i<= Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i-1);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
    }
}

