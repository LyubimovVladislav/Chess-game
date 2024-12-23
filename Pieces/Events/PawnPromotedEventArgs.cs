using System;
using System.Drawing;
using Chess.Pieces;

namespace Chess.Events
{
	public class PawnPromotedEventArgs : EventArgs
	{
		private readonly (Point position, Piece.PieceColour pieceColour) _eventInfo;
		
		public PawnPromotedEventArgs(Point position, Piece.PieceColour colour)
		{
			_eventInfo = new ValueTuple<Point, Piece.PieceColour>(position, colour);
		}
		
		public ValueTuple<Point, Piece.PieceColour> GetInfo()
		{
			return _eventInfo;
		}
	}
}