using System;
using System.Collections.Generic;
using System.Drawing;

namespace Chess.Pieces
{
	public class King : Pieces.Piece
	{
		private bool _canCastle;

		public King(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			_canCastle = true;
			Image = AssignImage("Chess_white_king.png", "Chess_black_king.png");
		}

		public override List<Point> PossibleMoves(Piece[,] board)
		{
			var possibleMoves = new List<Point>();
			for (var i = -1; i <= 1; i++)
			{
				for (var j = -1; j <= 1; j++)
				{
					if (i == 0 && j == 0)
						continue;
					if (Position.X + i is >= 0 and <= 7 && Position.Y + j is >= 0 and <= 7)
					{
						if (board[Position.X + i, Position.Y + j] != null)
						{
							if (board[Position.X + i, Position.Y + j].Colour != Colour)
								possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
						}
						else
							possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
					}
				}
			}

			return possibleMoves;
		}

		public override void Move(Point newPos)
		{
			base.Move(newPos);
			_canCastle = false;
		}
	}
}