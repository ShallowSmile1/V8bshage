using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace V8bshage.Models
{
    public class Advertisement
    {
        [Key]
        public int AdvId { get; set; }

        [Required(ErrorMessage = "Поле 'Название' обязательно!")]
        [DisplayName("Название")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле 'Описание' обязательно!")]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле 'Цена' обязательно!")]
        [Range(0, double.MaxValue, ErrorMessage = "Цена не может быть отрицательной")]
        [DisplayName("Цена")]
        public double Price { get; set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        public string UserId { get; set; }
    }
}
