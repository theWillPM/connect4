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
using System.Linq.Expressions;
// A lot of these dependencies aren't required as of .NET 6.0.

namespace connect4
{
    /// <summary>
    /// The game Connect Four. 
    /// </summary>
    partial class Program
    {   
        // A list of all game screens. This is used in the GameScreen.Interact() function.
        static List<GameScreen> Screens = new List<GameScreen>();
        static GameScreen CurrentScreen;
        static Player CurrentPlayer;

        // Our human players:
        static Player P1 = new HumanPlayer();
        static Player P2 = new HumanPlayer();

        // Out game clock, for handling frames and time-sensitive tasks:
        static GameClock Clock = new GameClock();

        // Creating our game screens:
        static GameScreen MainMenu = new("MainMenu");
        static GameScreen Game = new Board("Game");
        static GameScreen Exit = new("Exit");
        //static GameScreen Options = new("Options"); // To be added

        // Author
        static string Credits = "2023 - Willian P. Munhoz \n                    github.com/theWillPM";

        public static void Main(string[] args)
        {
            // Hide default console cursor.
            Console.CursorVisible = false;

            //Currently no effect for player colour, as we are printing one big string. 
            //P2.Colour = ConsoleColor.Red;
            CurrentPlayer = P1;
            
            // Add options to Main Menu:
            MainMenu.AddOption("New Game");
            MainMenu.AddOption("Continue Game");
            //MainMenu.AddOption("Options"); // TODO - Add options Menu
            MainMenu.AddOption("Exit");
            Exit.AddOption("Terminate");
            CurrentScreen = MainMenu;

            // Add seven column options to Game screen:
            for (int i = 0; i < 7; i++)
            {
                Game.AddOption($"{i}");
            }

            // Draw the introduction screen
            DrawIntro();

            // This controls what happens when 'refresh rate' elapses:
            Clock.RefreshRate.Elapsed += OnTimedEvent;
            static void OnTimedEvent(Object source, ElapsedEventArgs e)
            {
                Console.Clear();
                HandleFrames();
                try { 
                    CurrentScreen.Display();
                }
                catch(Exception ex)
                {
                    throw ex = new("Current screen not detected");
                }
            }

            // Handles the game's frames.
            static void HandleFrames()
            {
                if (GameClock.CurrentFrame == GameClock.FramesPerSecond - 1)
                    GameClock.CurrentFrame = 0;
                else GameClock.CurrentFrame++;
            }

            while (true)
            {
                InputHandler.HandleKey();
            }
        }

        // Function to draw our introduction scene
        static void DrawIntro()
        {
            string intro =  $"      ■■■■  ■■■■  ■■   ■  ■■   ■  ■■■■  ■■■■ ■■■■■      ■    ■\n" +
                            $"      ■     ■  ■  ■■   ■  ■■   ■  ■     ■      ■        ■    ■\n" +
                            $"      ■     ■  ■  ■ ■  ■  ■ ■  ■  ■     ■      ■        ■    ■\n" +
                            $"      ■     ■  ■  ■ ■  ■  ■ ■  ■  ■■■   ■      ■        ■■■■■■\n" +
                            $"      ■     ■  ■  ■  ■ ■  ■  ■ ■  ■     ■      ■             ■\n" +
                            $"      ■     ■  ■  ■  ■ ■  ■  ■ ■  ■     ■      ■             ■\n" +
                            $"      ■■■■  ■■■■  ■   ■■  ■   ■■  ■■■■  ■■■■   ■             ■ ";
            Console.SetCursorPosition(0, 5);
            for (int i = 0; i < 7; i++)
            {
                Thread.Sleep(100);
                for (int j = 0; j < 56; j++)
                {
                    Console.Write(intro[j + i * 63]);

                }
                for (int j = 56; j < 63; j++) { 
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(intro[j + i * 63]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n\n      A game by:  " + Credits);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n      Press any key to continue...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey(false);
        }
    }
}

