using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopCar.Db;
using ShopCar.Models;
using ShopCar.MyUtils;
using ShopCar.Repository;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopCar.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderRepository _orderRepository;

        public OrdersController(OrderRepository orderRepository)
            {
            _orderRepository = orderRepository;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? page, int? catId)
        {
            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            var pageNumber = page ?? 1;

           /* var qw = HttpContext.Session.GetString("order");

            var prodFromSort = (qw != null) ? JsonConvert.DeserializeObject<List<Order>>(qw) : _orderRepository._context.Orders.ToList();*/








            var listProd = _orderRepository._context.Orders.
            Include(q => q.Customer).Include(q => q.Employee).

            ToList().

            Where(q => catId == null || catId == 0 || q.EmployeeId == catId).ToList();

            ViewBag.Category = new SelectList(
            _orderRepository._context.Employees.ToList(), "Id", "Name");

            ViewBag.SelectedCat = catId.ToString();
            IPagedList<Order> products = listProd.ToPagedList(pageNumber, Setting.Pages);
           



            return View(products);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _orderRepository._context.Orders == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.ModelFirstofDefaultAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.Enrollment = /*(await _orderRepository.ModelIdAsync(id)).Enrollments;*/
          _orderRepository._context.Enrollment.Where(q => q.OrderId == id).
          Include(q=>q.Product).
          ToList();


            ViewData["Hidding"] = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_orderRepository._context.Customers, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_orderRepository._context.Employees, "Id", "Name");
            ViewBag.Products = _orderRepository._context.Products.Include(q=>q.Category).ToList();
            ViewBag.q = new List<int>() { 1, 2 };


            var enrollString = HttpContext.Session.GetString("Cart");
          var ttt=   (enrollString != null) ? JsonConvert.DeserializeObject<Dictionary<int, int>>(enrollString) : new Dictionary<int, int>();
            Dictionary<Product, int> super = new();

var pppp=_orderRepository._context.Products.ToList();
            foreach(var item in ttt)
                {
                Product pr = pppp.FirstOrDefault(q => q.Id == item.Key);

                super.Add(pr, item.Value);
                }




            ViewBag.orders = super;
            return View();
        }

        // POST: Orders/Create

        [HttpPost]
     /*   [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create(/*[Bind("Id,EmployeeId,MyDate,CustomerId")] Order order*/IFormCollection colProp)
        {


            string nameCustomer = colProp["Name"];
            string phoneCustomer = colProp["Phone"];
            string emailCustomer = colProp["Email"];
            Customer customer = new (){ Name = nameCustomer, Email = emailCustomer, Phone = phoneCustomer };


            Order order = new()
                {
                Customer = customer,

                EmployeeId = int.Parse(colProp["EmployeeId"]),
                MyDate = DateTime.Parse(colProp["MyDate"])

          
                };
          
            var enrollString = HttpContext.Session.GetString("Cart");

            var custFromSort = (enrollString != null) ? JsonConvert.DeserializeObject<Dictionary<int,int>>(enrollString) : new Dictionary<int, int>();


            List<Enrollment> tempEnr = new List<Enrollment>();

            foreach(var item in custFromSort)
                {
                Enrollment ent = new() { ProductId = item.Key, Count = item.Value, Order = order };
                tempEnr.Add(ent);  
                
                }


            order.Enrollments = tempEnr;



            HttpContext.Session.Remove("Cart");


            ModelState.Remove(nameof(Customer));
            ModelState.Remove(nameof(Employee));

            if(/*ModelState.IsValid*/true)
                {

                await _orderRepository.ModelAddAsync(order);
                return RedirectToAction(nameof(Index));
                }





            /*            ViewData["CustomerId"] = new SelectList(_orderRepository._context.Customers, "Id", "Name", order.CustomerId);
                        ViewData["EmployeeId"] = new SelectList(_orderRepository._context.Employees, "Id", "Email", order.EmployeeId);*/
            return View(/*order*/);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _orderRepository._context.Orders == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.ModelIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_orderRepository._context.Customers, "Id", "Name", order.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_orderRepository._context.Employees, "Id", "Name", order.EmployeeId);
       
            return View(order);
        }

        // POST: Orders/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,MyDate,CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(Customer));
            ModelState.Remove(nameof(Employee));
            if (ModelState.IsValid)
            {
                try
                {
                   
                    await _orderRepository.ModelUpdateAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_orderRepository._context.Customers, "Id", "Name", order.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_orderRepository._context.Employees, "Id", "Email", order.EmployeeId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _orderRepository._context.Orders == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.ModelFirstofDefaultAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_orderRepository._context.Orders == null)
            {
                return Problem("Entity set 'AppDbContent.Orders'  is null.");
            }
            var order = await _orderRepository.ModelIdAsync(id);


            TempData["ErrorOrder"] = $"Deleted Order {order.Id}";
            await _orderRepository.ModelDeleteAsync(order);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id) => _orderRepository.ModelExist(id);


          /*  Создаём метод сортировки*/
     /* Get*/
    
        public async Task<IActionResult> Sorting(string str,bool asc, int? catId = null)
            {
            if(catId != null)
                {
                return RedirectToAction("Index", new { catId = catId });
                }


            var orders = _orderRepository._context.Orders
            .Include(q => q.Customer).Include(q => q.Employee);
            ICollection<Order> ordersort = orders.MySorting(str, asc);



            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));







            ViewBag.Category = new SelectList(
            _orderRepository._context.Employees.ToList(), "Id", "Name");

            ViewBag.SelectedCat = "0";
            IPagedList<Order> products = ordersort.ToPagedList(1, Setting.PagesSort);
            ViewBag.products = products;

            /*  HttpContext.Session.SetString("order", JsonConvert.SerializeObject(ordersort));

              return RedirectToAction("Index");*/

            return View("Index", products);
            }



                [HttpPost]
        public void CreateSession(string product,string val) {
       
        bool parseBoolId= int.TryParse(product,out int parseProduct);
        bool parseBoolVal= int.TryParse(val,out int parseVal);

            if(parseBoolId && parseBoolVal)
                {
                var stringCart = HttpContext.Session.GetString("Cart");
                var cartDict = (stringCart != null) ? JsonConvert.DeserializeObject<Dictionary<int, int>>(stringCart) : new Dictionary<int, int>();
                cartDict.Add(parseProduct, parseVal);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartDict));

                }



        }

    }
}
