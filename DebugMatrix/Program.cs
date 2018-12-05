using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace DebugMatrix
{
    class Program
    {
        static void Main(string[] args) {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 2,4,4,2},
                { 8,6,4,1},
                { 0,0,0,1 } });
            RayTracerLib.Tuple t = new RayTracerLib.Tuple(1, 2, 3);
            RayTracerLib.Tuple r = new RayTracerLib.Tuple(18, 24, 33);
            RayTracerLib.Tuple at = MatrixOps.MatrixXTuple(a, t);
            if (at.Equals(r)) {
                System.Console.WriteLine("All is good");
            }
            else {
                System.Console.WriteLine("not so boss");
            }

            Matrix a2 = DenseMatrix.OfArray(new double[,] {
                {1,2,6},
                {-5,8,-4},
                {2,6,4}
            });
            Matrix a3 = DenseMatrix.OfArray(new double[,] {
                {-2,-8,3,5},
                {-3,1,7,3},
                {1,2,-9,6},
                {-6,7,7,-9}
            });
            Matrix ai = DenseMatrix.OfArray(new double[,] {
                {-5,2,6,-8},
                {1,-5,1,8},
                {7,7,-6,-7},
                {1,-3,7,4}
            });
            Matrix ia = DenseMatrix.OfArray(new double[,] {
                {0.21805,0.45113,0.24060,-0.04511},
                {-0.80827,-1.45677,-0.44361,0.52068},
                {-0.07895,-0.22368,-0.05263, 0.19737},
                {-0.52256,-0.81391,-0.30075,0.30639}
            });

            Console.WriteLine(a2.Determinant());
            Console.WriteLine(a3.Determinant());
            Console.WriteLine(MatrixOps.Cofactor(a3, 0, 3));
            Console.WriteLine(ai.ToString());
            Console.WriteLine(ai.Inverse().ToString());
            Console.WriteLine(ia.ToString());

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~");
            Matrix a4 = DenseMatrix.OfArray(new double[,] {
                {3,-9,7,3 },
                {3,-8,2,-9},
                {-4,4,4,1},
                {-6,5,-1,1}
            });
            Matrix b4 = DenseMatrix.OfArray(new double[,] {
                {8,2,2,2},
                {3,-1,7,0},
                {7,0,5,4},
                {6,-2,0,5}
            });
            Matrix c4 = (Matrix)(a4 * b4);

            Matrix d4 = (Matrix)b4.Inverse();

            Matrix e4 = (Matrix)(c4 * b4.Inverse());

            Console.WriteLine(c4.ToString());
            Console.WriteLine(d4.ToString());
            Console.WriteLine(e4.ToString());
            Console.WriteLine(a4.ToString());
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~");

            Matrix i = DiagonalMatrix.CreateIdentity(4);
            Console.WriteLine(i.Inverse().ToString());
            Console.WriteLine((b4 * d4).ToString());
            Console.WriteLine(a4.Transpose().Inverse().ToString());
            Console.WriteLine(a4.Inverse().Transpose().ToString());
            Console.Write("Press Enter to finish ... ");
            Console.Read();


        }
    }
}
