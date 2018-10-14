using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NonogramWPF.Model;


namespace NonogramWPF.Converters
{
    class CellStateToBrushValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is null)
                throw new ArgumentNullException(nameof(value));

            if(!(value is CellState))
                throw new ArgumentException($"Cannot convert from {value.GetType()} to {typeof(NonogramCell)}", nameof(value));

            if (!targetType.IsAssignableFrom(typeof(Brush)))
                throw new ArgumentException($"Cannot convert from {typeof(Brush)} to {targetType}", nameof(targetType));

            if(value is CellState state)
            {
                if (state == CellState.Undetermined)
                    return new SolidColorBrush(Colors.Gray);
                else if (state == CellState.Empty)
                    return new SolidColorBrush(Colors.Aqua);
                else if (state == CellState.Filled)
                    return new SolidColorBrush(Colors.Black);
                else
                    throw new ArgumentException($"Unhandled case of {nameof(CellState)}");
            }
            else
                throw new ArgumentException($"Cannot convert from {value.GetType()} to {typeof(CellState)}", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
