using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;

namespace ShopCar.Repository
    {
    public class OrderRepository : IReposytory<Order>
        {
        internal readonly AppDbContent _context;

        public OrderRepository(AppDbContent context)
            {
            _context = context;
            }

      

        async public Task ModelAddAsync(Order model)
            {
            _context.Add(model);
            await _context.SaveChangesAsync();
            }

      async  public Task<IEnumerable<Order>> ModelAllAsync()=>await _context.Orders.Include(o => o.Customer).Include(o => o.Employee).ToListAsync();
           

      async  public Task ModelDeleteAsync(Order model)
            {
            if(model != null)
                {
                _context.Orders.Remove(model);
                }

            await _context.SaveChangesAsync();
            }

        public bool ModelExist(int id)=> (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
           

       async public Task<Order> ModelFirstofDefaultAsync(int? id)=> await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);


       async public Task<Order> ModelIdAsync(int? id)=> await _context.Orders.FindAsync(id);


       async public Task ModelUpdateAsync(Order model)
            {
            _context.Update(model);
            await _context.SaveChangesAsync();
            }



        public (bool, string) CheckModel(Order model, string method)
            {
            throw new NotImplementedException();
            }










        /* public bool CheckModel(IFormCollection formcollection,out Order order)
             {
             foreach(var item in formcollection)
                 {
                 List<Enrollment> enrollments = new List<Enrollment>();
                 string key = item.Key;
                 if(int.TryParse(key, out int value) && item.Value=="true" ) {
                     foreach(var item1 in formcollection)
                         {
                         if(item1.Key.Equals("_" + value)) {

                         tr

                         enrollments.Add(new Enrollment() {ProductId=value,Count=item1.Value })

                         }
                         }

                 }

                 }




             }*/
        }
    }
