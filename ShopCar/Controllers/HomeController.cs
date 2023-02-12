using Microsoft.AspNetCore.Mvc;
using ShopCar.Db;
using ShopCar.Models;
using ShopCar.MyUtils;
using System.Diagnostics;

namespace ShopCar.Controllers
    {
    public class HomeController : Controller
        {
        private readonly ILogger<HomeController> _logger;
        internal readonly AppDbContent _context;

        public HomeController(ILogger<HomeController> logger, AppDbContent context
        )
            {
_context= context;
            _logger = logger;
            }

        public IActionResult Index()
            {

            var prodAll = _context.Products.ToList();


            var products1 = prodAll.OrderByDescending(q=>q.Sale).ToList();
            var qwe = products1.Take(9).ToList();
            foreach(var item in qwe)
                {
                item.About = item.About.Replace($"height:{Setting.sizeY}px; width:{Setting.sizeX}px", $"height:{Setting.circlemin}px; width:{Setting.circlemin}px;border-radius: 50%");

                }


            ViewBag.products = qwe;
/*----------------------------------------------------------------*/
            /* Самый продаваемый продукт*/
            var prod = _context.Enrollment.
            GroupBy(q => q.ProductId).
            Select(q => new { Id = q.Key, count = q.Sum(qw => qw.Count) }).
            OrderByDescending(q => q.count).Take(3);


            Dictionary<Product, int> bestproduct=new Dictionary<Product, int>();
            foreach(var item in prod)
                {
                bestproduct.Add(prodAll.First(q => q.Id == item.Id),item.count);
                }

            foreach(var item in bestproduct)
                {
                item.Key.About = item.Key.About.Replace($"height:{Setting.sizeY}px; width:{Setting.sizeX}px", $"height:{Setting.circle}px; width:{Setting.circle}px;border-radius: 50%");
                }



            ViewBag.productslast = bestproduct;

            ViewBag.category = _context.Categorys.ToList();

            return View();
            }

        public IActionResult Privacy()
            {
            return View();
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }