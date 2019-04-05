///-------------------------------------------------------------------------------------------------
// file:	RTPoint.cs
//
// summary:	Implements the right point class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A ray tracer point. </summary>
    ///
    /// <remarks>   Kemp, 11/7/2018. </remarks>
    /// <remarks>   This class implements the definition of a point in a 3D coordinate system.  It is 
    ///             implemented by specification of the usual X, Y and Z coordinates, plus a W coordinate
    ///             that, for Points, is always 1.  This convention makes the math easier.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Point : Tuple
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Point() {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="x">    The x value. </param>
        /// <param name="y">    The y value. </param>
        /// <param name="z">    The z value. </param>
        ///-------------------------------------------------------------------------------------------------

        public Point(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
            W = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="v">    A Vector to cast as a point. </param>
        ///-------------------------------------------------------------------------------------------------

        public Point(MathNet.Numerics.LinearAlgebra.Double.Vector v) {
            X = v[0];
            Y = v[1];
            Z = v[2];
            W = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="v">    A Vector to cast as a point. </param>
        ///-------------------------------------------------------------------------------------------------

        public Point(Vector v) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="t">    A Tuple to cast as a point. </param>
        ///-------------------------------------------------------------------------------------------------

        public Point(Tuple t) {
            X = t.X;
            Y = t.Y;
            Z = t.Z;
            W = 1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a Vector to this Point. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="a">    a to add. </param>
        ///
        /// <returns>   A Point. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point Add(Vector a) => new Point(X + a.X, Y + a.Y, Z + a.Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtracts the given Vector from this Point. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="a">    a to add. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector Subtract(Point a) => new Vector(X - a.X, Y - a.Y, Z - a.Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtracts the given Point a from this one. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="a">    a to add. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point Subtract(Vector a) => new Point(X - a.X, Y - a.Y, Z - a.Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transforms the Point by the given Matrix m. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="m">    A Transform Matrix. </param>
        ///
        /// <returns>   A Point. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point Transform(Matrix m) => new Point((MathNet.Numerics.LinearAlgebra.Double.Vector)(m * v));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Addition operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="p">    A Point to add. </param>
        /// <param name="v">    A Vector to add. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Point operator +(Point p, Vector v) => p.Add(v);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtraction operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="p">    A Point to subtract from. </param>
        /// <param name="v">    A Vector to subtract. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Point operator -(Point p, Vector v) => p.Subtract(v);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtraction operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="p1">   The first Point. </param>
        /// <param name="p2">   The second Point. </param>
        ///
        /// <returns>   The result of the operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Vector operator -(Point p1, Point p2) => p1.Subtract(p2);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Multiplication operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        /// <param name="p">    A Point to process. </param>
        ///
        /// <returns>   The result of the  multiplication operation. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Point operator *(Matrix m, Point p) => p.Transform(m);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override string ToString() => "P(" + v.ToString() + ")";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Query if this point is shadowed from a lightsource  in 'world'. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="world">    The world. </param>
        /// <param name="l">        A LightPoint to process. </param>
        ///
        /// <returns>   True if  the Point is shadowed from the LightPoint, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool IsShadowed(World world, LightPoint l) {
            Vector v = l.Position - this;
            double distance = v.Magnitude();
            Vector direction = v.Normalize();
            Ray ray = new Ray(this, direction);
            List<Intersection> intersections = world.Intersect(ray);
            List<Intersection> removeList = new List<Intersection>();
            // eliminate any transparent objects from intersection list
            // find the transparent ones
            foreach( Intersection i in intersections) {
                // Hack:  step function about shadowed.  > 0.5 transparency will turn off shadow.  Shouldn't be binary. Need a bulk opacity calculation.
                if (i.Obj.Material.Transparency > 0.5) removeList.Add(i);
            }
            // take them out of the intersection list
            foreach(Intersection i in removeList) {
                intersections.Remove(i);
            }
            Intersection h = Intersection.Hit(intersections);
            if (h != null) {
                h.Prepare(ray, intersections);
                if (h.T < distance)
                    return true;
            }
            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   A RTPoint. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point Copy() => new Point(X, Y, Z);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this Point is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 3/21/2019. </remarks>
        ///
        /// <param name="p">    A Point to compare with this point. </param>
        ///
        /// <returns>   True if all coordinates (W, X, Y, Z) are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(Point p) => Ops.Equals(W,p.W) && Ops.Equals(X, p.X) && Ops.Equals(Y, p.Y) && Ops.Equals(Z, p.Z);

    }
}
