using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using BoardGameParty.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace AppTests;

public class AppStorageServiceTests
{
    private readonly string _boardGameFileNameLocation;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<AppStorageService> _logger = Substitute.For<ILogger<AppStorageService>>();
    private readonly IAppStorageService _storageService;
    private readonly string _storageDirectory;
    private readonly IList<BoardGame> _testData;

    public AppStorageServiceTests()
    {
        _fileSystem = new MockFileSystem();
        _storageDirectory = _fileSystem.Path.Combine(_fileSystem.CurrentDirectory().ToString(), "BoardGameData");
        _storageService = new AppStorageService(_fileSystem, _logger, _storageDirectory);
        _boardGameFileNameLocation = _fileSystem.Path.Combine(_storageDirectory, "LocalBoardGames.json");
        _testData = JsonSerializer.Deserialize<List<BoardGame>>(File.ReadAllText("BoardGamesTestData.json"));
    }

    private void SetupDirectory(bool testNeedsDirectory)
    {
        if (!testNeedsDirectory && _fileSystem.Directory.Exists(_storageDirectory))
            _fileSystem.Directory.Delete(_storageDirectory, true);
        else if (testNeedsDirectory && !_fileSystem.Directory.Exists(_storageDirectory))
            _fileSystem.Directory.CreateDirectory(_storageDirectory);
    }

    private void SetupTestDataFile()
    {
        if (_fileSystem.File.Exists(_boardGameFileNameLocation)) _fileSystem.File.Delete(_boardGameFileNameLocation);

        _fileSystem.File.WriteAllText(_boardGameFileNameLocation, JsonSerializer.Serialize(_testData));
    }

    [Fact]
    public async Task Verify_local_storage_when_new()
    {
        SetupDirectory(false);

        var localData = await _storageService.SetupLocalStorage();

        Assert.True(_fileSystem.Directory.Exists(_storageDirectory));
        Assert.Empty(localData);
    }

    [Fact]
    public async Task Verify_local_storage_when_returning()
    {
        SetupDirectory(false);
        SetupDirectory(true);
        SetupTestDataFile();

        var localData = await _storageService.SetupLocalStorage();

        Assert.True(_fileSystem.Directory.Exists(_storageDirectory));
        Assert.True(_fileSystem.File.Exists(_boardGameFileNameLocation));
        Assert.Equal(_testData, localData);
    }

    [Fact]
    public async Task App_can_save_data_to_local_storage()
    {
        SetupDirectory(true);

        await _storageService.SaveLocalData(_testData);

        Assert.True(_fileSystem.File.Exists(_boardGameFileNameLocation));

        var localData =
            JsonSerializer.Deserialize<List<BoardGame>>(_fileSystem.File.ReadAllText(_boardGameFileNameLocation));
        Assert.Equal(_testData, localData);
    }

    [Fact]
    public async Task App_can_load_data_from_local_storage()
    {
        SetupDirectory(true);
        SetupTestDataFile();

        var localData = await _storageService.LoadLocalData();

        Assert.Equal(_testData, localData);
    }

    // [Fact]
    // public void Verify_data_stored_on_app_close()
    // {
    // TODO "Not Implemented"
    // }
}