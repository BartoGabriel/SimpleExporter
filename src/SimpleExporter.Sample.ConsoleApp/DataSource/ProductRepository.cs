using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using SimpleExporter.Sample.ConsoleApp.DataSource.Models;

namespace SimpleExporter.Sample.ConsoleApp.DataSource
{
    public static class TestRepository
    {
        public static List<Product> GetAllProducts()
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

        public static List<Person> GetAllPerson()
        {
            var rand = new Random();
            return Enumerable.Range(1, 200)
                .Select(i => new Person
                {
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    BirthDate = DateTime.Now.AddDays(rand.Next(1000)),
                    FileNumber = rand.Next(1000),
                    Height = rand.NextDouble() * 100
                })
                .ToList();
        }
    }
}