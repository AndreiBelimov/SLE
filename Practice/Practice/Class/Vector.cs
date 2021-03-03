using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    /// <summary>
    /// Вектор значений double
    /// </summary>
    public class Vector
    {
        private readonly double[] data;          
        private readonly int count;         

        public int Count => count;
        public double Norm  => GetNorm();

        public Vector(int count)
        {
            this.count = count;
            data = new double[count];
        }

        public Vector(double[] initArray)
        {
            data = new double[initArray.Length];
            initArray.CopyTo(data, 0);
        }

        public Vector(Matrix initMatrix)
        {
            if (initMatrix.Columns == 1)
            {
                initMatrix.Data.CopyTo(data, 0);
            }
            else throw new ArgumentException();
        }

        /// <summary>
        /// Возвращает копию вектора
        /// </summary>
        /// <returns></returns>
        public Vector Copy()
        {
            Vector v = new Vector(data);
            return v;
        }

        /// <summary>
        /// Обмен элементов внутри вектора
        /// </summary>
        /// <param name="i">Индекс первого элемента</param>
        /// <param name="j">Индекс второго элемента</param>
        public void Swap(int i, int j)
        {
            double temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        /// <summary>
        /// Поиск нормы вектора
        /// </summary>
        /// <returns></returns>
        private double GetNorm()
        {
            double max = data[0];
            for (int i = 1; i < Count; i++)
                max = Math.Max(max, Math.Abs(data[i]));

            return max;
        }

        /// <summary>
        /// Вывод вектора на консоль
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine("x" + (i + 1) + " = " + data[i]);
            Console.WriteLine();
        }

        public double this[int row]
        {
            get { return data[row]; }
            set { data[row] = value; }
        }

        public static Vector operator -(Vector left, Vector right)
        {
            Vector v = new Vector(left.count);
            for (int i = 0; i < left.count; i++)
                v[i] = left[i] - right[i];
            return v;
        }

        public static Vector operator +(Vector left, Vector right)
        {
            Vector v = new Vector(left.count);
            for (int i = 0; i < left.count; i++)
                v[i] = left[i] + right[i];
            return v;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("(");
            foreach (var e in data)
            {
                result.Append(e + " ");
            }
            result.Append(")");

            return result.ToString();
        }
    }
}
