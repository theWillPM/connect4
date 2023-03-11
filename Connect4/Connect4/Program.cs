using System;
using System.Runtime.CompilerServices;

namespace connect4
{
    /// <summary>
    /// The game Connect Four. 
    /// </summary>
    class Program
    {
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

            
        }
    }
}

