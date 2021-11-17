using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Chess.Pieces;

namespace Chess
{
	public class ChessBoard
	{
		private Piece[,] _board;
		private Piece.PieceColour PlayerTurn { get; set; }

		public ChessBoard(Piece[,] board)
		{
			_board = board;
			PlayerTurn = Piece.PieceColour.White;
		}

		public IEnumerable<Piece> GetEnumerable()
		{
			foreach (var cell in _board)
			{
				yield return cell;
			}
		}

		public static Piece[,] ChessBoardFromFen(string fen)
		{
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

		public Piece IsDragging(Point point, bool status)
		{
			if (_board[point.X, point.Y] == null)
				return null;
			_board[point.X, point.Y].IsDragging = status;
			return _board[point.X, point.Y];
		}

		public List<Point> GetPossibleMoves(Point point)
		{
			if (_board[point.X, point.Y] == null)
				return new List<Point>();
			return _board[point.X, point.Y].PossibleMoves(_board);
		}

		public bool MakeMove(Point newPos, Piece piece)
		{
			if (piece == null)
				return false;
			if (!piece.PossibleMoves(_board).Contains(newPos) || piece.Position == newPos)
			{
				piece.IsDragging = false;
				return false;
			}

			_board[piece.Position.X, piece.Position.Y] = null;
			_board[newPos.X, newPos.Y] = piece;
			piece.Move(newPos);
			ChangeTurn();
			return true;
		}

		private void ChangeTurn()
		{
			PlayerTurn = PlayerTurn == Piece.PieceColour.White ? Piece.PieceColour.Black : Piece.PieceColour.White;
		}

		public bool IsPlayerTurn(Point cell)
		{
			return _board[cell.X, cell.Y] != null && _board[cell.X, cell.Y].Colour == PlayerTurn;
		}
	}
}