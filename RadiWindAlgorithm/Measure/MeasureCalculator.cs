/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm.Measure
{
    public static class MeasureCalculator
    {
        /// <summary>
        /// Convert a text into the right format.
        /// </summary>
        /// <param name="number">number in text.</param>
        /// <param name="decimals">decimals count.</param>
        /// <returns>fomatted text.</returns>
        [Pythonable]
        public static string NumberDecimal(string number, int decimals)
        {
            decimal result;
            if (!decimal.TryParse(number, out result))
                throw new ArgumentOutOfRangeException(nameof(number), nameof(number) + "must be in a formular of number!");
            
            if(decimals >= 0)
            {
                return result.ToString("F" + decimals);
            }
            else
            {
                decimal t = (decimal)Math.Pow(10, Math.Abs(decimals));
                decimal multi = result / t;
                return (decimal.Parse(multi.ToString("F0")) * t).ToString();
            }
        }
    }
}
