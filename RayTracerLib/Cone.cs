///-------------------------------------------------------------------------------------------------
// file:	Cone.cs
//
// summary:	Implements the cone class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A cone, a shape. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   The cone as described, is a double cone, located  oriented on the Y axis with its
    ///             vertex at (0,0,0). By default, it is infinite, however bounds may be specified.
    ///             If bounded, the open ends of the cone may be capped. Normally, only one half of
    ///             the cone will be used. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Cone : Shape
    {
        /// <summary>   The minimum y coordinate of the cone. </summary>
        protected double minY;
        /// <summary>   The maximum y coordinate of the cone. </summary>
        protected double maxY;
        /// <summary>   True if the cone is closed. </summary>
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

        public Cone() : base() {
            minY = -double.MaxValue;
            maxY = double.MaxValue;
            closed = false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="ymin"> The minimum y coordinate. </param>
        /// <param name="ymax"> The maximum y coordinate. </param>
        /// <param name="cl">   True if the cone is to be closed (capped) </param>
        /// <param name="xf">   The Transform. </param>
        /// <param name="m">    RTMaterial of the cone. </param>
        ///-------------------------------------------------------------------------------------------------

        public Cone(double ymin, double ymax, bool cl, Matrix xf, Material m) : base() {
            minY = ymin;
            maxY = ymax;
            closed = cl;
            Transform = (Matrix)xf.Clone();
            material = m.Copy();
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Get Intersects with the cone's cap discs. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="ray">  The ray. </param>
        /// <param name="y0">   The y coordinate of the lower intersection with the cone. </param>
        /// <param name="y1">   The y coordinate of the upper intersection with the cone. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        protected List<Intersection> IntersectCaps(Ray ray, double y0, double y1) {
            List<Intersection> xs = new List<Intersection>();
            if (!closed) return xs; // redundant: we already know that the ray isn't parallel to Y

            if (y0 > y1) {
                double temp = y1;
                y1 = y0;
                y0 = temp;
            }

            if ((minY >= y0) && (minY <= y1)) {
                double t = (minY - ray.Origin.Y) / ray.Direction.Y;
                Point bottomInt = ray.Position(t);
                if (Math.Sqrt(bottomInt.X * bottomInt.X + bottomInt.Z * bottomInt.Z) < Math.Abs(minY)) xs.Add(new Intersection(t, this));
            }

            if ((maxY >= y0) && (maxY <= y1)) {
                double t = (maxY - ray.Origin.Y) / ray.Direction.Y;
                Point topInt = ray.Position(t);
                if (Math.Sqrt(topInt.X * topInt.X + topInt.Z * topInt.Z) < Math.Abs(maxY)) xs.Add(new Intersection(t, this));
            }

            return xs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   An RTShape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {

            Cone s = new Cone();
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
        /// <param name="ray">  The ray to intersect. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public override List<Intersection> LocalIntersect(Ray ray) {
            List<Intersection> xs = new List<Intersection>();

            /// Three Cases:
            /// 1. Line does not intersect Cone.  Complete miss can happen only if Direction.Y is zero, and SQRT(Origin.X^2 + Origin.Z^2) > Origin.Y. Discriminant is negative.
            /// 2. Line intersects cone once.  Line has slope = 1. Or it intersects at (0,0,0). Discriminant is zero
            /// 3. Line intersects cone twice. Normal case. Discriminent is positive.

            double t0, t1;
            /// x^2 + z^2 = y^2.  The radius = height.  If it's zero, the ray is tangent to the cone. If positive, ray intersects cone, if negative, misses.  Big problem is zero. This happens when
            /// vector lies in either the x plane or the z plane.
            double a = ray.Direction.X * ray.Direction.X - ray.Direction.Y * ray.Direction.Y + ray.Direction.Z * ray.Direction.Z;
            double b = 2 * ray.Origin.X * ray.Direction.X - 2 * ray.Origin.Y * ray.Direction.Y + 2 * ray.Origin.Z * ray.Direction.Z;
            double c = ray.Origin.X * ray.Origin.X - ray.Origin.Y * ray.Origin.Y + ray.Origin.Z * ray.Origin.Z;
            double disc = b * b - 4 * a * c; // discriminent

            /// Ray is parallel to cone.
            /// Two cases.  1) embedded in cone and 2) parallel to cone, inside or outside.  It intersects 1) infinite  2) once.  however, for the purposes of ray-tracing it is irrelevant, in
            /// that it will only affect one pixel (maybe) on the rendered cone.
            if (Ops.Equals(a, 0)) {
                return xs;
                // the following dead code attempted to handle the case where ray is parallel, but hits.  Didn't work.  It's a fringe case, so we accept the acne.
                /*if (RTOps.Equals(ray.Direction.Y, 0)) return xs;  // no y component; a complete miss
                // find xy plane intercept
                RTPlane xz = new RTPlane();
                List<Intersection> xss = xz.localIntersect(ray);
                RTPoint xzintercept = ray.Position(xss[0].T);
                double distFromOrigin = Math.Sqrt(xzintercept.X * xzintercept.X + xzintercept.Z * xzintercept.Z);
                double t = distFromOrigin * Math.Sqrt(2) / 2 + ray.Origin.Y * Math.Sqrt(2);
                t0 = t;
                t1 = t;
                */
            }

            /// ray misses
            if (disc < 0) return xs; // note: this permits the case where ray is on the boundary of the cone.  It is considered a hit and will return the cone's origin.


            double rootdisc = Math.Sqrt(disc);
            t0 = (-b - rootdisc) / (2 * a);
            t1 = (-b + rootdisc) / (2 * a);

            if (t0 > t1) {
                double temp = t1;
                t1 = t0;
                t0 = temp;
            }

            double y0 = ray.Origin.Y + t0 * ray.Direction.Y;
            if ((minY < y0) && (maxY > y0)) {
                xs.Add(new Intersection(t0, this));
            }

            double y1 = ray.Origin.Y + t1 * ray.Direction.Y;
            if ((minY < y1) && (maxY > y1)) {
                xs.Add(new Intersection(t1, this));
            }

            Point xi0 = ray.Origin + ray.Direction * t0;
            Point xi1 = ray.Origin + ray.Direction * t1;
            // now the endcaps (if any)
            xs.AddRange(IntersectCaps(ray, y0, y1));
            return xs;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="point">    The local point. </param>
        ///
        /// <returns>   A RTVector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point point) {

            double dist = point.X * point.X + point.Z * point.Z;
            double ySquared = point.Y * point.Y;

            if ((dist < ySquared) && (point.Y >= (maxY - Ops.EPSILON*5))) {
                return new Vector(0, 1, 0); // top cap
            }
            if ((dist < ySquared) && (point.Y <= (minY + Ops.EPSILON*5))) {
                return new Vector(0, -1, 0); // bottom cap
            }
            double y = Math.Abs(point.Y);
            if (point.Y > 0) y = -y;
            Vector normal = new Vector(point.X, y, point.Z);
            return normal; // cone sidewall
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in the local coordinate space (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Bounds LocalBounds() {
            Bounds b = new Bounds(new Point(-double.MaxValue, -double.MaxValue, -double.MaxValue), new Point(double.MaxValue, double.MaxValue, double.MaxValue));
            if (minY == -double.MaxValue || maxY == double.MaxValue) return b;
            /*
            List<Point> verticies = new List<Point>();
            verticies.Add(new Point(-Math.Abs(minY), minY, -Math.Abs(minY)));
            verticies.Add(new Point(-Math.Abs(minY), minY,  Math.Abs(minY)));
            verticies.Add(new Point( Math.Abs(minY), minY, -Math.Abs(minY)));
            verticies.Add(new Point( Math.Abs(minY), minY,  Math.Abs(minY)));

            verticies.Add(new Point(-Math.Abs(maxY), maxY, -Math.Abs(maxY)));
            verticies.Add(new Point(-Math.Abs(maxY), maxY,  Math.Abs(maxY)));
            verticies.Add(new Point( Math.Abs(maxY), maxY, -Math.Abs(maxY)));
            verticies.Add(new Point( Math.Abs(maxY), maxY,  Math.Abs(maxY)));
            */
            b.MinCorner.X = -Math.Max(Math.Abs(minY), Math.Abs(maxY));
            b.MinCorner.Y = Math.Min(minY,maxY);
            b.MinCorner.Z = b.MinCorner.X;
            b.MaxCorner.X = Math.Max(Math.Abs(minY), Math.Abs(maxY));
            b.MaxCorner.Y = Math.Max(minY, maxY);
            b.MaxCorner.Z = b.MaxCorner.X;
            return b;
        }
    }
}
