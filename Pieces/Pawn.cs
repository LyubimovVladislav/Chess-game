using System.Collections.Generic;
using System.Drawing;
using Chess.Events;

namespace Chess.Pieces
{
    public delegate void PawnHasPromotedEventHandler(object source, PawnPromotedEventArgs e);

    public class Pawn : Piece
    {
        public event PawnHasPromotedEventHandler OnPawnPromote;
        private bool _canDoubleMove;
        private Point _enPassantPos;
        private Point _enPassantCapture;

        public Pawn(PieceColour pieceColour, Point position) : base(pieceColour, position)
        {
            Image = AssignImage("Chess_white_pawn.png", "Chess_black_pawn.png");
            _canDoubleMove = true;
        }

        public override List<Point> PossibleMoves(Piece[,] board, Move previousMove)
        {
            var possibleMoves = new List<Point>();
            if (previousMove != null && previousMove.IsCheck)
                return possibleMoves;
            var oneCellMove = Colour == PieceColour.White ? -1 : 1;
            var twoCellMove = Colour == PieceColour.White ? -2 : 2;

            if (board[Position.X, Position.Y + oneCellMove] == null)
            {
                possibleMoves.Add(new Point(Position.X, Position.Y + oneCellMove));
                if (_canDoubleMove && board[Position.X, Position.Y + twoCellMove] == null)
                    possibleMoves.Add(new Point(Position.X, Position.Y + twoCellMove));
            }

            if (Position.X - 1 >= 0 && board[Position.X - 1, Position.Y + oneCellMove] != null)
                if (!IsFriendly(board[Position.X - 1, Position.Y + oneCellMove]))
                    possibleMoves.Add(new Point(Position.X - 1, Position.Y + oneCellMove));
            if (Position.X + 1 <= 7 && board[Position.X + 1, Position.Y + oneCellMove] != null)
                if (!IsFriendly(board[Position.X + 1, Position.Y + oneCellMove]))
                    possibleMoves.Add(new Point(Position.X + 1, Position.Y + oneCellMove));
            if (previousMove != null && previousMove.IsEnPassantPossible == true && CanEnPassantCapture(previousMove))
            {
                _enPassantPos = new Point(previousMove.EnPassantFile, Position.Y + oneCellMove);
                possibleMoves.Add(_enPassantPos);
                _enPassantCapture = new Point(previousMove.EnPassantFile, Position.Y);
            }
            else
            {
                _enPassantPos = Point.Empty;
            }

            return possibleMoves;
        }

        private bool CanEnPassantCapture(Move previousMove)
        {
            return (Position.X - 1 == previousMove.EnPassantFile || Position.X + 1 == previousMove.EnPassantFile) &&
                   previousMove.MoveTo.Y == Position.Y;
        }

        public override void Move(Point newPos, out Move move)
        {
            var twoCellMove = Colour == PieceColour.White ? -2 : 2;
            bool canEnPassant = (newPos.Y - twoCellMove) == Position.Y;
            bool isEnPassantMove = IsEnPassantMove(newPos);
            base.Move(newPos, out move);
            _canDoubleMove = false;

            if (canEnPassant)
                move.EnPassantFile = Position.X;

            if (isEnPassantMove)
                move.EnPassantCapturedCell = _enPassantCapture;

            if (Position.Y == (Colour == PieceColour.White ? 7 : 0))
            {
                OnPawnPromote?.Invoke(this, new PawnPromotedEventArgs(Position, Colour));
            }
        }

        private bool IsEnPassantMove(Point newPos)
        {
            return newPos == _enPassantPos;
        }
    }
}