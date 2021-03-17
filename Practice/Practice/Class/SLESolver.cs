using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    /// <summary>
    /// Предоставляет методы решения СЛАУ
    /// </summary>
    static class SLESolver
    {
        
        /// <summary>
        /// Метод Крамера
        /// </summary>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector Kramer(Matrix data, Vector values)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");

            if (data.CalculateDeterminant() == 0) throw new ArgumentException("Матрица является вырожденной");
            
            Vector x = new Vector(values.Count);

            for (int i = 0; i < values.Count; i++)
                x[i] = data.InsertColumn(values, i).CalculateDeterminant() / data.CalculateDeterminant();

            return x;
        }

        /// <summary>
        /// Методом обратной матрицы
        /// </summary>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector InvertibleMatrix(Matrix data, Vector values)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");
            return data.CreateInvertibleMatrix() * values;
        }

        /// <summary>
        /// Метод Гаусса
        /// </summary>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector Gauss(Matrix data, Vector values)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");

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
                }

                x[i] /= gaussMatrix[i, i];
            }

            return x;
        }

        /// <summary>
        /// Метод прогонки
        /// </summary>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector TridiagonalMatrixAlgorithm(Matrix data, Vector values)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");
            if (!data.IsTridiagonal) throw new ArgumentException("Матрица не трехдиагональная");

            var n = data.Rows;
            var a = new double[n - 1];  // нижняя диагональ
            var b = new double[n]; // средняя диагональ
            var c = new double[n - 1]; // верхняя диагональ
            var y = new double[n]; // вектор значений

            b[n - 1] = data[n - 1, n - 1];

            y[n - 1] = values[n - 1];

            for (int i = 0; i < n - 1; i++)
            {
                a[i] = data[i + 1, i];
                b[i] = data[i, i];
                c[i] = data[i, i + 1];
                y[i] = values[i];
            }

            return TridiagonalMatrixAlgorithm(a, b, c, y);
        }

        /// <summary>
        /// Метод прогонки
        /// </summary>
        /// <param name="a">Нижняя диагональ</param>
        /// <param name="b">Средняя диагональ</param>
        /// <param name="c">Верхняя диагональ</param>
        /// <param name="y">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector TridiagonalMatrixAlgorithm(double[] a, double[] b, double[] c, double[] y)
        {
            var n = y.Length;
            var alpha = new double[n];
            var beta = new double[n];

            alpha[0] = c[0] / b[0];
            beta[0] = y[0] / b[0];

            for (int i = 1; i < n - 1; i++)
            {
                alpha[i] = c[i] / (b[i] - a[i - 1] * alpha[i - 1]);
                beta[i] = (y[i] - a[i - 1] * beta[i - 1]) / (b[i] - a[i - 1] * alpha[i - 1]);
            }

            beta[n - 1] = (y[n - 1] - a[n - 2] * beta[n - 2]) / (b[n - 1] - a[n - 2] * alpha[n - 2]);

            Vector x = new Vector(n);
            x[n - 1] = beta[n - 1];
            for (int i = n - 1; i > 0; i--)
                x[i - 1] = beta[i - 1] - alpha[i - 1] * x[i];

            return x;
        }

        /// <summary>
        /// Метод квадратных корней
        /// </summary>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector CholeskyDecomposition(Matrix data, Vector values)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");
            if (!data.IsSymmetrical) throw new ArgumentException("Матрица не симметричная");

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

            return x;
        }

        /// <summary>
        /// Метод простых итераций
        /// </summary>
        /// <param name="eps">Погрешность</param>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector Itera(Matrix data, Vector values, double eps)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");

            Matrix initMatrix = data.Copy();
            Vector initValue = values.Copy();
            if (!initMatrix.CheckIterationAlgorithm(initValue)) 
                throw new ArgumentException("Метод не подходит для решения данной системы");

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
                throw new ArgumentException("Метод не подходит для решения данной системы");

            Vector beta = x0.Copy();
            do
            {
                Vector temp = x.Copy();
                x = alpha * x0 + beta;
                x0 = temp.Copy();
                count++;
            } while ((x0 - x).Norm >= eps);

            return x;
        }

        /// <summary>
        /// Метод Зейделя
        /// </summary>
        /// <param name="eps">Погрешность</param>
        /// <param name="data">Матрица значений</param>
        /// <param name="values">Вектор значений</param>
        /// <returns>Вектор решений</returns>
        public static Vector Seidel(Matrix data, Vector values, double eps)
        {
            if (!data.IsSquare) throw new ArgumentException("Матрица не квадратная");

            Matrix initMatrix = data.Copy();
            Vector initValue = values.Copy();
            if (!initMatrix.CheckIterationAlgorithm(initValue))
                throw new ArgumentException("Метод не подходит для решения данной системы");

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
                throw new ArgumentException("Метод не подходит для решения данной системы");

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
                throw new ArgumentException("Метод не подходит для решения данной системы");

            Vector beta = x0.Copy();
            do
            {
                Vector temp = x.Copy();
                x = ((E - L).CreateInvertibleMatrix() * U) * x + ((E - L).CreateInvertibleMatrix()) * beta;
                x0 = temp.Copy();
                count++;
            } while ((x0 - x).Norm >= eps);

            return x;
        }
    }
}

