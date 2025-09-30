using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearIndependence
{
    /// <summary>
    /// Абстрактний клас для представлення системи векторів.
    /// </summary>
    abstract class VectorSystem
    {
        /// <summary>
        /// Список векторів системи.
        /// </summary>
        protected List<double[]> Vectors { get; }

        /// <summary>
        /// Допустима похибка для перевірки незалежності.
        /// </summary>
        protected const double Epsilon = 1e-9;

        protected VectorSystem(IEnumerable<IEnumerable<double>> vectors)
        {
            if (vectors == null || !vectors.Any())
                throw new ArgumentException("Система векторів не може бути порожньою.");

            Vectors = vectors.Select(v =>
            {
                if (v == null) throw new ArgumentException("Вектор не може бути null.");
                return v.ToArray();
            }).ToList();
        }

        /// <summary>
        /// Перевірка, чи система є лінійно незалежною.
        /// </summary>
        public abstract bool IsLinearlyIndependent();

        /// <summary>
        /// Перевірка, чи вектор є нульовим.
        /// </summary>
        protected static bool IsZeroVector(double[] vector)
        {
            return vector.All(x => Math.Abs(x) < Epsilon);
        }
    }

    /// <summary>
    /// Клас для роботи з системою векторів у 2D.
    /// </summary>
    class Vector2System : VectorSystem
    {
        public Vector2System(IEnumerable<IEnumerable<double>> vectors) : base(vectors)
        {
            if (Vectors.Any(v => v.Length != 2))
                throw new ArgumentException("Усі вектори мають бути розмірності 2.");
        }

        public override bool IsLinearlyIndependent()
        {
            if (Vectors.Any(IsZeroVector)) return false;

            double[,] matrixForDeterminant =
            {
                { Vectors[0][0], Vectors[1][0] },
                { Vectors[0][1], Vectors[1][1] }
            };

            return Math.Abs(Determinant2x2(matrixForDeterminant)) > Epsilon;
        }

        /// <summary>
        /// Обчислення визначника 2x2.
        /// </summary>
        private static double Determinant2x2(double[,] matrixForDeterminant)
        {
            return matrixForDeterminant[0, 0] * matrixForDeterminant[1, 1]
                 - matrixForDeterminant[0, 1] * matrixForDeterminant[1, 0];
        }
    }

    /// <summary>
    /// Клас для роботи з системою векторів у 3D.
    /// </summary>
    class Vector3System : VectorSystem
    {
        public Vector3System(IEnumerable<IEnumerable<double>> vectors) : base(vectors)
        {
            if (Vectors.Any(v => v.Length != 3))
                throw new ArgumentException("Усі вектори мають бути розмірності 3.");
        }

        public override bool IsLinearlyIndependent()
        {
            if (Vectors.Any(IsZeroVector)) return false;

            double[,] matrixForDeterminant =
            {
                { Vectors[0][0], Vectors[1][0], Vectors[2][0] },
                { Vectors[0][1], Vectors[1][1], Vectors[2][1] },
                { Vectors[0][2], Vectors[1][2], Vectors[2][2] }
            };

            return Math.Abs(Determinant3x3(matrixForDeterminant)) > Epsilon;
        }

        /// <summary>
        /// Обчислення визначника 3x3.
        /// </summary>
        private static double Determinant3x3(double[,] matrixForDeterminant)
        {
            return matrixForDeterminant[0, 0] * (matrixForDeterminant[1, 1] * matrixForDeterminant[2, 2] - matrixForDeterminant[1, 2] * matrixForDeterminant[2, 1])
                 - matrixForDeterminant[0, 1] * (matrixForDeterminant[1, 0] * matrixForDeterminant[2, 2] - matrixForDeterminant[1, 2] * matrixForDeterminant[2, 0])
                 + matrixForDeterminant[0, 2] * (matrixForDeterminant[1, 0] * matrixForDeterminant[2, 1] - matrixForDeterminant[1, 1] * matrixForDeterminant[2, 0]);
        }
    }

    class Program
    {
        static void Main()
        {
            var vectorSystem2D = new Vector2System(new[]
            {
                new double[] { 1, 2 },
                new double[] { 3, 4 }
            });

            var vectorSystem3D = new Vector3System(new[]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 },
                new double[] { 7, 8, 9 }
            });

            PrintResult("2D", vectorSystem2D.IsLinearlyIndependent());
            PrintResult("3D", vectorSystem3D.IsLinearlyIndependent());
        }

        /// <summary>
        /// Виведення результату у консоль.
        /// </summary>
        private static void PrintResult(string dimension, bool isIndependent)
        {
            Console.WriteLine($"{dimension} вектори є {(isIndependent ? "лінійно незалежними" : "лінійно залежними")}");
        }
    }
}
