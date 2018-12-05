///-------------------------------------------------------------------------------------------------
// file:	MatrixOps.cs
//
// summary:	Implements the matrix ops class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A matrix ops. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This abstract class contains static methods for operating on matricies.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class MatrixOps {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts a Tuple to a Vector. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="t">    A Tuple to process. </param>
        ///
        /// <returns>   T as a MathNet.Numerics.LinearAlgebra.Double.Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static MathNet.Numerics.LinearAlgebra.Double.Vector ToVector(Tuple t) {
            MathNet.Numerics.LinearAlgebra.Double.Vector v = DenseVector.OfArray(new double[] { t.X, t.Y, t.Z, 1});
            return v;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts a Vector to a Tuple. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        ///
        /// <returns>   V as a Tuple. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Tuple ToTuple(MathNet.Numerics.LinearAlgebra.Double.Vector v) {
            Tuple r = new Tuple();
            r.X = v[0];
            r.Y = v[1];
            r.Z = v[2];
             return r;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Matrix x  Tuple. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to multiply. </param>
        /// <param name="t">    A Tuple to multiply. </param>
        ///
        /// <returns>   A Tuple. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Tuple MatrixXTuple(Matrix m, Tuple t) {
            MathNet.Numerics.LinearAlgebra.Double.Vector vr = (MathNet.Numerics.LinearAlgebra.Double.Vector)MathNet.Numerics.LinearAlgebra.Double.Vector.Build.Dense(4);
            MathNet.Numerics.LinearAlgebra.Double.Vector v = ToVector(t);
            m.Multiply(v, vr);
            return ToTuple(vr);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sub matrix. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        /// <param name="row">  The row to delete. </param>
        /// <param name="col">  The column to delete. </param>
        ///
        /// <returns>   A Matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix SubMatrix(Matrix m, int row, int col) => (Matrix)((Matrix)m.RemoveColumn(col)).RemoveRow(row);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate Minor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        /// <param name="row">  The row to delete. </param>
        /// <param name="col">  The column to delete. </param>
        ///
        /// <returns>   A Double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double Minor(Matrix m, int row, int col) => SubMatrix(m, row, col).Determinant();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate Cofactor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        /// <param name="row">  The row to delete. </param>
        /// <param name="col">  The column to delete. </param>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double Cofactor(Matrix m, int row, int col) {
            const int z = 0;
            return ((row + col) % 2) != z ? -Minor(m, row, col) : Minor(m, row, col);
        }

         ///-------------------------------------------------------------------------------------------------
         /// <summary>  Creates translation transform. </summary>
         ///
         /// <remarks>  Kemp, 11/9/2018. </remarks>
         ///
         /// <param name="x">   The x coordinate. </param>
         /// <param name="y">   The y coordinate. </param>
         /// <param name="z">   The z coordinate. </param>
         ///
         /// <returns>  The new translation transform. </returns>
         ///-------------------------------------------------------------------------------------------------

         public static Matrix CreateTranslationTransform(double x, double y, double z) => DenseMatrix.OfArray(new double[,] { { 1, 0, 0, x }, { 0, 1, 0, y }, { 0, 0, 1, z }, { 0, 0, 0, 1 } });

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Creates scaling transform. </summary>
        ///
        /// <remarks>  Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="x">   The x coordinate scale factor. </param>
        /// <param name="y">   The y coordinate scale factor. </param>
        /// <param name="z">   The z coordinate scale factor. </param>
        ///
        /// <returns>  The new scaling transform. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix CreateScalingTransform(double x, double y, double z) => DenseMatrix.OfArray(new double[,] { { x, 0, 0, 0 }, { 0, y, 0, 0 }, { 0, 0, z, 0 }, { 0, 0, 0, 1 } });

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates rotation x coordinate transform. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="r">    The angle in radians. </param>
        ///
        /// <returns>   The new rotation x coordinate transform. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix CreateRotationXTransform(double r) => DenseMatrix.OfArray(new double[,] { { 1, 0, 0, 0 }, { 0, Math.Cos(r), -Math.Sin(r), 0 }, {0, Math.Sin(r), Math.Cos(r), 0},{0, 0, 0, 1}});

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates rotation y coordinate transform. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="r">    The angle in radians. </param>
        ///
        /// <returns>   The new rotation y coordinate transform. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix CreateRotationYTransform(double r) => DenseMatrix.OfArray(new double[,] { { Math.Cos(r), 0, Math.Sin(r), 0 }, { 0, 1, 0, 0 }, { -Math.Sin(r), 0, Math.Cos(r), 0 }, { 0, 0, 0, 1 } });

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Creates rotation z coordinate transform. </summary>
        ///
        /// <remarks>  Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="r">   The angle in radians. </param>
        ///
        /// <returns>  The new rotation z coordinate transform. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix CreateRotationZTransform(double r) => DenseMatrix.OfArray(new double[,] { { Math.Cos(r), -Math.Sin(r),0, 0 }, { Math.Sin(r), Math.Cos(r), 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } });

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Creates shearing transform. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="x2y">  The x2y factor. </param>
        /// <param name="x2z">  The x2z factor. </param>
        /// <param name="y2x">  The y2x factor. </param>
        /// <param name="y2z">  The y2z factor. </param>
        /// <param name="z2x">  The z2x factor. </param>
        /// <param name="z2y">  The z2y factor. </param>
        ///
        /// <returns>   The new shearing transform. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix CreateShearingTransform(double x2y, double x2z, double y2x, double y2z, double z2x, double z2y) => DenseMatrix.OfArray(new double[,] { { 1, x2y, x2z, 0 }, { y2x, 1, y2z, 0 }, { z2x, z2y, 1, 0 }, { 0, 0, 0, 1 } });

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Create View transform. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        /// <remarks>   This transform is almost exclusively used to position a Camera.</remarks>
        ///
        /// <param name="from"> Source for the. </param>
        /// <param name="to">   to. </param>
        /// <param name="up">   The up. </param>
        ///
        /// <returns>   A Matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix CreateViewTransform(Point from, Point to, Vector up) {
            Vector forward = (to - from).Normalize();
            Vector left = forward.Cross(up.Normalize());
            Vector trueUp = left.Cross(forward);
            Matrix orientation = DenseMatrix.OfArray(new double[,] { 
                {  left.X,     left.Y,     left.Z,    0 },
                {  trueUp.X,   trueUp.Y,   trueUp.Z,  0 },
                { -forward.X, -forward.Y, -forward.Z, 0 },
                {  0,          0,          0,         1 } });
            return (Matrix)(orientation * CreateTranslationTransform(-from.X, -from.Y, -from.Z));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Multiply matrix. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        /// <remarks>   This method is a by-hand implementation of a matrix multiplication operation for two 4x4 matricies.
        ///             I implemented it to see if it would be faster than the generic.</remarks>
        ///
        /// <param name="a">    A Matrix to process. </param>
        /// <param name="b">    A Matrix to process. </param>
        ///
        /// <returns>   A Matrix. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Matrix MultiplyMatrix(Matrix a, Matrix b) {
            Matrix o = DenseMatrix.CreateIdentity(4); // output
            for ( int row = 0; row < 4; row++) {
                for (int col = 0; col < 4; col++) {
                    o[row, col] = a[row, 0] * b[0, col] + a[row, 1] * b[1, col] + a[row, 2] * b[2, col] + a[row, 3] * b[3, col];
                }
            }
            return o;
        }
    }
}
