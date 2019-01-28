///-------------------------------------------------------------------------------------------------
// file:	LineSegment.cs
//
// summary:	Implements the line segment class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A line segment. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class LineSegment : Shape
    {
        /// <summary>   The line fatness. This is set as a convenience for rendering and not dropping pixels.</summary>
        protected static double lineFatness = 2 * Ops.EPSILON;
        /// <summary>   The lower Y coordinate. </summary>
        protected double pLo;
        /// <summary>   The higher Y coordinate. </summary>
        protected double pHi;
        /// <summary>   The direction of the presumed ray for this line segment pLo -> pHi. </summary>
        protected Vector direction;
        /// <summary>   The origin of the presumed ray for this line segment pLo -> pHi. </summary>
        protected Point origin;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the lower Point on the LineSegment. </summary>
        ///
        /// <value> The lower Point. </value>
        ///-------------------------------------------------------------------------------------------------

        public double PLo { get { return pLo; } set { pLo = value; origin.Y = value;  BoundsCalc(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the higher Point on the LineSegment. </summary>
        ///
        /// <value> The higher Point. </value>
        ///-------------------------------------------------------------------------------------------------

        public double PHi { get { return pHi; } set { pHi = value; BoundsCalc(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the direction. </summary>
        ///
        /// <value> The direction. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Direction { get { return direction; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the origin. </summary>
        ///
        /// <value> The origin. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Origin { get { return origin; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the line fatness. </summary>
        ///
        /// <value> The line fatness. </value>
        ///-------------------------------------------------------------------------------------------------

        public static double LineFatness { get { return lineFatness; } set { lineFatness = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public LineSegment() {
            pLo = -double.MaxValue;
            pHi = double.MaxValue;
            direction = new Vector(0, 1, 0);
            origin = new Point(0, -1, 0);
            Material.Ambient = new Color(1, 1, 1);
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="pt1">  The first point. </param>
        /// <param name="pt2">  The second point. </param>
        ///-------------------------------------------------------------------------------------------------

        public LineSegment(double pt1, double pt2) {
            pLo = pt1;
            pHi = pt2;
            direction = new Vector(0, 1, 0);
            Material.Ambient = new Color(1, 1, 1);
            origin = new Point(0, -1, 0);
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Shape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {
            LineSegment s =  new LineSegment(pLo, pHi);
            s.xform = (Matrix)xform.Clone();
            s.material = material.Copy();
            s.bounds = bounds.Copy();
            return s;
        }

        public override Bounds LocalBounds() => new Bounds(new Point(-lineFatness, pLo, -lineFatness), new Point(lineFatness, pHi, lineFatness));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local intersect. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="ray">  The ray. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public override List<Intersection> LocalIntersect(Ray ray) {
            List<Intersection> xs = new List<Intersection>();
            Vector u = direction;
            Vector v = ray.Direction;
            Vector w = origin - ray.Origin;
            double a = u.Dot(u);         // always >= 0
            double b = u.Dot(v);
            double c = v.Dot(v);         // always >= 0
            double d = u.Dot(w);
            double e = v.Dot(w);
            double D = a * c - b * b;        // always >= 0
            double sc, tc;

            // compute the line parameters of the two closest points
            if (D < Ops.EPSILON) {          // the lines are almost parallel
                sc = 0.0;
                tc = (b > c ? d / b : e / c);    // use the largest denominator
            }
            else {
                sc = (b * e - c * d) / D;
                tc = (a * e - b * d) / D;
            }

            // get the difference of the two closest points
            Vector dP = w + (u * sc) - (v * tc);  // =  L1(sc) - L2(tc)

            if (tc < 0) return xs; // ray points away
            double close = (Math.Sqrt(dP.Dot(dP)));
            if ( close < lineFatness){
                Point intercept = ray.Origin + (ray.Direction * tc);
                if (intercept.Y < pHi && intercept.Y > PLo) {
                    xs.Add(new Intersection(tc, this));
                }
            }

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
            if ((dist < 1) && (localPoint.Y >= (pHi - Ops.EPSILON))) return new Vector(0, 1, 0); // top cap
            if ((dist < 1) && (localPoint.Y <= (pLo + Ops.EPSILON))) return new Vector(0, -1, 0); // bottom cap
            return new Vector(localPoint.X, 0, localPoint.Z); // line side sidewall
        }
    }
}
