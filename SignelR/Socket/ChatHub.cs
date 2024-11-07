using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SignalRApp;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendAsync(string message, string to)
    {
        var user = Context.User;
        var userName = user?.Identity?.Name;

        var userRole = user?.FindFirst(ClaimTypes.Role)?.Value;
        var isAdmin = user?.IsInRole("admin");

    }

    public async Task Send(string message, string to)
    {
        // получение текущего пользователя, который отправил сообщение
        //var userName = Context.UserIdentifier;
        if (Context.UserIdentifier is string userName)
        {
            await Clients.Users(to, userName).SendAsync("Receive", message, userName);
        }
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
        await base.OnConnectedAsync();
    }

}
