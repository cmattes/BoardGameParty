using Xunit.Abstractions;

namespace UITests.Android;

[Collection("UICollection")]
public class AndroidTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AndroidTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void AppLaunches()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(AppLaunches)}.png");
    }

    [Fact]
    public void Test1()
    {
        // Arrange
        // Act
        // Assert
    }
}