using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDbContext context = new MyDbContext();

            // 1
            var comps = context.Database.SqlQuery<Product>("SELECT * FROM Products Where Measure='шт'");
            foreach (var prod in comps)
            {
                Log(prod.Name);
            }
        }
    }
}
