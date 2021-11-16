using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Extensions;

namespace Chess
{
	public partial class MainForm : Form
	{
		private const int Offset = 20;

		// private ResourceManager _manager = new ResourceManager();
		private ChessBoard _chessBoard;

		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		public MainForm()
		{
			InitializeComponent();
			_chessBoard = new ChessBoard(ChessBoard.ChessBoardFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"));
			pictureBox1.Refresh();
		}

		private void DrawChessBoard(Graphics picture)
		{
			float cellHeight;
			float cellWidth;
			cellHeight = cellWidth = (Math.Min(pictureBox1.Size.Height, pictureBox1.Size.Width) - Offset) / 8f;
			var deltaWidth = (pictureBox1.Size.Height - pictureBox1.Size.Width).Clamp(Offset) / 2f;
			var deltaHeight = (pictureBox1.Size.Width - pictureBox1.Size.Height).Clamp(Offset) / 2f;
			var whiteBrush = new SolidBrush(Color.FromArgb(240, 217, 181));
			var blackBrush = new SolidBrush(Color.FromArgb(181, 136, 99));

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					var rect = new RectangleF(deltaHeight + cellWidth * i, deltaWidth + cellHeight * j, cellWidth,
						cellHeight);
					picture.FillRectangle((i + j) % 2 == 0 ? whiteBrush : blackBrush, rect);
				}
			}


			foreach (var cell in _chessBoard.GetEnumerable())
			{
				if (cell == null)
					continue;
				var posX = deltaHeight + cellWidth * cell.Position.Y;
				var posY = deltaWidth + cellHeight * cell.Position.X;
				picture.DrawImage(cell.Image, posX, posY, cellWidth, cellHeight);
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
	}
}