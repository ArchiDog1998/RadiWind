/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm.Sort
{
    /// <summary>
    /// A struct that include the value's index.
    /// </summary>
    /// <typeparam name="T">Value Type.</typeparam>
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
