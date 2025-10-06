using System;

/// <summary>
/// Клас для роботи із системою з двох векторів у 2D.
/// </summary>
class SystemOf2Vectors
{
    protected double[] A = new double[3]; // Вектор A (x, y, 0)
    protected double[] B = new double[3]; // Вектор B (x, y, 0)

    /// <summary>
    /// Задати значення векторів A і B (2D).
    /// </summary>
    public void SetVectors(double a1, double a2, double b1, double b2)
    {
        A[0] = a1; A[1] = a2; A[2] = 0;
        B[0] = b1; B[1] = b2; B[2] = 0;
    }

    /// <summary>
    /// Вивести вектори у консоль.
    /// </summary>
    public void Print2DVectors()
    {
        Console.WriteLine($"A = ({A[0]}, {A[1]})");
        Console.WriteLine($"B = ({B[0]}, {B[1]})");
    }

    /// <summary>
    /// Перевірити, чи вектори A і B лінійно незалежні.
    /// </summary>
    public bool IsLinearlyIndependent2D()
    {
        double det = A[0] * B[1] - A[1] * B[0];
        return Math.Abs(det) > 1e-9;
    }
}

/// <summary>
/// Клас для роботи із системою з трьох векторів у 3D.
/// </summary>
class SystemOf3Vectors : SystemOf2Vectors
{
    private double[] C = new double[3]; // Третій вектор

    /// <summary>
    /// Задати значення векторів A, B, C (3D).
    /// </summary>
    public void SetVectors(double a1, double a2, double a3,
                           double b1, double b2, double b3,
                           double c1, double c2, double c3)
    {
        A = new double[] { a1, a2, a3 };
        B = new double[] { b1, b2, b3 };
        C = new double[] { c1, c2, c3 };
    }

    /// <summary>
    /// Вивести вектори у консоль.
    /// </summary>
    public void Print3DVectors()
    {
        Console.WriteLine($"A = ({A[0]}, {A[1]}, {A[2]})");
        Console.WriteLine($"B = ({B[0]}, {B[1]}, {B[2]})");
        Console.WriteLine($"C = ({C[0]}, {C[1]}, {C[2]})");
    }

    /// <summary>
    /// Перевірити, чи вектори A, B, C лінійно незалежні.
    /// </summary>
    public bool IsLinearlyIndependent3D()
    {
        double det =
            A[0] * (B[1] * C[2] - B[2] * C[1]) -
            A[1] * (B[0] * C[2] - B[2] * C[0]) +
            A[2] * (B[0] * C[1] - B[1] * C[0]);

        return Math.Abs(det) > 1e-9;
    }
}

/// <summary>
/// Тестова програма.
/// </summary>
class Program
{
    static void Main()
    {
        // --- Система з 2 векторів ---
        SystemOf2Vectors sys2 = new SystemOf2Vectors();
        sys2.SetVectors(1, 2, 3, 4);
        Console.WriteLine("Система 2-х векторів:");
        sys2.Print2DVectors();
        Console.WriteLine(sys2.IsLinearlyIndependent2D()
