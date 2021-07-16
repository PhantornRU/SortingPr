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
            this.Invalidate();

            containers.Add(new Container(1, "Контейнер1", 10, 5));
            containers.Add(new Container(2, "Контейнер2", 5, 5));
            containers.Add(new Container(3, "Контейнер3", 15, 15));
            objectsGoods.Add(new ObjectGoods(1, 1, "Товар", 10, 10));


            Console.WriteLine($"До сортировки:");
            foreach (var item in containers)
            {
                item.GetInfo();
            }

            //Сортируем контейнеры от большего к меньшему
            SizeComparer sc = new SizeComparer();
            containers.Sort(sc);

            Console.WriteLine($"");
            Console.WriteLine($"После сортировки:");
            foreach (var item in objectsGoods)
            {
                item.GetInfo();
                item.x = 50;
                item.y = 50;
                item.Visual(dra, pen); //!!! проблема с dra
            }

            //List<int> sortingToContainers = new List<int>();
            //sortingToContainers.Add((int)AlgorithmType.EB_AFIT);
            //List<ContainerPackingResult> result = PackingService.Pack(containers, objectsGoods, sortingToContainers);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            loadList(@"ListContainersObjects#1.txt");


        }
        private void sortForBigger() 
        { 

        }


        private void sortingToContainers()
        {
            //проблема в том что неясно как выразить itemObjHight чтобы он стал нашим временным значением

            foreach (var container in containers)
            {
                foreach (var product in objectsGoods.OrderByDescending(o => o.height))
                {
                    if (container.heightNoUse > product.height & container.weightNoUse > product.weight)
                    {
                        container.heightNoUse -= product.height;
                        container.weightNoUse -= product.weight;
                        container.products.Add(product);
                    }
                };
            }

            //Сортируем контейнеры по возрастанию
            var sortedContainers = from c in containers
                              orderby c.number
                              select c;
            foreach (Container c in sortedContainers) c.GetInfo();
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
                //Console.WriteLine($"Номер: #C{item.number}, Наименование: {item.name} [{item.height}x{item.weight}x{item.depth}], Координаты: [{item.x};{item.y};{item.z}]");
            };

            foreach (var item in objectsGoods)
            {
                Console.WriteLine($"Номер: #O{item.number}, Партия: #P{item.localnumber}, Наименование: {item.name} [{item.height}x{item.weight}], Координаты: [{item.x};{item.y};{item.z}]");
            };
        }
    }

    class SizeComparer : IComparer<Container> //сортировка по объему
    {
        public int Compare(Container o1, Container o2)
        {
            if (o1.weight * o1.height > o2.weight * o2.height)
            {
                return 1;
            }
            else if (o1.weight * o1.height < o2.weight * o2.height)
            {
                return -1;
            }

            return 0;
        }
    }
}
