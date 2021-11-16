using System.Drawing;

namespace Chess.Pieces
{
	public class King : Pieces.Piece
	{
		public King(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_king.png","Chess_black_king.png");
		}

		public override void Move()
		{
			throw new System.NotImplementedException();
		}
	}
}