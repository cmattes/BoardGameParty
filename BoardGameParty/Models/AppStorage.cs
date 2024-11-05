using System.Text.Json;
using BoardGameParty.Interfaces;
using Microsoft.Extensions.Logging;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace BoardGameParty.Models;

public class AppStorage : IAppStorage
{
    private readonly ILogger<AppStorage> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly string _storageDirectory;
    private readonly string _localFileName = "LocalBoardGames.json";
    
    public AppStorage(IFileSystem fileSystem, ILogger<AppStorage> logger, string storageDirectory)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        _storageDirectory = storageDirectory;
    }

    public async Task<IList<BoardGame>> SetupLocalStorage()
    {
        
        if (!_fileSystem.Directory.Exists(_storageDirectory))
        {
            _fileSystem.Directory.CreateDirectory(_storageDirectory);
        }

        var localData = await LoadLocalData();
        if (localData.Count > 0)
        {
            return localData;
        }
        
        // If internet access, load data from cloud and return

        return new List<BoardGame>();
    }

    public async Task SaveToCloud()
    {
        //todo needs implemented
    }

    public async Task<IList<BoardGame>> LoadFromCloud()
    {
        //todo needs implemented
        return null;
    }

    public async Task SaveLocalData(IList<BoardGame> boardGames)
    {
        await using var writeStream = _fileSystem.File.Create(_storageDirectory + _localFileName);
        await JsonSerializer.SerializeAsync(writeStream, boardGames);
    }

    public async Task AddNewBoardGame(BoardGame boardGame)
    {
        // Should be moved to model view of the Main Page
    }

    public async Task DeleteBoardGame(BoardGame boardGame)
    {
        // Should be moved to model view of the Main Page
    }

    public async Task UpdateBoardGame(BoardGame boardGame)
    {
        // Should be moved to model view of the Main Page
    }

    public async Task<IList<BoardGame>> LoadLocalData()
    {
        if (!_fileSystem.File.Exists(_storageDirectory + _localFileName)) return new List<BoardGame>();
        
        await using var openStream = _fileSystem.File.OpenRead(_storageDirectory + _localFileName);
        return await JsonSerializer.DeserializeAsync<List<BoardGame>>(openStream) ?? new List<BoardGame>();
    }
}