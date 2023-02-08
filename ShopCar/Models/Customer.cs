using System.ComponentModel.DataAnnotations;

namespace ShopCar.Models
    {
    public class Customer:IComparable,IComparable<Customer> 
        {
  
        public int Id { get; set; }
      /*  [Required(ErrorMessage = "Input Name")]
        [StringLength(15,MinimumLength =2,ErrorMessage ="incorrect length name")]*/
        public string Name { get; set; }
      /*  [EmailAddress]*/
        public string Email { get; set; }

        public string  Phone { get; set; }

        public List<Order> Orders { get; set; }=new List<Order>();

        public int CompareTo(object? obj)
            {
            return Name.CompareTo((obj as Customer).Name);
            }

        public int CompareTo(Customer? other)
            {
            return Name.CompareTo(other.Name);
            }
        }
    }
