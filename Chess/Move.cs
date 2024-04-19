﻿using System;
using System.Drawing;
using Chess.Pieces;

namespace Chess
{
	public class Move
	{
		public Point MoveFrom { get; }
		public Point MoveTo { get; }
		public Piece.PieceColour MoveColour { get; set; }

		public Type PieceType { get; set; }
		public Type PromotedPieceType { get; set; }
		public int EnPassantFile { get; set; }
		public bool HasEnPassantCapture => EnPassantCapturedCell != Point.Empty;
		public Point EnPassantCapturedCell { get; set; }
		public bool IsInCheck { get; set; }
		public bool IsCastlesQueenSide { get; set; }
		public bool IsCastlesKingSide { get; set; }

		public bool IsEnPassantPossible => EnPassantFile is >= 0 and <= 7;
		public bool HasPawnPromoted => PromotedPieceType != null;

		public Move(Point moveFrom, Point moveTo, Piece piece, Piece.PieceColour moveColour)
		{
			MoveFrom = moveFrom;
			MoveTo = moveTo;
			MoveColour = moveColour;
			PieceType = piece.GetType();
			PromotedPieceType = null;
			EnPassantFile = -1;
			IsInCheck = false;
			IsCastlesKingSide = false;
			IsCastlesQueenSide = false;
			EnPassantCapturedCell = Point.Empty;
		}
	}
}