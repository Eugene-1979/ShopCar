using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;

namespace ShopCar.Repository
    {
    public class EnrollmentRepository : IReposytory<Enrollment>
        {
        internal readonly AppDbContent _context;

        public EnrollmentRepository(AppDbContent context)
            {
            _context = context;
            }

     

        async public Task ModelAddAsync(Enrollment model)
            {
            _context.Add(model);
            await _context.SaveChangesAsync();
            }

      async  public Task<IEnumerable<Enrollment>> ModelAllAsync()=>await _context.Enrollment.Include(e => e.Order).Include(e => e.Product).ToListAsync();


      async  public Task ModelDeleteAsync(Enrollment model)
            {
            if(model != null)
                {
                _context.Enrollment.Remove(model);
                }

            await _context.SaveChangesAsync();
            }



        public bool ModelExist(int id)=> (_context.Enrollment?.Any(e => e.OrderId == id)).GetValueOrDefault();


       async public Task<Enrollment> ModelFirstofDefaultAsync(int? id)=> await _context.Enrollment
                .Include(e => e.Order)
                .Include(e => e.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);


       async public Task<Enrollment> ModelIdAsync(int? id)=> await _context.Enrollment.FindAsync(id);


        async public Task ModelUpdateAsync(Enrollment model)
            {
            _context.Update(model);
            await _context.SaveChangesAsync();
            }



        public (bool, string) CheckModel(Enrollment model, string method)
            {
            throw new NotImplementedException();
            }

        }
    }
