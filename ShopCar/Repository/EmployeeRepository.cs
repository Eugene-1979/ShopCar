using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;

namespace ShopCar.Repository
    {
    public class EmployeeRepository : IReposytory<Employee>
        {

        internal readonly AppDbContent _context;

        public EmployeeRepository(AppDbContent context)
            {
            _context = context;
            }

      

        async public Task ModelAddAsync(Employee model)
            {
            _context.Add(model);
            await _context.SaveChangesAsync();
            }

        async public Task<IEnumerable<Employee>> ModelAllAsync() => await _context.Employees.ToListAsync();



       async public Task ModelDeleteAsync(Employee model)
            {
            if(model!= null)
                {
                _context.Employees.Remove(model);
                }

            await _context.SaveChangesAsync();
            }

        public bool ModelExist(int id)=> (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
            

      async public Task<Employee> ModelFirstofDefaultAsync(int? id)=> await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);


      async  public Task<Employee> ModelIdAsync(int? id)=> await _context.Employees.FindAsync(id);


      async  public Task ModelUpdateAsync(Employee model)
            {
            _context.Update(model);
            await _context.SaveChangesAsync();
            }



        public (bool, string) CheckModel(Employee model, string method)
            {
            if(method.Equals("Create"))
                {
                if(_context.Employees.ToList().Select(q => q.Email).Any(q => q == model.Email)) return (false, $"No Create Emploee  Email-  {model.Email}  is Exist");
          


                }




            return (true, $"{method} ok in {model.Name}");
            }



        }
    }
