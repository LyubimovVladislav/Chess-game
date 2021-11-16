using System.Drawing;
using System.IO;

namespace Chess.Pieces
{
	public class Queen : Piece
	{
		public Queen(PieceColour pieceColour, Point position) : base(pieceColour, position)
		{
			Image = AssignImage("Chess_white_queen.png","Chess_black_queen.png");
		}
		

		public override void Move()
		{
			throw new System.NotImplementedException();
		}
	}
}