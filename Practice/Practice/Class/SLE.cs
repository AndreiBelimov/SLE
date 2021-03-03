using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Practice.Class
{
    /// <summary>
    /// СЛАУ
    /// </summary>
    public class SLE
    {
        private readonly Matrix data;        //Матрица коэффициентов системы
        private readonly Vector values;       //Вектор свободных членов
        public Matrix Data => data.Copy();
        public Vector Values => values.Copy();

        public SLE(Matrix data, Vector values)
        {
            this.data = data.Copy();
            this.values = values.Copy();
        }

        /// <summary>
        /// Вывод системы на консоль
        /// </summary>
        public void PrintSystem()
        {
            for (int i = 0; i < data.Rows; i++)
            {
                for (int j = 0; j < data.Columns; j++)
                {
                    Console.Write("{0, 3}x{1} ", j == 0 ? data[i, j] : Math.Abs(data[i, j]), (j + 1));
                    if (j != data.Columns - 1)
                        Console.Write("{0, 3} ", data[i, j + 1] >= 0 ? "+" : "-");
                }
                Console.WriteLine("= {0, 3}", values[i]);
            }
        }

        /// <summary>
        /// Метод Крамера
        /// </summary>
        public void Kramer()
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }

            if (data.CalculateDeterminant() == 0) {
                Console.WriteLine("Матрица является вырожденной, метод Крамера не подходит для решения этого СЛАУ" + '\n');
                return;
            }

            Vector x = new Vector(values.Count);

            for (int i = 0; i < values.Count; i++)
                x[i] = data.InsertColumn(values, i).CalculateDeterminant() / data.CalculateDeterminant();

            x.Print();
        }

        /// <summary>
        /// Методом обратной матрицы
        /// </summary>
        public void InvertibleMatrix()
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }
            (data.CreateInvertibleMatrix() * values).Print();
        }

        /// <summary>
        /// Метод Гаусса
        /// </summary>
        public void Gauss()
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }

            Matrix gaussMatrix = data.Copy();
            Vector gaussVector = values.Copy();

            for (int i = 0; i < gaussMatrix.Columns - 1; i++)
            {
                gaussMatrix.SortRows(gaussVector, i);
                for (int j = i + 1; j < gaussMatrix.Columns; j++)
                {
                    if (gaussMatrix[i, i] != 0)
                    {
                        double multElement = gaussMatrix[j, i] / gaussMatrix[i, i];
                        for (int k = i; k < gaussMatrix.Columns; k++)
                            gaussMatrix[j, k] -= gaussMatrix[i, k] * multElement;
                        gaussVector[j] -= gaussVector[i] * multElement;
                    }
                }
            }

            Vector x = new Vector(gaussMatrix.Rows);

            for (int i = (gaussMatrix.Columns - 1); i >= 0; i--)
            {
                x[i] = gaussVector[i];

                for (int j = (gaussMatrix.Columns - 1); j > i; j--)
                    x[i] -= gaussMatrix[i, j] * x[j];

                if (gaussMatrix[i, i] == 0 && gaussVector[i] != 0)
                {
                    Console.WriteLine("Решений нет");
                    return;
                }

                x[i] /= gaussMatrix[i, i];
            }

            x.Print();
        }

        /// <summary>
        /// Метод прогонки
        /// </summary>
        public void TridiagonalMatrixAlgorithm()
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }
            if (!data.IsTridiagonal) { Console.WriteLine("Матрица не трехдиагональная" + '\n'); return; }

            var a = new double[data.Rows - 1];  // нижняя диагональ
            var b = new double[data.Rows]; // средняя диагональ
            var c = new double[data.Rows - 1]; // верхняя диагональ
            var y = new double[data.Rows]; // вектор значений

            b[data.Rows - 1] = data[data.Rows - 1, data.Rows - 1];
            y[data.Rows - 1] = values[data.Rows - 1];

            for (int i = 0; i < data.Rows - 1; i++)
            {
                a[i] = data[i + 1, i];
                b[i] = data[i, i];
                c[i] = data[i, i + 1];
                y[i] = values[i];
            }
            
            var alpha = new double[data.Rows];
            var beta = new double[data.Rows];

            alpha[0] = c[0] / b[0];
            beta[0] = y[0] / b[0];

            for (int i = 1; i < data.Rows - 1; i++)
            {
                alpha[i] = c[i] / (b[i] - a[i - 1] * alpha[i - 1]);
                beta[i] = (y[i] - a[i - 1] * beta[i - 1]) / (b[i] - a[i - 1] * alpha[i - 1]);
            }
            
            Vector x = new Vector(data.Rows);
            x[data.Rows - 1] = beta[data.Rows - 1];
            for (int i = data.Rows - 1; i > 0; i--)
                x[i - 1] = beta[i - 1] - alpha[i - 1] * x[i];

            x.Print();
        }

        /// <summary>
        /// Метод квадратных корней
        /// </summary>
        public void CholeskyDecomposition()
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }
            if (!data.IsSymmetrical) { Console.WriteLine("Матрица не симметричная" + '\n'); return; }

            Matrix u = new Matrix(data.Rows, data.Columns);
            Vector x = new Vector(data.Rows);
            Vector y = new Vector(data.Rows);

            u[0, 0] = Math.Sqrt(data[0, 0]);
            for (int i = 1; i < data.Rows; i++)
                u[0, i] = data[0, i] / u[0, 0];

            for (int i = 1; i < data.Rows; i++)
            {
                for (int j = 0; j < data.Columns; j++)
                {
                    if (i == j)
                    {
                        double sum = 0;
                        for (int k = 0; k < data.Rows; k++)
                            sum += u[k, i] * u[k, i];

                        u[i, j] = Math.Sqrt(data[i, j] - sum);
                    }
                    else if (i < j)
                    {
                        double sum = 0;
                        for (int k = 0; k < i; k++)
                            sum += u[k, i] * u[k, j];
                        u[i, j] = (data[i, j] - sum) / u[i, i];
                    }
                }
            }

            Matrix uT = u.GetTransposedMatrix();
            double temp;

            for (int i = 0; i < data.Rows; i++)
            {
                temp = 0;
                for (int j = 0; j < i; j++)
                    temp += uT[i, j] * y[j];

                y[i] = (values[i] - temp) / uT[i, i];
            }

            for (int i = data.Rows - 1; i >= 0; i--)
            {
                temp = 0;
                for (int j = data.Rows - 1; j > i; j--)
                    temp += u[i, j] * x[j];

                x[i] = (y[i] - temp) / u[i, i];
            }

            x.Print();
        }

        /// <summary>
        /// Метод простых итераций
        /// </summary>
        /// <param name="eps">Эпсилон</param>
        public void Itera(double eps)
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }

            Matrix initMatrix = data.Copy();
            Vector initValue = values.Copy();
            if (!initMatrix.CheckIterationAlgorithm(initValue))
                { Console.WriteLine("Метод неподходит для решение данной системы" + '\n'); return; }

            Matrix alpha = initMatrix.Copy();
            Vector x = new Vector(initMatrix.Rows);
            Vector x0 = initValue.Copy();
            int count = 0;

            for (int i = 0; i < initMatrix.Rows; i++)
            {
                for (int j = 0; j < initMatrix.Columns; j++)
                    if (i != j)
                        alpha[i, j] /= -alpha[i, i];
                x0[i] /= alpha[i, i];
                alpha[i, i] = 0;
            }

            if (alpha.ColumnNorm >= 1 || alpha.RowNorm >= 1)
                { Console.WriteLine("Метод неподходит для решение данной системы" + '\n'); return; }

            Vector beta = x0.Copy();
            do
            {
                Vector temp = x.Copy();
                x = alpha * x0 + beta;
                x0 = temp.Copy();
                count++;
            } while ((x0 - x).Norm >= eps);

            Console.WriteLine("Кол-во итераций: {0}", count);
            x.Print();
        }

        /// <summary>
        /// Метод Зейделя
        /// </summary>
        /// <param name="eps">Эпсилон</param>
        public void Seidel(double eps)
        {
            if (!data.IsSquare) { Console.WriteLine("Матрица не квадратная" + '\n'); return; }

            Matrix initMatrix = data.Copy();
            Vector initValue = values.Copy();
            if (!initMatrix.CheckIterationAlgorithm(initValue))
                { Console.WriteLine("Метод неподходит для решение данной системы" + '\n'); ; return; }

            Matrix L = new Matrix(initMatrix.Rows, initMatrix.Columns);
            Matrix U = new Matrix(data.Rows, data.Columns);
            Matrix E = Matrix.CreateIdentityMatrix(data.Rows);


            Matrix alpha = initMatrix.Copy();
            Vector x = new Vector(initMatrix.Rows);
            Vector x0 = initValue.Copy();
            int count = 0;

            for (int i = 0; i < initMatrix.Rows; i++)
            {
                for (int j = 0; j < initMatrix.Columns; j++)
                    if (i != j)
                        alpha[i, j] /= -alpha[i, i];
                x0[i] /= alpha[i, i];
                alpha[i, i] = 0;
            }

            if (alpha.ColumnNorm >= 1 || alpha.RowNorm >= 1)
                { Console.WriteLine("Метод неподходит для решение данной системы" + '\n'); return; }

            for (int i = 0; i < initMatrix.Rows; i++)
            {
                for (int j = 0; j < initMatrix.Columns; j++)
                {
                    if (i <= j)
                        U[i, j] = alpha[i, j];
                    else
                        L[i, j] = alpha[i, j];
                }
            }

            if (((E - L).CreateInvertibleMatrix() * U).ColumnNorm >= 1 ||
                    ((E - L).CreateInvertibleMatrix() * U).RowNorm >= 1)
                        { Console.WriteLine("Метод неподходит для решение данной системы" + '\n'); return; }

            Vector beta = x0.Copy();
            do
            {
                Vector temp = x.Copy();
                x = ((E - L).CreateInvertibleMatrix() * U) * x + ((E - L).CreateInvertibleMatrix()) * beta;
                x0 = temp.Copy();
                count++;
            } while ((x0 - x).Norm >= eps);

            Console.WriteLine("Кол-во итераций: {0}", count);
            x.Print();
        }
    }
}
