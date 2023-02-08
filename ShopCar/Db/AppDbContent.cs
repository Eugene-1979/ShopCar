using ShopCar.Models;
using Microsoft.EntityFrameworkCore;


namespace ShopCar.Db
    {
    public class AppDbContent : DbContext
        {
        public AppDbContent()
            {

            }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder
               .Entity<Product>()
               .HasMany(c => c.Orders)
               .WithMany(s => s.Products)
               .UsingEntity<Enrollment>(
                  j => j


                   .HasOne(pt => pt.Order)
                   .WithMany(t => t.Enrollments)
                   .HasForeignKey(pt => pt.OrderId),

               j => j
                   .HasOne(pt => pt.Product)
                   .WithMany(p => p.Enrollments)
                   .HasForeignKey(pt => pt.ProductId),
               j =>
               {

                   j.Property(pt => pt.Count).HasDefaultValue(3);
                   j.HasKey(t => new { t.OrderId, t.ProductId });
                   j.ToTable("Enrollments");
               });

            } 




        public AppDbContent(DbContextOptions<AppDbContent> options) : base(options)
            {


       
        }
   


        public virtual DbSet<Product> Products { get; set; } 
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Enrollment> Enrollment { get; set; } = default!;
   

        }
    }
