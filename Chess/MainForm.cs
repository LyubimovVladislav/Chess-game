using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Extensions;
using Chess.Pieces;

namespace Chess
{
	public partial class MainForm : Form
	{
		private const int Offset = 20;
		private readonly ChessBoard _chessBoard;

		private struct DraggingPiece
		{
			public Piece Piece { get; set; }
			public Point UserInput { get; set; }
		};

		private DraggingPiece _draggingPiece;

		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		public MainForm()
		{
			InitializeComponent();
			_chessBoard = new ChessBoard(ChessBoard.ChessBoardFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"));
			_draggingPiece = new DraggingPiece();
			pictureBox1.Refresh();
		}

		private void DrawChessBoard(Graphics picture)
		{
			var cellSize = GetCellWidthAndHeight();
			var whiteBrush = new SolidBrush(Color.FromArgb(240, 217, 181));
			var blackBrush = new SolidBrush(Color.FromArgb(181, 136, 99));

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					var rect = new RectangleF(cellSize.WidthOffset + cellSize.Width * i,
						cellSize.HeightOffset + cellSize.Height * j,
						cellSize.Width,
						cellSize.Height);
					picture.FillRectangle((i + j) % 2 == 0 ? whiteBrush : blackBrush, rect);
				}
			}

			foreach (var cell in _chessBoard.GetEnumerable())
			{
				if (cell == null || cell.IsDragging)
					continue;
				var posX = cellSize.WidthOffset + cellSize.Width * cell.Position.Y;
				var posY = cellSize.HeightOffset + cellSize.Height * cell.Position.X;
				picture.DrawImage(cell.Image, posX, posY, cellSize.Width, cellSize.Height);
			}

			if (_draggingPiece.Piece is {IsDragging: true})
			{
				picture.DrawImage(_draggingPiece.Piece.Image,
					_draggingPiece.UserInput.X - cellSize.Width / 2f,
					_draggingPiece.UserInput.Y - cellSize.Height / 2f,
					cellSize.Width,
					cellSize.Height);
			}
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			DrawChessBoard(e.Graphics);
		}

		private void pictureBox1_ClientSizeChanged(object sender, EventArgs e)
		{
			pictureBox1.Refresh();
		}

		private Point? CalculateBoardCell(PointF userInput)
		{
			var cellSize = GetCellWidthAndHeight();
			userInput.X -= cellSize.WidthOffset;
			userInput.Y -= cellSize.HeightOffset;
			if (userInput.X < 0
			    || userInput.Y < 0
			    || userInput.X > cellSize.Width * 8
			    || userInput.Y > cellSize.Height * 8)
				return null;
			int i = 0;
			while (!(i > userInput.X / cellSize.Width - 1 && i < userInput.X / cellSize.Width))
			{
				if (i > 7)
					throw new IndexOutOfRangeException();
				i++;
			}

			int j = 0;
			while (!(j > userInput.Y / cellSize.Height - 1 && j < userInput.Y / cellSize.Height))
			{
				if (j > 7)
					throw new IndexOutOfRangeException();
				j++;
			}

			return new Point(j, i);
		}

		private (float Height, float Width, float WidthOffset, float HeightOffset)
			GetCellWidthAndHeight()
		{
			// ReSharper disable once JoinDeclarationAndInitializer
			float height, width;
			height = width = (Math.Min(pictureBox1.Size.Height, pictureBox1.Size.Width) - Offset) / 8f;
			var heightOffset = (pictureBox1.Size.Height - pictureBox1.Size.Width).Clamp(Offset) / 2f;
			var widthOffset = (pictureBox1.Size.Width - pictureBox1.Size.Height).Clamp(Offset) / 2f;
			return (height, width, widthOffset, heightOffset);
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			var cell = CalculateBoardCell(e.Location);
			if (!cell.HasValue || !_chessBoard.IsPlayerTurn((Point)cell))
				return;
			_draggingPiece.Piece = _chessBoard.IsDragging((Point) cell, true);
			_draggingPiece.UserInput = e.Location;


			// TODO: replace with invalidate
			pictureBox1.Refresh();
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (_draggingPiece.Piece is {IsDragging: false})
				return;
			_draggingPiece.UserInput = e.Location;

			// TODO: replace with invalidate
			pictureBox1.Refresh();
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			var cell = CalculateBoardCell(e.Location);
			if (!cell.HasValue || _draggingPiece.Piece is {IsDragging:false})
				return;
			_chessBoard.Capture((Point) cell, _draggingPiece.Piece);

			// TODO: replace with invalidate
			pictureBox1.Refresh();
		}
	}
}