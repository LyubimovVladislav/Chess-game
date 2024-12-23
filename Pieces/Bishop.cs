using System.Collections.Generic;

namespace Chess.Pieces
{
	public class Bishop : Piece
	{
		public Bishop(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_bishop.png", "Chess_black_bishop.png");
		}

		public override List<Point> PossibleMoves(Piece[,] board,Move previousMove)
		{
			var possibleMoves = new List<Point>();
			int i = 0;
			int j = 0;
			while (Position.X + ++i <= 7 && Position.Y + ++j <= 7 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
                if (IsEnemy(board[Position.X + i - 1, Position.Y + j - 1]))
                    break;
                possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			i = j = 0;
			while (Position.X + --i >= 0 && Position.Y + --j >= 0 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
                if (IsEnemy(board[Position.X + i + 1, Position.Y + j + 1]))
                    break;
                possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			i = j = 0;

			while (Position.X + ++i <= 7 && Position.Y + --j >= 0 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
                if (IsEnemy(board[Position.X + i - 1, Position.Y + j + 1]))
                    break;
                possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			i = j = 0;

			while (Position.X + --i >= 0 && Position.Y + ++j <= 7 && IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
			{
                if (IsEnemy(board[Position.X + i + 1, Position.Y + j - 1]))
                    break;
                possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
			}

			return possibleMoves;
		}
		
	}
}