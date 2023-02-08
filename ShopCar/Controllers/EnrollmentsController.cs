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
    public class EnrollmentsController : Controller
    {
        private readonly EnrollmentRepository _enrollmentRepository;

        public EnrollmentsController(AppDbContent context,EnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {

            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            return View(await _enrollmentRepository.ModelAllAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _enrollmentRepository._context.Enrollment == null)
            {
                return NotFound();
            }

            var enrollment = await _enrollmentRepository.ModelFirstofDefaultAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

        
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_enrollmentRepository._context.Orders, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_enrollmentRepository._context.Products, "Id", "Name");
            return View();
        }

        // POST: Enrollments/Create

        [HttpPost]
      /*  [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create([Bind("ProductId,OrderId,Count")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
              
                await _enrollmentRepository.ModelAddAsync(enrollment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_enrollmentRepository._context.Orders, "Id", "Id", enrollment.OrderId);
            ViewData["ProductId"] = new SelectList(_enrollmentRepository._context.Products, "Id", "Name", enrollment.ProductId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _enrollmentRepository._context.Enrollment == null)
            {
                return NotFound();
            }

            var enrollment = await _enrollmentRepository.ModelIdAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_enrollmentRepository._context.Orders, "Id", "Id", enrollment.OrderId);
            ViewData["ProductId"] = new SelectList(_enrollmentRepository._context.Products, "Id", "Name", enrollment.ProductId);
          
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,OrderId,Count")] Enrollment enrollment)
        {
            if (id != enrollment.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
                    await _enrollmentRepository.ModelUpdateAsync(enrollment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.OrderId))
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
            ViewData["OrderId"] = new SelectList(_enrollmentRepository._context.Orders, "Id", "Id", enrollment.OrderId);
            ViewData["ProductId"] = new SelectList(_enrollmentRepository._context.Products, "Id", "Name", enrollment.ProductId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _enrollmentRepository._context.Enrollment == null)
            {
                return NotFound();
            }

            var enrollment = await _enrollmentRepository.ModelFirstofDefaultAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_enrollmentRepository._context.Enrollment == null)
            {
                return Problem("Entity set 'AppDbContent.Enrollment'  is null.");
            }
            var enrollment = await _enrollmentRepository.ModelIdAsync(id);


            TempData["ErrorEnrollment"] = $"Deleted Enrollment {enrollment.Count}";
            await _enrollmentRepository.ModelDeleteAsync(enrollment);
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id) => _enrollmentRepository.ModelExist(id);

        /*  Создаём метод сортировки*/
        /* Get*/

        public async Task<IActionResult> Sorting(string str, bool asc)
            {
            var enrollments = _enrollmentRepository._context.Enrollment;
            ICollection<Enrollment> enrollmentssort = enrollments.MySorting(str, asc);
            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));
            return View("Index", enrollmentssort);
            }


        }
}
