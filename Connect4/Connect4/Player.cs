namespace connect4
{
    partial class Program
    {
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
    }
}

