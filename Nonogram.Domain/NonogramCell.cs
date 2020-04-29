using Stylet;

namespace Nonogram.Domain
{
    public enum CellState { Undetermined, Empty, Filled };

    public class NonogramCell : PropertyChangedBase
    {
        public NonogramCell() : this(CellState.Undetermined)
        {
        }

        public NonogramCell(CellState state)
        {
            CellState = state;
        }

        public NonogramCell(CellState state, int row, int column)
        {
            CellState = state;
            Row = row;
            Column = column;
        }

        private CellState cellState;
        public CellState CellState
        {
            get => cellState;
            set => SetAndNotify(ref cellState, value);
        }

        private int _row;
        public int Row
        {
            get => _row;
            set => SetAndNotify(ref _row, value);
        }

        private int _column;
        public int Column
        {
            get => _column;
            set => SetAndNotify(ref _column, value);
        }
    }
}
