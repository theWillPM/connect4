namespace connect4
{
    partial class Program
    {
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
    }
}

