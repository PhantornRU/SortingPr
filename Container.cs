using System;
using System.Collections.Generic;
using System.Drawing;

namespace Sorting2D
{
    //Формат CSV / JSON
    // Сериализация, десеариализация

    public class Container
    {
        public string name; //наименование контейнера
        public int number; //номер контейнера
        public int weight, height, depth; //ширина, высота, глубина
        public int x, y, z; //координаты
        //public bool full = false; //Заполнен или нет
        //public int heightNoUse, weightNoUse, depthNoUse; //неиспользованная ширина, высота, глубина
        public List<ObjectGoods> products = new List<ObjectGoods>(); //содержимое

        public Container(int n, string na, int w, int h) //конструктор двухмерный
        { 
            number = n; name = na; weight = w; height = h;
            //weightNoUse = w;
            //heightNoUse = h;

            int qwe = default;
            bool asd = default;
        }

        //public Container(int n, string na, int h, int w, int d) //конструктор трехмерный
        //{ number = n; name = na; height = h; weight = w; depth = d; }

        public void GetInfo()
        {
            Console.WriteLine($"Номер: #C{number}, Наименование: {name} [{weight}x{height}x{depth}]");
        }

        public void Visual(Graphics dra, Pen pen)
        {
            dra.DrawRectangle(new Pen(new SolidBrush(Color.DarkGray), 3), x, y, weight, height);
        }
    }
}
