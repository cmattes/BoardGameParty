using System.Text.Json;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;
using Microsoft.Extensions.Logging;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace BoardGameParty.Services;

public class AppStorageService : IAppStorageService
{
    private readonly string _boardGameFileNameLocation;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<AppStorageService> _logger;
    private readonly string _storageDirectory;
    private static SemaphoreSlim _semaphoreLock;

    public AppStorageService(IFileSystem fileSystem, ILogger<AppStorageService> logger, string storageDirectory)
    {
        _fileSystem = fileSystem;
        _logger = logger;
        _storageDirectory = storageDirectory;
        _boardGameFileNameLocation = _fileSystem.Path.Combine(storageDirectory, "LocalBoardGames.json");
        _semaphoreLock = new SemaphoreSlim(1, 1);
    }

    public async Task<IList<BoardGame>> SetupLocalStorage()
    {
        _logger.LogInformation("Creating local storage");
        if (!_fileSystem.Directory.Exists(_storageDirectory)) _fileSystem.Directory.CreateDirectory(_storageDirectory);
        
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
        try
        {
            await _semaphoreLock.WaitAsync();
            await using var writeStream = _fileSystem.File.Create(_boardGameFileNameLocation);
            await JsonSerializer.SerializeAsync(writeStream, boardGames);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error saving to: {_boardGameFileNameLocation}{Environment.NewLine}{e}");
        }
        finally
        {
            _semaphoreLock.Release();
        }
    }

    public async Task<IList<BoardGame>> LoadLocalData()
    {
        var boardGames = new List<BoardGame>();
        
        try
        {
            await _semaphoreLock.WaitAsync();
            if (!_fileSystem.File.Exists(_boardGameFileNameLocation)) return new List<BoardGame>();

            await using var openStream = _fileSystem.File.OpenRead(_boardGameFileNameLocation);
            boardGames = await JsonSerializer.DeserializeAsync<List<BoardGame>>(openStream) ?? new List<BoardGame>();
        }
        catch (Exception e)
        {
            _logger.LogError($"Error loading from: {_boardGameFileNameLocation}{Environment.NewLine}{e}");
        }
        finally
        {
            _semaphoreLock.Release();
        }

        return boardGames;
    }
}