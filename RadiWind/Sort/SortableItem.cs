using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWind.Sort
{
    public struct SortableItem<T>
    {
        public int Index { get; }
        public T Value { get; set; }
        public SortableItem(int index, T value )
        {
            this.Index = index;
            this.Value = value;
        }
    }
}
