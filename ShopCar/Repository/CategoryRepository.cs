using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;

namespace ShopCar.Repository
    {
    public class CategoryRepository : IReposytory<Category> {
        internal readonly AppDbContent _context;
        public ILogger Log { get; set; }
        public CategoryRepository(AppDbContent context)
            {
           
            _context = context;
            }

        async public Task ModelAddAsync(Category model)
            {
           _context.Add(model);
            await _context.SaveChangesAsync();
            }

        async public Task<IEnumerable<Category>> ModelAllAsync() => await _context.Categorys.ToListAsync();



       async public Task ModelDeleteAsync(Category model)
            {
            if(model != null)
                {
                _context.Categorys.Remove(model);
                }

            await _context.SaveChangesAsync();
            }

        public bool ModelExist(int id) => (_context.Categorys?.Any(e => e.Id == id)).GetValueOrDefault();


        async public Task<Category> ModelFirstofDefaultAsync(int? id) => await _context.Categorys
                     .FirstOrDefaultAsync(m => m.Id == id);


        async public Task<Category> ModelIdAsync(int? id) =>await  _context.Categorys.FindAsync(id);
        
        
        async  public Task ModelUpdateAsync(Category model)
            {
            _context.Update(model);
            await _context.SaveChangesAsync();
            }


        public (bool, string) CheckModel(Category model, string method)
            {

        
            if(
            method.Equals("Create")
            && 
            _context.Categorys.ToList().Contains(model)
            ) return (false, $"Category {model.Name} is Exist");


            return (true, $"{method} ok in {model.Name}");

            }

        }
    }
