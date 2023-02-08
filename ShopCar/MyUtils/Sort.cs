using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace ShopCar.MyUtils
    {
    public static class Helps
        {
        public static ICollection<T> MySorting <T>(this IQueryable<T> source,
        string propertyName,
        bool descending) where T :class
            {
            System.Reflection.PropertyInfo? propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            return descending ? source.AsEnumerable().OrderBy(q => propertyInfo.GetValue(q)).ToList() :
            source.AsEnumerable().OrderByDescending(q => propertyInfo.GetValue(q)).ToList();
            }

        
        }
    }
