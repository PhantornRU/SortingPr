using System.Collections.Generic;

namespace Sorting2D
{
    class SizeComparerContainers : IComparer<Container> //сортировка по объему
    {
        public int Compare(Container o1, Container o2)
        {
            if (o1.weight * o1.height < o2.weight * o2.height)
            {
                return 1;
            }
            else if (o1.weight * o1.height > o2.weight * o2.height)
            {
                return -1;
            }

            return 0;
        }
    }
    class SizeComparerProducts : IComparer<ObjectGoods> //сортировка по объему
    {
        public int Compare(ObjectGoods o1, ObjectGoods o2)
        {
            if (o1.weight * o1.height < o2.weight * o2.height)
            {
                return 1;
            }
            else if (o1.weight * o1.height > o2.weight * o2.height)
            {
                return -1;
            }

            return 0;
        }
    }
}
