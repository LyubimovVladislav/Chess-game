using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using Chess.Pieces;
using System.Runtime.InteropServices;

namespace Chess
{
    public static class FenNotation
    {
        public static Piece[,] ChessBoardFromFenNotation(string fen)
        {
            if (!IsValidFenNotation(fen))
                throw new FormatException("Incorrect Forsyth–Edwards notation.");
            fen = fen.Trim(' ');
            var dictionary = new Dictionary<char, Func<Piece.PieceColour, Point, Piece>>()
            {
                ['b'] = (a, b) => new Bishop(a, b),
                ['k'] = (a, b) => new King(a, b),
                ['n'] = (a, b) => new Knight(a, b),
                ['p'] = (a, b) => new Pawn(a, b),
                ['q'] = (a, b) => new Queen(a, b),
                ['r'] = (a, b) => new Rook(a, b)
            };
            Piece[,] chessBoard = new Piece[8, 8];
            int i, j;
            i = j = 0;
            foreach (var letter in fen)
            {
                if (letter == '/')
                {
                    i++;
                    j = -1;
                }
                else if (letter >= '1' && letter <= '9')
                {
                    j += letter - '1';
                }
                else
                {
                    if (char.IsLetter(letter))
                        chessBoard[j, i] = dictionary[char.ToLower(letter)].Invoke(
                            Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
                            new Point(j, i));
                }

                j++;
            }

            return chessBoard;
        }

        public static bool IsValidFenNotation(string fen)
        {
            fen = fen.Trim(' ');
            bool success = Regex.Match(fen, "^([bBkKnNpPqQrR1-8]{1,8}/){7}[bBkKnNpPqQrR1-8]{1,8}$").Success;
            if (!success)
                return false;
            var split = fen.Split('/');
            return split.All(subString =>
            {
                int digitSum = Regex.Matches(subString, @"\d+").Cast<Match>().Select(match => int.Parse(match.Value)).Sum();
                int charSum = Regex.Matches(subString, @"\D").Count;
                return digitSum + charSum == 8;
            });
        }
    }
}