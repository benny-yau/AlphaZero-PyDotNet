using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    public class GameState
    {
        private int[] board;
        private int playerTurn;

        private int value;
        private Boolean done;

        private string id;
        private List<int> binary;
        public string Id
        {
            get
            {
                if (id == null)
                {
                    IEnumerable<int> player1_position = this.Board.Select(m => (m == 1) ? 1 : 0);
                    IEnumerable<int> other_position = this.Board.Select(m => (m == -1) ? 1 : 0);
                    StringBuilder sb = new StringBuilder();
                    foreach (int i in player1_position)
                    {
                        sb.Append(i);
                    }
                    foreach (int i in other_position)
                    {
                        sb.Append(i);
                    }
                    id = sb.ToString();
                }
                return id;
            }
        }

        public List<int> Binary
        {
            get
            {
                if (binary == null)
                {
                    List<int> currentplayer_position = this.Board.Select(m => (m == this.PlayerTurn) ? 1 : 0).ToList<int>();
                    List<int> other_position = this.Board.Select(m => (m == -this.PlayerTurn) ? 1 : 0).ToList<int>();
                    currentplayer_position.AddRange(other_position);
                    binary = currentplayer_position;
                }
                return binary;
            }
        }
        public int[] Board
        {
            get
            {
                return board;
            }
        }

        public int PlayerTurn
        {
            get
            {
                return playerTurn;
            }
            set
            {
                playerTurn = value;
            }
        }

        public List<int> AllowedActions
        {
            get
            {
                List<int> allowed = new List<int>();
                for (int i = 0; i <= this.board.Length - 1; i++)
                {
                    if (i >= this.board.Length - Game.SizeX)
                    {
                        if (this.board[i] == 0)
                            allowed.Add(i);
                    }
                    else
                    {
                        if (this.board[i] == 0 && this.board[i + Game.SizeX] != 0)
                            allowed.Add(i);
                    }
                }
                return allowed;
            }
        }

        public int Value
        {
            set
            {
                this.value = value;
            }
            get
            {
                return value;
            }
        }

        public Boolean Done
        {
            set
            {
                done = value;
            }
            get
            {
                return done;
            }
        }

        public enum BoardStatus
        {
            None,
            Win,
            Draw
        }
        
        private static int[,] winners =
        {
            {0,1,2,3},
            {1,2,3,4},
            {2,3,4,5},
            {3,4,5,6},
            {7,8,9,10},
            {8,9,10,11},
            {9,10,11,12},
            {10,11,12,13},
            {14,15,16,17},
            {15,16,17,18},
            {16,17,18,19},
            {17,18,19,20},
            {21,22,23,24},
            {22,23,24,25},
            {23,24,25,26},
            {24,25,26,27},
            {28,29,30,31},
            {29,30,31,32},
            {30,31,32,33},
            {31,32,33,34},
            {35,36,37,38},
            {36,37,38,39},
            {37,38,39,40},
            {38,39,40,41},

            {0,7,14,21},
            {7,14,21,28},
            {14,21,28,35},
            {1,8,15,22},
            {8,15,22,29},
            {15,22,29,36},
            {2,9,16,23},
            {9,16,23,30},
            {16,23,30,37},
            {3,10,17,24},
            {10,17,24,31},
            {17,24,31,38},
            {4,11,18,25},
            {11,18,25,32},
            {18,25,32,39},
            {5,12,19,26},
            {12,19,26,33},
            {19,26,33,40},
            {6,13,20,27},
            {13,20,27,34},
            {20,27,34,41},

            {3,9,15,21},
            {4,10,16,22},
            {10,16,22,28},
            {5,11,17,23},
            {11,17,23,29},
            {17,23,29,35},
            {6,12,18,24},
            {12,18,24,30},
            {18,24,30,36},
            {13,19,25,31},
            {19,25,31,37},
            {20,26,32,38},

            {3,11,19,27},
            {2,10,18,26},
            {10,18,26,34},
            {1,9,17,25},
            {9,17,25,33},
            {17,25,33,41},
            {0,8,16,24},
            {8,16,24,32},
            {16,24,32,40},
            {7,15,23,31},
            {15,23,31,39},
            {14,22,30,38},
        };

        public static Dictionary<String, String> Pieces = new Dictionary<String, String>(){ { "1", "X" }, { "0", "-" }, { "-1", "O" } };

        public GameState(int[] board, int playerTurn)
        {
            this.board = board;
            this.playerTurn = playerTurn;
        }

        private BoardStatus CheckForEndGame()
        {
            for (int i = winners.GetLowerBound(0); i <= winners.GetUpperBound(0); i++)
            {
                if (board[winners[i, 0]] + board[winners[i, 1]] + board[winners[i, 2]] + board[winners[i, 3]] == 4 * -this.playerTurn)
                    return BoardStatus.Win;
            }
            if (board.Where(m => m != 0).Count() == Game.SizeX * Game.SizeY)
                return BoardStatus.Draw;
            return BoardStatus.None;
        }

        public GameState TakeAction(int action)
        {
            int[] newBoard = new int[board.Length];
            Array.Copy(board, newBoard, board.Length);
            newBoard[action] = this.playerTurn;
            GameState newState = new GameState(newBoard, -this.playerTurn);
            BoardStatus boardStatus = newState.CheckForEndGame();
            if (boardStatus == BoardStatus.Win || boardStatus == BoardStatus.Draw)
            {
                newState.done = true;
                if (boardStatus == BoardStatus.Win)
                    newState.value = this.playerTurn;
            }
            return newState;
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('\n');
            for (int i = 0; i < Game.SizeY; i++)
            {
                sb.Append('[');
                for (int j = 0; j < Game.SizeX; j++)
                {
                    int v = this.board[i * Game.SizeX + j];
                    sb.Append("'" + Pieces[v.ToString()] + "'");
                    if (j < Game.SizeX - 1)
                        sb.Append(", ");
                }
                sb.Append(']');
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public void PrintStatus()
        {
            if (this.done)
            {
                if (value == 1)
                    Console.WriteLine("Player 1 wins.");
                else if (value == -1)
                    Console.WriteLine("Player 2 wins.");
                else
                    Console.WriteLine("Draw.");
            }
            else
                Console.WriteLine("Game in progress.");
        }

        public static void DisplayHelp()
        {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            for (int i = 0; i < Game.SizeY; i++)
            {
                sb.Append('[');
                for (int j = 0; j < Game.SizeX; j++)
                {
                    sb.Append(counter.ToString().PadLeft(2,' '));
                    if (j < Game.SizeX - 1)
                        sb.Append(", ");
                    counter += 1;
                }
                sb.Append(']');
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
