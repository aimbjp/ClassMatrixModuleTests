// See https://aka.ms/new-console-template for more information

using System;
using System.Numerics;

namespace ClassMatrixModuleTests
{
    internal class Programm
    {
        static void Main()
        {
            var flag = true;
            Matrix matrix = null;
            while (flag)
            {
                ShowMenu(matrix);
                switch (char.ToLower(Console.ReadKey(true).KeyChar))
                {
                    case '1':
                        matrix = InputMatrix(matrix);
                        break;
                    case '2':
                        break;
                    case '3':
                        if (matrix != null)
                        {
                            matrix = MultiplyMatrixNumber(matrix);
                        }
                        else
                        {
                            matrix = InputMatrix(matrix);
                            matrix = MultiplyMatrixNumber(matrix);
                        }

                        break;
                    case '4':
                        if (matrix != null)
                        {
                            Console.WriteLine(matrix.ToString());
                        }
                        else
                        {
                            matrix = InputMatrix(matrix);
                            Console.WriteLine(matrix.ToString());
                        }

                        Console.ReadKey();
                        break;
                    case '0':
                        Environment.Exit(0);
                        break;
                    default:
                        Main();
                        break;
                }
            }

            Matrix MultiplyMatrixNumber(Matrix m)
            {
                ShowMenu();

                var n = double.Parse(Console.ReadLine());

                m = m * n;

                void ShowMenu()
                {
                    Console.Clear();
                    Console.WriteLine("Ваша матрица: " +
                                      m.ToString() +
                                      "Введите число, на которое ее нужно домножить: \n");
                }

                return m;
            }

            void ShowMenu(Matrix matrix)
            {
                Console.Clear();
                Console.WriteLine("1 — Ввести матрицу\n" +
                                  "2 — Свойства матрицы\n" +
                                  "3 — Операция с матрицей (умножение на число) \n" +
                                  "0 — Выход\n" +
                                  "Ваша матрица сейчас:\n" +
                                  (matrix == null ? "Не введена" : matrix.ToString()));
                
            }
        }

        static Matrix InputMatrix(Matrix matrix)
        {
            var flag = true;
            Matrix res = matrix;
            ShowMenu();
            while (flag)
            {
                var input = Console.ReadLine();
                if (input == "00" || input == "exit" || input == "e")
                {
                    return matrix;
                }
                else
                {
                    if (Matrix.TryParse(input, out var m))
                    {
                        Console.WriteLine("Введенная матрица:");
                        Console.WriteLine(m.ToString());
                        Console.WriteLine("Нажмите любую кнопку, чтобы вернуться в меню");
                        Console.ReadKey();
                        return m;
                    }
                }
            }

            void ShowMenu()
            {
                Console.Clear();
                Console.WriteLine(
                    "Введите матрицу, элементы отделяйте пробелами, ряды — точкой с запятой (например: 1 ; 1)\n" +
                    "00 — для выхода в меню и сохранения матрицы (также exit, e)");
            }

            return res;
        }
    }

    class Matrix
    {
        public double[][] data;

        public Matrix(int nRows, int nCols)
        {
            if (nRows < 1 || nCols < 1)
            {
                throw new Exception(
                    "Невозможно создать матрицу кол-во строк и столбцов должно быть больше единицы или равно ей.");
            }

            Rows = nRows;
            Cols = nCols;
            data = new double[nRows][];
            for (int i = 0; i < nRows; i++)
            {
                data[i] = new double[nCols];
                for (int j = 0; j < nCols; j++)
                {
                    this[i, j] = (double)j;
                }
            }
        }

        public Matrix(double[][] initData)
        {
            Rows = initData.Length;
            Cols = initData[0].Length;
            data = new double[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                data[i] = new double[Cols];
                for (int j = 0; j < Cols; j++)
                {
                    this[i, j] = (double)j;
                }
            }
        }

        public double this[int i, int j]
        {
            get => data[i][j];
            set { data[i][j] = (double)value; }
        }

        public int Rows { get; }
        public int Cols { get; }
        
        //нужно добавить определения (тру или фолс для матриц, добавить в конструктор)
        public int? Size { get; }
        public bool IsSquared { get; }
        public bool IsEmpty { get; }
        public bool IsUnity { get; }
        public bool IsDiagonal { get; }


        public static Matrix operator *(Matrix m1, double d)
        {
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Cols; j++)
                {
                    m1[i, j] = d * m1[i, j];
                }
            }

            return m1;
        }
        // нужно доделать методы некоторые для класса матриц
        public static explicit operator Matrix(double[][] arr)
        {
            Matrix res = new Matrix(1, 1);
            return res;
        }

        public Matrix Transpose()
        {
            Matrix res = new Matrix(1, 1);
            return res;
        }

        public override string ToString()
        {
            if (this == null) return "Не введена";
            string res = "";
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    res += (this[i, j]) + " ";
                }

                res += "\n";
            }

            return res;
        }

        public static Matrix GetUnity(int Size)
        {
            Matrix res = new Matrix(1, 1);
            return res;
        }

        public static Matrix GetEmpty(int Size)
        {
            Matrix res = new Matrix(1, 1);
            return res;
        }

        public static bool TryParse(string s, out Matrix m)
        {
            if (s == null)
            {
                m = new Matrix(1, 1);
                return false;
            }

            var subsRows = s.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var lenCols = subsRows[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            m = new Matrix(subsRows.Length, lenCols);
            for (int i = 0; i < subsRows.Length; i++)
            {
                var element = subsRows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (lenCols == element.Length)
                {
                    for (int j = 0; j < lenCols; j++)
                    {
                        m[i, j] = double.Parse(element[j]);
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод");
                    return false;
                }
            }

            return true;
        }
    }
}