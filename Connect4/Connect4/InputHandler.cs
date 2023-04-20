namespace connect4
{
    partial class Program
    {
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
    }
}

