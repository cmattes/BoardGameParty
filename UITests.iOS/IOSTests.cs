using System.Diagnostics;
using System.Globalization;
using Xunit.Abstractions;
using BoardGameParty.Converters;

namespace UITests;

[Collection("UICollection")]
public class IOSTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IOSTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    // Arrange
    // Act
    // Assert
    
    [Fact]
    public void Verify_app_launches()
    {
        // Arrange
        var screenshotName = "app_screenshot.png";
        
        // Act
        App.GetScreenshot().SaveAsFile(screenshotName);
        Thread.Sleep(500);
        
        // Assert
        Assert.True(File.Exists(screenshotName), "Failed to gather a screenshot.");
    }
    
    [Fact]
    public void DoAThing()
    {
        
    }
}