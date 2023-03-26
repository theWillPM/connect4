using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Timers;
using System.Security.Cryptography;
using static connect4.Program;
using System.Transactions;
using System.Security.Principal;

namespace connect4
{
    /// <summary>
    /// The game Connect Four. 
    /// </summary>
    class Program
    {   
        static List<GameScreen> Screens = new List<GameScreen>();
        static GameScreen CurrentScreen;
        static Player CurrentPlayer;

        static GameClock Clock = new GameClock();
        static GameScreen MainMenu = new("MainMenu");
        static GameScreen Options = new("Options");
        static Board Game = new Board("Game");
        static GameScreen Exit = new("Exit");

        static List<string> msg = new List<string>();
        static bool keyboardIsAvailable = true;
        static int usedFrame = 0;

        // Game Clock:
        public class GameClock
        {
            static int CurrentFrame = 0;
            static int FramesPerSecond = 25;

            public System.Timers.Timer RefreshRate = new()
            {
                Interval = 1000 / FramesPerSecond,
                AutoReset = true,
                Enabled = true,
            };

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

            public override string ToString()
            {
                return Name;
            }

            // ToDo
            protected void AddPlayerCount()
            {
                PlayerCount++;
            }
        }

        /// <summary>
        /// A human <see cref="Player"/>.
        /// </summary>
        public class HumanPlayer : Player
        {
            public override string Name { get; protected set; } = $"Player_{PlayerCount + 1}";
            public override int Score { get; protected set; } = 0;
            public override int TokenType { get; set; } = PlayerCount + 1;
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
            public override string Name { get; protected set; } = $"Computer_{PlayerCount + 1}";
            public override int Score { get; protected set; } = 0;
            public override int TokenType { get; set; } = PlayerCount + 1;
            public override ConsoleColor Colour { get; set; } = ConsoleColor.Red;
            public ComputerPlayer()
            {
                AddPlayerCount();
            }
        }

        public interface IGameScreen
        {
            public abstract string Title { get; }
            public abstract List<string> Options { get; }
            public abstract int CursorPosition { get; }

            public static int Width {get; set;}
            public static int Height {get; set;}

        }

        public class GameScreen : IGameScreen
        {
            // Screen Title
            public string Title { get; set; }

            // List of options.
            public virtual List<string> Options { get; set; } = new List<string>();

            // To identify selected option.
            public virtual int CursorPosition { get; set; } = 0;

            public static int Width
            {
                get
                {
                    return Console.WindowWidth;
                }
                set
                {
                    try
                    {
                        Console.WindowWidth = value;
                    }
                    catch (PlatformNotSupportedException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    };
                }
            }
             public static int Height
            {
                get
                {
                    return Console.WindowHeight;
                }
                set
                {
                    try
                    {
                        Console.WindowHeight = value;
                    }
                    catch (PlatformNotSupportedException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    };
                }
             }

            public string Credits = "2023 - Willian P. Munhoz \ngithub.com/theWillPM";

            // Constructor - uses title string
            public GameScreen(string title)
            {
                Title = title;
                // Adds the newly created Game Screen to the Screens List.
                Screens.Add(this);
            }

            // Adds options to the "Options" list.
            public void AddOption(string option)
            {
                this.Options.Add(option);
            }
            public void MoveUp()
            {
                if (CursorPosition > 0)
                    CursorPosition--;
                else CursorPosition = Options.Count;
            }
            public void MoveDown()
            {
                if (CursorPosition < Options.Count - 1)
                    CursorPosition++;
                else CursorPosition = 0;
            }
            public void Interact()
            {
                // Retrieves the selected option
                var o = Options[CursorPosition];
                // matches option inside our screens list
                try
                {
                    GameScreen s = Screens.Find(screen => o.Contains(screen.Title));
                    CurrentScreen = s;
                } catch
                {
                    Exception e = new Exception("Could not find selected option. ");
                }
            }

            public string Display()
            {
                return ToString();
            }

            public override string ToString()
            {
                Console.Clear();
                Resize();
                String content = new("");

                for (int i = 0; i < Options.Count; i++)
                {
                    content += $"{Options[i]}\n";
                } 
                return content;
            }
            public virtual void Resize()
            {
                if (Width < 40)
                    Width = 40;
                if (Height < 30)
                    Height = 30;
            }   
        }

