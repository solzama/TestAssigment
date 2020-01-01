using System;

namespace GameEngine
{
    /// <summary>
    /// Tic-Tac-Toe 
    /// </summary
    public class Game
    {
        // null, X, O
        private int layer = 0;
        private bool sameColumn = false; 
        private CellState[,] Board { get;  set; }

        public int BoardWidth { get; }
        public int BoardHeight { get; }

        private bool _playerZeroMove;
        
        public Game(GameSettings settings)
        {
            if (settings.BoardHeight < 3 || settings.BoardWidth < 3)
            {
                throw new ArgumentException("Board size has to be at least 3x3!");
            }

            BoardHeight = settings.BoardHeight;
            BoardWidth = settings.BoardWidth;
            // initialize the board
            Board = new CellState[BoardHeight, BoardWidth];
        }
        
        public CellState[,] GetBoard()
        {
            var result = new CellState[BoardHeight, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }


        public bool Move(int posY, int posX)
        {
            layer = 0;
            bool done = false;
            int count = 0;
            int i = posY;
            if (Board[posY, posX] != CellState.Empty)
            {
                while (i >= 0)
                {
                    if (Board[i, posX] != CellState.Empty)
                    {
                        layer++;
                    }
                    i = i - 1;
                }
            }
            posY = posY - layer;
            if (posY < 0)
            {
                layer = 0;
                Console.WriteLine("Choose different row!");
            }
            else
            {
                for (int xIn = 0; xIn < BoardWidth; xIn++)
                {
                    for (int yIn = 0; yIn < BoardHeight; yIn++)
                    {
                        if (Board[yIn, xIn] != CellState.Empty)
                        {
                            count++;
                        }
                    }
                }
                
                if (count >= (BoardWidth * BoardHeight)-1)
                {
                    Board[posY, posX] = _playerZeroMove ? CellState.R : CellState.Y;
                    done = true;
                }
                Board[posY, posX] = _playerZeroMove ? CellState.R : CellState.Y;
                _playerZeroMove = !_playerZeroMove;
            }
            return done;
        }

    }
}