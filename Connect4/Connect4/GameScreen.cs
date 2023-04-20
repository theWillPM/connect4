namespace connect4
{
    partial class Program
    {
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
                if(CurrentScreen == MainMenu)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nCredits:\n" + Credits);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            public override string ToString()
            {
                String content = (
                    // Add current screen name and frame to top of the screen:
                    $"Current Screen: [{CurrentScreen.Title}] Current Frame: [{GameClock.CurrentFrame}]\n\n");
                
                // Draws the menu and cursor on the current 'cursor position' line.
                for (int i = 0; i < Options.Count; i++)
                {
                    // if line is not current cursor position
                    if (CursorPosition != i)
                        content += ($"{" ",3}{Options[i]} \n");

                    // Blinks the cursor for visual clarity. Disappears for 1/8th of a second (125ms) every second.
                    else if (GameClock.CurrentFrame % GameClock.FramesPerSecond > GameClock.FramesPerSecond / 8)  
                        content += ($"{"-> "}{Options[i]} \n");
                    else if (GameClock.CurrentFrame % GameClock.FramesPerSecond <= GameClock.FramesPerSecond / 8)
                        content += ($"{" ",3}{Options[i]} \n");

                }

                // Bottom of screen messages:
                content += "\nCursor Position: " + CursorPosition;

                
                return content;
               
            }
        }
    }
}

