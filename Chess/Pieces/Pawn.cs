using System.Collections.Generic;
using System.Drawing;
using Chess.Events;

namespace Chess.Pieces
{
	public delegate void PawnHasPromotedEventHandler(object source, PawnPromotedEventArgs e);

	public class Pawn : Pieces.Piece
	{
		public event PawnHasPromotedEventHandler OnPawnPromote;
		private bool _canDoubleMove;

		public Pawn(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_pawn.png", "Chess_black_pawn.png");
			_canDoubleMove = true;
		}

		public override List<Point> PossibleMoves(Piece[,] board)
		{
			var possibleMoves = new List<Point>();
			var oneCellMove = Colour == PieceColour.White ? -1 : 1;
			var twoCellMove = Colour == PieceColour.White ? -2 : 2;

			if (board[Position.X, Position.Y + oneCellMove] == null)
			{
				possibleMoves.Add(new Point(Position.X, Position.Y + oneCellMove));
				if (_canDoubleMove && board[Position.X, Position.Y + twoCellMove] == null)
					possibleMoves.Add(new Point(Position.X, Position.Y + twoCellMove));
			}

			if (Position.X - 1 >= 0 && board[Position.X - 1, Position.Y + oneCellMove] != null)
				if (board[Position.X - 1, Position.Y + oneCellMove].Colour != Colour)
					possibleMoves.Add(new Point(Position.X - 1, Position.Y + oneCellMove));
			if (Position.X + 1 <= 7 && board[Position.X + 1, Position.Y + oneCellMove] != null)
				if (board[Position.X + 1, Position.Y + oneCellMove].Colour != Colour)
					possibleMoves.Add(new Point(Position.X + 1, Position.Y + oneCellMove));
			return possibleMoves;
		}

		public override void Move(Point newPos)
		{
			base.Move(newPos);
			_canDoubleMove = false;

			if (Position.Y == (Colour == PieceColour.White ? 7 : 0))
			{
				OnPawnPromote?.Invoke(this, new PawnPromotedEventArgs(Position, Colour));
			}
		}
	}
}