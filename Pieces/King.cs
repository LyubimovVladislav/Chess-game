using System.Collections.Generic;

namespace Chess.Pieces
{
    public class King : Piece
    {
        private bool _canCastle;

        public King(PieceColour pieceColour, Point position) : base(pieceColour, position)
        {
            _canCastle = true;
            Image = AssignImage("Chess_white_king.png", "Chess_black_king.png");
        }

        public override List<Point> PossibleMoves(Piece[,] board, Move previousMove)
        {
            var possibleMoves = new List<Point>();
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    var x = Position.X + i;
                    var y = Position.Y + j;

                    if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
                        if (IsAllowedToMoveTo(board[Position.X + i, Position.Y + j]))
                            possibleMoves.Add(new Point(Position.X + i, Position.Y + j));
                }
            }

            return possibleMoves;
        }

        public override void Move(Point newPos, out Move move)
        {
            base.Move(newPos, out move);
            _canCastle = false;
        }
    }
}