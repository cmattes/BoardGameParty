﻿using System.Text.Json;
using BoardGameParty.Interfaces;
using BoardGameParty.Models;

namespace BoardGameParty;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        //BoardGameCollectionView.ItemsSource = MainPageViewModel.GetBoardGames();
        //BoardGameCollectionView.ItemsSource = null;
        BoardGameCollectionView.ItemsSource = GetBoardGames();
    }
    
    public List<BoardGame> GetBoardGames()
    {
        return new List<BoardGame>
        {
            new("TestGame1")
            {
                Description = "A game that is for testing", MinutesPerGame = 30,
                MinimumNumberOfPlayers = 1, MaximumNumberOfPlayers = 4, ImageURI = "dotnet_bot.png"
            },
            new("TestGame2")
            {
                Description = "A game that is for testing", MinutesPerGame = 20,
                MinimumNumberOfPlayers = 3, MaximumNumberOfPlayers = 5, ImageURI = "dotnet_bot.png"
            }
        };
    }
}