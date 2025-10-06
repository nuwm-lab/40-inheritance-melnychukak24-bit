using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearIndependence
{
    /// <summary>
    /// Інтерфейс для систем векторів.
    /// </summary>
    public interface ILinearSystem
    {
        /// <summary>
        /// Перевіряє, чи система векторів є лінійно незалежною.
        /// </summary>
        /// <returns>true – якщо незалежні, false – якщо залежні.</returns>
        bool IsLinearlyIndependent();
        
        /// <summary>
        /// Розмірність векторів у системі.
        /// </summary>
        int Dimension { get; }
        
        /// <summary>
        /// Кількість векторів у системі.
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// Абстрактний базовий клас для представлення системи векторів.
    /// </summary>
    public abstract class VectorSystem : ILinearSystem
    {
        /// <summary>
        /// Список векторів системи.
        /// </summary>
        protected IReadOnlyList<double[]> Vectors { get; }

        /// <summary>
        /// Допустима похибка для перевірки незалежності.
        /// </summary>
        protected const double Epsilon = 1e-9;

        protected VectorSystem(IEnumerable<IEnumerable<double>> vectors)
        {
            if (vectors == null)
                throw new ArgumentNullException(nameof(vectors), "Система векторів не може бути null.");

            var vectorsList = vectors.ToList();
            
            if (!vectorsList.Any())
                throw new ArgumentException("Система векторів не може бути порожньою.", nameof(vectors));

            Vectors = vectorsList.Select((v, index) =>
            {
                if (v == null)
                    throw new ArgumentException($"Вектор з індексом {index} не може бути null.", nameof(vectors));
                    
                var vectorArray = v.ToArray();
                
                if (vectorArray.Length != Dimension)
                    throw new ArgumentException($"Всі вектори мають бути розмірності {Dimension}. " +
                                              $"Вектор з індексом {index} має розмірність {vectorArray.Length}.", nameof(vectors));
                
                return vectorArray;
            }).ToList().AsReadOnly();

            if (Vectors.Count < RequiredVectorCount)
                throw new ArgumentException($"Система векторів у {Dimension}D має містити щонайменше {RequiredVectorCount} векторів. " +
                                          $"Надано {Vectors.Count} векторів.", nameof(vectors));
        }

        /// <summary>
        /// Розмірність векторів (2 для 2D, 3 для 3D тощо).
        /// </summary>
        public abstract int Dimension { get; }

        /// <summary>
        /// Необхідна кількість векторів для перевірки незалежності.
        /// </summary>
        protected abstract int RequiredVectorCount { get; }

        /// <summary>
        /// Кількість векторів у системі.
        /// </summary>
        public int Count => Vectors.Count;

        /// <inheritdoc />
        public abstract bool IsLinearlyIndependent();

        /// <summary>
        /// Перевіряє, чи в системі присутні нульові вектори.
        /// </summary>
        protected bool ContainsZeroVector()
        {
            return Vectors.Any(v => v.All(x => Math.Abs(x) < Epsilon));
        }

        /// <summary>
        /// Перевіряє, чи всі вектори мають однакову розмірність.
        /// </summary>
        protected bool AllVectorsHaveSameDimension()
        {
            return Vectors.All(v => v.Length == Dimension);
        }
    }

    /// <summary>
    /// Клас для роботи з системою векторів у 2D.
    /// </summary>
    public class Vector2System : VectorSystem
    {
        public Vector2System(IEnumerable<IEnumerable<double>> vectors) : base(vectors)
        {
        }

        /// <inheritdoc />
        public override int Dimension => 2;

        /// <inheritdoc />
        protected override int RequiredVectorCount => 2;

        /// <inheritdoc />
        public override bool IsLinearlyIndependent()
        {
            // Якщо є нульовий вектор, система залежна
            if (ContainsZeroVector()) 
                return false;

            // Для 2D використовуємо визначник
            if (Count == 2)
            {
                return CalculateDeterminant2x2(Vectors[0], Vectors[1]) > Epsilon;
            }

            // Для більшої кількості векторів у 2D вони завжди залежні
            return false;
        }

        /// <summary>
        /// Обчислює визначник для двох 2D векторів.
        /// </summary>
        /// <param name="v1">Перший вектор.</param>
        /// <param name="v2">Другий вектор.</param>
        /// <returns>Визначник матриці 2x2.</returns>
        private static double CalculateDeterminant2x2(double[] v1, double[] v2)
        {
            return v1[0] * v2[1] - v1[1] * v2[0];
        }
    }

    /// <summary>
    /// Клас для роботи з системою векторів у 3D.
    /// </summary>
    public class Vector3System : VectorSystem
    {
        public Vector3System(IEnumerable<IEnumerable<double>> vectors) : base(vectors)
        {
        }

        /// <inheritdoc />
        public override int Dimension => 3;

        /// <inheritdoc />
        protected override int RequiredVectorCount => 3;

        /// <inheritdoc />
        public override bool IsLinearlyIndependent()
        {
            // Якщо є нульовий вектор, система залежна
            if (ContainsZeroVector()) 
                return false;

            // Для 3D використовуємо визначник для перших трьох векторів
            if (Count == 3)
            {
                return Math.Abs(CalculateDeterminant3x3(Vectors[0], Vectors[1], Vectors[2])) > Epsilon;
            }

            // Для більшої кількості векторів у 3D використовуємо ранг матриці
            return CalculateMatrixRank() == 3;
        }

        /// <summary>
        /// Обчислює визначник для трьох 3D векторів.
        /// </summary>
        private static double CalculateDeterminant3x3(double[] v1, double[] v2, double[] v3)
        {
            return v1[0] * (v2[1] * v3[2] - v2[2] * v3[1])
                 - v1[1] * (v2[0] * v3[2] - v2[2] * v3[0])
                 + v1[2] * (v2[0] * v3[1] - v2[1] * v3[0]);
        }

        /// <summary>
        /// Обчислює ранг матриці, складеної з векторів.
        /// </summary>
        private int CalculateMatrixRank()
        {
            // Спрощена реалізація обчислення рангу через Gaussian elimination
            var matrix = Vectors.Select(v => v.ToArray()).ToArray();
            int rows = matrix.Length;
            int cols = matrix[0].Length;
            int rank = 0;

            for (int col = 0; col < cols && rank < rows; col++)
            {
                // Шукаємо ненульовий елемент в поточному стовпці
                int pivotRow = -1;
                for (int row = rank; row < rows; row++)
                {
                    if (Math.Abs(matrix[row][col]) > Epsilon)
                    {
                        pivotRow = row;
                        break;
                    }
                }

                if (pivotRow == -1) continue;

                // Міняємо місцями рядки
                if (pivotRow != rank)
                {
                    var temp = matrix[rank];
                    matrix[rank] = matrix[pivotRow];
                    matrix[pivotRow] = temp;
                }

                // Нормуємо головний рядок
                var pivot = matrix[rank][col];
                for (int j = col; j < cols; j++)
                {
                    matrix[rank][j] /= pivot;
                }

                // Обнуляємо елементи нижче
                for (int i = rank + 1; i < rows; i++)
                {
                    var factor = matrix[i][col];
                    for (int j = col; j < cols; j++)
                    {
                        matrix[i][j] -= factor * matrix[rank][j];
                    }
                }

                rank++;
            }

            return rank;
        }
    }

    /// <summary>
    /// Фабрика для створення систем векторів.
    /// </summary>
    public static class VectorSystemFactory
    {
        /// <summary>
        /// Створює систему векторів для заданої розмірності.
        /// </summary>
        public static VectorSystem CreateSystem(int dimension, IEnumerable<IEnumerable<double>> vectors)
        {
            return dimension switch
            {
                2 => new Vector2System(vectors),
                3 => new Vector3System(vectors),
                _ => throw new ArgumentException($"Розмірність {dimension} не підтримується. Підтримувані розмірності: 2, 3.")
            };
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                TestBasicCases();
                TestEdgeCases();
                TestFactory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка: {ex.Message}");
            }
        }

        static void TestBasicCases()
        {
            Console.WriteLine("=== ОСНОВНІ ТЕСТИ ===");
            
            // Тест 1: Лінійно незалежні 2D вектори
            var independent2D = new Vector2System(new[]
            {
                new double[] { 1, 2 },
                new double[] { 3, 4 }
            });
            PrintResult("2D незалежні", independent2D.IsLinearlyIndependent(), true);

            // Тест 2: Лінійно залежні 2D вектори
            var dependent2D = new Vector2System(new[]
            {
                new double[] { 1, 2 },
                new double[] { 2, 4 }
            });
            PrintResult("2D залежні", dependent2D.IsLinearlyIndependent(), false);

            // Тест 3: Лінійно незалежні 3D вектори
            var independent3D = new Vector3System(new[]
            {
                new double[] { 1, 0, 0 },
                new double[] { 0, 1, 0 },
                new double[] { 0, 0, 1 }
            });
            PrintResult("3D незалежні", independent3D.IsLinearlyIndependent(), true);

            // Тест 4: Лінійно залежні 3D вектори
            var dependent3D = new Vector3System(new[]
            {
                new double[] { 1, 2, 3 },
                new double[] { 2, 4, 6 },
                new double[] { 3, 6, 9 }
            });
            PrintResult("3D залежні", dependent3D.IsLinearlyIndependent(), false);
        }

        static void TestEdgeCases()
        {
            Console.WriteLine("\n=== КРАЙОВІ ВИПАДКИ ===");
            
            try
            {
                // Тест з нульовим вектором
                var withZeroVector = new Vector2System(new[]
                {
                    new double[] { 1, 2 },
                    new double[] { 0, 0 }
                });
                PrintResult("2D з нульовим вектором", withZeroVector.IsLinearlyIndependent(), false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при тестуванні нульового вектора: {ex.Message}");
            }

            try
            {
                // Тест з неправильною розмірністю
                var wrongDimension = VectorSystemFactory.CreateSystem(2, new[]
                {
                    new double[] { 1, 2, 3 } // 3D вектор для 2D системи
                });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Очікувана помилка розмірності: {ex.Message}");
            }
        }

        static void TestFactory()
        {
            Console.WriteLine("\n=== ТЕСТ ФАБРИКИ ===");
            
            var factory2D = VectorSystemFactory.CreateSystem(2, new[]
            {
                new double[] { 1, 1 },
                new double[] { 1, -1 }
            });
            PrintResult("Фабрика 2D", factory2D.IsLinearlyIndependent(), true);

            var factory3D = VectorSystemFactory.CreateSystem(3, new[]
            {
                new double[] { 1, 0, 0 },
                new double[] { 0, 1, 0 },
                new double[] { 0, 0, 1 }
            });
            PrintResult("Фабрика 3D", factory3D.IsLinearlyIndependent(), true);
        }

        /// <summary>
        /// Виводить результат тесту з перевіркою на очікуване значення.
        /// </summary>
        private static void PrintResult(string testName, bool actual, bool? expected = null)
        {
            var status = expected.HasValue 
                ? (actual == expected.Value ? "ПРОЙДЕНО" : "НЕ ПРОЙДЕНО")
                : "";
                
            Console.WriteLine($"{testName}: {actual} {status}");
        }
    }
}
