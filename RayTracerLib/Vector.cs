///-------------------------------------------------------------------------------------------------
// file:	Vector.cs
//
// summary:	Implements the vector class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A vector. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Vector : Tuple
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Vector() {
            X = 0;
            Y = 0;
            Z = 0;
            W = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="x">    The x coordinate. </param>
        /// <param name="y">    The y coordinate. </param>
        /// <param name="z">    The z coordinate. </param>
        ///-------------------------------------------------------------------------------------------------

        public Vector(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
            W = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Vector(MathNet.Numerics.LinearAlgebra.Double.Vector v) {
            X = v[0];
            Y = v[1];
            Z = v[2];
            W = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Vector(Vector v) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="t">    A Tuple to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Vector(Tuple t) {
            X = t.X;
            Y = t.Y;
            Z = t.Z;
            W = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Vector to be negated. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Add(Vector a) => new Vector(X + a.X, Y + a.Y, Z + a.Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Vector to be negated. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point Add(Point a) => new Point(X + a.X, Y + a.Y, Z + a.Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtracts the given a. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Vector to be negated. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Subtract(Vector a) => new Vector(X - a.X, Y - a.Y, Z - a.Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Unary minus (-) operator; Negate Vector. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Vector to be negated. </param>
        ///
        /// <returns>   Negated Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator -(Vector a) => new Vector(0, 0, 0).Subtract(a);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtract Vectors. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    A Vector to process. </param>
        /// <param name="b">    A Vector to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator -(Vector a, Vector b) => a.Subtract(b);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Add Vector and Point. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        /// <param name="a">    A Point to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Point operator +(Vector v, Point a) => v.Add(a);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Add two vectors. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        /// <param name="a">    A Vector to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator +(Vector v, Vector a) => v.Add(a);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Multiply Vectory by scalar. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        /// <param name="a">    A double to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator *(Vector v, double a) => new Vector(v.X * a, v.Y * a, v.Z * a);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Multiply Matrix by Vector. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        /// <param name="v">    A Vector to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator *(Matrix m, Vector v) => v.Transform(m);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Divide the Vector by a scalar. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v">    A Vector to process. </param>
        /// <param name="a">    A double to process. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator /(Vector v, double a) => new Vector(v.X / a, v.Y / a, v.Z / a);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the magnitude of this vector. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Normalize this Vector. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Normalize() { // optimized to minimize divisions and not divide by zero.
            double magnitude = Magnitude();
            if (magnitude > 0) {
                double inverseMagnitude = 1.0 / magnitude;
                return new Vector(X * inverseMagnitude, Y * inverseMagnitude, Z * inverseMagnitude);
            }
            return new Vector(X, Y, Z); // copy of this.
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the Dot Product between this and the the given a. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Vector to be negated. </param>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double Dot(Vector a) => X * a.X + Y * a.Y + Z * a.Z;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the Cross Product between this and the given b. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="b">    A Vector to process. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Cross(Vector b) => new Vector(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString() => "V(" + v.ToString() + ")";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transforms the given m. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Transform(Matrix m) => new Vector((MathNet.Numerics.LinearAlgebra.Double.Vector)(m * v));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Reflects this vector around the given normal. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="normal">   The normal. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Reflect(Vector normal) => this - normal * (2 * Dot(normal));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Copy() {
            return new Vector(X, Y, Z);
        }

    }

}
