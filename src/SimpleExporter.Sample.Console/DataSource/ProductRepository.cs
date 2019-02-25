using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using SimpleExporter.Sample.Console.DataSource.Models;

namespace SimpleExporter.Sample.Console.DataSource
{
    public static class ProductRepository
    {
        public static List<Product> GetAll()
        {
            var rand = new Random();
            return Enumerable.Range(1, 200)
                .Select(i => new Product
                {
                    Id = i,
                    Name = "Product" + i,
                    Description =
                        "This is an example description showing long text in some of the items. Here is some UTF text €",
                    Price = rand.NextDouble() * 100,
                    OrderCount = rand.Next(1000),
                    LastPurchase = DateTime.Now.AddDays(rand.Next(1000)),
                    UnitsInStock = rand.Next(0, 1000)
                })
                .ToList();
        }

        public static IEnumerable<ExpandoObject> GetAllExpando()
        {
            foreach (var product in GetAll())
            {
                dynamic item = new ExpandoObject();

                item.Id = product.Id;
                item.Name = product.Name;
                item.Description = product.Description + " (dynamic)";
                item.Price = product.Price;
                item.OrderCount = product.OrderCount;
                item.LastPuchase = product.LastPurchase;
                item.UnitsInStock = product.UnitsInStock;
                item.LowStock = product.LowStock;

                yield return item;
            }
        }
    }
}