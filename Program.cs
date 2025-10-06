using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearIndependence
{
    /// <summary>
    /// Інтерфейс для будь-якої системи векторів.
    /// </summary>
    public interface ILinearSystem
    {
        /// <summary>
        /// Перевіряє, чи система векторів є лінійно незалежною.
        /// </summary>
        /// <returns>true – якщо незалежні, false – якщо залежні.</returns>
        bool IsLinearlyIndependent();
    }

    /// <summary>
    /// Абстрактний клас, що реалізує базову логіку для систем векторів.
    /// </summary>
    abstract class VectorSystem : ILinearSystem
    {
        /// <summary>
        /// Колекція векторів у системі.
        /// </summary>
        protected List<double[]> Vectors { get; }

        /// <summary>
        /// Допустима похибка для порівнянь.
        /// </summary>
        protected const double Epsilon = 1e-9;

        protected VectorSystem(IEnumerable<IEnumerable<double>> vectors)
        {
            if (vectors == null || !vectors.Any())
                throw new ArgumentException("Система векторів не може бути порожньою.");

            // Перетворення у список і перевірка узгодженості розмірностей
            Vectors = vectors.Select(v =>
            {
                if (v == null)
                    throw new ArgumentException("Вектор не може бути null.");
                var arr = v.ToArray();
                if (arr.Length == 0)
                    throw new ArgumentException("Вектор не може бути порожнім.");
                return arr;
            }).ToList();

            int dim = Vectors[0].Length;
            if (Vectors.Any(v => v.Length != dim))
                throw new ArgumentException("Усі вектори повинні бути однакової розмірності.");
        }

        /// <summary>
        /// Перевіряє, чи система містить нульовий вектор.
        /// </summary>
        protected bool ContainsZeroVector() =>
            Vectors.Any(v => v.All(x => Math.Abs(x) < Epsilon));

        /// <inheritdoc />
        public abstract bool IsLinearlyIndependent();

        /// <summary>
        /// Обчислює визначник квадратної матриці будь-якої розмірності за допомогою рекурсії.
        /// </summary>
        protected static double CalculateDeterminant(double[,] matrix)
        {
            int n = matrix.GetLength(0);

            if (n == 1)
                return matrix[0, 0];
            if (n == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            double det = 0;
            for (int i = 0; i < n; i++)
            {
                double[,] minor = GetMinor(matrix, 0, i);
                det += (i % 2 == 0 ? 1 : -1) * matrix[0, i] * CalculateDeterminant(minor);
            }
            return det;
        }

        /// <summary>
        /// Отримує мінор матриці (видаляє заданий рядок і стовпець).
        /// </summary>
        private static double[,] GetMinor(double[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            var minor = new double[n - 1, n - 1];
            int r = 0, c;
            for (int i = 0; i < n; i++)
            {
                if (i == row) continue;
                c = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == col) continue;
                    minor[r, c++] = matrix[i, j];
                }
                r++;
            }
            return minor;
        }
    }

    /// <summary>
    /// Універсальний клас для роботи з n-вимірними системами векторів.
    /// </summary>
    class GeneralVectorSystem : VectorSystem
    {
        public GeneralVectorSystem(IEnumerable<IEnumerable<double>> vectors) : base(vectors)
        {
            int dim = Vectors[0].Length;
            if (Vectors.Count != dim)
                throw new ArgumentException($"Для перевірки незалежності потрібно {dim} векторів у {dim}-вимірному просторі.");
        }

        /// <inheritdoc />
        public override bool IsLinearlyIndependent()
        {
            if (ContainsZeroVector()) return false;

            int n = Vectors[0].Length;
            var matrix = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[j, i] = Vectors[i][j];

            return Math.Abs(CalculateDeterminant(matrix)) > Epsilon;
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                // ✅ Приклад для 2D
                var system2D = new GeneralVectorSystem(new[]
                {
                    new double[] { 1, 2 },
                    new double[] { 3, 4 }
                });

                // ✅ Приклад для 3D
                var system3D = new GeneralVectorSystem(new[]
                {
                    new double[] { 1, 2, 3 },
                    new double[] { 4, 5, 6 },
                    new double[] { 7, 8, 10 }
                });

                PrintResult("2D", system2D.IsLinearlyIndependent());
                PrintResult("3D", system3D.IsLinearlyIndependent());
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Виводить результат перевірки лінійної незалежності.
        /// </summary>
        private static void PrintResult(string dimension, bool isIndependent)
        {
            Console.ForegroundColor = isIndependent ? ConsoleColor.Green : ConsoleColor.Yellow;
            Console.WriteLine($"{dimension} вектори є {(isIndependent ? "лінійно незалежними" : "лінійно залежними")}.");
            Console.ResetColor();
        }
    }
}
