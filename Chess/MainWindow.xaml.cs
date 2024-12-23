using Chess.Pieces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChessBoard _chessBoard;
        private List<Pieces.Point> _possibleMoves;
        private float _cellWidth = 75;
        private float _cellHeight = 75;
        private float _initialOffset = 50;

        private struct DraggingPiece
        {
            public Piece Piece { get; set; }
            public bool IsDragging { get; set; }
            public System.Windows.Point InitialPos { get; set; }
            public Image ObjRef { get; set; }
        };


        private DraggingPiece _draggingPiece;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
        }
        
        private void InitializeBoard()
        {
            InitializeBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        }

        private void InitializeBoard(string fenNotation)
        {
            try
            {
                _chessBoard =
                    new ChessBoard(FenNotation.ChessBoardFromFenNotation(fenNotation));
            }
            catch (FormatException ex)
            {
                string messageBoxText = "Fen String is invalid!";
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                return;
            }
            _draggingPiece = new DraggingPiece() { IsDragging = false };
            _possibleMoves = new List<Pieces.Point>();
            RedrawChessBoard();
        }

        private void RedrawChessBoard()
        {
            var whiteBrush = new SolidColorBrush(Color.FromRgb(240, 217, 181));
            var blackBrush = new SolidColorBrush(Color.FromRgb(181, 136, 99));
            var redBrush = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
            canvas.Children.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var newRect = new Rectangle { Width = _cellWidth, Height = _cellHeight, Fill = (i + j) % 2 == 0 ? whiteBrush : blackBrush };
                    canvas.Children.Add(newRect);
                    UpdateDraggingPiecePosGrid(newRect, i, j);
                }
            }

            foreach (var move in _possibleMoves)
            {
                var newRect = new Rectangle { Width = _cellWidth, Height = _cellHeight, Fill = redBrush };
                canvas.Children.Add(newRect);
                UpdateDraggingPiecePosGrid(newRect, move);
            }

            foreach (var piece in _chessBoard.Board)
            {
                if (piece == null)
                    continue;
                
                Image img = new Image
                {
                    Width = _cellWidth,
                    Height = _cellHeight,
                    Source = new BitmapImage(piece.Image)
                };
                if (piece == _draggingPiece.Piece)
                    _draggingPiece.ObjRef = img;
                canvas.Children.Add(img);
                UpdateDraggingPiecePosGrid(img, piece.Position);
            }
        }


        private void CancelMove()
        {
            if (_draggingPiece.IsDragging == false)
                return;

            _draggingPiece.IsDragging = false;
            Canvas.SetLeft(_draggingPiece.ObjRef, _draggingPiece.InitialPos.X);
            Canvas.SetTop(_draggingPiece.ObjRef, _draggingPiece.InitialPos.Y);

        }

        private Pieces.Point? CalculateBoardCell()
        {
            System.Windows.Point pos = Mouse.GetPosition(canvas);
            pos.X -= _initialOffset;
            pos.Y -= _initialOffset;
            if (pos.X < 0
                || pos.Y < 0
                || pos.X > _cellWidth * 8
                || pos.Y > _cellHeight * 8)
                return null;
            var i = (int)(pos.X / _cellWidth);
            var j = (int)(pos.Y / _cellHeight);

            if (i > 8 || j > 8)
                throw new ArgumentException();
            return new Pieces.Point(i, j);
        }

        private void UpdateDraggingPiecePos(UIElement obj)
        {
            var posX = Mouse.GetPosition(canvas).X;
            var posY = Mouse.GetPosition(canvas).Y;
            Canvas.SetLeft(obj, posX - (_cellWidth / 2));
            Canvas.SetTop(obj, posY - (_cellHeight / 2));
        }

        private void UpdateDraggingPiecePosGrid(UIElement obj, int x, int y)
        {
            Canvas.SetLeft(obj, _initialOffset + x * _cellWidth);
            Canvas.SetTop(obj, _initialOffset + y * _cellHeight);
        }
        private void UpdateDraggingPiecePosGrid(UIElement obj, Pieces.Point pos)
        {
            UpdateDraggingPiecePosGrid(obj, pos.X, pos.Y);  
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Image))
                return;

            var cell = CalculateBoardCell();
            if (!cell.HasValue)
                return;

            var img = (Image)e.OriginalSource;

            if (!_chessBoard.IsPlayerTurn((Pieces.Point)cell))
                return;
            _draggingPiece.Piece = _chessBoard.Board[((Pieces.Point)cell).X, ((Pieces.Point)cell).Y];
            _draggingPiece.ObjRef = img;
            _draggingPiece.IsDragging = true;
            _draggingPiece.InitialPos = new System.Windows.Point(Canvas.GetLeft(img), Canvas.GetTop(img));
            _possibleMoves = _chessBoard.GetPossibleMoves(_draggingPiece.Piece.Position);
            UpdateDraggingPiecePos(img);
            RedrawChessBoard();
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draggingPiece.IsDragging == false)
                return;
            var cell = CalculateBoardCell();
            if (!cell.HasValue || !_chessBoard.MakeMove((Pieces.Point)cell, _draggingPiece.Piece))
            {
                CancelMove();
                return;
            }

            UpdateDraggingPiecePosGrid(_draggingPiece.ObjRef, (Pieces.Point)cell);
            _draggingPiece.IsDragging = false;
            _draggingPiece.Piece = null;

            _possibleMoves = new List<Pieces.Point>();
            RedrawChessBoard();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingPiece.IsDragging == false)
                return;
            UpdateDraggingPiecePos(_draggingPiece.ObjRef);
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = System.IO.Directory.GetParent(@"..\..\..\").FullName;


            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                try
                {
                    string fileContent = File.ReadAllText(selectedFilePath);
                    InitializeBoard(fileContent);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
