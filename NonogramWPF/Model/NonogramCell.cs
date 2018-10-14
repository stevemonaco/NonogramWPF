using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramWPF.Model
{
    public enum CellState { Undetermined, Empty, Filled };

    public class NonogramCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private CellState cellState = CellState.Undetermined;
        public CellState CellState
        {
            get => cellState;
            set => SetField(ref cellState, value, nameof(CellState));
        }
    }
}
