using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GalaSoft.MvvmLight;
using NonogramWPF.Model;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows;

namespace NonogramWPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private NonogramMatrix game = new NonogramMatrix();
        private DateTime startTime;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool IsSolved = false;

        public int GridColumns => game.Columns;
        public int GridRows => game.Rows;

        private ObservableCollection<NonogramCell> board;
        public ObservableCollection<NonogramCell> Board
        {
            get => board;
            set => Set(ref board, value);
        }

        private TimeSpan timeElapsed;
        public TimeSpan TimeElapsed
        {
            get => timeElapsed;
            set => Set(ref timeElapsed, value);
        }

        private string puzzleName = "";
        public string PuzzleName
        {
            get => puzzleName;
            set => Set(ref puzzleName, value);
        }

        public IEnumerable<string> SolutionRowConstraints
        {
            get
            {
                foreach (var constraints in game.SolutionRowConstraints)
                    yield return string.Join(" ", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public IEnumerable<string> SolutionColumnConstraints
        {
            get
            {
                foreach (var constraints in game.SolutionColumnConstraints)
                    yield return string.Join("\n", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public IEnumerable<string> RowConstraints
        {
            get
            {
                foreach (var constraints in game.RowConstraints)
                    yield return string.Join(" ", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public IEnumerable<string> ColumnConstraints
        {
            get
            {
                foreach (var constraints in game.ColumnConstraints)
                    yield return string.Join(" ", constraints.Items.Select(x => x.ToString("d")));
            }
        }

        public RelayCommand OpenPuzzle { get; }
        public RelayCommand CloseApplication { get; }

        public RelayCommand<NonogramCell> ToggleCellFilled { get; }
        public RelayCommand<NonogramCell> ToggleCellEmpty { get; }
        public RelayCommand<NonogramCell> CellMouseEnter { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            OpenPuzzle = new RelayCommand(() => OnOpenNewPuzzle());
            CloseApplication = new RelayCommand(() => OnCloseApplication());

            ToggleCellFilled = new RelayCommand<NonogramCell>((cell) =>
            {
                OnToggleCellFilled(cell);
            }, (cellState) => !IsSolved);

            ToggleCellEmpty = new RelayCommand<NonogramCell>((cell) =>
            {
                OnToggleCellEmpty(cell);
            }, (cellState) => !IsSolved);

            CellMouseEnter = new RelayCommand<NonogramCell>((cell) =>
            {
                OnCellMouseEnter(cell);
            });
        }

        private void OnOpenNewPuzzle()
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "Open new puzzle";
            ofd.ValidateNames = true;
            ofd.CheckFileExists = true;
            ofd.DefaultExt = ".xml";
            ofd.Filter = "Puzzle Files|*.xml";

            if (ofd.ShowDialog() == true)
                LoadPuzzle(ofd.FileName);
        }

        private void OnCloseApplication()
        {
            Application.Current.MainWindow.Close();
        }

        private void OnToggleCellFilled(NonogramCell cell)
        {
            if (cell.CellState == CellState.Filled)
                cell.CellState = CellState.Undetermined;
            else
                cell.CellState = CellState.Filled;

            if (game.CheckWinState())
                PuzzleSolved();
        }

        private void OnToggleCellEmpty(NonogramCell cell)
        {
            if (cell.CellState == CellState.Empty)
                cell.CellState = CellState.Undetermined;
            else
                cell.CellState = CellState.Empty;

            if (game.CheckWinState())
                PuzzleSolved();
        }

        private void OnCellMouseEnter(NonogramCell cell)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                OnToggleCellFilled(cell);
            else if(Mouse.RightButton == MouseButtonState.Pressed)
                OnToggleCellEmpty(cell);
        }

        private void LoadPuzzle(string puzzleFileName)
        {
            game = new NonogramMatrix();
            game.LoadFromXml(puzzleFileName);
            IsSolved = false;

            Board = new ObservableCollection<NonogramCell>(game.Board);
            PuzzleName = Path.GetFileNameWithoutExtension(puzzleFileName);
            RaisePropertyChanged(nameof(GridRows));
            RaisePropertyChanged(nameof(GridColumns));
            RaisePropertyChanged(nameof(SolutionRowConstraints));
            RaisePropertyChanged(nameof(SolutionColumnConstraints));

            timer.Stop();
            startTime = DateTime.Now;
            TimeElapsed = new TimeSpan(0);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void PuzzleSolved()
        {
            IsSolved = true;
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeElapsed = DateTime.Now - startTime;
        }
    }
}