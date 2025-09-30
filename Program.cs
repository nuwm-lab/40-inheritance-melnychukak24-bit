using System;

/// <summary>
/// Базовий клас для роботи з системою векторів.
/// Містить спільну логіку для виводу та допоміжних обчислень.
/// </summary>
abstract class VectorSystem
{
    /// <summary>
    /// Вектори системи.
    /// </summary>
    protected double[][] Vectors { get; protected set; }

    /// <summary>
    /// Константа для порівняння з нулем при обчисленнях.
    /// </summary>
    protected const double Epsilon = 1e-9;

    /// <summary>
    /// Виводить усі вектори на екран.
    /// </summary>
    public virtual void PrintVectors()
    {
        for (int i = 0; i < Vectors.Length; i++)
        {
            Console.WriteLine($"V{i + 1} = ({string.Join(", ", Vectors[i])})");
        }
    }

    /// <summary>
    /// Перевіряє, чи є вектори системи лінійно незалежними.
    /// </summary>
    public abstract bool IsLinearlyIndependent();

    /// <summary>
    /// Обчислює визначник матриці 2x2.
    /// </summary>
    protected double Determinant2x2(double[,] m)
    {
        return m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
    }

    /// <summary>
    /// Обчислює визначник матриці 3x3.
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
/// Система з двох векторів у 2D.
/// </summary>
class Vector2System : VectorSystem
{
    public Vector2System(double[] a, double[] b)
    {
        if (a == null || b == null || a.Length != 2 || b.Length != 2)
            throw new ArgumentException("Усі вектори мають бути непорожніми та розмірності 2.");

        Vectors = new double[2][] { a, b };
    }

    /// <inheritdoc/>
    public override bool IsLinearlyIndependent()
    {
        double[,] matrix = {
            { Vectors[0][0], Vectors[0][1] },
            { Vectors[1][0], Vectors[1][1] }
        };
        return Math.Abs(Determinant2x2(matrix)) > Epsilon;
    }
}

/// <summary>
/// Система з трьох векторів у 3D.
/// </summary>
class Vector3System : VectorSystem
{
    public Vector3System(double[] a, double[] b, double[] c)
    {
        if (a == null || b == null || c == null ||
            a.Length != 3 || b.Length != 3 || c.Length != 3)
            throw new ArgumentException("Усі вектори мають бути непорожніми та розмірності 3.");

        Vectors = new double[3][] { a, b, c };
    }

    /// <inheritdoc/>
    public override bool IsLinearlyIndependent()
    {
        double[,] matrix = {
            { Vectors[0][0], Vectors[0][1], Vectors[0][2] },
            { Vectors[1][0], Vectors[1][1], Vectors[1][2] },
            { Vectors[2][0], Vectors[2][1], Vectors[2][2] }
        };
        return Math.Abs(Determinant3x3(matrix)) > Epsilon;
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
