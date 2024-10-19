using System.Globalization;

namespace BoardGameParty.Converters;

public class ObjectNullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? new object() : null;
    }
}