using BlazorApp1.Models;
using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    private static Game game;

    public async Task JoinGame(string playerName, int numberOfPlayers)
    {
        if (game == null || game.IsGameOver())
        {
            game = new Game(numberOfPlayers);
        }

        var player = new Player(Context.ConnectionId, playerName);
        game.AddPlayer(player);

        await Clients.All.SendAsync("PlayerJoined", playerName);
    }

    public async Task StartGame()
    {
        if (game != null && game.Players.Count > 0)
        {
            game.StartGame();
            await Clients.All.SendAsync("GameStarted");
        }
    }

    public async Task PlayCard(string playerName, string cardName)
    {
        if (game == null || game.Players.Count == 0)
        {
            // Jogo não iniciado ou sem jogadores
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
            // Jogo não iniciado ou sem jogadores
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
