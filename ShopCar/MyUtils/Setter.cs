using System.Reflection;

namespace ShopCar.MyUtils
    {
    public class Setter
        {
        public static void SetValue(Object obj, IFormCollection collection) {
            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach(var item in propertyInfos)
                {
                if(collection.ContainsKey(item.Name))
                {
                    
                var str = collection[item.Name].First();
                
                if(item.PropertyType == typeof(Int32))
                        {
                        
                        if(Int32.TryParse(str, out var val))


                       { item.SetValue(obj, val); }
                       else if (Double.TryParse(str, out var vat))


                            { item.SetValue(obj, vat); }




                        else { item.SetValue(obj, 0); }
                        }
                    else {
                   

                        item.SetValue(obj, str);
                    
                    }


                     ;
                  
                    
                }

               
                }
        
        
        }


        }
    }
