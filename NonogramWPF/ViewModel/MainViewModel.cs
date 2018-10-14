using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GalaSoft.MvvmLight;
using NonogramWPF.Model;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;

namespace NonogramWPF.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private NonogramMatrix game = new NonogramMatrix();

        public int GridColumns => game.Columns;
        public int GridRows => game.Rows;
        public ObservableCollection<NonogramCell> Board { get; set; }
        public string PuzzleName { get; set; } = "";

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

        public RelayCommand<NonogramCell> ToggleCellFilled { get; }
        public RelayCommand<NonogramCell> ToggleCellEmpty { get; }

        private void OnToggleCellFilled(NonogramCell cell)
        {
            if (cell.CellState == CellState.Filled)
                cell.CellState = CellState.Undetermined;
            else
                cell.CellState = CellState.Filled;
            RaisePropertyChanged(() => Board);
        }

        private void OnToggleCellEmpty(NonogramCell cell)
        {
            if (cell.CellState == CellState.Empty)
                cell.CellState = CellState.Undetermined;
            else
                cell.CellState = CellState.Empty;
            RaisePropertyChanged(() => Board);
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            string defaultGameFile = @"D:\Projects\Nonogram\test2.xml";
            game.LoadFromXml(defaultGameFile);

            Board = new ObservableCollection<NonogramCell>(game.Board);
            PuzzleName = Path.GetFileNameWithoutExtension(defaultGameFile);

            ToggleCellFilled = new RelayCommand<NonogramCell>((cellState) =>
            {
                OnToggleCellFilled(cellState);
            });

            ToggleCellEmpty = new RelayCommand<NonogramCell>((cellState) =>
            {
                OnToggleCellEmpty(cellState);
            });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}