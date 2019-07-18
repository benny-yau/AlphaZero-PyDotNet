using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFour
{
    public class Game
    {
        public static string Name = "connect4";
        public static int SizeX = 7;
        public static int SizeY = 6;
        public static List<int> InputShape = new List<int>() { 2, SizeY, SizeX };
        public static int StateSize = SizeX * SizeY;
        public static int ActionSize = SizeX * SizeY;
        private GameState gameState;
        private List<int> lastMoves;

        public GameState GameState
        {
            get
            {
                return gameState;
            }
        }

        public List<int> LastMoves
        {
            get
            {
                if (lastMoves == null)
                    lastMoves = new List<int>();
                return lastMoves;
            }
            set
            {
                lastMoves = value;
            }
        }

        public Game()
        {
            int[] board = Enumerable.Repeat(0, SizeX * SizeY).ToArray();
            gameState = new GameState(board, 1);
        }

        public Game(Game game)
        {
            gameState = new GameState(game.GameState.Board, game.GameState.PlayerTurn);
            gameState.Done = game.GameState.Done;
            gameState.Value = game.gameState.Value;
            this.LastMoves.AddRange(game.LastMoves);
        }

        public GameState Reset()
        {
            int[] board = Enumerable.Repeat(0, SizeX * SizeY).ToArray();
            this.gameState = new GameState(board, 1);
            return this.gameState;
        }

        public void Step(int action)
        {
            GameState nextState = gameState.TakeAction(action);
            this.gameState = nextState;
            this.LastMoves.Add(action);
        }

        public List<object> Identities(GameState state, int[] actionValues)
        {
            List<object> identities = new List<object>() { new List<object>() { state, actionValues } };
            int[] currentBoard = state.Board;
            currentBoard = new int[] { currentBoard[6], currentBoard[5], currentBoard[4], currentBoard[3], currentBoard[2], currentBoard[1], currentBoard[0]
            , currentBoard[13], currentBoard[12], currentBoard[11], currentBoard[10], currentBoard[9], currentBoard[8], currentBoard[7]
            , currentBoard[20], currentBoard[19], currentBoard[18], currentBoard[17], currentBoard[16], currentBoard[15], currentBoard[14]
            , currentBoard[27], currentBoard[26], currentBoard[25], currentBoard[24], currentBoard[23], currentBoard[22], currentBoard[21]
            , currentBoard[34], currentBoard[33], currentBoard[32], currentBoard[31], currentBoard[30], currentBoard[29], currentBoard[28]
            , currentBoard[41], currentBoard[40], currentBoard[39], currentBoard[38], currentBoard[37], currentBoard[36], currentBoard[35] };

            int[] currentAV = actionValues;
            currentAV = new int[] { currentAV[6], currentAV[5], currentAV[4], currentAV[3], currentAV[2], currentAV[1], currentAV[0]
            , currentAV[13], currentAV[12], currentAV[11], currentAV[10], currentAV[9], currentAV[8], currentAV[7]
            , currentAV[20], currentAV[19], currentAV[18], currentAV[17], currentAV[16], currentAV[15], currentAV[14]
            , currentAV[27], currentAV[26], currentAV[25], currentAV[24], currentAV[23], currentAV[22], currentAV[21]
            , currentAV[34], currentAV[33], currentAV[32], currentAV[31], currentAV[30], currentAV[29], currentAV[28]
            , currentAV[41], currentAV[40], currentAV[39], currentAV[38], currentAV[37], currentAV[36], currentAV[35] };

            GameState s = new GameState(currentBoard, state.PlayerTurn);
            identities.Add(new List<object>() { s, currentAV });
            return identities;
        }


        public override string ToString()
        {
            String msg = "";
            for (int i = 0; i <= this.LastMoves.Count - 1; i++)
            {
                int p = this.LastMoves[i];
                msg += p.ToString();
                if (i < this.LastMoves.Count - 1)
                    msg += ", ";
            }
            return msg;
        }

    }
}
