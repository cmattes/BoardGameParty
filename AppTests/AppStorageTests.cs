using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using BoardGameParty;
using BoardGameParty.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;
using BoardGameParty.Models;

namespace AppTests;

public class AppStorageTests
{
    private readonly ILogger<AppStorage> _logger = Substitute.For<ILogger<AppStorage>>();
    private readonly System.IO.Abstractions.IFileSystem _fileSystem;
    private readonly string _storageDirectory;
    private readonly IAppStorage _storage;
    private readonly IList<BoardGame> _testData;
    private readonly string _localFileName = "LocalBoardGames.json";
    
    public AppStorageTests()
    {
        _fileSystem = new MockFileSystem();
        _storageDirectory = _fileSystem.Path.Combine(_fileSystem.CurrentDirectory().ToString(), "/BoardGameData/");
        _storage = new AppStorage(_fileSystem, _logger, _storageDirectory);
        _testData = new List<BoardGame>
        {
            new()
            {
                GameName = "TestGame1", GameDescription = "A game that is for testing", MinutesPerGame = 30,
                MinimumNumberOfPlayers = 1, MaximumNumberOfPlayers = 4, GameImageURI = "dotnet_bot.png"
            },
            new()
            {
                GameName = "TestGame2", GameDescription = "A game that is for testing", MinutesPerGame = 20,
                MinimumNumberOfPlayers = 3, MaximumNumberOfPlayers = 5, GameImageURI = "dotnet_bot.png"
            }
        };
    }

    private void SetupDirectory(bool testNeedsDirectory)
    {
        if (!testNeedsDirectory && _fileSystem.Directory.Exists(_storageDirectory))
        {
            _fileSystem.Directory.Delete(_storageDirectory, true);
        }
        else if (testNeedsDirectory && !_fileSystem.Directory.Exists(_storageDirectory))
        {
            _fileSystem.Directory.CreateDirectory(_storageDirectory);
        }
    }
    
    private void SetupTestDataFile()
    {
        if (_fileSystem.File.Exists(_storageDirectory + _localFileName))
        {
            _fileSystem.File.Delete(_storageDirectory + _localFileName);
        }

        _fileSystem.File.WriteAllText(_storageDirectory + _localFileName, JsonSerializer.Serialize(_testData));
    }

    [Fact]
    public async Task Verify_local_storage_when_new()
    {
        SetupDirectory(false);
        
        var localData = await _storage.SetupLocalStorage();
        
        Assert.True(_fileSystem.Directory.Exists(_storageDirectory));
        Assert.Empty(localData);
    }
    
    [Fact]
    public async Task Verify_local_storage_when_returning()
    {
        SetupDirectory(false);
        SetupDirectory(true);
        SetupTestDataFile();
        
        var localData = await _storage.SetupLocalStorage();
        
        Assert.True(_fileSystem.Directory.Exists(_storageDirectory));
        Assert.True(_fileSystem.File.Exists(_storageDirectory + _localFileName));
        Assert.Equal(_testData, localData);
    }

    [Fact]
    public async Task App_can_save_data_to_local_storage()
    {
        SetupDirectory(true);
        
        await _storage.SaveLocalData(_testData);

        Assert.True(_fileSystem.File.Exists(_storageDirectory + _localFileName));
        
        var localData = JsonSerializer.Deserialize<List<BoardGame>>(_fileSystem.File.ReadAllText(_storageDirectory + _localFileName));
        Assert.Equal(_testData, localData);
    }
    
    [Fact]
    public async Task App_can_load_data_from_local_storage()
    {
        SetupDirectory(true);
        SetupTestDataFile();

        var localData = await _storage.LoadLocalData();
        
        Assert.Equal(_testData, localData);
    }

    [Fact]
    public void Verify_data_stored_on_app_close()
    {
        Assert.Fail("Not Implemented");
    }
}