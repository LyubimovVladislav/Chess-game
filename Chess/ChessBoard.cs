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

		public ChessBoard(Piece[,] board)
		{
			_board = board;
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
			
			// rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR 
			
			Piece[,] chessBoard = new Piece[8, 8];
			int i, j;
			i = j = 0;
			foreach (var letter in fen)
			{
				switch (letter)
				{
					case 'b':
						chessBoard[i, j] = new Bishop(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'B':
						chessBoard[i, j] = new Bishop(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'k':
						chessBoard[i, j] = new King(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'K':
						chessBoard[i, j] = new King(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'n':
						chessBoard[i, j] = new Knight(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'N':
						chessBoard[i, j] = new Knight(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'p':
						chessBoard[i, j] = new Pawn(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'P':
						chessBoard[i, j] = new Pawn(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'q':
						chessBoard[i, j] = new Queen(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'Q':
						chessBoard[i, j] = new Queen(Piece.PieceColour.Black, new Point(i, j));
						break;
					case 'r':
						chessBoard[i, j] = new Rook(Piece.PieceColour.White, new Point(i, j));
						break;
					case 'R':
						chessBoard[i, j] = new Rook(Piece.PieceColour.Black, new Point(i, j));
						break;
					case '/':
						i++;
						j = -1;
						break;
					case char n when (n >= '1' && n <= '9'):
						j += n - '1';
						break;
				}

				j++;
			}
			

			return chessBoard;
		}
	}
}