using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;

namespace ShopCar.Repository
    {
    public class CustomerRepository : IReposytory<Customer>
        {
       internal readonly AppDbContent _context;

        public CustomerRepository(AppDbContent context)
            {
            _context = context;
            }

      

        async public Task ModelAddAsync(Customer model)
            {
            _context.Add(model);
            await _context.SaveChangesAsync();
            }



        async public Task<IEnumerable<Customer>> ModelAllAsync() => await _context.Customers.ToListAsync();



      async  public Task ModelDeleteAsync(Customer model)
            {
            if(model != null)
                {
                _context.Customers.Remove(model);
                }

            await _context.SaveChangesAsync();
            }

        public bool ModelExist(int id) => (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
           

       async public Task<Customer> ModelFirstofDefaultAsync(int? id) => await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);


      async public Task<Customer> ModelIdAsync(int? id)=> await _context.Customers.FindAsync(id);
         

       async public Task ModelUpdateAsync(Customer model)
            {
            _context.Update(model);
            await _context.SaveChangesAsync();
            }



        public (bool, string) CheckModel(Customer model, string method)
            {

            if(method.Equals("Create"))
            {
            if(_context.Customers.ToList().Select(q=>q.Email).Any(q=>q==model.Email)) return (false, $"No Create Customer  Email-  {model.Email}  is Exist");
            if(_context.Customers.ToList().Select(q=>q.Phone).Any(q=>q==model.Phone)) return (false, $"No Create Customer Phone-  {model.Phone} is Exist");
                

            }
            
            
      

            return (true, $"{method} ok in {model.Name}");
            }



        }
    }
