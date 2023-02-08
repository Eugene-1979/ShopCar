using Microsoft.EntityFrameworkCore;
using ShopCar.Db;
using ShopCar.Models;

namespace ShopCar.Repository
    {
    public interface IReposytory<T>
        {

         Task<IEnumerable<T>> ModelAllAsync();
        Task<T> ModelIdAsync(int? id);
         Task ModelUpdateAsync(T model); 
         Task ModelAddAsync(T model);
         Task ModelDeleteAsync(T model);

        Task<T> ModelFirstofDefaultAsync(int? id);

        bool ModelExist(int id);


        (bool, string) CheckModel(T model, string method);
          


        }
    }
