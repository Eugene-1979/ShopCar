/*using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Reflection;
using System.Text.Encodings.Web;

namespace ShopCar.MyUtils
    {
    public static class  MyHelperList
        {
        public static HtmlString CreateList<T>(this IHtmlHelper html, List<T> collections) where T:AbstractModel

            {
            Type type = typeof(T);
            List<PropertyInfo> propertyInfos = type.GetProperties().
               Where(
               q => (q.PropertyType == typeof(Int32)) ||
               (q.PropertyType == typeof(double)) ||
               (q.PropertyType == typeof(string))).ToList();

            TagBuilder tabl = new TagBuilder("table");
            tabl.MergeAttribute("class", "table table-striped table-bordered table-hover table-sm table-dark");

            TagBuilder thead = new TagBuilder("thead");
thead.MergeAttribute("class", "thead-dark");
            TagBuilder tr = new TagBuilder("tr");
            thead.InnerHtml.AppendHtml(tr);


           *//* Шапка таблицы*//*
            propertyInfos.ForEach(propertyInfo => {
                TagBuilder th = new TagBuilder("th");
                
                th.InnerHtml.Append(propertyInfo.Name);
                tr.InnerHtml.AppendHtml(th);                    
            });
            TagBuilder th11 = new TagBuilder("th");
            th11.InnerHtml.Append("Link");
            tr.InnerHtml.AppendHtml(th11);

            tabl.InnerHtml.AppendHtml(thead);





            foreach(var item in collections)
                {
                TagBuilder trVal = new TagBuilder("tr");
                foreach(var temp in propertyInfos)
                    {
                    TagBuilder tdVal = new TagBuilder("td");                
                    tdVal.InnerHtml.Append(temp.GetValue(item).ToString());
                    trVal.InnerHtml.AppendHtml(tdVal);
                    }

                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", $"/{typeof(T).Name}/Details/{item.PrimaryKey}");
                a.InnerHtml.Append($"{item.Name}");

                TagBuilder tdValEnd = new TagBuilder("td");
                tdValEnd.InnerHtml.AppendHtml(a);
                trVal.InnerHtml.AppendHtml(tdValEnd);

                tabl.InnerHtml.AppendHtml(trVal);
                }





            var writer = new System.IO.StringWriter();
            tabl.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());






            }
        }
    }
*/