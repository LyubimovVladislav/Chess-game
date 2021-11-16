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
					case 'b':
						chessBoard[i, j] = new Bishop(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'B':
						chessBoard[i, j] = new Bishop(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'k':
						chessBoard[i, j] = new King(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'K':
						chessBoard[i, j] = new King(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'n':
						chessBoard[i, j] = new Knight(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'N':
						chessBoard[i, j] = new Knight(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'p':
						chessBoard[i, j] = new Pawn(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'P':
						chessBoard[i, j] = new Pawn(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'q':
						chessBoard[i, j] = new Queen(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'Q':
						chessBoard[i, j] = new Queen(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'r':
						chessBoard[i, j] = new Rook(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'R':
						chessBoard[i, j] = new Rook(Piece.PieceColour.White, new Point(i, j));
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

		public void Capture(Point newPos, Piece piece)
		{
			if (piece == null)
				return;
			if (piece.Position == newPos)
			{
				piece.IsDragging = false;
				return;
			}
			_board[piece.Position.X, piece.Position.Y] = null;
			piece.Position = newPos;
			_board[newPos.X, newPos.Y] = piece;
			piece.IsDragging = false;
			ChangeTurn();
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