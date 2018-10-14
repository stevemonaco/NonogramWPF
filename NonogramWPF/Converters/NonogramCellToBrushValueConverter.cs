using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NonogramWPF.Model;


namespace NonogramWPF.Converters
{
    class NonogramCellToBrushValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null)
                throw new ArgumentNullException(nameof(value));

            if(!(value is NonogramCell))
                throw new ArgumentException($"Cannot convert from {value.GetType()} to {typeof(NonogramCell)}", nameof(value));

            if (!targetType.IsAssignableFrom(typeof(Brush)))
                throw new ArgumentException($"Cannot convert from {typeof(Brush)} to {targetType}", nameof(targetType));

            if(value is NonogramCell cell)
            {
                if (cell.CellState == CellState.Undetermined)
                    return new SolidColorBrush(Colors.Gray);
                else if (cell.CellState == CellState.Empty)
                    return new SolidColorBrush(Colors.Aqua);
                else if (cell.CellState == CellState.Filled)
                    return new SolidColorBrush(Colors.Black);
                else
                    throw new ArgumentException($"Unhandled case of {nameof(NonogramCell)}");
            }
            else
                throw new ArgumentException($"Cannot convert from {value.GetType()} to {typeof(NonogramCell)}", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
