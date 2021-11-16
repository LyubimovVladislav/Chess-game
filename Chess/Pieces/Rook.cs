using System.Drawing;

namespace Chess.Pieces
{
	public class Rook : Piece
	{
		public Rook(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_rook.png","Chess_black_rook.png");
		}

		public override void Move()
		{
			throw new System.NotImplementedException();
		}
	}
}