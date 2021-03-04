using System;
using System.ComponentModel.DataAnnotations;

namespace UStore.Models {
    public class Product {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите наименование товара")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите производителя товара")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Укажите цену товара")]
        [Range(0, double.MaxValue, ErrorMessage = "Только положительная величина")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Укажите количество товара")]
        [Range(0, double.MaxValue, ErrorMessage = "Только положительная величина")]
        public int CountInStock { get; set; }
    }
}
