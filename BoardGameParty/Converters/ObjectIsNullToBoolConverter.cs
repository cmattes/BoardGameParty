using System.Globalization;
using Microsoft.Extensions.Logging;

namespace BoardGameParty.Converters;

public class ObjectIsNullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool)
        {
            return (bool)value ? null : new object();
        }
        
        (GetParameter(parameter) ?? throw new InvalidOperationException()).LogError("Can't use type {type} to convert to null or new object.", value.GetType());
        return new object();
    }

    ILogger<ObjectIsNullToBoolConverter>? GetParameter(object parameter)
    {
        if (parameter is ILogger<ObjectIsNullToBoolConverter>)
        {
            return parameter as ILogger<ObjectIsNullToBoolConverter>;
        }
        
        return null;
    }
}