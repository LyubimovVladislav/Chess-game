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


		public override List<Point> PossibleMoves(Piece[,] board)
		{
			var possibleMoves = new List<Point>();
			int i = 0;
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

			return possibleMoves;
		}

		public override void Move(Point newPos)
		{
			base.Move(newPos);
			_canCastle = false;
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