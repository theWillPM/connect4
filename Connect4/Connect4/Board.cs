using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;

namespace connect4
{
    partial class Program
    {
        /// <summary>
        /// The <see cref="Board"/> class keeps track of all token positions, displays <see cref="Player"/> scores, names and colours. A <see cref="Menu"/> is also available for the user to pause, reset, exit.
        /// </summary>
        public class Board : GameScreen
        {
            static List<string> msgLog = new();
            // Screen Title
            public override string Title { get; set; }

            // List of options.
            public override List<string> Options { get; set; } = new List<string>();

            // To identify selected option.
            public override int CursorPosition { get; set; } = 0;
            // keeps track of player tokens
            public static int[,] Arr = new int[6, 7];

            // We can add a Board object to the Screens<GameScreen> List:
            public Board(string title) : base(title)
            {
                Title = title;
                Screens.Add(this);

                msgLog.Add($"Created Board name {Title}\n");
                msgLog.Add($"Sucessfully started \n");
                msgLog.Add($"Welcome, {P1.Name}.\n");
                msgLog.Add($"Welcome, {P2.Name}\n");
                msgLog.Add($"\n");
            }

            public override void Display()
            {
                Console.WriteLine(ToString());
            }

            // This ToString override feeds the string "content" with everything to be displayed on the screen.
            // When using multiple, different Console.Write method calls, the console application would eventually bug after a long enough execution time, resulting in scrambled display.
            // To fix this, I decided to used one single big string that contains everything to be displayed on the Console screen.
            // The drawback is I cannot <easily> use different colours for the Console output.
            public override string ToString()
            {
                // Top of screen: Screen Title, Frames, Player Names, Scores
                string content = (
                $"Current Screen: [{CurrentScreen.Title}] Current Frame: [{GameClock.CurrentFrame}]\n\n" +
                $"{P1.Name,10} (O) {P2.Name,13} (X)\n" +
                $"{P1.Score,8}   {P2.Score,15}  \n\n  ");

                // This loops represents the cursor position. Adds enough spaces to the left so that the cursor is in the proper position.
                for (int i=0; i < CursorPosition; i++)
                {
                    content += $"{" ", 4}";
                
                }
                string Token = "O";
                if (CurrentPlayer.TokenType == 1)
                    Token = "O";
                else if (CurrentPlayer.TokenType == 2)
                    Token = "X";
                content += $"{Token}\n";
                
                // This is the loop responsible for drawing the board itself. It uses Arr[,], which holds the token positions. 
                for (int i = 0; i < 6; i++)
                {
                    content += $"_____________________________\n" +
                    $"|";
                    for (int j = 0; j <= 6; j++)
                    {
                        // If field is clear
                        if (Arr[i, j] == 0)
                            content += $"{" ",2} |";
                        // if P1 has a token on the field, print "O" [todo: print p1.token and allow changing token]
                        else if (Arr[i, j] == 1)
                            content += $"{"O",2} |";
                        // ...same, P2:
                        else if (Arr[i, j] == 2)
                            content += $"{"X",2} |";
                        if (j == 6) content += "\n";
                    }
                }
                content += $"-----------------------------\n\n";

                // Bottom of screen messages:
                content += "Cursor Position: " + CursorPosition + "\n";
                content += "Current Player: " + CurrentPlayer + "\n\n";

                // Last actions log (set to last message only, can easily be changed to show more:)
                for (int i = msgLog.Count - 1; i >= msgLog.Count - 1; i--)
                {
                    if (i >= 0)
                        content += msgLog[i];
                }
                return content;
            }
            // Manages player's action
            public override void Interact()
            {   
                // check the selected column for empty slots
                for (int i = 5; i >= 0; i--)
                {   
                    // If empty
                    if (Arr[i, CursorPosition] == 0 && Arr[0, CursorPosition] == 0)
                    {
                        // drop token and change player
                        msgLog.Add($"{CurrentPlayer} dropped at Column {CursorPosition+1}, Row {5-i+1}].\n");
                        Arr[i, CursorPosition] = CurrentPlayer.TokenType;
                        if (CheckForGameEndingCondition())
                        {
                        GivePoints();
                        Console.ReadKey(false);
                        ResetBoard();

                        }
                        ChangePlayer();
                        break;
                    }
                    else // error
                        msgLog.Add($"{CurrentPlayer} please chose another column. Column {CursorPosition+1} is full!\n");
                }
            }

            // change current player
            public void ChangePlayer()
            {
                if (CurrentPlayer == P1)
                    CurrentPlayer = P2;
                else CurrentPlayer = P1;
            }
            
            public void GivePoints()
            {
                CurrentPlayer.Score++;
                msgLog.Add($"{CurrentPlayer.Name} WON ! ! --- Press any key to reset board. ");
            }

            public void ResetBoard()
            {
                Array.Clear(Arr);
            }

            // Function to check for game-ending conditions:
            // Connect 4 (Row, Column, Diagonal)
            // No more empty fields
            public bool CheckForGameEndingCondition()
            {
                if (CheckHorizontal()) return true;
                else if (CheckVertical()) return true;
                else if (CheckDiagonal1()) return true;
                else if (CheckDiagonal2()) return true;
                else if (NoMoreSlots()) return true;
                else return false;

                bool CheckHorizontal()
                {
                    for (int i = 0; i < 6; i++) 
                    {
                        for (int j=0; j < 4; j++)
                        {
                            if (Arr[i, j] != 0 && Arr[i, j] == Arr[i, j + 1] && Arr[i, j + 1] == Arr[i, j + 2] && Arr[i, j + 2] == Arr[i, j + 3])
                                return true;
                        }
                    }
                return false;
                }

                bool CheckVertical()
                {
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (Arr[j, i] != 0 && Arr[j, i] == Arr[j + 1, i] && Arr[j + 1, i] == Arr[j + 2, i] && Arr[j + 2, i] == Arr[j + 3, i])
                                return true;
                        }
                    }
                    return false;
                }
                bool CheckDiagonal1()
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (Arr[i, j] != 0 && Arr[i, j] == Arr[i + 1, j + 1] && Arr[i + 1, j + 1] == Arr[i + 2, j + 2] && Arr[i + 2, j + 2] == Arr[i + 3, j + 3])
                                return true;
                        }
                    }
                    return false;
                }
                bool CheckDiagonal2()
                {
                    for (int i = 3; i < 6; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (Arr[i, j] != 0 && Arr[i, j] == Arr[i - 1, j + 1] && Arr[i - 1, j + 1] == Arr[i - 2, j + 2] && Arr[i - 2, j + 2] == Arr[i - 3, j + 3])
                                return true;
                        }
                    }
                    return false;
                }
                bool NoMoreSlots()
                {
                    if (Arr[0, 0] != 0 && Arr[0, 1] != 0 && Arr[0, 2] != 0 && Arr[0, 3] != 0 && Arr[0, 4] != 0 && Arr[0, 5] != 0 && Arr[0, 6] != 0)
                    return true;
                    else return false;
                }
            }
        }
    }
}

