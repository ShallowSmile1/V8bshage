using System;
using Microsoft.AspNetCore.Identity;


namespace V8bshage.Models
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Vk { get; set; }
        public string Telegram { get; set; }
        public int Reputation { get; set; }
        public int AccessLevel { get; set; }
        public byte[] Photo { get; set; }
    }
}
