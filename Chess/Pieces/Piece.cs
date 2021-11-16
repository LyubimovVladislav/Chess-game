using System.Drawing;
using System.IO;

namespace Chess.Pieces
{
	public abstract class Piece
	{
		protected PieceColour Colour { get; private set; }
		public Point Position { get; protected set; }
		public Image Image { get; protected set; }

		public enum PieceColour
		{
			White,
			Black
		}

		public Piece(PieceColour pieceColour, Point position)
		{
			Colour = pieceColour;
			Position = position;
		}

		protected Image AssignImage(string whitePieceFileName, string blackPieceFileName)
		{
			var rootPath = Directory.GetCurrentDirectory();
			var imagePath = rootPath + (Colour == PieceColour.White
				? $"\\Resources\\Pieces\\{whitePieceFileName}"
				: $"\\Resources\\Pieces\\{blackPieceFileName}");
			return Image.FromFile(imagePath);
		}

		public abstract void Move();
	}
}