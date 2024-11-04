using System.Globalization;
using BoardGameParty.Converters;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AppTests;

public class ConverterTests
{
    private readonly ILogger<ObjectIsNullToBoolConverter> _logger = Substitute.For<ILogger<ObjectIsNullToBoolConverter>>();
    private readonly ObjectIsNullToBoolConverter _converter = new();
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    [Fact]
    public void Null_value_converts_to_true()
    {
        object? value = null;
        var targetType = typeof(bool);

        var result = _converter.Convert(value, targetType, _logger, _culture);

        Assert.True((bool)result);
    }

    [Fact]
    public void Not_null_value_converts_to_false()
    {
        var value = new object();
        var targetType = typeof(bool);

        var result = _converter.Convert(value, targetType, _logger, _culture);

        Assert.False((bool)result);
    }

    [Fact]
    public void True_boolean_value_converts_to_null()
    {
        var value = true;
        var targetType = typeof(object);

        var result = _converter.ConvertBack(value, targetType, _logger, _culture);

        Assert.Null(result);
    }

    [Fact]
    public void False_boolean_value_converts_to_not_null_value()
    {
        var value = false;
        var targetType = typeof(object);

        var result = _converter.ConvertBack(value, targetType, _logger, _culture);

        Assert.NotNull(result);
    }

    [Fact]
    public void Non_boolean_value_creates_error_and_not_null_value()
    {
        var value = "error";
        var targetType = typeof(object);
        var expectedErrorMessage = "Can't use type System.String to convert to null or new object.";

        var result = _converter.ConvertBack(value, targetType, _logger, _culture);

        Assert.Equal(1, _logger.ReceivedCalls()
            .Select(call => call.GetArguments())
            .Count(arguments => ((LogLevel)arguments[0]).Equals(LogLevel.Error) &&
                                arguments[2].ToString().Equals(expectedErrorMessage)));
        Assert.NotNull(result);
    }
}