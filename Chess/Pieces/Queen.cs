using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Chess.Pieces
{
	public class Queen : Piece
	{
		public Queen(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_queen.png", "Chess_black_queen.png");
		}


		public override List<Point> PossibleMoves(Piece[,] board)
		{
			var possibleMoves = new List<Point>();
			int i = 0;
			int j = 0;
			while (Position.X + ++i <= 7)
			{
				if (!CheckPossible(ref board, ref possibleMoves, i, 0))
					break;
			}

			i = 0;
			while (Position.X + --i >= 0)
			{
				if (!CheckPossible(ref board, ref possibleMoves, i, 0))
					break;
			}

			i = 0;
			while (Position.Y + ++i <= 7)
			{
				if (!CheckPossible(ref board, ref possibleMoves, 0, i))
					break;
			}

			i = 0;
			while (Position.Y + --i >= 0)
			{
				if (!CheckPossible(ref board, ref possibleMoves, 0, i))
					break;
			}

			i = j = 0;
			while (Position.X + ++i <= 7 && Position.Y + ++j <= 7)
			{
				if (!CheckPossible(ref board, ref possibleMoves, i, j))
					break;
			}

			i = j = 0;
			while (Position.X + --i >= 0 && Position.Y + --j >= 0)
			{
				if (!CheckPossible(ref board, ref possibleMoves, i, j))
					break;
			}

			i = j = 0;

			while (Position.X + ++i <= 7 && Position.Y + --j >= 0)
			{
				if (!CheckPossible(ref board, ref possibleMoves, i, j))
					break;
			}

			i = j = 0;

			while (Position.X + --i >= 0 && Position.Y + ++j <= 7)
			{
				if (!CheckPossible(ref board, ref possibleMoves, i, j))
					break;
			}

			return possibleMoves;
		}


		private bool CheckPossible(ref Piece[,] board, ref List<Point> possibleMoves, int i, int j)
		{
			if (board[Position.X + i, Position.Y + j] == null)
				possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			else
			{
				if (board[Position.X + i, Position.Y + j].Colour != Colour)
					possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
				return false;
			}

			return true;
		}
	}
}