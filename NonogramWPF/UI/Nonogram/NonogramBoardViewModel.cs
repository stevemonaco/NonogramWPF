using Stylet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using NonogramWPF.Model;
using System.Linq;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;

namespace NonogramWPF.ViewModels
{
    class NonogramBoardViewModel : Screen
    {
        private NonogramMatrix _board = new NonogramMatrix();
        private bool IsSolved = false;

        private int _gridColumns;
        public int GridColumns
        {
            get => _gridColumns;
            set => SetAndNotify(ref _gridColumns, value);
        }

        private int _gridRows;
        public int GridRows
        {
            get => _gridRows;
            set => SetAndNotify(ref _gridRows, value);
        }

        private BindableCollection<NonogramCell> board;
        public BindableCollection<NonogramCell> Board
        {
            get => board;
            set => SetAndNotify(ref board, value);
        }

        private TimeSpan timeElapsed;
        public TimeSpan TimeElapsed
        {
            get => timeElapsed;
            set => SetAndNotify(ref timeElapsed, value);
        }

        private string puzzleName = "";
        public string PuzzleName
        {
            get => puzzleName;
            set => SetAndNotify(ref puzzleName, value);
        }

        public IEnumerable<string> SolutionRowConstraints
        {
            get
            {
                foreach (var constraints in _board.SolutionRowConstraints)
                    yield return string.Join(" ", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public IEnumerable<string> SolutionColumnConstraints
        {
            get
            {
                foreach (var constraints in _board.SolutionColumnConstraints)
                    yield return string.Join("\n", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public IEnumerable<string> RowConstraints
        {
            get
            {
                foreach (var constraints in _board.RowConstraints)
                    yield return string.Join(" ", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public IEnumerable<string> ColumnConstraints
        {
            get
            {
                foreach (var constraints in _board.ColumnConstraints)
                    yield return string.Join(" ", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        //public RelayCommand OpenPuzzle { get; }
        //public RelayCommand CloseApplication { get; }

        //public RelayCommand<NonogramCell> ToggleCellFilled { get; }
        //public RelayCommand<NonogramCell> ToggleCellEmpty { get; }
        //public RelayCommand<NonogramCell> CellMouseEnter { get; }

        public NonogramBoardViewModel(NonogramMatrix board)
        {
            _board = board;
            Board = new BindableCollection<NonogramCell>(_board.Board);
            GridRows = _board.Rows;
            GridColumns = _board.Columns;

            NotifyOfPropertyChange(nameof(SolutionRowConstraints));
            NotifyOfPropertyChange(nameof(SolutionColumnConstraints));
        }

        public void ToggleCellFilled(NonogramCell cell)
        {
            if (cell.CellState == CellState.Filled)
                cell.CellState = CellState.Undetermined;
            else
                cell.CellState = CellState.Filled;

            if (_board.CheckWinState())
                PuzzleSolved();
        }

        public void ToggleCellEmpty(NonogramCell cell)
        {
            if (cell.CellState == CellState.Empty)
                cell.CellState = CellState.Undetermined;
            else
                cell.CellState = CellState.Empty;

            if (_board.CheckWinState())
                PuzzleSolved();
        }

        public void CellMouseEnter(NonogramCell cell)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                ToggleCellFilled(cell);
            else if (Mouse.RightButton == MouseButtonState.Pressed)
                ToggleCellEmpty(cell);
        }

        public void PuzzleSolved()
        {
            IsSolved = true;
        }
    }
}
