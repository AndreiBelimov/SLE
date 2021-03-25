using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    static class DifferentialEquationSolver
    {
        public static Vector DifferenceMethod(Func<double, double> f, Func<double, double> g, int n, double alpha = 0, double beta = 1)
        {
            double h = (beta - alpha) / n;
            var u = new double[n]; //главная диагональ
            var a = new double[n - 1]; // верхняя диагональ
            var c = new double[n - 1]; // нижняя диагональ
            var y = new double[n]; //вектор значений

            for (int i = 0; i < n; i++)
            {
                u[i] = -2 - h * h * g(alpha + h * i);
                a[i] = 1;
                c[i] = 1;
                y[i] = f(alpha + h * i);
            }
            return SLESolver.TridiagonalMatrixAlgorithm(a, u, a, y);
        }
    }
}
