using System.Collections.Generic;
using System.Drawing;

namespace Chess.Pieces
{
	public class Rook : Piece
	{
		private bool _canCastle;

		public Rook(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			_canCastle = true;
			Image = AssignImage("Chess_white_rook.png", "Chess_black_rook.png");
		}


		public override List<Point> PossibleMoves(Piece[,] board, Move previousMove)
		{
			var possibleMoves = new List<Point>();
			int i = 0;
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

			return possibleMoves;
		}

		public override void Move(Point newPos, out Move move)
		{
			base.Move(newPos, out move);
			_canCastle = false;
		}
	}
}