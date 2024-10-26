using Xunit.Abstractions;

namespace UITests;

[Collection("UICollection")]
public class MainPageTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    public MainPageTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
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