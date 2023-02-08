using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCar.Models
    {
    public class Order :IComparable,IComparable<Order> 
        {
     
        public int Id { get; set; }


       public Employee Employee { get; set; }
        public int ?EmployeeId { get; set; }

        public DateTime MyDate { get; set; }
        public Customer Customer{ get; set; }
        public int ?CustomerId { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public int CompareTo(object? obj)
            {
            return MyDate.CompareTo((obj as Order).MyDate);
            }

        public int CompareTo(Order? other)
            {
          return  MyDate.CompareTo(other.MyDate);
            }
        }
    }
