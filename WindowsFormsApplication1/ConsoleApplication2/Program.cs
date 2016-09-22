using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<MyDbContext>());

            // Создать объект контекста
            MyDbContext context = new MyDbContext();

            /* 1
                Выбирает из таблицы ТОВАРЫ информацию о товарах, 
                единицей измерения которых является «шт» (штуки)
            */
            Console.WriteLine("1. Названия товаров, единицей измерения которых является «шт»:");
            var sql1 = context.Database.SqlQuery<Product>("SELECT * FROM Products Where Measure='шт'");
            foreach (var prod in sql1)
            {
                Console.WriteLine(prod.Name);
            }

            /* 2
             *  Выбирает из таблицы ТОВАРЫ информацию о товарах, 
             *  цена закупки которых находится в диапазоне 
             *  больше 500 руб за единицу товара
             */
            Console.WriteLine("2. Товары от 500руб :");
            var sql2 = context.Database.SqlQuery<Product>("SELECT * FROM Products Where Price>500");
            foreach (var prod in sql2)
            {
                Console.WriteLine(prod.Name);
            }

            /* 3
             Выбирает из таблицы ПРОДАВЦЫ информацию о продавцах, 
             для которых установлен процент комиссионных в диапазоне от 5% до 10%
             */
            Console.WriteLine("3. ПРОДАВЦЫ для которых установлен процент комиссионных в диапазоне от 5 до 10 (%):");
            var sql3 = context.Database.SqlQuery<Seller>("SELECT * FROM Sellers Where Commission>=5 and Commission<=10");
            foreach (var item in sql3)
            {
                Console.WriteLine(item.FirstName + ' ' + item.MiddleName + ' ' + item.Sername);
            }

            /* 4
             * Выбирает из таблицы ПРОДАВЦЫ информацию о продавцах 
             * с заданной фамилией. Фамилия вводится при выполнении запроса
             */
            Console.Write("4. Введите фамилию продавца:");
            string input = Console.ReadLine();
            var sql4 = context.Database.SqlQuery<Seller>("SELECT * FROM Sellers Where Sername='" + input + "'");
            Console.WriteLine("С такой фамилией было найдено продавцов:"+sql4.Count());
            foreach (var item in sql4)
            {
                Console.WriteLine(item.FirstName + ' ' + item.MiddleName + ' ' + item.Sername);
            }

            /* 5 
             *  Выбирает из таблиц ТОВАРЫ, ПРОДАВЦЫ и ПРОДАЖИ информацию
             *  обо всех зафиксированных фактах продажи товаров 
             *  (Наименование товара, Цена закупки, Цена продажи, дата продажи),
             *  для которых Цена продажи оказалась в некоторых заданных границах. 
             *  Нижняя и верхняя границы интервала цены продажи задаются
             *  при выполнении запроса
             */
            Console.Write("5. Введите диапазон цены для поиска ПРОДАЖ:");

            input = Console.ReadLine();
            int min;
            Int32.TryParse(input, out min);

            input = Console.ReadLine();
            int max;
            Int32.TryParse(input, out max);

            var sql5 = context.Database.SqlQuery<class5>("SELECT Name, Price, SalePrice, DateCreated " +
                " FROM Sales, Products Where Sales.ProductId=Products.ProductId and Sales.SalePrice>=" + min+" and Sales.SalePrice<="+max).ToList();
            Console.WriteLine("В таком ценовом диапазоне товаров:" + sql5.Count());
            foreach (var item in sql5)
            {
                Console.WriteLine(item.Name+" "+item.Price + " " + item.SalePrice + " " + item.DateCreated);
            }

            /* 6
             * Вычисляет прибыль от продажи за каждый проданный товар.
             *  Включает поля Дата продажи, Код продавца, 
             *  Наименование товара, Цена закупки, Цена продажи, 
             *  Количество проданных единиц, Прибыль. Сортировка по полю Дата продажи
             * 
            */
            /*Console.WriteLine("6. Прибыль от продаж:");
            // получаем коды всех товаров, что продали
            var sql61 = context.Database.SqlQuery<int>("SELECT SaleId FROM Sales;").ToList();
            foreach (var item in sql61)
            {
                var sql62 = context.Database.SqlQuery<class6>("SELECT CreMIN(Price) as min,MAX(Price) as max FROM Products Where Name='" + item + "'").ToList();

                Console.Write("Для продукта '" + item + "'");
                
            }*/

            /* 7
                Выполняет группировку по полю Наименование товара. 
                Для каждого наименования вычисляет минимальную и максимальную 
                цену закупки товара
            */
            Console.WriteLine("7. Цены на товары:");
            // получаем все имена товаров
            // в данном методе группировка по имени делает то же, что и сортировка по имени
            var sql71 = context.Database.SqlQuery<string>("SELECT DISTINCT Name FROM Products GROUP BY Name;").ToList();
            // для каждого имени ищем минимум и максимум цен
            foreach (var item in sql71)
            {
                var sql72 = context.Database.SqlQuery<class7>("SELECT MIN(Price) as min,MAX(Price) as max FROM Products Where Name='" + item + "'").ToList();

                Console.Write("Для продукта '" + item + "'");
                if (sql72[0].min== sql72[0].max)
                {
                    Console.WriteLine(" одна закупачная цена - " + sql72[0].max);
                }
                else
                {
                    Console.WriteLine(" минимальная цена закупки - "+sql72[0].min+", максимальная - " + sql72[0].max);
                }
            }

            /* 8
                	
                Выполняет группировку по полю Код товара из таблицы ПРОДАЖИ. 
                Для каждого товара вычисляет суммарное значение по полю 
                Количество проданных единиц товара
            */
            
            Console.WriteLine("8. Общее количество продаж товаров:");
            var sql81 = context.Database.SqlQuery<int>("SELECT DISTINCT Sales.ProductId FROM Sales GROUP BY Sales.ProductId;");
            // для каждого товара ищем количество проданых единиц
            foreach (var item in sql81)
            {
                var sql82 = context.Database.SqlQuery<int>("SELECT SUM(Amount) FROM Sales Where ProductId=" + item).FirstOrDefault();

                Console.WriteLine("Продукт с id=" + item + " был продан " + sql82+" раз.");

            }

            /* 9
               Создает таблицу ДЕШЕВЫЕ_ТОВАРЫ, содержащую информацию о товарах,
               цена закупки которых меньше 200 за единицу товара
            */
            Console.WriteLine("9. Вид таблицы ДЕШЕВЫЕ_ТОВАРЫ:");
            // удаляем таблицу с дешевыми товарами, если есть (чтобы создать :)
            try{

                context.Database.ExecuteSqlCommand("DROP TABLE Cheap_Products");
            }catch(Exception){}
        
            // Создание таблицы ДЕШЕВЫЕ_ТОВАРЫ
            context.Database.ExecuteSqlCommand("SELECT * INTO Cheap_Products FROM Products Where Price<200;");
            // вывод  товаров из таблицы ДЕШЕВЫЕ_ТОВАРЫ
            var sql92 = context.Database.SqlQuery<Product>("SELECT DISTINCT * FROM Cheap_Products ");
            foreach (var prod in sql92)
            {
                Console.WriteLine(prod.Name+"\t"+prod.Price);
            }

            /* 10
                Создает копию таблицы ТОВАРЫ с именем КОПИЯ_ ТОВАРЫ
            */
            Console.WriteLine("10. Вид копии таблицы ТОВАРЫ:");
            // удаляем таблицу-копию товаров, если есть [чтобы создать снова :]
            try
            {
                context.Database.ExecuteSqlCommand("DROP TABLE Copy_Products");
            }catch (Exception){}
            // Создание таблицы КОПИЯ_ТОВАРЫ
            context.Database.ExecuteSqlCommand("SELECT * INTO Copy_Products FROM Products;");
            // вывод  товаров из таблицы КОПИЯ_ ТОВАРЫ
            var sql102 = context.Database.SqlQuery<Product>("SELECT * FROM Copy_Products ");
            foreach (var prod in sql102)
            {
                Console.WriteLine(prod.ProductId + " " + prod.Name);
            }

            /* 11
                Удаляет из таблицы КОПИЯ_ТОВАРЫ записи, 
                в которых значение в поле Единица измерения товара равна «шт» (штуки)
            */
            Console.WriteLine("11. Вид копии таблицы ТОВАРЫ после удаление продуктов, которые измеряются поштучно:");
            // Создание таблицы ДЕШЕВЫЕ_ТОВАРЫ
            context.Database.ExecuteSqlCommand("DELETE FROM Copy_Products Where Measure='шт';");
            // вывод  товаров из таблицы ДЕШЕВЫЕ_ТОВАРЫ
            var sql112 = context.Database.SqlQuery<Product>("SELECT * FROM Copy_Products ");
            foreach (var prod in sql112)
            {
                Console.WriteLine(prod.ProductId + " " + prod.Name);
            }

            /* 12
               Устанавливает значение в поле Процент комиссионных 
               таблицы ПРОДАВЦЫ равным 10% для тех продавцов, 
               процент комиссионных которых составляет меньше 10 процентов
            */
            Console.WriteLine("12. Новая процент комисионных для продавцов:");
            // Создание таблицы ДЕШЕВЫЕ_ТОВАРЫ
            var sql121 = context.Database.ExecuteSqlCommand("UPDATE Sellers SET commission=10 WHERE commission<10;");
            // вывод  всех продавцов
            var sql122 = context.Database.SqlQuery<Seller>("SELECT * FROM Sellers ");
            foreach (var item in sql122)
            {
                Console.WriteLine(item.FirstName + ' ' + item.MiddleName + ' ' + item.Sername+" - комиссионные "+item.Commission+"%");
            }

            //context.SaveChanges();
        }
        public partial class class5
        {
            //Name, Price, SalePrice, DateCreated
            public string Name { get; set; }
            public float Price { get; set; }
            public int SalePrice { get; set; }
            public DateTime DateCreated { get; set; }
        }
        public partial class class7
        {
            public float min { get; set; }
            public float max { get; set; }

        }
        public partial class class6
        {
            public float Profit { get; set; }
            public int SoldAmount { get; set; }
            public int SalePrice { get; set; }
            public float Price { get; set; }
            public string ProductName { get; set; }
            public int SallerId { get; set; }
            public DateTime DateCreated { get; set; }
        }
    }
}
