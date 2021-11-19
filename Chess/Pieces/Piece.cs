using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Chess.Pieces
{
	public abstract class Piece
	{
		// TODO: implement directions to prevent repeating code
		public PieceColour Colour { get; private set; }
		public Point Position { get; set; }
		public Image Image { get; protected set; }

		public bool IsDragging { get; set; }

		public enum PieceColour
		{
			White,
			Black
		}

		public Piece(PieceColour pieceColour, Point position)
		{
			Colour = pieceColour;
			Position = position;
			IsDragging = false;
		}

		protected Image AssignImage(string whitePieceFileName, string blackPieceFileName)
		{
			var rootPath = Directory.GetCurrentDirectory();
			var imagePath = rootPath + (Colour == PieceColour.White
				? $"\\Resources\\Pieces\\{whitePieceFileName}"
				: $"\\Resources\\Pieces\\{blackPieceFileName}");
			return Image.FromFile(imagePath);
		}

		public abstract List<Point> PossibleMoves(Piece[,] board, Move previousMove);

		public virtual void Move(Point newPos, out Move move)
		{
			move = new Move(Position, newPos, this, Colour);
			Position = newPos;
			IsDragging = false;
		}

		protected bool IsFriendly(Piece piece)
		{
			return this.Colour == piece.Colour;
		}

		protected virtual bool IsAllowedToMoveTo(Piece piece)
		{
			return piece == null || !IsFriendly(piece);
		}
	}
}