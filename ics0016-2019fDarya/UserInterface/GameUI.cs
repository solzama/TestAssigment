using System;
using System.ComponentModel;
using GameEngine;

namespace UserInterface
{
    public class GameUI
    {
        private static readonly string _verticalSeparator = "|";
        private static readonly string _horizontalSeparator = "----";
        private static readonly string _centralSeparator = "+";
        private static readonly string _verticalLeftBorderMid = "\u251d";
        private static readonly string _verticalRightBorderMid = "\u2525";
        private static readonly string _verticalLeftBorderTop = "\u250f";
        private static readonly string _verticalRightBorderTop = "\u2511";
        private static readonly string _verticalLeftBorderBottom = "\u2515";
        private static readonly string _verticalRightBorderBottom = "\u2519";

        public static void PrintBoard(Game game)
        {
            Console.Clear();
            var board = game.GetBoard();
            for (int yIndex = 0; yIndex < game.BoardHeight; yIndex++)
            {
                var line = "";
                for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                {
                    if (xIndex == 0 && yIndex == 0)
                    {
                        line += _verticalLeftBorderTop;
                    } else if (xIndex == 0 && yIndex != 0)
                    {
                        line += _verticalLeftBorderMid;
                    }
                    line += _horizontalSeparator;
                    if (xIndex < game.BoardWidth - 1)
                    {
                        line += _centralSeparator;
                    }
                    else 
                    {
                        if (yIndex == 0)
                        {
                            line += _verticalRightBorderTop;
                        }
                        else
                        {
                            line += _verticalRightBorderMid;
                        }
                    }
                }
                Console.WriteLine(line);
                
                line = "";
                for (int xIndex = 0; xIndex < game.BoardWidth; xIndex++)
                {
                    line = line + _verticalSeparator + " " + GetSingleState(board[yIndex, xIndex]) + "  ";
                    if (xIndex == game.BoardWidth - 1)
                    {
                        line = line + _verticalSeparator;
                    }
                }
                
                Console.WriteLine(line); 
            }

            // TODO Add indexes under the board
            // TODO Add saved game name above the board, if defined
            var bottom = _verticalLeftBorderBottom;
            for (int xIndex = 0; xIndex < game.BoardWidth - 1; xIndex++)
            {
                bottom += _horizontalSeparator + _centralSeparator;
            }
            bottom += _horizontalSeparator + _verticalRightBorderBottom;
            Console.WriteLine(bottom);

            string indexLine = "  ";
            for (int xIndex = 1; xIndex <= game.BoardWidth; xIndex++)
            {
                indexLine += " " + $"{xIndex}" + "   ";
            }
            Console.WriteLine(indexLine);
        }

        public static string GetSingleState(CellState state)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            
            switch (state)
            {
                case CellState.Empty:
                    return " ";
                case CellState.BlackBall:
                    return "\u25ef";
                case CellState.WhiteBall:
                    return "\u2b24";
                default:
                    throw new InvalidEnumArgumentException("Unknown item!");
            }
        }
    }
}