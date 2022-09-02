using System;
using Microsoft.AspNetCore.Http;

namespace V8bshage.Models
{
    public class NewsViewModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public IFormFile Photo { get; set; }

        public DateTime PostTime { get; set; }
    }
}
