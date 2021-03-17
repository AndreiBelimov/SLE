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
            var matrix = new double[,]{{5, 3, 0, 0 },
                                       {3, 6, 1, 0 },
                                       {0, 1, 4, -2 },
                                       {0, 0, 1, -3 } };

            var values = new double[] { 8, 10, 3, -2 };

            Console.WriteLine(SLESolver.TridiagonalMatrixAlgorithm(new Matrix(matrix), new Vector(values)));
        }

        static void Main(string[] args)
        {
            Input();
        }
    } 
}
