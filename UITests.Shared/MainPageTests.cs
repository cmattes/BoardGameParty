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
    public void App_Launches()
    {
        App.GetScreenshot().SaveAsFile($"{nameof(App_Launches)}.png");
    }
    
    [Fact]
    public void Test1()
    {
        // Arrange
        // Act
        // Assert
    }
}