using BlazorApp1.Models;
using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    private static Game game;

    public async Task JoinGame(string playerName)
    {
        if (game == null || game.IsGameOver())
        {
            game = new Game(2); // Inicialmente configurado para dois jogadores
        }

        var player = new Player(Context.ConnectionId, playerName);
        game.AddPlayer(player);

        await Clients.All.SendAsync("PlayerJoined", playerName);
    }

    public async Task StartGame(int numberOfPlayers)
    {
        if (game == null || game.IsGameOver())
        {
            game = new Game(numberOfPlayers);
        }

        game.StartGame();
        await Clients.All.SendAsync("GameStarted");

        // Distribuir cartas aos jogadores
        foreach (var player in game.Players)
        {
            await Clients.Client(player.Id).SendAsync("CardsDistributed", player.Name, player.Hand);
        }
    }

    public async Task PlayCard(string playerName, string cardName)
    {
        if (game == null || game.Players.Count == 0)
        {
            return;
        }

        var player = game.GetCurrentPlayer();
        if (player == null || player.Name != playerName)
        {
            return;
        }

        var card = player.Hand.FirstOrDefault(c => c.Name == cardName);
        if (card != null)
        {
            game.PlayCard(player, card);
            await Clients.All.SendAsync("CardPlayed", playerName, cardName);

            if (game.IsGameOver())
            {
                var winner = game.GetCurrentPlayer().Name;
                await Clients.All.SendAsync("GameOver", winner);
            }
        }
    }

    public async Task EndTurn(string playerName)
    {
        if (game == null || game.Players.Count == 0)
        {
            return;
        }

        var currentPlayer = game.GetCurrentPlayer();
        if (currentPlayer == null || currentPlayer.Name != playerName)
        {
            return;
        }

        game.EndTurn();
        await Clients.All.SendAsync("TurnEnded", playerName);

        if (game.IsGameOver())
        {
            var winner = game.GetCurrentPlayer().Name;
            await Clients.All.SendAsync("GameOver", winner);
        }
    }
}
