using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    /// <summary>
    /// Класс вектора
    /// </summary>
    public class Vector
    {
        private double[] data;          //Коэфиценты вектора
        private int count;              //Количество элементов в векторе

        public int Count { get => this.count; }
        public double Norm { get => FindNorm(); }

        public Vector(int count)
        {
            this.count = count;
            data = new double[count];
        }

        public Vector(double[] initArray)
        {
            data = new double[initArray.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = initArray[i];
            count = data.Length;
        }

        public Vector(Matrix initArray)
        {
            if (initArray.Columns == 1)
            {
                double[] temp = new double[initArray.Rows];
                for (int i = 0; i < initArray.Rows; i++)
                    temp[i] = initArray[i, 0];
                data = (double[])temp.Clone();
                count = data.Length;
            }
        }

        /// <summary>
        /// Копирование вектора
        /// </summary>
        /// <returns></returns>
        public Vector Copy()
        {
            Vector v = new Vector(data);
            return v;
        }

        /// <summary>
        /// Обращение к элементу вектора
        /// </summary>
        /// <param name="row">Индекс элемента</param>
        /// <returns></returns>
        public double this[int row]
        {
            get { return data[row]; }
            set { data[row] = value; }
        }

        /// <summary>
        /// Вычитание двух векторов
        /// </summary>
        /// <param name="left">Первый вектор</param>
        /// <param name="right">Второй вектор</param>
        /// <returns></returns>
        public static Vector operator -(Vector left, Vector right)
        {
            Vector v = new Vector(left.count);
            for (int i = 0; i < left.count; i++)
                v[i] = left[i] - right[i];
            return v;
        }

        /// <summary>
        /// Сложение двух векторов
        /// </summary>
        /// <param name="left">Первый вектор</param>
        /// <param name="right">Второй вектор</param>
        /// <returns></returns>
        public static Vector operator +(Vector left, Vector right)
        {
            Vector v = new Vector(left.count);
            for (int i = 0; i < left.count; i++)
                v[i] = left[i] + right[i];
            return v;
        }

        /// <summary>
        /// Обмен элементов
        /// </summary>
        /// <param name="i">Индекс первого элемента</param>
        /// <param name="j">Индекс второго элемента</param>
        public void Swap(int i, int j)
        {
            if (i == j) return;
            double temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        /// <summary>
        /// Вывод вектора 
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine("x" + (i + 1) + " = " + data[i]);
            Console.WriteLine();
        }

        /// <summary>
        /// Поиск нормы вектора
        /// </summary>
        /// <returns></returns>
        private double FindNorm()
        {
            double max = double.MinValue;
            for (int i = 0; i < Count; i++)
                max = Math.Max(max, Math.Abs(data[i]));

            return max;
        }
    }
}
