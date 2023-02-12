using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
/*using System.Web.Mvc;*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using ShopCar.Db;
using ShopCar.Models;
using ShopCar.MyUtils;
using ShopCar.Repository;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Newtonsoft.Json;

namespace ShopCar.Controllers
    {
    public class ProductsController : Controller
        {
        private readonly ProductRepository _productRepository;
        public ILogger Log { get; }

        public ProductsController(ProductRepository productRepository, ILogger<ProductsController> log)
            {
            _productRepository = productRepository;
            Log = log;
            }







        // GET: Products
        public async Task<IActionResult> Index(int? page, int? catId)
            {


            ViewData["Hidding"] = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));


            var pageNumber = page ?? 1;

           var qw= HttpContext.Session.GetString("product");

            var prodFromSort = (qw != null) ? JsonConvert.DeserializeObject<List<Product>>(qw) : _productRepository.GetAllProducts();



            var listProd = prodFromSort.
            Where(q => catId == null || catId == 0 || q.CategoryId == catId).ToList();

            ViewBag.Category = new SelectList(
        _productRepository._context.Categorys.ToList(), "Id", "Name");
            ViewBag.SelectedCat = catId.ToString();


            IPagedList<Product> products = listProd.ToPagedList(pageNumber, Setting.Pages);


            return View(products);







            }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
            {
            if(id == null || _productRepository._context.Products == null)
                {
                return NotFound();
                }

            var product = await _productRepository.ModelFirstofDefaultAsync(id);
            if(product == null)
                {
                return NotFound();
                }




            ViewBag.Enrollment =
        _productRepository._context.Enrollment.Where(q => q.ProductId == id).
        Include(q => q.Order).
        ToList();


            ViewBag.img = $"\\Content\\Products\\{product.Name}\\{product.Photos}";
            ViewData["Hidding"] = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            return View(product);
            }

        // GET: Products/Create
        /* [Authorize(Roles ="Admin")]*/
        public IActionResult Create()
            {

            ViewData["CategoryId"] = new SelectList(_productRepository._context.Categorys, "Id", "Name");
            return View();
            }

        // POST: Products/Create

        [HttpPost]
        /*        [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create([Bind("Id,Name,Sale,CategoryId,About,Reviews,Photos")] Product product, IFormFile Photos, IFormFileCollection file)
            {

            ModelState.Remove("Category");
            ModelState.Remove("Photos");
            var temp = _productRepository.CheckModel(product, nameof(Create));





            if(Photos != null && Photos.Length > 0)

                {
                string ext = Photos.ContentType.ToLower();

                if(

                                ext != "image/jpg" &&
                                ext != "image/jpeg" &&
                                ext != "image/pjpeg" &&
                                ext != "image/gif" &&
                                ext != "image/x-png" &&
                                ext != "image/png"
                               )

                    {
                    temp.Item1 = false;
                    temp.Item2 = $"Product {product.Name} Not Format Photo";

                    }

                else
                    {
                    product.Photos = Photos.FileName;

                    byte[] imageData = null;
                    using(var binaryReader = new BinaryReader(Photos.OpenReadStream()))
                        {
                        imageData = binaryReader.ReadBytes((int)Photos.Length);
                        }



                    string root = Setting.rootProd+product.Name;

                    string galery = Path.Combine(root, "Galery");



                        DirectoryInfo directoryInfo = new DirectoryInfo(root);
                    if(!directoryInfo.Exists) directoryInfo.Create();

                  
                    DirectoryInfo directoryInfoGal = new DirectoryInfo(galery);
                    if(!directoryInfo.Exists) directoryInfo.Create();


                   string qq=($"{root}\\{Photos.FileName}");

                    using(FileStream fs = new FileStream(qq, FileMode.OpenOrCreate))
                        {

                    using(var binaryWriter = new BinaryWriter(fs))
                        {
                         binaryWriter.Write(imageData);
                        }


                      
                        }

                    



                    }
                }


            string value = temp.Item2;

            TempData["ErrorProduct"] = value;


            if(ModelState.IsValid && temp.Item1)
                {

                HttpContext.Session.Remove("product");
                await _productRepository.ModelAddAsync(product);
                return RedirectToAction(nameof(Index));
                }





            Log.LogInformation($"Create {DateTime.Now.ToString("d")} {this.GetType().Name} {value}");


            ViewData["CategoryId"] = new SelectList(_productRepository._context.Categorys, "Id", "Name", product.CategoryId);
            return RedirectToAction("Index");
            }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
            {
            if(id == null || _productRepository._context.Products == null)
                {
                return NotFound();
                }

            var product = await _productRepository.ModelIdAsync(id);
            if(product == null)
                {
                return NotFound();
                }

            ViewData["CategoryId"] = new SelectList(_productRepository._context.Categorys, "Id", "Name", product.CategoryId);
            return View(product);
            }

        // POST: Products/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Category,Sale,About,Reviews,Photo")] Product product, IFormFile Photos)
            {


            /*My Validasion*/
            var temp = _productRepository.CheckModel(product, nameof(Edit));





            if(Photos != null && Photos.Length > 0)

                {
                string ext = Photos.ContentType.ToLower();

                if(

                                ext != "image/jpg" &&
                                ext != "image/jpeg" &&
                                ext != "image/pjpeg" &&
                                ext != "image/gif" &&
                                ext != "image/x-png" &&
                                ext != "image/png"
                               )

                    {
                    temp.Item1 = false;
                    temp.Item2 = $"Product {product.Name} Not Format Photo";

                    }

                else
                    {
                    product.Photos = Photos.FileName;

                    byte[] imageData = null;
                    using(var binaryReader = new BinaryReader(Photos.OpenReadStream()))
                        {
                        imageData = binaryReader.ReadBytes((int)Photos.Length);
                        }



                    string root = Setting.rootProd + product.Name;

                    string galery = Path.Combine(root, "Galery");



                    DirectoryInfo directoryInfo = new DirectoryInfo(root);
                    if(!directoryInfo.Exists) directoryInfo.Create();


                    DirectoryInfo directoryInfoGal = new DirectoryInfo(galery);
                    if(!directoryInfo.Exists) directoryInfo.Create();


                    string qq = ($"{root}\\{Photos.FileName}");

                    using(FileStream fs = new FileStream(qq, FileMode.OpenOrCreate))
                        {

                        using(var binaryWriter = new BinaryWriter(fs))
                            {
                            binaryWriter.Write(imageData);
                            }



                        }





                    }
                }






            string value = temp.Item2;
            TempData["ErrorProduct"] = value;


            if(id != product.Id)
                {
                return NotFound();
                }
            /*  product.Category=_productRepository._context.Categorys.FirstOrDefault(x => x.Id == product.CategoryId);*/


            ModelState.Remove("Category");
            ModelState.Remove("Photos");

            if(ModelState.IsValid && temp.Item1)
                {
                try
                    {
                    HttpContext.Session.Remove("product");
                    await _productRepository.ModelUpdateAsync(product);
                    }
                catch(DbUpdateConcurrencyException)
                    {
                    if(!ProductExists(product.Id))
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



            ViewData["CategoryId"] = new SelectList(_productRepository._context.Categorys, "Id", "Name", product.CategoryId);
            return RedirectToAction("Index");
            }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
            {
            if(id == null || _productRepository._context.Products == null)
                {
                return NotFound();
                }

            var product = await _productRepository.ModelFirstofDefaultAsync(id);
            if(product == null)
                {
                return NotFound();
                }

            return View(product);
            }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            if(_productRepository._context.Products == null)
                {
                return Problem("Entity set 'AppDbContent.Products'  is null.");
                }
            var product = await _productRepository.ModelIdAsync(id);



            DirectoryInfo di = new DirectoryInfo(Path.Combine(Setting.rootProd, product.Name));
            if(di.Exists) Directory.Delete(Path.Combine(Setting.rootProd, product.Name), true);

            HttpContext.Session.Remove("product");




            TempData["ErrorProduct"] = $"Deleted Product {product.Name}";
            await _productRepository.ModelDeleteAsync(product);
            return RedirectToAction(nameof(Index));
            }

        private bool ProductExists(int id) => _productRepository.ModelExist(id);


        /*  Создаём метод сортировки*/
        /* Get*/

        public async Task<IActionResult> Sorting(string str, bool asc, int? catId = null)
            {
            if(catId != null)
                {
                return RedirectToAction("Index", new { catId = catId });
                }


            var products = _productRepository._context.Products;
            ICollection<Product> productssort = products.MySorting(str, asc);

/*
            ViewBag.Hidding = ((User.IsInRole("Admin") || User.IsInRole("Moderator")));

            ViewBag.Category = new SelectList(
            _productRepository._context.Categorys.ToList(), "Id", "Name");



            ViewBag.SelectedCat = "0";
            IPagedList<Product> productslst = productssort.ToPagedList(1, Setting.PagesSort);*/




            /* return View("Index", productslst);*/
         
            HttpContext.Session.SetString("product", JsonConvert.SerializeObject(productssort));

            return RedirectToAction("Index");



            }





        }
    }
