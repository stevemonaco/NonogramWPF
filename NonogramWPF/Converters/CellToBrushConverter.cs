using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Nonogram.Domain;

namespace Nonogram.WPF.Converters
{
    class CellToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!targetType.IsAssignableFrom(typeof(Brush)))
                throw new ArgumentException($"Cannot convert from {typeof(Brush)} to {targetType}", nameof(targetType));

            if (value is NonogramCell cell)
            {
                if (cell.CellState == CellState.Undetermined)
                {
                    int index = cell.Row % 2 + cell.Column % 2;
                    if (index == 0)
                        return new SolidColorBrush(Color.FromRgb(222, 231, 255));
                    else if (index == 1)
                        return new SolidColorBrush(Color.FromRgb(239, 247, 255));
                    else if (index == 2)
                        return new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
                else if (cell.CellState == CellState.Empty)
                    return new SolidColorBrush(Colors.OrangeRed);
                else if (cell.CellState == CellState.Filled)
                    return new LinearGradientBrush(Colors.Navy, Colors.AliceBlue, 0d);
                else
                    throw new ArgumentException($"Unhandled case of {nameof(CellState)}");
            }

            throw new ArgumentException($"Cannot convert from {value.GetType()} to {typeof(NonogramCell)}", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