        /// <summary>
        /// The <see cref="Board"/> class keeps track of all token positions, displays <see cref="Player"/> scores, names and colours. A <see cref="Menu"/> is also available for the user to pause, reset, exit.
        /// </summary>
        public class Board : GameScreen
        {
            // Screen Title
            public string Title { get; set; }

            // List of options.
            public List<string> Options { get; set; } = new List<string>();

            // To identify selected option.
            public static int CursorPosition { get; set; } = 0;
            // keeps track of player tokens
            public static int[,] Arr = new int[6, 7];

            // Polymorphism: We can add a Board object to the Screens<GameScreen> List:
            public Board(string title) : base(title)
            {
                Title = title;
                Screens.Add(this);
            }


            // draw board
            // draw scoreboard
            // add a timer? (optional) - perhaps in the windowRefresh/Redraw function. 
            // cursor position (See if mouse control is doable)
            // menu button

            public override string ToString()
            {
                Console.Clear();
                Resize();
                String content = new("");

                for (int i = 0; i < 7; i++)
                {
                    content.Concat($"{i, 10}");
                }
                return content;

               // for (int i = 0; i <= 5; i++)
               // {
                    //        Console.WriteLine("_____________________________");
                    //        Console.Write("|");
                    //    for (int j = 0; j<=5; j++)
                    //        {

                    //            if (Board.Arr[i, j] == 0)
                    //            {
                    //                Console.ForegroundColor = ConsoleColor.Black;
                    //                Console.Write($"{Board.Arr[i, j],2} ");
                    //            }
                    //            else if (Board.Arr[i, j] == 1)
                    //            { 
                    //                Console.ForegroundColor = P1.Colour;
                    //                Console.Write($"{Board.Arr[i, j],2} ");
                    //            } else if (Board.Arr[i, j] == 2) {
                    //                Console.ForegroundColor = P2.Colour;
                    //                Console.Write($"{Board.Arr[i, j],2} ");
                    //            }
                    //                Console.ForegroundColor = ConsoleColor.White;
                    //                Console.Write("|");
                    //        }
                    //        if (Board.Arr[i, 6] == 0)
                    //            Console.ForegroundColor = ConsoleColor.Black;
                    //        else if (Board.Arr[i, 6] == 1)
                    //            Console.ForegroundColor = P1.Colour;
                    //        else if(Board.Arr[i, 6] == 2)
                    //            Console.ForegroundColor = P2.Colour;

                    //        Console.Write($"{Board.Arr[i, 6],2}");
                    //        Console.ForegroundColor = ConsoleColor.White;
                    //        Console.WriteLine(" |");

                    //    }
                    //        Console.WriteLine("-----------------------------");
            }
            public override void Resize()
            {
                if (Width < 80)
                    Width = 80;
                if (Height < 30)
                    Height = 30;
            }
        }

        public static class InputHandler
        {
            public static void HandleKey()
            {
                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    // Action buttons:
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        CurrentScreen.Interact();
                        break;

                    // Go Back / open Menu
                    case ConsoleKey.Escape:
                        CurrentScreen = MainMenu;
                        break;

                    // Move Up or Left
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        CurrentScreen.MoveUp();
                        break;

                    // Move Down or Right
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        CurrentScreen.MoveDown();
                        break;

                    case ConsoleKey.C:

                        break;


                    case ConsoleKey.X:
                        break;
                    default:
                        break;
                }
            }
        }


        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Player P1 = new HumanPlayer();
            Player P2 = new HumanPlayer();
            P2.Colour = ConsoleColor.Red;
            CurrentPlayer = P1;
            
            // Add options to Main Menu:
            MainMenu.AddOption("New Game");
            MainMenu.AddOption("Continue Game");
            MainMenu.AddOption("Options");
            MainMenu.AddOption("Exit");

            CurrentScreen = MainMenu;

            // Add seven column options to Game screen:
            for (int i = 0; i < 7; i++)
            {
                Game.AddOption($"{i}");
            }
            // Start Game - initial screen. Choose game type (vs. player or computer)



            // Enter player(s) name(s), choose token type, choose colour. Tokens and colours cannot be the same.


            Clock.RefreshRate.Elapsed += OnTimedEvent;

            static void OnTimedEvent(Object source, ElapsedEventArgs e)
            {
                Console.Write(CurrentScreen.Display());
            }



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


