using System;

class SystemOf2Vectors
{
    protected double[] A = new double[2];
    protected double[] B = new double[2];//


    public void SetVectors(double a1, double a2, double b1, double b2)
    {
        A[0] = a1; A[1] = a2;
        B[0] = b1; B[1] = b2;
    }


    public void PrintVectors()
    {
        Console.WriteLine($"A = ({A[0]}, {A[1]})");
        Console.WriteLine($"B = ({B[0]}, {B[1]})");
    }


    public bool IsLinearlyIndependent()
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


    public new void PrintVectors()
    {
        Console.WriteLine($"A = ({A[0]}, {A[1]}, {A[2]})");
        Console.WriteLine($"B = ({B[0]}, {B[1]}, {B[2]})");
        Console.WriteLine($"C = ({C[0]}, {C[1]}, {C[2]})");
    }


    public bool IsLinearlyIndependent3D()
    {

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
        sys2.PrintVectors();
        Console.WriteLine(sys2.IsLinearlyIndependent()
            ? "Вектори лінійно незалежні\n"
            : "Вектори лінійно залежні\n");


        SystemOf3Vectors sys3 = new SystemOf3Vectors();
        sys3.SetVectors(1, 0, 0,
                        0, 1, 0,
                        0, 0, 1);
        Console.WriteLine("Система 3-х векторів:");
        sys3.PrintVectors();
        Console.WriteLine(sys3.IsLinearlyIndependent3D()
            ? "Вектори лінійно незалежні"
            : "Вектори лінійно залежні");
    }
}


