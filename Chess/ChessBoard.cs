using Chess.Pieces;
using System.Collections.Generic;


namespace Chess
{
    public class ChessBoard
    {
        private List<Piece>[][] _allLists;
        private List<Piece>[] _bishops;
        private List<Piece>[] _knights;
        private List<Piece>[] _pawns;
        private List<Piece>[] _queens;
        private List<Piece>[] _rooks;
        private Piece[] _kings;
        private Piece[,] _board;
        private List<Move> _moveHistory;

        private Piece.PieceColour PlayerTurn { get; set; }

        public ChessBoard(Piece[,] board)
        {
            _board = board;
            PlayerTurn = Piece.PieceColour.White;
            _bishops = new[] { new List<Piece>(2), new List<Piece>(2) };
            _knights = new[] { new List<Piece>(2), new List<Piece>(2) };
            _pawns = new[] { new List<Piece>(8), new List<Piece>(8) };
            _queens = new[] { new List<Piece>(2), new List<Piece>(2) };
            _rooks = new[] { new List<Piece>(2), new List<Piece>(2) };
            _allLists = new[] { _bishops, _knights, _pawns, _queens, _rooks };
            _kings = new Piece[2];
            _moveHistory = new List<Move>();
            InitializeLists();
        }

        private void InitializeLists()
        {
            foreach (var piece in _board)
            {
                if (piece is Bishop) { _bishops[(int)piece.Colour].Add(piece); }
                if (piece is Knight) { _knights[(int)piece.Colour].Add(piece); }
                if (piece is Pawn) { _pawns[(int)piece.Colour].Add(piece); }
                if (piece is Queen) { _queens[(int)piece.Colour].Add(piece); }
                if (piece is Rook) { _rooks[(int)piece.Colour].Add(piece); }
                if (piece is King) { _kings[(int)piece.Colour] = piece; }
            }
        }

        public Piece[,] Board { get => _board; }


        private Move GetPreviousMove()
        {
            return _moveHistory.Count - 2 < 0 ? null : _moveHistory[_moveHistory.Count - 1];
        }

        public List<Point> GetPossibleMoves(Point point)
        {
            if (_board[point.X, point.Y] == null)
                return new List<Point>();

            return _board[point.X, point.Y].PossibleMoves(_board, GetPreviousMove());
        }

        public bool MakeMove(Point newPos, Piece piece)
        {
            if (piece == null)
                return false;
            if (!piece.PossibleMoves(_board, GetPreviousMove())
                .Contains(newPos) || piece.Position == newPos)
            {
                return false;
            }

            _board[piece.Position.X, piece.Position.Y] = null;
            _board[newPos.X, newPos.Y] = piece;
            piece.Move(newPos, out Move move);
            if (move.HasEnPassantCapture)
                _board[(move.EnPassantCapturedCell).X, ((Point)move.EnPassantCapturedCell).Y] = null;

            _moveHistory.Add(move);
            ChangeTurn();
            return true;
        }

        private void ChangeTurn()
        {
            PlayerTurn = PlayerTurn == Piece.PieceColour.White ? Piece.PieceColour.Black : Piece.PieceColour.White;
        }

        public bool IsPlayerTurn(Point cell)
        {
            return _board[cell.X, cell.Y] != null && _board[cell.X, cell.Y].Colour == PlayerTurn;
        }
    }
}