using System;
using System.Drawing;

namespace Sorting2D
{
    public class ObjectGoods //объекты товары
    {
        public string name; //наименование заказа
        public int number, localnumber; //номер заказа и личный номер заказа (партия заказов)
        public int height, weight, depth; //ширина, высота, глубина
        public int x, y, z; //координаты
        public bool place = false; //Размещен или нет

        public ObjectGoods(int n, int ln, string na, int h, int w) //конструктор двухмерный
        { number = n; localnumber = ln; name = na; height = h; weight = w; }

        public ObjectGoods(int n, int ln, string na, int h, int w, int d) //конструктор трехмерный
        { number = n; localnumber = ln; name = na; height = h; weight = w; depth = d; }

        public void GetInfo()
        {
            Console.WriteLine($"Номер: #O{number}, Партия: #P{localnumber}, Наименование: {name} [{height}x{weight}], Координаты: [{x};{y};{z}]");
        }
        public void Visual(Graphics dra, Pen pen)
        {
            dra.DrawRectangle(new Pen(new SolidBrush(Color.Black), 3), 0, 0, 100, 100);
        }
        //public void visual(e.Graphics dra, Pen pen)
        //{
        //    dra.DrawRectangle(pen, x, y, x + height, y + weight);
        //}
    }
}
