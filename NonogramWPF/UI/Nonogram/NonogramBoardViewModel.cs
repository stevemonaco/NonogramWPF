﻿using Stylet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using Nonogram.Domain;
using System.Linq;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using Nonogram.WPF.EventModels;

namespace Nonogram.WPF.ViewModels
{
    public enum CellTransition { None, ToUndetermined, ToEmpty, ToFilled }
    class NonogramBoardViewModel : Screen
    {
        private readonly NonogramMatrix _board;
        private readonly IEventAggregator _events;
        private CellTransition _transition;

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

        private string puzzleName = "";
        public string PuzzleName
        {
            get => puzzleName;
            set => SetAndNotify(ref puzzleName, value);
        }

        private bool _isSolved;
        public bool IsSolved
        {
            get => _isSolved;
            set => SetAndNotify(ref _isSolved, value);
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

        public NonogramBoardViewModel(NonogramMatrix board, IEventAggregator events)
        {
            _board = board;
            _events = events;

            Board = new BindableCollection<NonogramCell>(_board.Board);
            GridRows = _board.Rows;
            GridColumns = _board.Columns;

            NotifyOfPropertyChange(nameof(SolutionRowConstraints));
            NotifyOfPropertyChange(nameof(SolutionColumnConstraints));
        }

        public void ToggleCellFilled(NonogramCell cell)
        {
            if (IsSolved)
                return;

            _transition = cell.CellState == CellState.Filled ? CellTransition.ToUndetermined : CellTransition.ToFilled;
            ApplyCellTransition(cell);
        }

        public void ToggleCellEmpty(NonogramCell cell)
        {
            if (IsSolved)
                return;

            _transition = cell.CellState == CellState.Empty ? CellTransition.ToUndetermined : CellTransition.ToEmpty;
            ApplyCellTransition(cell);
        }

        public void CellMouseEnter(NonogramCell cell)
        {
            if (IsSolved)
                return;

             if ((Mouse.LeftButton == MouseButtonState.Pressed ^ Mouse.RightButton == MouseButtonState.Pressed)
                && _transition != CellTransition.None)
                ApplyCellTransition(cell);
        }

        private void ApplyCellTransition(NonogramCell cell)
        {
            cell.CellState = _transition switch
            {
                CellTransition.ToUndetermined => CellState.Undetermined,
                CellTransition.ToEmpty => CellState.Empty,
                CellTransition.ToFilled => CellState.Filled,
                _ => throw new InvalidOperationException($"{nameof(ApplyCellTransition)} attempted to apply invalid transition {_transition}")
            };

            if (_board.CheckWinState())
                PuzzleSolved();
        }

        public void PuzzleSolved()
        {
            IsSolved = true;
            _events.PublishOnUIThread(new GameWinEventModel());

            foreach (var cell in Board.Where(x => x.CellState != CellState.Filled))
                cell.CellState = CellState.Undetermined;
        }
    }
}
