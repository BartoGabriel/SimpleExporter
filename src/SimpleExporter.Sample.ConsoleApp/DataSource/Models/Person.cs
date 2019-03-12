using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleExporter.Sample.ConsoleApp.DataSource.Models
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int? FileNumber { get; set; }

        public double? Height { get; set; }

    }
}
