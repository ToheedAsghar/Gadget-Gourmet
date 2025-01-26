using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Product
    {
        [BindNever]
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Name Entered!")]
        [StringLength(100, ErrorMessage = "Invalid Name Entered!")]
        public string Name { get; set; } = string.Empty;

        public bool IsNew { get; set; } = true;

        [Required(ErrorMessage = "Description cannot be Empty!")]
        [StringLength(500, ErrorMessage = "Description cannot be Empty!")]
        public string Description { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "Range must be a positive number")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Invalid Price!")]
        [Range(1, double.MaxValue, ErrorMessage = "Range must be a positive number")]
        public decimal Price { get; set; }

        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Manufacturer Can't be Empty!")]
        [StringLength(100, ErrorMessage = "Manufacturer Can't be Empty!")]
        public string Manufacturer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category Can't be Empty!")]
        [StringLength(100, ErrorMessage = "Category Can't be Empty!")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Specify the Quantity of the Products")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity can't be Negative!")]
        public int Quantity { get; set; }

        //[Url(ErrorMessage = "Image URL must be a valid URL.")]
        public string ImageUrl { get; set; } = string.Empty;

        public Product() { }

        public Product(string name, string desc, decimal weight, decimal price, string color, string manufacturer, string category, int quantity)
        {
            Name = name;
            Description = desc;
            Weight = weight;
            Color = color;
            Manufacturer = manufacturer;
            Category = category;
            Price = price;
            Quantity = quantity;
        }
    }
}
