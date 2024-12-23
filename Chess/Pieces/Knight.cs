﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chess.Pieces
{
	public class Knight : Pieces.Piece
	{
		public Knight(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_knight.png", "Chess_black_knight.png");
		}

		public override List<Point> PossibleMoves(Piece[,] board)
		{
			var possibleMoves = new List<Point>();
			if (Position.X + 2 <= 7)
				MarkPossibleY(ref board, ref possibleMoves, 2);
			if (Position.X - 2 >= 0)
				MarkPossibleY(ref board, ref possibleMoves, -2);
			if (Position.Y + 2 <= 7)
				MarkPossibleX(ref board, ref possibleMoves, 2);
			if (Position.Y - 2 >= 0)
				MarkPossibleX(ref board, ref possibleMoves, -2);
			return possibleMoves;
		}

		private void MarkPossibleY(ref Piece[,] board, ref List<Point> possibleMoves, int i)
		{
			if (Position.Y + 1 <= 7)
			{
				if (board[Position.X + i, Position.Y + 1] != null)
				{
					if (board[Position.X + i, Position.Y + 1].Colour != Colour)
						possibleMoves.Add(new Point(Position.X + i, Position.Y + 1));
				}
				else
					possibleMoves.Add(new Point(Position.X + i, Position.Y + 1));
			}

			if (Position.Y - 1 >= 0)
			{
				if (board[Position.X + i, Position.Y - 1] != null)
				{
					if (board[Position.X + i, Position.Y - 1].Colour != Colour)
						possibleMoves.Add(new Point(Position.X + i, Position.Y - 1));
				}
				else
					possibleMoves.Add(new Point(Position.X + i, Position.Y - 1));
			}
		}

		private void MarkPossibleX(ref Piece[,] board, ref List<Point> possibleMoves, int i)
		{
			if (Position.X + 1 <= 7)
			{
				if (board[Position.X + 1, Position.Y + i] != null)
				{
					if (board[Position.X + 1, Position.Y + i].Colour != Colour)
						possibleMoves.Add(new Point(Position.X + 1, Position.Y + i));
				}
				else
					possibleMoves.Add(new Point(Position.X + 1, Position.Y + i));
			}


			if (Position.X - 1 >= 0)
			{
				if (board[Position.X - 1, Position.Y + i] != null)
				{
					if (board[Position.X - 1, Position.Y + i].Colour != Colour)
						possibleMoves.Add(new Point(Position.X - 1, Position.Y + i));
				}
				else
					possibleMoves.Add(new Point(Position.X - 1, Position.Y + i));
			}
		}
	}
}