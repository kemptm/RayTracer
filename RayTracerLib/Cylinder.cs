///-------------------------------------------------------------------------------------------------
// file:	Cylinder.cs
//
// summary:	Implements the cylinder class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A cylinder. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Cylinder : Shape
    {
        /// <summary>   The minimum y coordinate. </summary>
        protected double minY;
        /// <summary>   The maximum y coordinate. </summary>
        protected double maxY;
        /// <summary>   True if closed. </summary>
        protected bool closed;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the minimum y coordinate. </summary>
        ///
        /// <value> The minimum y coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double MinY { get { return minY; } set { minY = value; BoundsCalc(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the maximum y coordinate. </summary>
        ///
        /// <value> The maximum y coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double MaxY { get { return maxY; } set { maxY = value; BoundsCalc(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the closed. </summary>
        ///
        /// <value> True if closed, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Closed { get { return closed; } set { closed = value; BoundsCalc(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Cylinder() {
            minY = -double.MaxValue;
            maxY = double.MaxValue;
            closed = false;
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="ymin"> The ymin. </param>
        /// <param name="ymax"> The ymax. </param>
        /// <param name="cl">   True to cl. </param>
        /// <param name="xf">   The xf. </param>
        /// <param name="m">    A RTMaterial to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Cylinder(double ymin, double ymax, bool cl, Matrix xf, Material m) {
            minY = ymin;
            maxY = ymax;
            closed = cl;
            Transform = (Matrix)xf.Clone();
            material = m.Copy();
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Intersect capabilities. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="rayparm">  The rayparm. </param>
        /// <param name="y0">       The y coordinate 0. </param>
        /// <param name="y1">       The first y value. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        protected List<Intersection> IntersectCaps(Ray rayparm, double y0, double y1) {
            List<Intersection> xs = new List<Intersection>();
            if (!closed || Ops.Equals(rayparm.Direction.X * rayparm.Direction.X + rayparm.Direction.Z * rayparm.Direction.Z, 0)) return xs; // redundant: we already know that the ray isn't parallel to Y

            if (y0 > y1) {
                double temp = y1;
                y1 = y0;
                y0 = temp;
            }

            if ((minY >= y0) && (minY <= y1)) {
                double t = (minY - rayparm.Origin.Y) / rayparm.Direction.Y;
                xs.Add(new Intersection(t, this));
            }

            if ((maxY >= y0) && (maxY <= y1)) {
                double t = (maxY - rayparm.Origin.Y) / rayparm.Direction.Y;
                xs.Add(new Intersection(t, this));
            }

            return xs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Shape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {

            Cylinder s = new Cylinder();
            s.xform = (Matrix)xform.Clone();
            s.material = material.Copy();
            s.bounds = bounds.Copy();
            s.minY = minY;
            s.maxY = maxY;
            s.closed = closed;

            return s;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local intersect (abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="rayparm">  The ray to determine intersection with. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public override List<Intersection> LocalIntersect(Ray rayparm) {
            List<Intersection> xs = new List<Intersection>();

            /// calculate the discriminant
            double a = rayparm.Direction.X * rayparm.Direction.X + rayparm.Direction.Z * rayparm.Direction.Z;

            /// if a ~= 0, no intersection - ray is vertical
            if (Ops.Equals(a, 0)) {
                /// special case.  ray is inside the cylinder and is vertical it intersects the endcaps.
                if (closed && (rayparm.Origin.Y <= 1)) {
                    xs = Intersection.Intersections(new Intersection(minY, this), new Intersection(MaxY, this));
                }
                return xs;
            }
            /// the rest of the quadratic
            double b = 2 * rayparm.Origin.X * rayparm.Direction.X + 2 * rayparm.Origin.Z * rayparm.Direction.Z;
            double c = rayparm.Origin.X * rayparm.Origin.X + rayparm.Origin.Z * rayparm.Origin.Z - 1;
            double disc = b * b - 4 * a * c; // discriminent

            /// ray misses
            if (disc < 0) return xs;

            double rootdisc = Math.Sqrt(disc);
            double t0 = (-b - rootdisc) / (2 * a);
            double t1 = (-b + rootdisc) / (2 * a);

            if (t0 > t1) {
                double temp = t1;
                t1 = t0;
                t0 = temp;
            }
            /// calculate the y intercepts of the ray and cylinder.
            double y0 = rayparm.Origin.Y + t0 * rayparm.Direction.Y;
            if ((minY < y0) && (maxY > y0)) xs.Add(new Intersection(t0, this));

            double y1 = rayparm.Origin.Y + t1 * rayparm.Direction.Y;
            if ((minY < y1) && (maxY > y1)) xs.Add(new Intersection(t1, this));

            /// now the endcaps (if any)
            xs.AddRange(IntersectCaps(rayparm, y0, y1));
            return xs;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint) {
            double dist = localPoint.X * localPoint.X + localPoint.Z * localPoint.Z;
            if ((dist < 1) && (localPoint.Y >= (maxY - Ops.EPSILON))) return new Vector(0, 1, 0); // top cap
            if ((dist < 1) && (localPoint.Y <= (minY + Ops.EPSILON))) return new Vector(0, -1, 0); // bottom cap
            return new Vector(localPoint.X, 0, localPoint.Z); // cylinder sidewall
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Bounds LocalBounds() => new Bounds(new Point(-1, minY, -1), new Point(1, maxY, 1));
    }
}
