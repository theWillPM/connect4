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
        static int FramesPerSecond = 60;
        static string GameScreen = "MainMenu";
        static int CursorPosition = 1;
        /// <summary>
        /// The <see cref="Board"/> class keeps track of all token positions, displays <see cref="Player"/> scores, names and colours. A <see cref="Menu"/> is also available for the user to pause, reset, exit.
        /// </summary>
        static class Board
        {
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
            static protected int PlayerCount;
            public abstract string Name { get; protected set; }
            public abstract int Score { get; protected set; }
            public abstract int TokenType { get; protected set; }
            public abstract int Colour { get; protected set; }

            // ToDo

            /// <summary>
            /// The tokens that <see cref="Player"/> drops into the <see cref="Board"/> columns. Connect 4 to win.
            /// </summary>
            protected class PlayerToken
            {
                // instantiates the tokens that occupy the board's fields.
                // add design
                // add animation
            }
        }

        /// <summary>
        /// A human <see cref="Player"/>.
        /// </summary>
        public class HumanPlayer : Player
        {
            public override string Name { get; protected set; } = $"Player_{PlayerCount}";
            public override int Score { get; protected set; } = 0;
            public override int TokenType { get; protected set; } = 1;
            public override int Colour { get; protected set; } = 1;
        }
        /// <summary>
        /// A computer <see cref="Player"/>.
        /// </summary>
        public class ComputerPlayer : Player
        {
            public override string Name { get; protected set; } = $"Computer_{PlayerCount}";
            public override int Score { get; protected set; } = 0;
            public override int TokenType { get; protected set; } = 1;
            public override int Colour { get; protected set; } = 1;
        }


        public static void Main(string[] args)
        {
            bool Exit = false;
 
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
            // To avoid extremely fast key repetition, current delay = 133 ms (every 8 frames[16.6ms each])
            if (CurrentFrame % 8 == 0) 
            { 
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                {
                    if (CursorPosition == 2)
                        CursorPosition = 1;
                    else CursorPosition = 2;
                }
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                {
                    if (CursorPosition == 1)
                        CursorPosition = 2;
                    else CursorPosition = 1;
                }
            }
        }

        // Main Menu 
        public static void DisplayMainMenu()
        {
            string gameTitle = "- - - - CONNECT FOUR - - - -";
            string choice1 = "NEW GAME";
            string choice2 = "OPTIONS";
            string credits1 = "2023 - Willian P. Munhoz";
            string credits2 = "github.com/theWillPM";

            
            int c1_pos = (Console.WindowWidth - choice1.Length) / 2;
            int c2_pos = (Console.WindowWidth - choice2.Length) / 2;
            int credits1_pos = (Console.WindowWidth - credits1.Length) / 2;
            int credits2_pos = (Console.WindowWidth - credits2.Length) / 2;

            if (GameScreen == "MainMenu") 
            {
                // Display game title
                Console.WriteLine("\n\n");
                Console.SetCursorPosition((Console.WindowWidth - gameTitle.Length)/2, Console.CursorTop);
                Console.WriteLine(gameTitle);
                Console.WriteLine("\n\n");

                // Draw the indicator arrow besides "New Game" (option 1)
                if (CursorPosition == 1)
                {
                    // Blink the indicator arrow in a subtle frequency.
                    if (CurrentFrame % 20 > 5) {
                        choice1 = "-> " + choice1;
                        Console.SetCursorPosition(c1_pos - 3, Console.CursorTop);
                        Console.WriteLine($"{choice1}\n\n");
                    } 
                    else 
                    {
                        Console.SetCursorPosition(c1_pos, Console.CursorTop);
                        Console.WriteLine($"{choice1}\n\n");
                    }
                    Console.SetCursorPosition(c2_pos, Console.CursorTop);
                    Console.WriteLine($"{choice2}\n\n\n\n");
                }
                // Draw the indicator arrow besides "Options" (option 1)
                else if (CursorPosition == 2)
                {
                    Console.SetCursorPosition(c1_pos, Console.CursorTop);
                    Console.WriteLine($"{choice1}\n\n");
                    if (CurrentFrame % 20 > 5)
                    {
                        choice2 = "-> " + choice2;
                        Console.SetCursorPosition(c2_pos - 3, Console.CursorTop);
                        Console.WriteLine($"{choice2}\n\n\n\n");
                    }
                    else
                    {
                        Console.SetCursorPosition(c2_pos, Console.CursorTop);
                        Console.WriteLine($"{choice2}\n\n\n\n");
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
        } 

        // Handles the redrawing of the console screen, every frame.
        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Draw();
        }

        // Draws our current screen on the console app.
        public static void Draw()
        {
            // Clear screen
            Console.Clear();

            // Go to next frame
            CurrentFrame++;
            
            // Normal frames
            if (CurrentFrame <= 60)
            {
                // display FPS counter
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{CurrentFrame, 3}/{FramesPerSecond,2}");
                Console.ForegroundColor = ConsoleColor.White;

                DisplayMainMenu();
            }
            // resets counter
            if (CurrentFrame >= 60)
            {
                CurrentFrame = 0;
            }
        }
    }
}

