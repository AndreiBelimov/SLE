using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    class IntergralSolver
    {
        public interface IIntegralSolver
        {
            double Solve(Func<double, double> f, double a, double b, int n);
        }

        public class RectangleSolver : IIntegralSolver
        {
            public double Solve(Func<double, double> f, double a, double b, int n)
            {
                var h = (b - a) / n;
                double result = 0;

                for (int i = 0; i < n; i++)
                {
                    result += f(a + i * h + h / 2) * h;
                }
                return result;
            }
        }
        public class LeftRectangleSolver : IIntegralSolver
        {
            public double Solve(Func<double, double> f, double a, double b, int n)
            {
                var h = (b - a) / n;
                double result = 0;

                for (int i = 0; i < n; i++)
                {
                    result += f(a + i * h) * h;
                }
                return result;
            }
        }
        public class RightRectangleSolver : IIntegralSolver
        {
            public double Solve(Func<double, double> f, double a, double b, int n)
            {
                var h = (b - a) / n;
                double result = 0;

                for (int i = 0; i < n; i++)
                {
                    result += f(a + (i + 1) * h) * h;
                }
                return result;
            }
        }
        public class TrapezoidSolver : IIntegralSolver
        {
            public double Solve(Func<double, double> f, double a, double b, int n)
            {
                var h = (b - a) / n;
                double result = 0;

                for (int i = 0; i < n; i++)
                {
                    result += (f(a + i * h) + f(a + (i + 1) * h)) / 2 * h;
                }
                return result;
            }
        }
    }
}
