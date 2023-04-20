namespace connect4
{
    partial class Program
    {
        // Game Clock:
        // Cannot be static because it needs an instance of System.Timers.Timer
        public class GameClock
        {
            public static int CurrentFrame = 0;
            public static int FramesPerSecond = 12;

            public System.Timers.Timer RefreshRate = new()
            {
                Interval = 1000 / FramesPerSecond,
                AutoReset = true,
                Enabled = true,
            };

        }
    }
}

