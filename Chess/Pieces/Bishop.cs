using System.Drawing;

namespace Chess.Pieces
{
	public class Bishop : Pieces.Piece
	{
		public Bishop(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_bishop.png","Chess_black_bishop.png");
		}

		public override void Move()
		{
			throw new System.NotImplementedException();
		}
	}
}