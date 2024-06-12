using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task PlayCard(string user, string card)
    {
        await Clients.All.SendAsync("CardPlayed", user, card);
    }
}
