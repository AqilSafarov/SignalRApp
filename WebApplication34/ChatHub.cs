using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication34.Models;

namespace WebApplication34
{
    public class ChatHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnConnectedAsync()
        {
            string conId = Context.ConnectionId;


            AppUser user = null;
            if (Context.User.Identity.IsAuthenticated)
            {
                user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                if (user!=null)
                {
                    user.ConnectionId = Context.ConnectionId;
                    user.IsConnected = true;

                    var result = _userManager.UpdateAsync(user).Result;

                    Clients.All.SendAsync("UserJoin", user.Id,user.UserName);
                }
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {

            AppUser user = null;
            if (Context.User.Identity.IsAuthenticated)
            {
                user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                if (user != null)
                {
                    user.IsConnected = false;
                    user.LastSeen = DateTime.UtcNow.AddHours(4);

                    var result = _userManager.UpdateAsync(user).Result;

                    Clients.All.SendAsync("UserClose",user.Id, user.UserName, user.LastSeen.ToString("dd.MM.yyyy HH:mm"));

                }
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
