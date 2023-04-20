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
using System.Threading.Tasks.Sources;

namespace connect4
{
    /// <summary>
    /// The game Connect Four. 
    /// </summary>
    partial class Program
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

            // This controls what happens when 'refresh rate' elapses:
            Clock.RefreshRate.Elapsed += OnTimedEvent;

            static void OnTimedEvent(Object source, ElapsedEventArgs e)
            {
                Console.Clear();

                HandleFrames();
                CurrentScreen.Display();
            }

            // Handles the game's frames.
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

