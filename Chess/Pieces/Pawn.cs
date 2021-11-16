using System.Drawing;

namespace Chess.Pieces
{
	public class Pawn : Pieces.Piece
	{
		public Pawn(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_pawn.png","Chess_black_pawn.png");
		}

		public override void Move()
		{
			throw new System.NotImplementedException();
		}
	}
}