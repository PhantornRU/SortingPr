using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sorting2D
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            dra = CreateGraphics();
        }

        List<Container> containers = new List<Container>();
        List<ObjectGoods> objectsGoods = new List<ObjectGoods>();

        public Graphics dra;
        public SolidBrush sb = new SolidBrush(Color.Black);
        public Pen pen = new Pen(new SolidBrush(Color.Black), 3);

        private void buttonTest_Click(object sender, EventArgs e)
        {
            //this.Invalidate();

            containers.Add(new Container(1, "Контейнер1", 150, 150));
            //containers.Add(new Container(2, "Контейнер2", 80, 80));
            //containers.Add(new Container(3, "Контейнер3", 200, 200));
            objectsGoods.Add(new ObjectGoods(1, 1, "Монитор", 100, 100));
            objectsGoods.Add(new ObjectGoods(2, 2, "Клавиатура", 30, 30));
            objectsGoods.Add(new ObjectGoods(3, 2, "Товар3", 20, 20));
            objectsGoods.Add(new ObjectGoods(4, 1, "Товар4", 50, 30));
            objectsGoods.Add(new ObjectGoods(5, 1, "Товар5", 80, 20));
            objectsGoods.Add(new ObjectGoods(6, 1, "Товар6", 30, 20));
            objectsGoods.Add(new ObjectGoods(7, 1, "Товар7", 10, 20));
            objectsGoods.Add(new ObjectGoods(8, 1, "Товар8", 30, 10));
            objectsGoods.Add(new ObjectGoods(9, 1, "Товар9", 10, 40));

            //testSorting();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            loadList(@"ListContainersObjects#2.txt");
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {

            //Сортируем контейнеры от большего к меньшему
            SizeComparerContainers scc = new SizeComparerContainers();
            containers.Sort(scc);

            //Сортируем продукты от большего к меньшему
            SizeComparerProducts sco = new SizeComparerProducts();
            objectsGoods.Sort(sco);

            sortingToContainers();
            foreach (var container in containers) container.Visual(dra, pen);

            Console.WriteLine($"");
            Console.WriteLine($"Размещение:");
            foreach (var container in containers)
            {
                container.GetInfo();
                foreach (var product in container.products)
                {
                    product.GetInfo();
                    product.Visual(dra, pen);
                }
            }
        }

        Font fnt = new System.Drawing.Font("Arial", 10);
        Brush fntbr = new SolidBrush(Color.Black);

        private void sortingToContainers()
        {
            ////координаты контейнеров для более удобного визуализирования
            //int xCon = 0;
            //foreach (var container in containers) 
            //{
            //    xCon += container.x;
            //    container.x = xCon;
            //}

            //распределяем по контейнерам
            foreach (var container in containers) 
            {   
                //проверяем все продукты в списке objectGoods
                foreach (var product in objectsGoods.OrderByDescending(o => (o.weight * o.height)))
                {   
                    //проверяем условие помещается ли объект
                    if (container.weight >= product.weight && container.height >= product.height && (container.products.Count != 0))
                    {
                        bool check = false;
                        //сверяемся со списком товаров в контейнере
                        foreach (var productCont in container.products)
                        {
                            //проверяем возможно ли разместить сбоку
                            if (
                                (product.x < (productCont.x + productCont.weight))
                                && (container.height - (productCont.y + productCont.height) >= productCont.y + product.height)
                                && (container.weight - (productCont.x + productCont.weight) >= productCont.x + product.weight)
                               )
                            {
                                product.x = productCont.x + productCont.weight;
                                product.y = productCont.y;
                                check = true;

                                //проверяем не схож ли объект с остальными
                                foreach (var productC in container.products)
                                    if (
                                        (product.x >= productC.x) && (product.x < productC.x + productC.weight)
                                        && (product.y >= productC.y) && (product.y < productC.y + productC.height)
                                       )
                                    { check = false; Console.WriteLine($"Вышло: #{product.number}[{container.number}] [{product.x};{product.y}]"); }

                                //if (container.products.Find( x => x.x >= product.x) == true) { check = true; }

                                Console.WriteLine($"Тест №1: #{product.number}[{container.number}] [{product.x};{product.y}]");
                                //dra.DrawString($"#{product.number}[{container.number}]", fnt, fntbr, product.x + Convert.ToInt32(product.weight), product.y + Convert.ToInt32(product.weight / 2)); //номер
                            }
                            //проверяем возможно ли разместить сверху
                            else 
                            if (
                                (product.y < (productCont.y + productCont.height))
                                && (container.height - (productCont.y + productCont.height) >= product.y + product.height)
                                && (container.weight >= productCont.x + product.weight)
                               )
                            {
                                product.x = productCont.x;
                                product.y = productCont.y + productCont.height;
                                check = true;

                                //проверяем не схож ли объект с остальными
                                foreach (var productC in container.products)
                                    if (
                                        (product.x >= productC.x) && (product.x < productC.x + productC.weight)
                                        && (product.y >= productC.y) && (product.y < productC.y + productC.height)
                                       )
                                    { check = false; Console.WriteLine($"Вышло: #{product.number}[{container.number}] [{product.x};{product.y}]"); }

                                Console.WriteLine($"Тест №2: #{product.number}[{container.number}] [{product.x};{product.y}]");
                                //dra.DrawString($"#{product.number}", fnt, fntbr, product.x + Convert.ToInt32(product.weight / 2) + 50, product.y + Convert.ToInt32(product.weight / 2) + 50); //номер
                            }
                            else
                            {
                                //переворачиваем
                                int xw = product.weight;
                                product.weight = product.height;
                                product.height = xw;

                                //проверяем возможно ли перевернутый разместить сбоку
                                if (
                                    (product.x < (productCont.x + productCont.weight))
                                    && (container.height - (productCont.y + productCont.height) >= productCont.y + product.height)
                                    && (container.weight - (productCont.x + productCont.weight) >= productCont.x + product.weight)
                                   )
                                {
                                    product.x = productCont.x;
                                    product.y = productCont.y + productCont.height;
                                    check = true;

                                    //проверяем не схож ли объект с остальными
                                    foreach (var productC in container.products)
                                        if (
                                            (product.x >= productC.x) && (product.x < productC.x + productC.weight)
                                            && (product.y >= productC.y) && (product.y < productC.y + productC.height)
                                           )
                                        { check = false; Console.WriteLine($"Вышло: #{product.number}[{container.number}] [{product.x};{product.y}]"); }

                                    Console.WriteLine($"Тест №3: #{product.number}[{container.number}] [{product.x};{product.y}]");
                                    //dra.DrawString($"#{product.number}", fnt, fntbr, product.x + Convert.ToInt32(product.weight / 2) + 50, product.y + Convert.ToInt32(product.weight / 2) + 50); //номер
                                }
                                //проверяем возможно ли перевернутый разместить сверху
                                else
                                if (
                                    (product.y < (productCont.y + productCont.height))
                                    && (container.height - (productCont.y + productCont.height) >= product.y + product.height)
                                    && (container.weight >= productCont.x + product.weight)
                                   )
                                {
                                    product.x = productCont.x;
                                    product.y = productCont.y + productCont.height;
                                    check = true;

                                    //проверяем не схож ли объект с остальными
                                    foreach (var productC in container.products)
                                        if (
                                            (product.x >= productC.x) && (product.x < productC.x + productC.weight)
                                            && (product.y >= productC.y) && (product.y < productC.y + productC.height)
                                           )
                                        { check = false; Console.WriteLine($"Вышло: #{product.number}[{container.number}] [{product.x};{product.y}]"); }

                                    Console.WriteLine($"Тест №4: #{product.number}[{container.number}] [{product.x};{product.y}]");
                                    //dra.DrawString($"#{product.number}", fnt, fntbr, product.x + Convert.ToInt32(product.weight / 2) + 50, product.y + Convert.ToInt32(product.weight / 2) + 50); //номер
                                }
                            }
                            //Или сделать через интеграл по области допустимых значений


                            ////проверяем не схож ли объект с остальными
                            //foreach (var productC in container.products)
                            //    if (
                            //        (product.x >= productC.x) && (product.x < productC.x + productC.weight)
                            //        && (product.y >= productC.y) && (product.y < productC.y + productC.height)
                            //       )
                            //    { check = false; Console.WriteLine($"Вышло: #{product.number}[{container.number}] [{product.x};{product.y}]"); }
                        };

                        if (check == true)
                        {
                            container.products.Add(product); //перемещаем в другой список
                            objectsGoods.Remove(product);
                        }

                    }
                    //если список объектов контейнера пустой, то проверяем помещается ли он и делаем первый для него объект
                    else if ((container.weight >= product.weight & container.height >= product.height) & (container.products.Count == 0))
                    {
                        container.products.Add(product); //перемещаем в другой список
                        objectsGoods.Remove(product);
                    }
                };
            }
        }

        private void loadList(string pathFile) //загрузка списка для тестов
        {
            StreamReader rewriteMass = new StreamReader(pathFile, System.Text.Encoding.Default);

            string textline;
            string name = "0"; //наименование
            int number = 0; //номер контейнера
            int localnumber = 0; //личный номер заказа (входит в партию заказов или нет)
            int height = 0; //высота
            int weight = 0;//ширина
            int depth = 0; //глубина
            bool checkContainer = false;
            bool checkObject = false;

            while ((textline = rewriteMass.ReadLine()) != null)
            {
                string[] symbs = textline.Split(' ');
                //проверяем каждые разделенные символы
                foreach (var symb in symbs)
                {

                    switch (symb)
                    {
                        case "Container:":
                            checkContainer = true;
                            checkObject = false;
                            break;
                        case "Object:":
                            checkObject = true;
                            checkContainer = false;
                            break;
                        default:
                            break;
                    }
                    string t1;
                    if (checkContainer == true) t1 = "#C(ir)?"; else t1 = "#O(ir)?";
                    if (System.Text.RegularExpressions.Regex.IsMatch(symb, t1, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        string[] mass;
                        if (checkContainer == true) mass = symb.Split('C');
                        else mass = symb.Split('O');
                        foreach (var s in mass) { if (s != "#") number = Convert.ToInt32(s); }
                    }

                    string t2 = "#P(ir)?";
                    if (checkObject == true)
                        if (System.Text.RegularExpressions.Regex.IsMatch(symb, t2, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            string[] mass = symb.Split('P');
                            foreach (var s in mass) { if (s != "#") localnumber = Convert.ToInt32(s); }
                        }

                    if (System.Text.RegularExpressions.Regex.IsMatch(symb, "Name:(ir)?", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        string[] mass = symb.Split(':');
                        foreach (var s in mass) { if (s != "Name") name = s; }
                    }

                    if (System.Text.RegularExpressions.Regex.IsMatch(symb, "(ir)?x(ir)?", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        string[] mass = symb.Split('x');
                        height = Convert.ToInt32(mass[0]);
                        weight = Convert.ToInt32(mass[1]);
                        //try { depth = Convert.ToInt32(mass[2]); } catch (Exception exc) { Console.WriteLine(exc.Message + "\n" + Environment.NewLine); }
                        //checkContainer = false;
                    }
                }

                //Console.WriteLine($"Номер: {number}, Наименование: {name} [{height}х{weight}x{depth}]");
                if (localnumber == 0)
                {

                    containers.Add(new Container(number, name, weight, depth));
                }
                else
                {
                    objectsGoods.Add(new ObjectGoods(number, localnumber, name, weight, depth));
                }
            }

            foreach (var item in containers)
            {
                item.GetInfo();
            };

            foreach (var item in objectsGoods)
            {
                item.GetInfo();
            };
        }

        private void testSorting()
        {
            Console.WriteLine($"До сортировки:");
            foreach (var item in containers)
            {
                item.GetInfo();
            }

            //Сортируем контейнеры от большего к меньшему
            SizeComparerContainers sc = new SizeComparerContainers();
            containers.Sort(sc);

            Console.WriteLine($"");
            Console.WriteLine($"После сортировки:");
            foreach (var item in containers)
            {
                item.GetInfo();
            }

            Console.WriteLine($"");
            Console.WriteLine($"Объекты до сортировки:");
            foreach (var item in objectsGoods)
            {
                item.GetInfo();
                //item.Visual(dra, pen); //!!! проблема с dra
            }

            Console.WriteLine($"");
            Console.WriteLine($"Объекты после сортировки:");
            //Сортируем продукты от большего к меньшему
            SizeComparerProducts sco = new SizeComparerProducts();
            objectsGoods.Sort(sco);
            foreach (ObjectGoods o in objectsGoods) o.GetInfo();
        }
    }
}
