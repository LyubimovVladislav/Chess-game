using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chess.Pieces
{
    public abstract class Piece
    {
        public PieceColour Colour { get; private set; }
        public Point Position { get; set; }
        public Uri Image { get; protected set; }

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

        protected Uri AssignImage(string whitePieceFileName, string blackPieceFileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var imagePath = rootPath + (Colour == PieceColour.White
                ? $"\\..\\..\\..\\Pieces\\Resources\\Pieces\\{whitePieceFileName}"
                : $"\\..\\..\\..\\Pieces\\Resources\\Pieces\\{blackPieceFileName}");
            return new Uri(imagePath, UriKind.RelativeOrAbsolute);
        }

        public abstract List<Point> PossibleMoves(Piece[,] board, Move previousMove);

        public virtual void Move(Point newPos, out Move move)
        {
            move = new Move(Position, newPos, this, Colour);
            Position = newPos;
        }

        protected bool IsFriendly(Piece cell)
        {
            if (cell == null)
                return false;
            return this.Colour == cell.Colour;
        }
        protected bool IsEnemy(Piece cell)
        {
            if (cell == null)
                return false;
            return !IsFriendly(cell);
        }

        protected virtual bool IsAllowedToMoveTo(Piece cell)
        {
            return cell == null || IsEnemy(cell);
        }
    }
}