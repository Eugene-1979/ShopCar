using Microsoft.AspNetCore.Mvc;
using ShopCar.Db;
using System.ComponentModel.DataAnnotations;


namespace ShopCar.Models
    {
    public class Product
        {
   
        public int Id { get; set; }


       /* [Required(ErrorMessage = "Input Name")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "incorrect length name")]*/
        public string Name { get; set; }

      /*  [Required]
        [Range(ShopRepository._minSale, ShopRepository._maxSale, ErrorMessage = "Incorect Range 0-100_000")]*/
        public int Sale { get; set; }


        public Category Category { get; set; }
        /*  [Required]*/
        public int CategoryId { get; set; }



      /*  [Required(ErrorMessage = "Input about")]
        [StringLength(150, MinimumLength = 0, ErrorMessage = "incorrect length name")]*/
     
        public string About { get; set; }


        /*  [StringLength(50, MinimumLength = 0, ErrorMessage = "incorrect length name")]
          [Required(ErrorMessage = "Input Review")]*/
        /*   [Remote("CheckEmptyString", "Products")]*/

        public string Reviews { get; set; }
      
        public  string ? Photos { get; set; }
      
/*        public int Quantity  { get; set; }*/
    public List<Order> Orders { get; set; } = new List<Order>();

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public override bool Equals(object? obj)
            {
            return obj is Product product &&
                   Name.Equals(product.Name);
            }

        public override int GetHashCode()
            {
            return HashCode.Combine(Name);
            }
        }
    }
