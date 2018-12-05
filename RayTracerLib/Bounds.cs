///-------------------------------------------------------------------------------------------------
// file:	Bounds.cs
//
// summary:	Implements the bounds class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Bounds. The AABB of a shape.</summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    ///             
    /// <remarks>   This object keeps the Axis Aligned Bounding Box (AABB) of the containing shape or group. This
    ///             implementation represents an optimization, particularly over groups in that if a ray doesn't
    ///             intersect the bounding box, it doesn't intersect any of the enclosed shapes.  The intersection with
    ///             a bounding box is a fast test compared to tests on other shapes.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Bounds
    {
        protected Point maxCorner;
        protected Point minCorner;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum corner. </summary>
        ///
        /// <value> The maximum corner. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point MaxCorner { get { return maxCorner; } set { maxCorner = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the minimum corner. </summary>
        ///
        /// <value> The minimum corner. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point MinCorner { get { return minCorner; } set { minCorner = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Bounds() {
            maxCorner = new Point(0,0,0);
            minCorner = new Point(0,0,0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="min">  The minimum. </param>
        /// <param name="max">  The maximum. </param>
        ///-------------------------------------------------------------------------------------------------

        public Bounds(Point min, Point max) {
            minCorner = new Point(min.X, min.Y, min.Z);
            maxCorner = new Point(max.X, max.Y, max.Z);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Bounds Copy() => new Bounds(minCorner.Copy(), maxCorner.Copy());

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the Bounds of the given shape. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="shape">    The shape. </param>
        ///-------------------------------------------------------------------------------------------------

        internal void Calc(Shape shape) {
            Bounds lb = shape.LocalBounds();
            List<Point> box = new List<Point>();
            box.Add(new Point(lb.minCorner.X, lb.minCorner.Y, lb.minCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.minCorner.X, lb.minCorner.Y, lb.maxCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.minCorner.X, lb.maxCorner.Y, lb.minCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.minCorner.X, lb.maxCorner.Y, lb.maxCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.maxCorner.X, lb.minCorner.Y, lb.minCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.maxCorner.X, lb.minCorner.Y, lb.maxCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.maxCorner.X, lb.maxCorner.Y, lb.minCorner.Z).Transform(shape.Transform));
            box.Add(new Point(lb.maxCorner.X, lb.maxCorner.Y, lb.maxCorner.Z).Transform(shape.Transform));
            minCorner = new Point(double.MaxValue, double.MaxValue, double.MaxValue);
            maxCorner = new Point(-double.MaxValue, -double.MaxValue, -double.MaxValue);
            foreach (Point v in box) {
                minCorner.X = Math.Min(minCorner.X, v.X);
                minCorner.Y = Math.Min(minCorner.Y, v.Y);
                minCorner.Z = Math.Min(minCorner.Z, v.Z);
                maxCorner.X = Math.Max(maxCorner.X, v.X);
                maxCorner.Y = Math.Max(maxCorner.Y, v.Y);
                maxCorner.Z = Math.Max(maxCorner.Z, v.Z);
                /*Console.WriteLine(v.X.ToString() + ", " + v.Y.ToString() + ", " + v.Z.ToString() + " => (" + minCorner.X + ", " + minCorner.Y  + ", " + minCorner.Z + ") ("
                    + maxCorner.X + ", " + maxCorner.Y + ", " + maxCorner.Z + ")");
                    */
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Swaps two values.  They are passed by ref and so directly modified. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="a">    [in,out] A double to process. </param>
        /// <param name="b">    [in,out] A double to process. </param>
        ///-------------------------------------------------------------------------------------------------

        private void Swap(ref double a,ref double b) {
            double temp = a;
            a = b;
            b = temp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Intersect 1. Secondary implementation of RTRay intersecting with this Bounds</summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="r">    A RTRay to process. </param>
        ///
        /// <returns>   True if it intersects, false if it doesn't. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Intersect1(Ray r) {
            double tmin = (minCorner.X - r.Origin.X) / r.Direction.X;
            double tmax = (maxCorner.X - r.Origin.X) / r.Direction.X;

            if (tmin > tmax) Swap(ref tmin, ref tmax);

            double tymin = (minCorner.Y - r.Origin.Y) / r.Direction.Y;
            double tymax = (maxCorner.Y - r.Origin.Y) / r.Direction.Y;

            if (tymin > tymax) Swap(ref tymin, ref tymax);

            if ((tmin > tymax) || (tymin > tmax))
                return false;

            if (tymin > tmin)
                tmin = tymin;

            if (tymax < tmax)
                tmax = tymax;

            double tzmin = (minCorner.Z - r.Origin.Z) / r.Direction.Z;
            double tzmax = (maxCorner.Z - r.Origin.Z) / r.Direction.Z;

            if (tzmin > tzmax) Swap(ref tzmin, ref tzmax);

            if ((tmin > tzmax) || (tzmin > tmax))
                return false;

            if (tzmin > tmin)
                tmin = tzmin;

            if (tzmax < tmax)
                tmax = tzmax;

            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine whether RTRay r intersects with this Bounds. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="r">    A RTRay to process. </param>
        ///
        /// <returns>   True if it intersects, false if it doesn't. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Intersect(Ray r) {
            double tmin, tmax, tymin, tymax, tzmin, tzmax, degenerate;

            degenerate = 0;
            if (Ops.Equals(minCorner.X, maxCorner.X)) degenerate += 1;
            if (Ops.Equals(minCorner.Y, maxCorner.Y)) degenerate += 1;
            if (Ops.Equals(minCorner.Z, maxCorner.Z)) degenerate += 1;
            if (degenerate > 1)
                return true; // this is degenerate, we can't check it by bounding box.  It is always in.

            //tmin = (bounds[r.Sign[0]].x - r.Origin.X) * r.Invdir.X;
            tmin = ((r.Sign[0] ? maxCorner.X : minCorner.X) - r.Origin.X) * r.Invdir.X;
            //tmax = (bounds[1 - r.Sign[0]].x - r.Origin.X) * r.Invdir.X;
            tmax = ((r.Sign[0] ? minCorner.X : maxCorner.X) - r.Origin.X) * r.Invdir.X;
            //tymin = (bounds[r.Sign[1]].y - r.Origin.Y) * r.Invdir.Y;
            tymin = ((r.Sign[1] ? maxCorner.Y : minCorner.Y) - r.Origin.Y) * r.Invdir.Y;
            //tymax = (bounds[1 - r.Sign[1]].y - r.Origin.Y) * r.Invdir.Y;
            tymax = ((r.Sign[1] ? minCorner.Y : maxCorner.Y) - r.Origin.Y) * r.Invdir.Y;

            if ((tmin > tymax) || (tymin > tmax))
                return false;
            if (tymin > tmin)
                tmin = tymin;
            if (tymax < tmax)
                tmax = tymax;

            //tzmin = (bounds[r.Sign[2]].z - r.Origin.Z) * r.Invdir.Z;
            tzmin = ((r.Sign[2] ? maxCorner.Z : minCorner.Z) - r.Origin.Z) * r.Invdir.Z;
            //tzmax = (bounds[1 - r.Sign[2]].z - r.Origin.Z) * r.Invdir.Z;
            tzmax = ((r.Sign[2] ? minCorner.Z : maxCorner.Z) - r.Origin.Z) * r.Invdir.Z;

            if ((tmin > tzmax) || (tzmin > tmax))
                return false;
            if (tzmin > tmin)
                tmin = tzmin;
            if (tzmax < tmax)
                tmax = tzmax;

            if ((tmin < 0) && (tmax < 0)) return false;
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this Bounds is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="bds">  The bounds to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(Bounds bds) {
            bool b = true;
            b = b && minCorner.Equals(bds.minCorner);
            b = b && maxCorner.Equals(bds.maxCorner);
            return b;
        }
    }
}
