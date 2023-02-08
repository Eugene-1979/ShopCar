
using Microsoft.EntityFrameworkCore;
using ShopCar.Data;
using ShopCar.Models;
using ShopCar.Repository;

namespace ShopCar.Db
{
    public static class ShopRepository
        {
      public const int _minSale = 0;
       public const int _maxSale = 100000;






        public static void CreateServiceCollection(this IServiceCollection services)
            {
         /*   services.AddScoped<ApplicationDbContext>();*/
            services.AddScoped<DbObjects>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<EnrollmentRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<ProductRepository>();
            services.AddTransient<ITest,   TestClass>();

            }



/*        private readonly AppDbContent myBase;
        public ShopRepository(AppDbContent appDbContent) => myBase = appDbContent;

        *//*  public Task<List<Product>> GetProducts() => myBase.Products.ToListAsync();*//*

        public Task<List<Product>> AllProducts => myBase.Products.Include(q => q.Category).ToListAsync();

        public Task<List<Order>> AllOrders => myBase.Orders.Include(q => q.Employee).Include(q => q.Customer).ToListAsync();*/



        /* public Task<List<Category>> GetCategorys() => _appDbContent.Categorys.ToListAsync();
         public Task<List<Product>> GetProducts() => _appDbContent.Products.ToListAsync();
         public Task<List<Product>> GetProducts() => _appDbContent.Products.ToListAsync();
         public Task<List<Product>> GetProducts() => _appDbContent.Products.ToListAsync();
         public Task<List<Product>> GetProducts() => _appDbContent.Products.ToListAsync();*/




       






        }
    }
