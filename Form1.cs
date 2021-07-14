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
        Container[] cont = new Container[32];
        ObjectGoods[] obj = new ObjectGoods[128];

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            cont[1] = new Container(1, "Контейнер", 10, 5);
            cont[1].x = 10;
            cont[1].y = 5;
            cont[1].z = 0;
            cont[1].GetInfo();

            obj[1] = new ObjectGoods(1, 1, "Товар", 1, 1);
            obj[1].x = 1;
            obj[1].y = 2;
            obj[1].z = 3;
            obj[1].GetInfo();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            loadList(@"ListContainersObjects#1.txt");
            //loadObjects(@"ObjectsGoods#1.txt");

        }
        private void loadList(string pathFile)
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

            //string sPattern = "#C(ir)?";
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
                    cont[number] = new Container(number, name, weight, depth);
                    cont[number].GetInfo();
                }
                else
                {
                    obj[number] = new ObjectGoods(number, localnumber, name, weight, depth);
                    obj[number].GetInfo();
                }

            }
        }
    }


    //Формат CSV / JSON
    // Сереализация, десеариализация

    public class Container
    {
        public string name; //наименование контейнера
        public int number; //номер контейнера
        public int height, weight, depth; //ширина, высота, глубина
        public int x, y, z; //координаты

        public Container(int n, string na, int h, int w) //конструктор двухмерный
        { number = n; name = na; height = h; weight = w; }

        public Container(int n, string na, int h, int w, int d) //конструктор трехмерный
        { number = n; name = na; height = h; weight = w; depth = d; }

        public void GetInfo()
        {
            Console.WriteLine($"Номер: #C{number}, Наименование: {name} [{height}x{weight}x{depth}], Координаты: [{x};{y};{z}]");
        }
    }

    public class ObjectGoods //объекты товары
    {
        public string name; //наименование заказа
        public int number, localnumber; //номер заказа и личный номер заказа (партия заказов)
        public int height, weight, depth; //ширина, высота, глубина
        public int x, y, z; //координаты

        public ObjectGoods(int n, int ln, string na, int h, int w) //конструктор двухмерный
        { number = n; localnumber = ln; name = na; height = h; weight = w; }

        public ObjectGoods(int n, int ln, string na, int h, int w, int d) //конструктор трехмерный
        { number = n; localnumber = ln; name = na; height = h; weight = w; depth = d; }

        public void GetInfo()
        {
            Console.WriteLine($"Номер: #O{number}, Партия: #P{localnumber}, Наименование: {name} [{height}x{weight}], Координаты: [{x};{y};{z}]");
        }
    }
}
