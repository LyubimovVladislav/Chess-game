using System.Drawing;

namespace Chess.Pieces
{
	public class Knight : Pieces.Piece
	{
		public Knight(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_knight.png","Chess_black_knight.png");
		}

		public override void Move()
		{
			throw new System.NotImplementedException();
		}
	}
}