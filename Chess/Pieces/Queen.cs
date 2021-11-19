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


		public override List<Point> PossibleMoves(Piece[,] board,Move previousMove)
		{
			var possibleMoves = new List<Point>();
			int i = 0;
			int j = 0;
			while (Position.X + ++i <= 7 && IsAllowedToMoveTo(board[Position.X + i, Position.Y]))
			{
				possibleMoves.Add(new Point(Position.X + i, Position.Y));
			}

			i = 0;
			while (Position.X + --i >= 0 && IsAllowedToMoveTo(board[Position.X + i, Position.Y]))
			{
				possibleMoves.Add(new Point(Position.X + i, Position.Y));
			}

			i = 0;
			while (Position.Y + ++i <= 7 && IsAllowedToMoveTo(board[Position.X, Position.Y + i]))
			{
				possibleMoves.Add(new Point(Position.X, Position.Y + i));
			}

			i = 0;
			while (Position.Y + --i >= 0 && IsAllowedToMoveTo(board[Position.X, Position.Y + i]))
			{
				possibleMoves.Add(new Point(Position.X, Position.Y + i));
			}

			i = j = 0;
			while (Position.X + ++i <= 7 && Position.Y + ++j <= 7 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
				possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			i = j = 0;
			while (Position.X + --i >= 0 && Position.Y + --j >= 0 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
				possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			i = j = 0;

			while (Position.X + ++i <= 7 && Position.Y + --j >= 0 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
				possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			i = j = 0;

			while (Position.X + --i >= 0 && Position.Y + ++j <= 7 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
				possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			return possibleMoves;
		}
	}
}