using System;

namespace GameEngine
{
    public class Game
    {
        private CellState[,] Board { get;  set; }
        
        public string GameName { get; set; }
        
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        
        private bool _playerZeroMove;
        
        public Game(GameSettings settings)
        {
            if (settings.BoardHeight < 4 || settings.BoardWidth < 4)
            {
                throw new ArgumentException("Board size has to be at least 4x4!");
            }
            
            BoardHeight = settings.BoardHeight;
            BoardWidth = settings.BoardWidth;

            GameName = settings.SaveName ?? "";

            Board = settings.StartingBoard ?? new CellState[BoardHeight, BoardWidth];
        }
        
        public CellState[,] GetBoard()
        {
            var result = new CellState[BoardHeight, BoardWidth];
            Array.Copy(Board, result, Board.Length);
            return result;
        }
        
        public (bool boardFull, bool colFull) Move(int posX)
        {
            int posY;
            var colFilled = (Board[0, posX] != CellState.Empty);
            
            for(posY = BoardHeight - 1; posY >= 0 ; posY--)
            {
                if (Board[posY, posX] == CellState.Empty && !colFilled)
                {
                    Board[posY, posX] = _playerZeroMove ? CellState.WhiteBall : CellState.BlackBall;
                    _playerZeroMove = !_playerZeroMove;
                    break;
                }
            }

            var boardFilled = true;
            
            for (posX = 0; posX < BoardWidth; posX++)
            {
                if (Board[0, posX] == CellState.Empty)
                {
                    boardFilled = false;
                    break;
                }
            }

            return (boardFilled, colFilled);
        }
        
        public static (int result, bool wasCanceled, bool menuCall) GetUserIntInput(string prompt, int min, int max, int? exitIntValue = null, string? menuCallValue = null )
        {
            do
            {
                Console.WriteLine(prompt);
                
                if (exitIntValue.HasValue || !string.IsNullOrWhiteSpace(menuCallValue))
                {
                    Console.WriteLine($"*** To exit the input prompt press {exitIntValue}" +
                                      $"{ (exitIntValue.HasValue && !string.IsNullOrWhiteSpace(menuCallValue) ? ". To go to the inner menu press " : "") }" +
                                      $"{menuCallValue}");
                }

                Console.Write(">");
                
                var inputLine = Console.ReadLine();

                if (inputLine != null && (menuCallValue != null && inputLine.ToUpper() == menuCallValue)) return (1, false, true);
                
                if (int.TryParse(inputLine, out var userInt))
                {
                    if ((userInt > max || userInt < min) && userInt != exitIntValue)
                    {
                        Console.WriteLine($"{inputLine} is not a valid parameter! Please try again");
                    }
                    else
                    {
                        return userInt == exitIntValue ? (userInt, true, false) : (userInt, false, false);
                    }
                }
                else
                {
                    Console.WriteLine($"'{inputLine}' cannot be converted into a number! Please try again");
                }
            } while (true);
        }

        public static (string result, bool wasCanceled) GetUserStringInput(string prompt, int exitIntValue)
        {
            do
            {
                Console.WriteLine(prompt);
                
                Console.WriteLine($"*** To exit the input prompt press {exitIntValue}");

                Console.Write(">");
                
                var inputLine = Console.ReadLine();

                if (int.TryParse(inputLine, out var userInt))
                {
                    if (userInt == exitIntValue) return ("", true);
                }

                return (inputLine, false);
            } while (true);
        }
    }
}