// See https://aka.ms/new-console-template for more information

using System;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;

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
                        if (matrix != null)
                        {
                            matrix.SetProps();
                            ShowPropMatrix(matrix);
                        }
                        else
                        {
                            matrix = InputMatrix(matrix);
                            matrix.SetProps();
                            ShowPropMatrix(matrix);
                        }
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
                    Console.WriteLine("Ваша матрица:    \n" +
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

        static void ShowPropMatrix(Matrix matrix)
        {
            Console.Clear();
            Console.WriteLine("Ваша матрица:    \n" +
                              matrix.ToString());
            Matrix.ShowProp(matrix);
            Console.WriteLine("Нажмите на любую кнопку");
            Console.ReadKey();
            return;
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

    public class Matrix
    {
        public double[][] data;

        private Matrix(int n){}
        
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
            SetProps();
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
                    this[i, j] = (double)initData[i][j];
                }
            }
            
            SetProps();
        }

        public void SetProps()
        {
            IsSquared = true;
            IsUnity = true;
            IsDiagonal = true;
            IsEmpty = true;

            if (Rows == Cols)
            {
                Size = Rows;
                IsSquared = true;
                if (Rows == 0)
                {
                    IsUnity = false;
                    IsDiagonal = false;
                }
            }
            else
            {
                IsSquared = false;
                Size = 0;
            }

            if (Rows != Cols)
            {
                IsUnity = false;
            }
            
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (i != j && !data[i][j].Equals(0))
                    {
                        IsDiagonal = false;
                        IsUnity = false;
                    }
                    if (i == j && !data[i][j].Equals(1))
                    {
                        IsUnity = false;
                    }

                    if (!data[i][j].Equals(0))
                    {
                        IsEmpty = false;
                    }
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
        public int? Size { get; set; }
        public bool IsSquared { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsUnity { get; set;  }
        public bool IsDiagonal { get; set;  }


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

        public static explicit operator Matrix(double[][] arr)
        {
            return new Matrix(arr);
        }

        public Matrix Transpose()
        {
            Matrix res = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    res[j, i] = this[i, j];
                }
            }

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
            Matrix res = new Matrix(Size, Size);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i != j)
                    {
                        res[i, j] = 0;
                    }
                    else res[i, j] = 1;
                }
            }

            return res;
        }

        public static Matrix GetEmpty(int Size)
        {
            Matrix res = new Matrix(Size, Size);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    res[i, j] = 0;
                }
            }

            return res;
        }

        public static void ShowProp(Matrix matrix)
        {
            Console.WriteLine(  (matrix.IsSquared ? "Квадратная;" : "Не квадратная; ") + "\n" +
                                (matrix.IsDiagonal ? "Диагональная;" : "Не диагональная; ") + "\n" + 
                                (matrix.IsEmpty ? "Нулевая;" : "Ненулевая;") + "\n" +
                                (matrix.IsUnity ? "Единичная;" : "Не единичная; ") + "\n");
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
                if (lenCols == element.Length )
                {
                    for (int j = 0; j < lenCols; j++)
                    {
                        try
                        {
                            m[i, j] = double.Parse(element[j]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Неверный ввод, нужно вводить только числа");
                            return false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод, длина всех строк должна быть одинаковой");
                    return false;
                }
            }
            
            m.SetProps();

            return true;
        }
    }
}