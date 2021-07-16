using System;
using System.Collections.Generic;

namespace Sorting2D
{
    //Формат CSV / JSON
    // Сериализация, десеариализация

    public class Container
    {
        public string name; //наименование контейнера
        public int number; //номер контейнера
        public int height, weight, depth; //ширина, высота, глубина
        //public int x, y, z; //координаты
        //public bool full = false; //Заполнен или нет
        public int heightNoUse, weightNoUse, depthNoUse; //неиспользованная ширина, высота, глубина
        public List<ObjectGoods> products = new List<ObjectGoods>(); //содержимое

        public Container(int n, string na, int h, int w) //конструктор двухмерный
        { 
            number = n; name = na; height = h; weight = w;
            heightNoUse = h;
            weightNoUse = w;

            int qwe = default;
            bool asd = default;
            //full = false;
        }

        //public Container(int n, string na, int h, int w, int d) //конструктор трехмерный
        //{ number = n; name = na; height = h; weight = w; depth = d; }

        public void GetInfo()
        {
            Console.WriteLine($"Номер: #C{number}, Наименование: {name} [{height}x{weight}x{depth}]");
        }
        //public void Visual(Graphics dra, Pen pen)
        //{
        //    //e.Graphics.DrawRectangle(pen, x, y, x + height, y + weight);
        //    dra.DrawRectangle(pen, x, y, x + height, y + weight);
        //}
    }
}
