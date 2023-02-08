using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopCar.Filter
    {
    public class QueryFilterAttribute : Attribute, IResourceFilter
        {
        string temp;
        public void OnResourceExecuted(ResourceExecutedContext context)
            {
      var  temp1=context.HttpContext.Request.Query.
             Select(q => $"key{q.Key} Value{q.Value.ToString()}").ToArray();

            temp = string.Join(',', temp1);
            }

        public void OnResourceExecuting(ResourceExecutingContext context)
            {
            context.HttpContext.Response.Headers.Add("queryString", temp);
            }
        }
    }
