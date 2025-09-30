using System;

/// <summary>
/// Базовий клас для роботи з системою векторів.
/// Дозволяє зберігати та перевіряти лінійну незалежність.
/// </summary>
abstract class VectorSystem
{
    protected double[][] Vectors;

    /// <summary>
    /// Вивід векторів у консоль.
    /// </summary>
    public virtual void PrintVectors()
    {
        for (int i = 0; i < Vectors.Length; i++)
        {
            Console.WriteLine($"V{i + 1} = ({string.Join(", ", Vectors[i])})");
        }
    }

    /// <summary>
    /// Перевіряє, чи є вектори лінійно незалежними.
    /// </summary>
    public abstract bool IsLinearlyIndependent();

    /// <summary>
    /// Допоміжний метод: обчислення визначника матриці 2х2.
    /// </summary>
    protected double Determinant2x2(double[,] m)
    {
        return m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
    }

    /// <summary>
    /// Допоміжний метод: обчислення визначника матриці 3х3.
    /// </summary>
    protected double Determinant3x3(double[,] m)
    {
        return
            m[0, 0] * (m[1, 1] * m[2, 2] - m[1, 2] * m[2, 1]) -
            m[0, 1] * (m[1, 0] * m[2, 2] - m[1, 2] * m[2, 0]) +
            m[0, 2] * (m[1, 0] * m[2, 1] - m[1, 1] * m[2, 0]);
    }
}

/// <summary>
/// Клас для роботи з 2D векторами.
/// </summary>
class Vector2System : VectorSystem
{
    public Vector2System(double[] a, double[] b)
    {
        if (a.Length != 2 || b.Length != 2)
            throw new ArgumentException("Усі вектори мають бути розмірності 2.");

        Vectors = new double[2][] { a, b };
    }

    public override bool IsLinearlyIndependent()
    {
        double[,] matrix = {
            { Vectors[0][0], Vectors[0][1] },
            { Vectors[1][0], Vectors[1][1] }
        };
        return Math.Abs(Determinant2x2(matrix)) > 1e-9;
    }
}

/// <summary>
/// Клас для роботи з 3D векторами.
/// </summary>
class Vector3System : VectorSystem
{
    public Vector3System(double[] a, double[] b, double[] c)
    {
        if (a.Length != 3 || b.Length != 3 || c.Length != 3)
            throw new ArgumentException("Усі вектори мають бути розмірності 3.");

        Vectors = new double[3][] { a, b, c };
    }

    public override bool IsLinearlyIndependent()
    {
        double[,] matrix = {
            { Vectors[0][0], Vectors[0][1], Vectors[0][2] },
            { Vectors[1][0], Vectors[1][1], Vectors[1][2] },
            { Vectors[2][0], Vectors[2][1], Vectors[2][2] }
        };
        return Math.Abs(Determinant3x3(matrix)) > 1e-9;
    }
}

class Program
{
    static void Main()
    {
        // 2D приклад
        var sys2 = new Vector2System(
            new double[] { 1, 2 },
            new double[] { 3, 4 }
        );
        Console.WriteLine("Система 2-х векторів:");
        sys2.PrintVectors();
        Console.WriteLine(sys2.IsLinearlyIndependent()
            ? "Вектори лінійно незалежні\n"
            : "Вектори лінійно залежні\n");

        // 3D приклад
        var sys3 = new Vector3System(
            new double[] { 1, 0, 0 },
            new double[] { 0, 1, 0 },
            new double[] { 0, 0, 1 }
        );
        Console.WriteLine("Система 3-х векторів:");
        sys3.PrintVectors();
        Console.WriteLine(sys3.IsLinearlyIndependent()
            ? "Вектори лінійно незалежні"
            : "Вектори лінійно залежні");
    }
}
