using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;
using ShopCar.MyUtils;
using ShopCar.Repository;

namespace ShopCar.Controllers
    {
    public class CategoriesController : Controller
        {
       
        private readonly CategoryRepository _categoryRepository;

        public ILogger Log { get; }

        public CategoriesController(CategoryRepository categoryRepository, ILogger<CategoriesController> log)
            {
             Log = log;
            _categoryRepository = categoryRepository;
            }

        // GET: Categories
        public IActionResult Index()
            {

            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));
        
        /*    var t=  await _categoryRepository.ModelAllAsync();*/
            var t = _categoryRepository._context.Categorys;

            return _categoryRepository._context.Categorys != null?
      
             View(t):
                           Problem("Entity set 'AppDbContent.Categorys'  is null.");
                        
            }


        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
            {
            if(id == null || _categoryRepository._context.Categorys == null)
                {
                return NotFound();
                }

            var category = await _categoryRepository.ModelFirstofDefaultAsync(id); /*repository*/
            if(category == null)
                {
                return NotFound();
                }
            ViewBag.Product =_categoryRepository._context.Products.Where(q => q.CategoryId == id).
   Include(q => q.Category).
   ToList();
            ViewData["Hidding"] = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));
            return View(category);
            }

        // GET: Categories/Create
        public IActionResult Create()
            {
            return View();
            }
     

        // POST: Categories/Create
       
        [HttpPost]
      /*  [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create([Bind("Id,Name,Salary")] Category category)
            {
           /* Category category= new Category() { Name = name, Salary = salary };*/
            var temp = _categoryRepository.CheckModel(category, nameof(Create));
            string value = temp.Item2;
            TempData["ErrorCategory"] = value;



            if(ModelState.IsValid && temp.Item1)
                {
              await _categoryRepository.ModelAddAsync(category);/*repository*/
                return RedirectToAction("Index");
                /*      return View("Index",_categoryRepository._context.Categorys);*/
                }

            Log.LogInformation($"Create {DateTime.Now.ToString("d")} {this.GetType().Name} {value}");
      
            return View(category);
            }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
            if(id == null || _categoryRepository._context.Categorys == null)
                {
                return NotFound();
                }

            var category = await _categoryRepository.ModelIdAsync(id); /*repository*/
            if(category == null)
                {
                return NotFound();
                }
          
            return View(category);
            }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Salary")] Category category)
            {


            var temp = _categoryRepository.CheckModel(category, nameof(Edit));
            string value = temp.Item2;
            TempData["ErrorCategory"] = value;





            if(id != category.Id)
                {
                return NotFound();
                }

            if(ModelState.IsValid && temp.Item1)
                {
                try
                    {

                    await _categoryRepository.ModelUpdateAsync(category); /*repository*/
                    }
                catch(DbUpdateConcurrencyException)
                    {
                    if(!CategoryExists(category.Id))
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

            Log.LogInformation($"Edit {DateTime.Now.ToString("d")} {this.GetType().Name} {value}");

         



            return View(category);
            }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
            {
            if(id == null || _categoryRepository._context.Categorys == null)
                {
                return NotFound();
                }

            var category = await _categoryRepository.ModelFirstofDefaultAsync(id);
            if(category == null)
                {
                return NotFound();
                }

            return View(category);
            }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            if(_categoryRepository._context.Categorys == null)
                {
                return Problem("Entity set 'AppDbContent.Categorys'  is null.");
                }
            var category = await _categoryRepository.ModelIdAsync(id); /*repository*/

            TempData["ErrorCategory"] = $"Deleted Category  {category.Name}";
            await _categoryRepository.ModelDeleteAsync(category);
            return RedirectToAction(nameof(Index));
            }

        private bool CategoryExists(int id) => _categoryRepository.ModelExist(id); /*repository*/

        /*  Создаём метод сортировки*/
        /* Get*/

        public async Task<IActionResult> Sorting(string str, bool asc)
            {
            var categories = _categoryRepository._context.Categorys;
            ICollection<Category> categoriessort = categories.MySorting(str, asc);
            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));
            return View("Index", categoriessort);
            }
        }
    }
