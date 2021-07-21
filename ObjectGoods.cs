using System;
using System.Drawing;

namespace Sorting2D
{
    public class ObjectGoods //объекты товары
    {
        public string name; //наименование заказа
        public int number, localnumber; //номер заказа и личный номер заказа (партия заказов)
        public int weight, height, depth; //ширина, высота, глубина
        public int x, y, z; //координаты
        public bool place = false; //Размещен или нет

        public ObjectGoods(int n, int ln, string na, int w, int h) //конструктор двухмерный
        { number = n; localnumber = ln; name = na; weight = w; height = h; }

        public ObjectGoods(int n, int ln, string na, int w, int h, int d) //конструктор трехмерный
        { number = n; localnumber = ln; name = na; weight = w; height = h; depth = d; }

        public void GetInfo()
        {
            Console.WriteLine($"Номер: #O{number}, Партия: #P{localnumber}, Наименование: {name} [{weight}x{height}], Координаты: [{x};{y};{z}]");
        }
        public void Visual(Graphics dra, Pen pen)
        {
            dra.DrawRectangle(new Pen(new SolidBrush(Color.Black), 3), x, y, weight, height);
        }
        //public void visual(e.Graphics dra, Pen pen)
        //{
        //    dra.DrawRectangle(pen, x, y, height, weight);
        //}
    }
}
