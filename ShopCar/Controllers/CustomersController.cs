using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopCar.Db;
using ShopCar.Models;
using ShopCar.MyUtils;
using ShopCar.Repository;
using X.PagedList;

namespace ShopCar.Controllers
{
    public class CustomersController : Controller
    {

        private readonly CustomerRepository _customerRepository;

        public CustomersController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
               
        }

        // GET: Customers
        public async Task<IActionResult> Index(int? page, int? catId)
        {
            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            var pageNumber = page ?? 1;

            var customerString = HttpContext.Session.GetString("customer");

            var custFromSort = (customerString != null) ? JsonConvert.DeserializeObject<List<Customer>>(customerString) : _customerRepository._context.Customers.ToList();



           /* var listCategory = await _customerRepository.ModelAllAsync();*/
            IPagedList<Customer> categories = custFromSort.ToPagedList(pageNumber, Setting.Pages);




            return categories != null ?

              View(categories) :
                            Problem("Entity set 'AppDbContent.Customer'  is null.");
            }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _customerRepository._context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.ModelFirstofDefaultAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Order = _customerRepository._context.Orders.Where(q => q.CustomerId == id).
Include(q => q.Employee).
Include(q => q.Products).
ToList();
            ViewData["Hidding"] = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));
            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
       /* [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone")] Customer customer)
        {

            /*My Validasion*/
            var temp = _customerRepository.CheckModel(customer, "Create");
            string value = temp.Item2;
            TempData["ErrorcUSTOMER"] = value;

            if (ModelState.IsValid && temp.Item1)
            {

                await _customerRepository.ModelAddAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index"); /*View("Index");*/
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _customerRepository._context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.ModelIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
           
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            /*My Validasion*/
            var temp = _customerRepository.CheckModel(customer, nameof(Edit));
            string value = temp.Item2;
            TempData["ErrorCustomer"] = value;


            if (ModelState.IsValid && temp.Item1)
            {
                try
                {

                    await _customerRepository.ModelUpdateAsync(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return RedirectToAction("Index");
            }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _customerRepository._context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _customerRepository.ModelFirstofDefaultAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_customerRepository._context.Customers == null)
            {
                return Problem("Entity set 'AppDbContent.Customers'  is null.");
            }
            var customer = await _customerRepository.ModelIdAsync(id);

            TempData["ErrorCustomer"] = $"Deleted Customer{customer.Name}";
            await _customerRepository.ModelDeleteAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id) => _customerRepository.ModelExist(id);



        /*  Создаём метод сортировки*/
        /* Get*/

        public async Task<IActionResult> Sorting(string str, bool asc)
            {
            var customers = _customerRepository._context.Customers;
            ICollection<Customer> customerssort = customers.MySorting(str, asc);
         /*   ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            IPagedList<Customer> categoriesnew = customerssort.ToPagedList(1, Setting.PagesSort);*/

            HttpContext.Session.SetString("customer", JsonConvert.SerializeObject(customerssort));
            return RedirectToAction("Index");
         /*   return View("Index", categoriesnew);*/
            }

        }
}
