using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inftastructure.Hubs
{

    public class NotificationHub : Hub
    {
        public async Task Notification(string message)
        {
            if (message != null)
            { 
                await Clients.All.SendAsync("m", message);
            }
        }
    }

}