            while (true)
            {
                InputHandler.HandleKey();
            }
        }


        //            // Action Button - Drop Token
        //            else if (key.Key == ConsoleKey.Enter ||  key.Key == ConsoleKey.Spacebar)
        //            {
        //                keyboardIsAvailable = false;

        //                // Checks if the column has available spaces
        //                if (Board.Arr[0, CursorPosition-1] == 0) 
        //                {
        //                    // Logs action success to console.
        //                    string s = ($"{currentPlayer} dropped at column {CursorPosition}.");
        //                    msg.Add($"{s,55}");
        //                    DropToken(CursorPosition-1);
        //                }
        //                else { 
        //                    // Logs action failure to console.
        //                    string s = ($"{currentPlayer} please chose another column. Column {CursorPosition} is full!");
        //                    msg.Add($"{s,55}");
        //                }
        //            }
        //            // Return to Main Menu
        //            else if (key.Key == ConsoleKey.Escape)
        //            {
        //                keyboardIsAvailable = false;

        //                CursorPosition = 1;
        //                Console.Clear();
        //                DisplayMainMenu();
        //            }

        //            // Change Player1's colour. Does not allow same colour as P2.
        //            else if (key.Key == ConsoleKey.C)
        //            {
        //                keyboardIsAvailable = false;

        //                if (P1.Colour == ConsoleColor.White)
        //                    P1.Colour = ConsoleColor.Blue;
        //                else P1.Colour++;
        //                if (P1.Colour == P2.Colour)
        //                    P1.Colour++;
        //                msg.Add($"{P1} changed colour to {P1.Colour, 27}.");
        //            }

        //            // Change Player2's colour. Does not allow same colour as P1.
        //            else if (key.Key == ConsoleKey.X)
        //            {
        //                keyboardIsAvailable = false;

        //                if (P2.Colour == ConsoleColor.White)
        //                    P2.Colour = ConsoleColor.Blue;
        //                else P2.Colour++;
        //                if (P2.Colour == P1.Colour)
        //                    P2.Colour++;
        //                msg.Add($"{P2} changed colour to {P2.Colour, 27}.");
        //            }
        //        }
                
        //    }
        //}

        /// <summary>
        /// Current <see cref="Player"/> drops a token at the selected column.
        /// </summary>
        /// <param name="pos"></param>
        public static void DropToken(int pos)
        {
            for (int i=5; i>=0; i--)
            {
                if (Board.Arr[i, pos] == 0)
                { 
                    Board.Arr[i, pos] = CurrentPlayer.TokenType;
                    break;
                }
            }
            //ChangePlayer();
        }

        //public static void ChangePlayer()
        //{
        //    if (CurrentPlayer == P1)
        //        CurrentPlayer = P2;
        //    else CurrentPlayer = P1;
        //}

        //// Main Menu 
        //public static void DisplayMainMenu()
        //{
        //    GameScreen = "MainMenu";
        //    // Local variables to store long strings.
        //    string gameTitle = "- - - - CONNECT FOUR - - - -";
        //    string choice1 = "NEW GAME";
        //    string choice2 = "OPTIONS";
        //    string choice3 = "EXIT";
        //    string credits1 = "2023 - Willian P. Munhoz";
        //    string credits2 = "github.com/theWillPM";

        //    //Reset the console cursor to the top-left corner.
        //    Console.SetCursorPosition(0, 0);

        //    // Center content horizontaly
        //    int c1_pos = (Console.WindowWidth - choice1.Length) / 2 -3;
        //    int c2_pos = (Console.WindowWidth - choice2.Length) / 2 -3;
        //    int c3_pos = (Console.WindowWidth - choice3.Length) / 2 -4;
        //    int credits1_pos = (Console.WindowWidth - credits1.Length) / 2;
        //    int credits2_pos = (Console.WindowWidth - credits2.Length) / 2;

        //    // Display game title on line#3
        //    Console.SetCursorPosition(0, 2);
        //    Console.SetCursorPosition((Console.WindowWidth - gameTitle.Length) / 2, Console.CursorTop);
        //    Console.WriteLine(gameTitle);

        //    // Go to line#5
        //    Console.SetCursorPosition(0, 4);


        //    // Draw the indicator arrow besides "New Game" (option 1)
        //    if (CursorPosition == 1)
        //    {
        //        // Blink the indicator arrow in a subtle frequency.
        //        if (CurrentFrame % 20 > 5) {
        //            choice1 = "-> " + choice1;
        //            Console.SetCursorPosition(c1_pos, Console.CursorTop);
        //            Console.WriteLine($"{choice1}\n\n");
        //        }
        //        else
        //        {
        //            Console.SetCursorPosition(c1_pos, Console.CursorTop);
        //            Console.WriteLine($"   {choice1}\n\n");
        //        }
        //        Console.SetCursorPosition(c2_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice2}\n\n");
        //        Console.SetCursorPosition(c3_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice3}\n\n");
        //    }
        //    // Draw the indicator arrow besides "Options" (option 1)
        //    else if (CursorPosition == 2)
        //    {
        //        Console.SetCursorPosition(c1_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice1}\n\n");
        //        if (CurrentFrame % 20 > 5)
        //        {
        //            choice2 = "-> " + choice2;
        //            Console.SetCursorPosition(c2_pos, Console.CursorTop);
        //            Console.WriteLine($"{choice2}\n\n");
        //        }
        //        else
        //        {
        //            Console.SetCursorPosition(c2_pos, Console.CursorTop);
        //            Console.WriteLine($"   {choice2}\n\n");
        //        }
        //        Console.SetCursorPosition(c3_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice3}\n\n");
        //    }
        //    else if (CursorPosition == 3)
        //    {
        //        Console.SetCursorPosition(c1_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice1}\n\n");
        //        Console.SetCursorPosition(c2_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice2}\n\n");
        //        if (CurrentFrame % 20 > 5)
        //        {
        //            choice3 = "-> " + choice3;
        //            Console.SetCursorPosition(c3_pos, Console.CursorTop);
        //            Console.WriteLine($"{choice3}\n\n");
        //        }
        //        else
        //        {
        //        Console.SetCursorPosition(c3_pos, Console.CursorTop);
        //        Console.WriteLine($"   {choice3}\n\n");
        //        }
        //    }

        //    // Draw Credits, in dark grey, centered
        //    Console.ForegroundColor = ConsoleColor.DarkGray;
        //    Console.SetCursorPosition(credits1_pos, Console.CursorTop);
        //    Console.WriteLine(credits1);
        //    Console.SetCursorPosition(credits2_pos, Console.CursorTop);
        //    Console.WriteLine(credits2);
        //    Console.ForegroundColor = ConsoleColor.White;

        //}

        //// Changes game screen to GAME
        //public static void StartGame()
        //{
        //    GameScreen = "Game";
        //    Console.Clear();
        //}

        //// Visually represents the board's interface - score, playernames and board state
        //public static void DisplayGame()
        //{
        //    // Reset Console Cursor to top-left corner.
        //    Console.SetCursorPosition(0, 0);
        //    Console.ForegroundColor = ConsoleColor.White;
        //    Console.WriteLine($"{" ",35} {"CONTROLS", 28}");

        //    Console.SetCursorPosition(0, 1);
        //    Console.WriteLine($"{P1.Name,10}{" ",10}{P2.Name,10}{" ",5} {"ENTER, SPACE",28 } = Drop token");
        //    Console.WriteLine($"{P1.Score,10}{" ",10}{P2.Score,10}{" ",5} {"A, D, LeftArrow, RightArrow",28} = Change column");
        //    Console.WriteLine($"{" ",35} {"C, X",28} = Change Colour");

        //    // Blink the indicator arrow in a subtle frequency.
        //    if (CurrentFrame % 30 > 5)
        //    {
        //        Console.Write(new string(' ', Console.WindowWidth));
        //        Console.SetCursorPosition(0, 3);
        //        Console.SetCursorPosition(CursorPosition*4-2, 3);
        //        if (currentPlayer == P1) { 
        //            Console.ForegroundColor = P1.Colour;
        //            Console.WriteLine($"{P1.TokenType}");
        //        }
        //        else if (currentPlayer == P2)
        //        {
        //            Console.ForegroundColor = P2.Colour;
        //            Console.WriteLine($"{P2.TokenType}");
        //        }
        //        Console.ForegroundColor = ConsoleColor.White;

        //    }
        //    else 
        //    {
        //        Console.SetCursorPosition(0, 4);
        //    }
        //    DrawBoard();
        //    ShowLog();
        //}

        //// keeps a log of last moves played
        //public static void ShowLog()
        //{
        //    for(int i = msg.Count-1; i >= msg.Count-5; i--)
        //    { 
        //        if(i!=msg.Count-1)
        //        Console.ForegroundColor = ConsoleColor.DarkGray;
        //        if(i>=0)
        //        Console.WriteLine(msg[i]);
        //    }

        //    // Removes extra elements of the list, to save memory (If you hold a button, msg log could store almost 100 entries per second).
        //    Console.SetCursorPosition(0, 22);
        //    Console.Write(new string(' ', 5));
        //    Console.SetCursorPosition(0, 22);
        //    Console.Write(msg.Count);
        //    if (msg.Count > 5)
        //    msg.RemoveRange(0, msg.Count-5);
        //}

        //// Draws the board with the tokens in their corresponding position.
        //public static void DrawBoard()
        //{
        //    for (int i = 0; i<=5; i++) {
        //        Console.WriteLine("_____________________________");
        //        Console.Write("|");
        //    for (int j = 0; j<=5; j++)
        //        {

        //            if (Board.Arr[i, j] == 0)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Black;
        //                Console.Write($"{Board.Arr[i, j],2} ");
        //            }
        //            else if (Board.Arr[i, j] == 1)
        //            { 
        //                Console.ForegroundColor = P1.Colour;
        //                Console.Write($"{Board.Arr[i, j],2} ");
        //            } else if (Board.Arr[i, j] == 2) {
        //                Console.ForegroundColor = P2.Colour;
        //                Console.Write($"{Board.Arr[i, j],2} ");
        //            }
        //                Console.ForegroundColor = ConsoleColor.White;
        //                Console.Write("|");
        //        }
        //        if (Board.Arr[i, 6] == 0)
        //            Console.ForegroundColor = ConsoleColor.Black;
        //        else if (Board.Arr[i, 6] == 1)
        //            Console.ForegroundColor = P1.Colour;
        //        else if(Board.Arr[i, 6] == 2)
        //            Console.ForegroundColor = P2.Colour;

        //        Console.Write($"{Board.Arr[i, 6],2}");
        //        Console.ForegroundColor = ConsoleColor.White;
        //        Console.WriteLine(" |");

        //    }
        //        Console.WriteLine("-----------------------------");
        //}

        //public static void OpenOptions()
        //{

        //}

        //// Handles the redrawing of the console screen, every frame.


        //// Draws our current screen on the console app.
        //public static void Draw()
        //{
        //    Console.SetCursorPosition(0, 0);
        //    Console.Write($"{CurrentFrame % FramesPerSecond,2}, {(usedFrame % FramesPerSecond) + 8,2}");
        //    // Normal frames (0 to FPS-1)
        //    if (CurrentFrame <= FramesPerSecond)
        //    {
        //        CurrentFrame++;
        //        if (GameScreen == "MainMenu") DisplayMainMenu();
        //        if (GameScreen == "Game") DisplayGame();


        //    }
        //        // resets counter
        //        if (CurrentFrame >= FramesPerSecond)
        //            CurrentFrame = 0;
        //}
        //public static void PromptExit()
        //{

        //    GameScreen = "Exit";
        //    Console.ForegroundColor = ConsoleColor.White;

        //    Console.SetCursorPosition(0, 0);
        //    Console.WriteLine("\n\n\n\n");
        //    Console.WriteLine("Are you sure you want to exit?");
        //    Console.WriteLine("Press ESC to cancel.");
        //    Console.WriteLine("Press ENTER to exit.");

        //    ConsoleKeyInfo keyPressed;

        //    while (GameScreen == "Exit") {

        //        while(Console.KeyAvailable) 
        //        Console.ReadKey(false);

        //         keyPressed = Console.ReadKey();
        //        if (keyPressed.Key == ConsoleKey.Enter)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("\nPress any key to exit...");

        //            Console.ForegroundColor = ConsoleColor.Black;
        //            Environment.Exit(0);
        //        }
        //        else if (keyPressed.Key == ConsoleKey.Escape) { 
        //        Console.Clear();
        //        DisplayMainMenu();
        //        DisplayMainMenu();
        //        }
        //    }
        //}

        //// Experimenting with a custom ClearScreen function instead of Console.Clear();
        //public static void ClearScreen()
        //{
        //    for (int i = 1; i<= Console.WindowHeight; i++)
        //    {
        //        Console.SetCursorPosition(0, i-1);
        //        Console.Write(new string(' ', Console.WindowWidth));
        //    }
        //}
    }
}

