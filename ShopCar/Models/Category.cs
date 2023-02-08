using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShopCar.Models
    {
    public class Category:IComparable<Category> ,IComparable
        {
      
        public int Id { get; set; }
        /*  [Required(ErrorMessage = "Input Name")]
          [StringLength(15, MinimumLength = 2, ErrorMessage = "incorrect length name")]*/
      /*  [Remote("CheckEmptyString", "Products")]*/
        public string Name { get; set; }
      /*  [Required(ErrorMessage = "Input Salary")]
        [Range(0, 100, ErrorMessage = "Incorect Range 0-100")]*/
        public int Salary { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public int CompareTo(Category? other)
            {
          return  Name.CompareTo(other.Name);
            }

        public int CompareTo(object? obj)
            {
            return Name.CompareTo(((Category)obj).Name);
            }

        public override bool Equals(object? obj)
            {
            return obj is Category category &&
                   Name == category.Name;
            }

        public override int GetHashCode()
            {
            return HashCode.Combine(Name);
            }
        }
    }
