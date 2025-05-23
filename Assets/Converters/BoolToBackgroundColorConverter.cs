using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Avalonia.Media;

namespace Organizer.Converters
{
    public class BoolToBackgroundColorConverter : IValueConverter
    {
        // U�ycie nullable reference types
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Obs�uguje null i je�li zadanie jest wykonane (IsDone == true), ustawia t�o na zielone
            if (value is bool isDone)
            {
                return isDone ? Brushes.LightGreen : Brushes.Transparent;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
