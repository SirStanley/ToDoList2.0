using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace Organizer.Converters
{
    public class ExclamationToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var text = value as string;
            if (!string.IsNullOrEmpty(text) && text.Contains('!'))
                return Brushes.Red;
            return Brushes.Black; // albo domyślna
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
