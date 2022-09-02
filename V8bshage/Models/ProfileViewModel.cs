using System;
using System.Collections;
using System.Collections.Generic;

namespace V8bshage.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public IEnumerable<Advertisement> Advertisments { get; set; }
    }
}
