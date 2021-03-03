using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Class
{
    /// <summary>
    /// Матрица значений double
    /// </summary>
    public class Matrix
    {
        private readonly double[,] data;
        
        public int Rows => data.GetLength(0);
        public int Columns => data.GetLength(1);
        public double[,] Data => data;
        public double Determinant => CalculateDeterminant();
        public bool IsSquare  => Rows == Columns;
        public bool IsTridiagonal => CheckTridiagonalMatrix();
        public bool IsSymmetrical => CheckSymmetryMatrix();
        public double ColumnNorm => CalculateNormColum();
        public double RowNorm => CalculateNormRow();

        public Matrix(int rows, int columns)
        {
            data = new double[rows, columns];
        }

        public Matrix(double[,] initArray)
        {
            data = new double[initArray.GetLength(0), initArray.GetLength(1)];
            for (int i = 0; i < initArray.GetLength(0); i++)
                for (int j = 0; j < initArray.GetLength(1); j++)
                    data[i, j] = initArray[i, j];
        }

        public void SwapRows(int x, int y)
        {
            for (int i = 0; i < Columns; i++)
            {
                double temp = this[x, i];
                this[x, i] = this[y, i];
                this[y, i] = temp;
            }
        }
       

        /// <summary>
        /// Проходит циклом по всем элементам
        /// </summary>
        /// <param name="func">Функция для изменения элементов</param>
        public void GoThroughElements(Action<int, int> func)
        {
            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    func(i, j);
        }

        /// <summary>
        /// Создает единичную матрицу
        /// </summary>
        /// <param name="size">Размерность матрицы</param>
        /// <returns></returns>
        public static Matrix CreateIdentityMatrix(int size)
        {
            var result = new Matrix(size, size);
            for (var i = 0; i < size; i++)
                result[i, i] = 1;

            return result;
        }

        /// <summary>
        /// Возвращает транспонированную матрицу
        /// </summary>
        /// <returns></returns>
        public Matrix GetTransposedMatrix()
        {
            var result = new Matrix(Columns, Rows);
            result.GoThroughElements((i, j) => result[i, j] = this[j, i]);
            return result;
        }

        /// <summary>
        /// Возвращает определитель
        /// </summary>
        /// <returns></returns>
        public double CalculateDeterminant()
        {
            if (!IsSquare)
                throw new InvalidOperationException("Определитель существует только у квадратных матриц");

            if (Columns == 1)
                return this[0, 0];
            if (Columns == 2)
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

            double result = 0;
            for (var j = 0; j < Columns; j++)
                result += (j % 2 == 1 ? 1 : -1) * this[1, j] *
                    CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(1).CalculateDeterminant();

            return result;
        }

        /// <summary>
        /// Возвращает обратную матрицу
        /// </summary>
        /// <returns></returns>
        public Matrix CreateInvertibleMatrix()
        {
            if (Rows != Columns)
                return null;
            double determinant = CalculateDeterminant();
            if (Math.Abs(determinant) < Constants.DoubleComparisonDelta)
                return null;

            Matrix result = new Matrix(Rows, Rows);
            GoThroughElements((i, j) =>
            {
                result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) * CalculateMinor(i, j) / determinant;
            });

            return result.GetTransposedMatrix();
        }

        /// <summary>
        /// Возвращает минор
        /// </summary>
        /// <param name="i">Номер строки</param>
        /// <param name="j">Номер столбца</param>
        /// <returns></returns>
        private double CalculateMinor(int i, int j)
        {
            return CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(i).CalculateDeterminant();
        }

        /// <summary>
        /// Возвращает матрицу без строки
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <returns></returns>
        private Matrix CreateMatrixWithoutRow(int row)
        {
            if (row < 0 || row >= Rows)
                throw new ArgumentException("Строки с данным номером не существует");

            var result = new Matrix(Rows - 1, Columns);
            result.GoThroughElements((i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
            return result;
        }

        /// <summary>
        /// Возвращает матрицу без столбца
        /// </summary> 
        /// <param name="column">Номер столбца</param>
        /// <returns></returns>
        private Matrix CreateMatrixWithoutColumn(int column)
        {
            if (column < 0 || column >= Columns)
                throw new ArgumentException("Столбец не существует");
            var result = new Matrix(Rows, Columns - 1);
            result.GoThroughElements((i, j) => result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
            return result;
        }

        /// <summary>
        /// Выводит матрицу на консоль
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    Console.Write("{0} ", data[i, j]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Заменяет выбранный столбец матрицы
        /// </summary>
        /// <param name="vector">Вектор для замены</param>
        /// <param name="index">Номер столбца</param>
        /// <returns></returns>
        public Matrix InsertColumn(Vector vector, int index)
        {
            if(Columns != vector.Count)
                throw new ArgumentException("Расхождение с размерами");

            Matrix rez = new Matrix(data);
            for (int i = 0; i < rez.Rows; i++)
                rez[i, index] = vector[i];
            return rez;
        }

        /// <summary>
        /// Возвращяет копию матрицы
        /// </summary>
        /// <returns></returns>
        public Matrix Copy()
        {
            Matrix temp = new Matrix(Rows, Columns);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    temp[i, j] = this[i, j];

            return temp;
        }

        /// <summary>
        /// Проверка на трехдиагональную матрицу
        /// </summary>
        /// <returns></returns>
        private bool CheckTridiagonalMatrix()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i == j || i == j - 1 || i == j + 1)
                        continue;

                    if (this[i, j] != 0)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Основное условие сходимости итерационного метода
        /// </summary>
        /// <returns></returns>
        private bool BasicConditionIterationAlgorithm()
        {
            double sum = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    if (i != j)
                        sum += Math.Abs(this[i, j]);
                if (Math.Abs(this[i, i]) < sum)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка основного условия итерационного метода
        /// </summary>
        /// <param name="vector">Проверяемый вектор свободных членов системы</param>
        /// <returns></returns>
        public bool CheckIterationAlgorithm(Vector vector)
        {
            int last = 1;
            do
            {
                last = Permutations.NextPermutation(this, vector, last);
                if (BasicConditionIterationAlgorithm())
                    return true;
            } while (last != 1);

            return false;
        }

        /// <summary>
        /// Проверка на симметричность матрицы
        /// </summary>
        /// <returns></returns>
        private bool CheckSymmetryMatrix()
        {
            if (!IsSquare)
                return false;
            for (int i = 0; i < Rows; i++)
                for (int j = i; j < Columns; j++)
                    if (this[i, j] != this[j, i])
                        return false;

            return true;
        }

        /// <summary>
        /// Подсчет нормы по строкам
        /// </summary>
        /// <returns></returns>
        private double CalculateNormColum()
        {
            double max = double.MinValue;
            for (int i = 0; i < Rows; i++)
            {
                double temp = 0;
                for (int j = 0; j < Columns; j++)
                    temp += data[i, j];
                max = Math.Max(max, temp);
            }
            return max;
        }

        /// <summary>
        /// Подсчет нормы по столбцам
        /// </summary>
        /// <returns></returns>
        private double CalculateNormRow()
        {
            double max = double.MinValue;
            for (int i = 0; i < Rows; i++)
            {
                double temp = 0;
                for (int j = 0; j < Columns; j++)
                    temp += data[j, i];
                max = Math.Max(max, temp);
            }
            return max;
        }

        /// <summary>
        /// Сортирует строки матрицы(?)
        /// </summary>
        /// <param name="value">Вектор</param>
        /// <param name="index">Индекс</param>
        public void SortRows(Vector value, int index)
        {
            double maxElement = data[index, index];
            int maxElementIndex = index;
            for (int i = index + 1; i < Columns; i++)
            {
                if (data[i, index] > maxElement)
                {
                    maxElement = data[i, index];
                    maxElementIndex = i;
                }
            }

            if (maxElementIndex > index)
            {
                double temp;

                temp = value[maxElementIndex];
                value[maxElementIndex] = value[index];
                value[index] = temp;

                for (int i = 0; i < Rows; i++)
                {
                    temp = data[maxElementIndex, i];
                    data[maxElementIndex, i] = data[index, i];
                    data[index, i] = temp;
                }
            }
        }

        public double this[int x, int y]
        {
            get
            {
                return data[x, y];
            }
            set
            {
                data[x, y] = value;
            }
        }

        public static Matrix operator +(Matrix matrixOne, Matrix matrixTwo)
        {
            if (matrixOne.Rows != matrixTwo.Rows || matrixOne.Columns != matrixTwo.Columns)
                throw new ArgumentException("Эти матрицы не могут быть сложены");

            var result = new Matrix(matrixOne.Rows, matrixOne.Columns);
            result.GoThroughElements((i, j) => result[i, j] = matrixOne[i, j] + matrixTwo[i, j]);
            return result;
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            return matrix1 + (matrix2 * -1);
        }

        public static Matrix operator *(Matrix matrix, double number)
        {
            var result = new Matrix(matrix.Rows, matrix.Columns);
            result.GoThroughElements((i, j) => result[i, j] = matrix[i, j] * number);
            return result;
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (matrix.Columns != vector.Count)
                throw new ArgumentException("Эти матрицы не могут быть умножены");

            var result = new Matrix(matrix.Rows, 1);
            result.GoThroughElements((i, j) =>
            {
                for (var k = 0; k < matrix.Columns; k++)
                {
                    result[i, j] += matrix[i, k] * vector[k];
                }
            });
            return new Vector(result);
        }

        public static Matrix operator *(Matrix matrixOne, Matrix matrixTwo)
        {
            if (matrixOne.Columns != matrixTwo.Rows)
                throw new ArgumentException("Эти матрицы не могут быть умножены");

            var result = new Matrix(matrixOne.Rows, matrixTwo.Columns);
            result.GoThroughElements((i, j) =>
            {
                for (var k = 0; k < matrixOne.Columns; k++)
                {
                    result[i, j] += matrixOne[i, k] * matrixTwo[k, j];
                }
            });
            return result;
        }

    }
}
