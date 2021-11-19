using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Chess.Pieces;

namespace Chess
{
	public static class FenNotation
	{
		//TODO : add full fen notation support

		public static Piece[,] ChessBoardFromFenNotation(string fen)
		{
			if (IsValidFenNotation(fen))
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
				switch (letter)
				{
					case '/':
						i++;
						j = -1;
						break;
					case var n and >= '1' and <= '9':
						j += n - '1';
						break;
					default:
					{
						if (char.IsLetter(letter))
							chessBoard[j, i] = dictionary[char.ToLower(letter)].Invoke(
								Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
								new Point(j, i));
						break;
					}
				}

				j++;
			}

			return chessBoard;
		}

		public static bool IsValidFenNotation(string fen)
		{
			fen = fen.Trim(' ');
			if (Regex.IsMatch(fen, "^([bBkKnNpPqQrR1-8]{1,8}/){7}[bBkKnNpPqQrR1-8]{1,8}$") == false)
				return false;
			var split = fen.Split('/');
			return split.Any(subString =>
			{
				var matches = Regex.Matches(subString, @"\d");
				var coverage = subString.Length - matches.Count +
				               matches.Cast<Match>().Sum(match => int.Parse(match.Value));
				return coverage != 8;
			});
		}
	}
}