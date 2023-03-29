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

        static Player P1 = new HumanPlayer();
        static Player P2 = new HumanPlayer();

        static GameClock Clock = new GameClock();
        static GameScreen MainMenu = new("MainMenu");
        static GameScreen Options = new("Options");
        static GameScreen Game = new Board("Game");
        static GameScreen Exit = new("Exit");

        static string Credits = "2023 - Willian P. Munhoz \ngithub.com/theWillPM";


        // Game Clock:
        // Cannot be static because it needs an instance of System.Timers.Timer
        public class GameClock
        {
            public static int CurrentFrame = 0;
            public static int FramesPerSecond = 10;

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
            public abstract string Name { get; set; }
            public abstract int Score { get; set; }
            public abstract int TokenType { get; set; }
            public abstract ConsoleColor Colour { get; set; }

            public override string ToString()
            {
                return Name;
            }
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
            public override string Name { get; set; } = $"Player_{PlayerCount + 1}";
            public override int Score { get; set; } = 0;
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
            public override string Name { get; set; } = $"Computer_{PlayerCount + 1}";
            public override int Score { get; set; } = 0;
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
        }

        public class GameScreen : IGameScreen
        {
            // Screen Title
            public virtual string Title { get; set; }

            // List of options.
            public virtual List<string> Options { get; set; } = new List<string>();

            // To identify selected option.
            public virtual int CursorPosition { get; set; } = 0;


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
                else CursorPosition = Options.Count - 1;
            }
            public void MoveDown()
            {
                if (CursorPosition < Options.Count - 1)
                    CursorPosition++;
                else CursorPosition = 0;
            }
            public virtual void Interact()
            {
                // Retrieves the selected option
                var o = Options[CursorPosition];
                try
                {
                    // match option inside our screens list
                    GameScreen s = Screens.Find(screen => o.Contains(screen.Title));
                    CurrentScreen = s;
                    if (o == "New Game")
                    {
                        P1.Score = 0;
                        P2.Score = 0;
                        Array.Clear(Board.Arr);
                        s.CursorPosition = 0;
                    }
                } catch
                {
                    Exception e = new Exception("Could not find selected option. ");
                }
            }

            public virtual void Display()
            {
                Console.WriteLine(ToString());
            }

            public override string ToString()
            {
                String content = (
                    // Add current screen name and frame to top of the screen:
                    $"Current Screen: [{CurrentScreen.Title}] Current Frame: [{GameClock.CurrentFrame}]\n\n");
                
                // Draws current cursor position.
                for (int i = 0; i < Options.Count; i++)
                {
                    if (CursorPosition != i)
                    content += ($"{" ",3}{Options[i]} \n");
                    else content += ($"{"-> "}{Options[i]} \n");
                }

                // Bottom of screen messages:
                content += "Cursor Position: " + CursorPosition + "\n";
                return content;
            }
        }

        /// <summary>
        /// The <see cref="Board"/> class keeps track of all token positions, displays <see cref="Player"/> scores, names and colours. A <see cref="Menu"/> is also available for the user to pause, reset, exit.
        /// </summary>
        public class Board : GameScreen
        {
            static List<string> msgLog = new();
            // Screen Title
            public override string Title { get; set; }

            // List of options.
            public override List<string> Options { get; set; } = new List<string>();

            // To identify selected option.
            public override int CursorPosition { get; set; } = 0;
            // keeps track of player tokens
            public static int[,] Arr = new int[6, 7];

            // We can add a Board object to the Screens<GameScreen> List:
            public Board(string title) : base(title)
            {
                Title = title;
                Screens.Add(this);

                msgLog.Add($"Created Board name {Title}\n");
                msgLog.Add($"Sucessfully started board.\n");
                msgLog.Add($"Welcome, {P1.Name}.\n");
                msgLog.Add($"Welcome, {P2.Name}\n");
                msgLog.Add($"\n");
            }

            public override void Display()
            {
                Console.WriteLine(ToString());
            }
            public override string ToString()
            {
                string content = (
                $"Current Screen: [{CurrentScreen.Title}] Current Frame: [{GameClock.CurrentFrame}]\n\n" +
                $"{P1.Name,10} {P2.Name,10}\n" +
                $"{P1.Score,8}   {P2.Score,8}  \n\n  ");

                for (int i=0; i < CursorPosition; i++)
                {
                    content += $"{" ", 4}";
                }
                content += $"{CurrentPlayer.TokenType}\n";
                    
                for (int i = 0; i < 6; i++)
                {
                    content += $"_____________________________\n" +
                    $"|";
                    for (int j = 0; j <= 6; j++)
                    {
                        if (Board.Arr[i, j] == 0)
                            content += $"{" ",2} |";
                        else if (Board.Arr[i, j] == 1)
                            content += $"{"O",2} |";
                        else if (Board.Arr[i, j] == 2)
                            content += $"{"X",2} |";

                        if (j == 6) content += "\n";
                    }
                }
                content += $"-----------------------------\n\n";
                // Bottom of screen messages:
                content += "Cursor Position: " + CursorPosition + "\n";
                content += "Current Player: " + CurrentPlayer + "\n\n";

                for (int i = msgLog.Count - 1; i >= msgLog.Count - 1; i--)
                {
                    if (i >= 0)
                        content += msgLog[i];
                }

                return content;
            }
            public override void Interact()
            {
                for (int i = 5; i >= 0; i--)
                {
                    if (Board.Arr[i, CursorPosition] == 0 && Board.Arr[0, CursorPosition] == 0)
                    {
                        msgLog.Add($"{CurrentPlayer} dropped at column {CursorPosition}.\n");
                        Board.Arr[i, CursorPosition] = CurrentPlayer.TokenType;
                        ChangePlayer();
                        break;
                    }
                    else
                        msgLog.Add($"{CurrentPlayer} please chose another column. Column {CursorPosition} is full!\n");
                }
            }

            public void ChangePlayer()
            {
                if (CurrentPlayer == P1)
                    CurrentPlayer = P2;
                else CurrentPlayer = P1;
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

                    default:
                        break;
                }
            }
        }


        public static void Main(string[] args)
        {
            Console.CursorVisible = false;

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

            Clock.RefreshRate.Elapsed += OnTimedEvent;

            static void OnTimedEvent(Object source, ElapsedEventArgs e)
            {
                Console.Clear();
                HandleFrames();
                CurrentScreen.Display();
            }

            static void HandleFrames()
            {
                if (GameClock.CurrentFrame == GameClock.FramesPerSecond - 1)
                    GameClock.CurrentFrame = 0;
                else GameClock.CurrentFrame++;
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



            while (true)
            {
                InputHandler.HandleKey();
            }
        }
    }
}

