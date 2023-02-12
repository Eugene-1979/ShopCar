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
    public class EmployeesController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int? page, int? catId)
        {

            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));



            var pageNumber = page ?? 1;

            var employeeString = HttpContext.Session.GetString("employee");

            var employeeFromSort = (employeeString != null) ? JsonConvert.DeserializeObject<List<Employee>>(employeeString) : _employeeRepository._context.Employees.ToList();

          /*  var listCategory = await _employeeRepository.ModelAllAsync();*/
            IPagedList<Employee> categories = employeeFromSort.ToPagedList(pageNumber, Setting.Pages);




            return categories != null ?

              View(categories) :
                            Problem("Entity set 'AppDbContent.Emploee'  is null.");
            }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _employeeRepository._context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.ModelFirstofDefaultAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Order = _employeeRepository._context.Orders.Where(q => q.EmployeeId == id).
Include(q => q.Customer).
Include(q => q.Products).
ToList();
            ViewData["Hidding"] = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        
        [HttpPost]
      /*  [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create([Bind("Id,Name,Salary,Email")] Employee employee)
        {


            /*My Validasion*/
            var temp = _employeeRepository.CheckModel(employee, "Create");
            string value = temp.Item2;
            TempData["ErrorEmployee"] = value;


            ModelState.Remove("Email");
      
            if (ModelState.IsValid && temp.Item1)
            {

                await _employeeRepository.ModelAddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _employeeRepository._context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.ModelIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
          
            return View(employee);
        }

        // POST: Employees/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Salary,Email")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            /*My Validasion*/
            var temp = _employeeRepository.CheckModel(employee, nameof(Edit));
            string value = temp.Item2;
            TempData["ErrorEmployee"] = value;


            if (ModelState.IsValid && temp.Item1)
            {
                try
                {

                    await _employeeRepository.ModelUpdateAsync(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _employeeRepository._context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.ModelFirstofDefaultAsync(id); 
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_employeeRepository._context.Employees == null)
            {
                return Problem("Entity set 'AppDbContent.Employees'  is null.");
            }
            var employee = await _employeeRepository.ModelIdAsync(id);

            TempData["ErrorEmployee"] = $"Deleted Employee{employee.Name}";
            await _employeeRepository.ModelDeleteAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id) => _employeeRepository.ModelExist(id);

        /*  Создаём метод сортировки*/
        /* Get*/

        public async Task<IActionResult> Sorting(string str, bool asc)
            {
            var employees = _employeeRepository._context.Employees;
            ICollection<Employee> employeessort = employees.MySorting(str, asc);

            HttpContext.Session.SetString("employee", JsonConvert.SerializeObject(employeessort));
            return RedirectToAction("Index");
            }


        }
}
