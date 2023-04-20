namespace connect4
{
    partial class Program
    {
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
    }
}

