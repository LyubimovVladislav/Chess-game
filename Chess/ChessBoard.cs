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
			Piece[,] chessBoard = new Piece[8, 8];
			int i, j;
			i = j = 0;
			foreach (var letter in fen)
			{
				switch (letter)
				{
					case 'b' or 'B':
						chessBoard[j, i] = new Bishop(
							Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
							new Point(j, i));
						break;
					case 'k' or 'K':
						chessBoard[j, i] =
							new King(Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
								new Point(j, i));
						break;
					case 'n' or 'N':
						chessBoard[j, i] =
							new Knight(Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
								new Point(j, i));
						break;
					case 'p' or 'P':
						chessBoard[j, i] =
							new Pawn(Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
								new Point(j, i));
						break;
					case 'q' or 'Q':
						chessBoard[j, i] =
							new Queen(Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
								new Point(j, i));
						break;
					case 'r' or 'R':
						chessBoard[j, i] =
							new Rook(Char.IsLower(letter) ? Piece.PieceColour.Black : Piece.PieceColour.White,
								new Point(j, i));
						break;
					case '/':
						i++;
						j = -1;
						break;
					case var n and <= '9' and >= '1':
						j += n - '1';
						break;
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