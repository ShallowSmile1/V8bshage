using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace V8bshage.Models
{
    public class News
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required]
        [Display(Name ="Описание")]
        public string Summary { get; set; }

        [Display(Name = "Фото")]
        public byte[] Photo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PostTime { get; set; }
    }
}
