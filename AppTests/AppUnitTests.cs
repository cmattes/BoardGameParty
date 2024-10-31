using System.Globalization;
using BoardGameParty.Converters;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AppTests;

public class AppUnitTests
{
    private readonly ILogger<ObjectIsNullToBoolConverter> _logger = Substitute.For<ILogger<ObjectIsNullToBoolConverter>>();

    [Fact]
    public void Null_value_converts_to_true()
    {
        // Arrange
        var converter = new ObjectIsNullToBoolConverter();
        object? value = null;
        Type targetType = typeof(bool);
        object parameter = _logger;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        var result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.True((bool)result);
    }
    
    [Fact]
    public void Not_null_value_converts_to_false()
    {
        // Arrange
        var converter = new ObjectIsNullToBoolConverter();
        object? value = new object();
        Type targetType = typeof(bool);
        object parameter = _logger;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        var result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.False((bool)result);
    }
    
    [Fact]
    public void True_boolean_value_converts_to_null()
    {
        // Arrange
        var converter = new ObjectIsNullToBoolConverter();
        bool value = true;
        Type targetType = typeof(object);
        object parameter = _logger;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        var result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void False_boolean_value_converts_to_not_null_value()
    {
        // Arrange
        var converter = new ObjectIsNullToBoolConverter();
        bool value = false;
        Type targetType = typeof(object);
        object parameter = _logger;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        var result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public void Non_boolean_value_creates_error_and_not_null_value()
    {
        // Arrange
        var converter = new ObjectIsNullToBoolConverter();
        string value = "error";
        Type targetType = typeof(object);
        object parameter = _logger;
        CultureInfo culture = CultureInfo.InvariantCulture;
        var expectedErrorMessage = "Can't use type System.String to convert to null or new object.";

        // Act
        var result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.Equal(1, _logger.ReceivedCalls()
            .Select(call => call.GetArguments())
            .Count(arguments => ((LogLevel)arguments[0]).Equals(LogLevel.Error) && arguments[2].ToString().Equals(expectedErrorMessage)));
        Assert.NotNull(result);
    }
}