using System.Text.Json;
using BoardGameParty.Interfaces;
using Microsoft.Extensions.Logging;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace BoardGameParty.Models;

public class AppStorage : IAppStorage
{
    private readonly string _boardGameFileNameLocation;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<AppStorage> _logger;
    private readonly string _storageDirectory;

    public AppStorage(IFileSystem fileSystem, ILogger<AppStorage> logger, string storageDirectory)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        _storageDirectory = storageDirectory;
        _boardGameFileNameLocation = _fileSystem.Path.Combine(storageDirectory, "LocalBoardGames.json");
    }

    public async Task<IList<BoardGame>> SetupLocalStorage()
    {
        _logger.LogError("Creating local storage");
        if (!_fileSystem.Directory.Exists(_storageDirectory)) _fileSystem.Directory.CreateDirectory(_storageDirectory);

        _logger.LogError("Storage directory created!");
        var localData = await LoadLocalData();
        if (localData.Count > 0) return localData;

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
        await using var writeStream = _fileSystem.File.Create(_boardGameFileNameLocation);
        await JsonSerializer.SerializeAsync(writeStream, boardGames);
    }

    public async Task<IList<BoardGame>> LoadLocalData()
    {
        //await Task.Delay(5000); for testing progress spinner later
        if (!_fileSystem.File.Exists(_boardGameFileNameLocation)) return new List<BoardGame>();

        await using var openStream = _fileSystem.File.OpenRead(_boardGameFileNameLocation);
        return await JsonSerializer.DeserializeAsync<List<BoardGame>>(openStream) ?? new List<BoardGame>();
    }
}