using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopCar.Db;
using ShopCar.Models;
using ShopCar.Repository;
using ShopCar.TestX;

using System.Net.Sockets;
using System.Reflection.Metadata;

namespace ShopCar.ShopCar.TestX
{
    public class UnitTest
        {
        ProductRepository service;


        public UnitTest()
            {
            var data = new List<Product>
            {
                new Product { Name = "BBB" },
                new Product { Name = "ZZZ" },
                new Product { Name = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<AppDbContent>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

             service = new ProductRepository(mockContext.Object);

            }



                [Fact]
        public void GetAllproducts_orders_by_name()
            {
          
            var products = service.GetAllProducts();
            
            Assert.Equal(3, products.Count);
            Assert.Equal("AAA", products[0].Name);
            Assert.Equal("BBB", products[1].Name);
            Assert.Equal("ZZZ", products[2].Name);
            }


        public DbContextOptions<AppDbContent> DummyOptions { get; } = new DbContextOptionsBuilder<AppDbContent>().Options;


       /* Создали фейк Db*/
        [Fact]
            public async void DbSetTest()
                {
                var initialEntities = new[]
                    {
                new Product { Id =1, Name = "Eric Cartoon" },
                new Product { Id = 2, Name = "Billy Jewel" },
            };

                var dbContextMock = new DbContextMock<AppDbContent>(DummyOptions);
                var usersDbSetMock = dbContextMock.CreateDbSetMock(x => x.Products, initialEntities);
            Assert.Equal(2, dbContextMock.Object.Products.Count());

            }


    /*    Добавили через ProductRepository через фейк DB* 2+1=3*/
        [Fact]
        public async void DbSetTestAdd()
            {
            var initialEntities = new[]
                {
                new Product { Id =1, Name = "Eric Cartoon" },
                new Product { Id = 2, Name = "Billy Jewel" },
            };

            var dbContextMock = new DbContextMock<AppDbContent>(DummyOptions);
            var usersDbSetMock = dbContextMock.CreateDbSetMock(x => x.Products, initialEntities);

            service = new ProductRepository(dbContextMock.Object);

            await service.ModelAddAsync(new Product { Id = 10 });

            Assert.Equal(3, (await service.GetAllProductsAsync()).Count());

            }



       /* Добавили через ProductRepository через фейк DB 2-1=1*/

      [Fact]
        public async void DbSetTestRemove()
            {
            var initialEntities = new[]
                {
                new Product { Id =1, Name = "Eric Cartoon" },
                new Product { Id = 2, Name = "Billy Jewel" },
            };

            var dbContextMock = new DbContextMock<AppDbContent>(DummyOptions);
            var usersDbSetMock = dbContextMock.CreateDbSetMock(x => x.Products, initialEntities);

            service = new ProductRepository(dbContextMock.Object);

            await service.ModelDeleteAsync(initialEntities[0]);
            Assert.Equal(1, (await service.GetAllProductsAsync()).Count());
            }






/*
        [Fact]
        public async void DbSetTestfindAndEdit()
            {
            var initialEntities = new[]
                {
                new Product { Id =1, Name = "Eric Cartoon" },
                new Product { Id = 2, Name = "Billy Jewel" },
            };

            var dbContextMock = new DbContextMock<AppDbContent>(DummyOptions);
            var usersDbSetMock = dbContextMock.CreateDbSetMock(x => x.Products, initialEntities);

            service = new ProductRepository(dbContextMock.Object);

            await service.ModelUpdateAsync(new Product { Id = 2, Name = "Bob" });

            var product = await service.ModelFirstofDefaultAsync(2);
            Assert.Equal("Bob", product.Name);

            }*/








        }
    }

