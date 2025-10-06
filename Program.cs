using System;


class SystemOf2Vectors
{
<<<<<<< revert-1-main
    
    protected double[] A = new double[3];
    protected double[] B = new double[3];

    
    public void SetVectors(double a1, double a2, double b1, double b2)
    {
        A[0] = a1; A[1] = a2; A[2] = 0;
        B[0] = b1; B[1] = b2; B[2] = 0;
    }

    
    public void Print2DVectors()
    {
        Console.WriteLine($"A = ({A[0]}, {A[1]})");
        Console.WriteLine($"B = ({B[0]}, {B[1]})");
    }

    
    public bool IsLinearlyIndependent2D()
    {
        double det = A[0] * B[1] - A[1] * B[0];
        return Math.Abs(det) > 1e-9;
    }
}


class SystemOf3Vectors : SystemOf2Vectors
{
    private double[] C = new double[3];

    
    public void SetVectors(double a1, double a2, double a3,
                           double b1, double b2, double b3,
                           double c1, double c2, double c3)
    {
        A = new double[] { a1, a2, a3 };
        B = new double[] { b1, b2, b3 };
        C = new double[] { c1, c2, c3 };
    }

    
    public void Print3DVectors()
    {
        Console.WriteLine($"A = ({A[0]}, {A[1]}, {A[2]})");
        Console.WriteLine($"B = ({B[0]}, {B[1]}, {B[2]})");
        Console.WriteLine($"C = ({C[0]}, {C[1]}, {C[2]})");
=======
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
>>>>>>> feedback
    }

    
    public bool IsLinearlyIndependent3D()
    {
<<<<<<< revert-1-main
        double det =
            A[0] * (B[1] * C[2] - B[2] * C[1]) -
            A[1] * (B[0] * C[2] - B[2] * C[0]) +
            A[2] * (B[0] * C[1] - B[1] * C[0]);

        return Math.Abs(det) > 1e-9;
    }
}

class Program
{
    static void Main()
    {
        
        SystemOf2Vectors sys2 = new SystemOf2Vectors();
        sys2.SetVectors(1, 2, 3, 4);
        Console.WriteLine("Система 2-х векторів:");
        sys2.Print2DVectors();
        Console.WriteLine(sys2.IsLinearlyIndependent2D()
            ? "Вектори лінійно незалежні\n"
            : "Вектори лінійно залежні\n");

        
        SystemOf3Vectors sys3 = new SystemOf3Vectors();
        sys3.SetVectors(1, 0, 0,
                        0, 1, 0,
                        0, 0, 1);
        Console.WriteLine("Система 3-х векторів:");
        sys3.Print3DVectors();
        Console.WriteLine(sys3.IsLinearlyIndependent3D()
            ? "Вектори лінійно незалежні"
            : "Вектори лінійно залежні");
=======
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
>>>>>>> feedback
    }
}
