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
		private const int MinOffset = 20;
		private const float FontScale = 0.2f;
		private const float CharIndexOffsetX = 0.14f;
		private const float CharIndexOffsetY = 0.22f;
		private const float IntIndexOffsetX = 0.01f;
		private const float IntIndexOffsetY = 0.01f;
		private const string FontName = "Times New Roman";
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
			_chessBoard =
				new ChessBoard(FenNotation.ChessBoardFromFenNotation("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"));
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
			var font = new Font(FontName, cellSize.Width * (FontScale * 0.7f));

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					var rect = new RectangleF(cellSize.WidthOffset + cellSize.Width * i,
						cellSize.HeightOffset + cellSize.Height * j,
						cellSize.Width,
						cellSize.Height);
					picture.FillRectangle((i + j) % 2 == 0 ? whiteBrush : blackBrush, rect);
					if (j == 7)
					{
						var numberRect = new PointF(
							cellSize.WidthOffset + cellSize.Width * i + cellSize.Width * (1 - CharIndexOffsetX),
							cellSize.HeightOffset + cellSize.Height * j + cellSize.Width * (1 - CharIndexOffsetY));
						picture.DrawString(char.ToString((char) ('a' + i)), font,
							(i + j) % 2 == 0 ? blackBrush : whiteBrush, numberRect);
					}

					if (i == 0)
					{
						var numberRect = new PointF(
							cellSize.WidthOffset + cellSize.Width * i + cellSize.Width * IntIndexOffsetX,
							cellSize.HeightOffset + cellSize.Height * j + cellSize.Width * IntIndexOffsetY);
						picture.DrawString(char.ToString((char) ('1' + 7 - j)), font,
							(i + j) % 2 == 0 ? blackBrush : whiteBrush, numberRect);
					}
				}
			}

			foreach (var move in _possibleMoves)
			{
				var posX = (int) (cellSize.WidthOffset + cellSize.Width * move.X);
				var posY = (int) (cellSize.HeightOffset + cellSize.Height * move.Y);
				picture.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 0, 0)),
					new RectangleF(posX, posY, cellSize.Width, cellSize.Height));
			}

			foreach (var cell in _chessBoard.GetEnumerable())
			{
				if (cell == null || cell.IsDragging)
					continue;
				var posX = cellSize.WidthOffset + cellSize.Width * cell.Position.X;
				var posY = cellSize.HeightOffset + cellSize.Height * cell.Position.Y;
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
			height = width = (Math.Min(pictureBox1.Size.Height, pictureBox1.Size.Width) - MinOffset) / 8f;
			var heightOffset = (pictureBox1.Size.Height - pictureBox1.Size.Width).Clamp(MinOffset) / 2f;
			var widthOffset = (pictureBox1.Size.Width - pictureBox1.Size.Height).Clamp(MinOffset) / 2f;
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