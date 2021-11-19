using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chess.Pieces;

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
			_bishops = new[] {new List<Piece>(2), new List<Piece>(2)};
			_knights = new[] {new List<Piece>(2), new List<Piece>(2)};
			_pawns = new[] {new List<Piece>(8), new List<Piece>(8)};
			_queens = new[] {new List<Piece>(2), new List<Piece>(2)};
			_rooks = new[] {new List<Piece>(2), new List<Piece>(2)};
			_allLists = new[] {_bishops, _knights, _pawns, _queens, _rooks};
			_kings = new Piece[2];
			_moveHistory = new List<Move>();
			InitializeLists();
		}

		//TODO: improve this, maybe throw it in fen class
		private void InitializeLists()
		{
			foreach (var cell in _board)
			{
				switch (cell)
				{
					case null:
						continue;
					case Bishop:
						_bishops[(int) cell.Colour].Add(cell);
						break;
					case Knight:
						_knights[(int) cell.Colour].Add(cell);
						break;
					case Pawn:
						_pawns[(int) cell.Colour].Add(cell);
						break;
					case Queen:
						_queens[(int) cell.Colour].Add(cell);
						break;
					case Rook:
						_rooks[(int) cell.Colour].Add(cell);
						break;
					case King:
						_kings[(int) cell.Colour] = cell;
						break;
				}
			}
		}

		public IEnumerable<Piece> GetEnumerable()
		{
			return _board.Cast<Piece>();
		}

		public Piece IsDragging(Point point, bool status)
		{
			if (_board[point.X, point.Y] == null)
				return null;
			_board[point.X, point.Y].IsDragging = status;
			return _board[point.X, point.Y];
		}

		public List<Point> GetPossibleMoves(Point point)
		{
			if (_board[point.X, point.Y] == null)
				return new List<Point>();
			return _board[point.X, point.Y].PossibleMoves(_board);
		}

		public bool MakeMove(Point newPos, Piece piece)
		{
			if (piece == null)
				return false;
			if (!piece.PossibleMoves(_board).Contains(newPos) || piece.Position == newPos)
			{
				piece.IsDragging = false;
				return false;
			}

			_board[piece.Position.X, piece.Position.Y] = null;
			_board[newPos.X, newPos.Y] = piece;
			piece.Move(newPos);
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