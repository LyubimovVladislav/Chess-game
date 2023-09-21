using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		private Point _initialMousePosition;
		private List<Point> _possibleMoves;

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
			_initialMousePosition = new Point();
			pictureBox1.Refresh();
			_possibleMoves = new List<Point>();
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
				var posX = cellSize.WidthOffset + cellSize.Width * cell.Position.X;
				var posY = cellSize.HeightOffset + cellSize.Height * cell.Position.Y;
				picture.DrawImage(cell.Image, posX, posY, cellSize.Width, cellSize.Height);
			}

			var pen = new Pen(Color.DarkRed) {DashStyle = DashStyle.Solid, Width = cellSize.Width/30f};
			foreach (var move in _possibleMoves)
			{
				var posX = (int) (cellSize.WidthOffset + cellSize.Width * move.X);
				var posY = (int) (cellSize.HeightOffset + cellSize.Height * move.Y);
				picture.DrawEllipse(pen,
					new Rectangle(posX, posY, (int) cellSize.Width, (int) cellSize.Height));
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
			var i = (int) (userInput.X / cellSize.Width);
			var j = (int) (userInput.Y / cellSize.Height);

			if (i > 8 || j > 8)
				throw new ArgumentException();
			return new Point(i, j);
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
			if (e.Button == MouseButtons.Right)
			{
				CancelMove(e.Location);
				return;
			}

			_initialMousePosition = e.Location;
			var cell = CalculateBoardCell(e.Location);
			if (!cell.HasValue || !_chessBoard.IsPlayerTurn((Point) cell))
				return;
			_draggingPiece.Piece = _chessBoard.IsDragging((Point) cell, true);
			_draggingPiece.UserInput = e.Location;
			_possibleMoves = _chessBoard.GetPossibleMoves(_draggingPiece.Piece.Position);

			// InvalidatePictureBox(e.Location);
			pictureBox1.Refresh();
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (_draggingPiece.Piece is {IsDragging: false})
				return;
			_draggingPiece.UserInput = e.Location;

			InvalidatePictureBox(e.Location);
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			if (_draggingPiece.Piece is {IsDragging: false})
				return;
			var cell = CalculateBoardCell(e.Location);
			if (!cell.HasValue || !_chessBoard.MakeMove((Point) cell, _draggingPiece.Piece))
			{
				CancelMove(e.Location);
				return;
			}

			_possibleMoves = new List<Point>();
			pictureBox1.Refresh();
			// InvalidatePictureBox(e.Location);
		}

		private void InvalidatePictureBox(Point location)
		{
			var info = GetCellWidthAndHeight();
			pictureBox1.Invalidate(new Rectangle(
				(int) (location.X - info.Width),
				(int) (location.Y - info.Height),
				(int) (info.Width * 2),
				(int) (info.Height * 2)));
		}

		private void CancelMove(Point mouseLocation)
		{
			if (_draggingPiece.Piece is {IsDragging: true})
			{
				_draggingPiece.Piece.IsDragging = false;
			}

			InvalidatePictureBox(mouseLocation);
			InvalidatePictureBox(_initialMousePosition);
		}
	}
}