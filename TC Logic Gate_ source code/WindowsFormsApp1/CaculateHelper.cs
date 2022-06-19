using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_Logic_Gate
{
    public class CaculateHelper
    {
        /// <summary>
        /// Calculating mid
        /// </summary>
        /// <param name="arr">array</param>
        /// <returns></returns>
        public double Median(double[] arr)
        {
            //In order not to modify the "arr" value, the calculation and modification of the array are performed in the "tempArr" array
            double[] tempArr = new double[arr.Length];
            arr.CopyTo(tempArr, 0);

            //Sorting the array
            double temp;
            for (int i = 0; i < tempArr.Length; i++)
            {
                for (int j = i; j < tempArr.Length; j++)
                {
                    if (tempArr[i] > tempArr[j])
                    {
                        temp = tempArr[i];
                        tempArr[i] = tempArr[j];
                        tempArr[j] = temp;
                    }
                }
            }

            //Parity Classification Discussion for Array Elements
            if (tempArr.Length % 2 != 0)
            {
                return tempArr[arr.Length / 2 + 1];
            }
            else
            {
                return (tempArr[tempArr.Length / 2] +
                    tempArr[tempArr.Length / 2 + 1]) / 2.0;
            }
        }
        /// <summary>
        /// Calculating max
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public double GetMax(List<double> list, double median)
        {
            double[] tempArr = new double[list.Count];
            list.CopyTo(tempArr, 0);
            //var median = Median(tempArr);


            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i] -= median;
            }
            return tempArr.Max();
        }
    }
}
