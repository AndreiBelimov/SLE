using Practice.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    class Program
    {
        static void Input()
        {
            Console.WriteLine("Введите количество неизвестных");
            int n = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите коэффиценты матрицы системы");
            double[,] matrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                string[] str = Console.ReadLine().Split();
                for (int j = 0; j < n; j++)
                    matrix[i, j] = Convert.ToDouble(str[j]);
            }

            Console.WriteLine("Введите вектор свободных коэффицентов");
            double[] value = new double[n];
            string[] temp = Console.ReadLine().Split();
            for (int i = 0; i < n; i++)
                value[i] = double.Parse(temp[i]);

            SLE system = new SLE(new Matrix(matrix), new Vector(value));
            Console.WriteLine();

            Console.WriteLine("Метод прогонки");
            system.TridiagonalMatrixAlgorithm();
        }

        static void Main(string[] args)
        {
            Input();
        }
    }


    
}
