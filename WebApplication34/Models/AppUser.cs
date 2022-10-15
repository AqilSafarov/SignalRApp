using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication34.Models
{
    public class AppUser:IdentityUser
    {
        public string ConnectionId { get; set; }
        public bool IsConnected { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
