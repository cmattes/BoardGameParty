using System.Globalization;

namespace BoardGameParty.Converters;

public class ObjectIsNullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool) return (bool)value ? null : new object();

        return null;
    }
}