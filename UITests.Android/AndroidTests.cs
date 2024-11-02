using Xunit.Abstractions;

namespace UITests;

[Collection("UICollection")]
public class AndroidTests : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AndroidTests(AppiumSetup setup, ITestOutputHelper testOutputHelper) : base(setup)
    {
        _testOutputHelper = testOutputHelper;
    }
}