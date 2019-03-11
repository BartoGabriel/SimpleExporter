using System;

namespace SimpleExporter.Sample.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Sample App Started");
            Console.WriteLine();

            //Sample1
            Console.WriteLine("-----------------");
            Console.WriteLine("Running Sample 1");
            Sample1.Run();
            Console.WriteLine("Sample 1 Finish");
            Console.WriteLine("-----------------");
            Console.WriteLine();

            //Sample2
            Console.WriteLine("-----------------");
            Console.WriteLine("Running Sample 2");
            Sample2.Run();
            Console.WriteLine("Sample 2 Finish");
            Console.WriteLine("-----------------");
            Console.WriteLine();

            //Sample3
            Console.WriteLine("-----------------");
            Console.WriteLine("Running Sample 3");
            Sample3.Run();
            Console.WriteLine("Sample 3 Finish");
            Console.WriteLine("-----------------");
            Console.WriteLine();

            Console.WriteLine("Sample App Finish");
            Console.ReadLine();
        }
    }
}