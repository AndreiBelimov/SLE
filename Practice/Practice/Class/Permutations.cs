using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    public class Permutations
    {
        private static bool LEFT_TO_RIGHT = true;
        private static bool RIGHT_TO_LEFT = false;

        /// <summary>
        /// Служебная функция для нахождения положения наибольшего мобильного целого числа в a[]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="n"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private static int SearchArr(int[] a, int n, int mobile)
        {
            for (int i = 0; i < n; i++)
                if (a[i] == mobile)
                    return i + 1;

            return 0;
        }

        /// <summary>
        /// Поиск наибольшего подвижного числа
        /// </summary>
        /// <param name="a"></param>
        /// <param name="dir"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int GetMobile(int[] a, bool[] dir, int n)
        {
            int mobile_prev = 0, mobile = 0;

            for (int i = 0; i < n; i++)
            {
                if (dir[a[i] - 1] == RIGHT_TO_LEFT && i != 0)
                    if (a[i] > a[i - 1] && a[i] > mobile_prev)
                    {
                        mobile = a[i];
                        mobile_prev = mobile;
                    }

                if (dir[a[i] - 1] == LEFT_TO_RIGHT && i != n - 1)
                    if (a[i] > a[i + 1] && a[i] > mobile_prev)
                    {
                        mobile = a[i];
                        mobile_prev = mobile;
                    }
            }

            if (mobile == 0 && mobile_prev == 0)
                return 0;
            else
                return mobile;
        }

        /// <summary>
        /// Вывод одной перестановки
        /// </summary>
        /// <param name="a"></param>
        /// <param name="dir"></param>
        /// <param name="n"></param>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        public static void PrintOnePerm(int[] a, bool[] dir, int n, Matrix matrix, Vector vector)
        {
            int mobile = GetMobile(a, dir, n);
            int pos = SearchArr(a, n, mobile);

            if (dir[a[pos - 1] - 1] == RIGHT_TO_LEFT)
            {
                int temp = a[pos - 1];
                a[pos - 1] = a[pos - 2];
                a[pos - 2] = temp;
                matrix.SwapRows(pos - 1, pos - 2);
                vector.Swap(pos - 1, pos - 2);
            }
            else if (dir[a[pos - 1] - 1] == LEFT_TO_RIGHT)
            {
                int temp = a[pos];
                a[pos] = a[pos - 1];
                a[pos - 1] = temp;
                matrix.SwapRows(pos, pos - 1);
                vector.Swap(pos, pos - 1);
            }

            for (int i = 0; i < n; i++)
            {
                if (a[i] > mobile)
                {
                    if (dir[a[i] - 1] == LEFT_TO_RIGHT)
                        dir[a[i] - 1] = RIGHT_TO_LEFT;

                    else if (dir[a[i] - 1] == RIGHT_TO_LEFT)
                        dir[a[i] - 1] = LEFT_TO_RIGHT;
                }
            }
        }

        /// <summary>
        /// Вывод следующей перестановки
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static int NextPermutation(Matrix matrix, Vector vector, int last)
        {
            int n = vector.Count;
            if (last == FactFactor(n))
            {
                return 1;
            }

            int[] a = new int[n];
            bool[] dir = new bool[n];

            for (int i = 0; i < n; i++)
                a[i] = i + 1;

            for (int i = 0; i < n; i++)
                dir[i] = RIGHT_TO_LEFT;

            for (int i = last; i < FactFactor(n); i++)
                PrintOnePerm(a, dir, n, matrix, vector);
            last++;
            return last;
        }

        /// <summary>
        /// Факториал числа
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int FactFactor(int n)
        {
            if (n < 0)
                return 0;
            if (n == 0)
                return 1;
            if (n == 1 || n == 2)
                return n;
            bool[] u = new bool[n + 1];
            List<Tuple<int, int>> p = new List<Tuple<int, int>>();
            for (int i = 2; i <= n; ++i)
                if (!u[i]) 
                {
                    int k = n / i;
                    int c = 0;
                    while (k > 0)
                    {
                        c += k;
                        k /= i;
                    }

                    p.Add(new Tuple<int, int>(i, c));
          
                    int j = 2;
                    while (i * j <= n)
                    {
                        u[i * j] = true;
                        ++j;
                    }
                }

            int r = 1;
            for (int i = p.Count() - 1; i >= 0; --i)
                r *= (int)Math.Pow(p[i].Item1, p[i].Item2);
            return r;
        }
    }
}
