///-------------------------------------------------------------------------------------------------
// file:	Triangle.cs
//
// summary:	Implements the RTTriangle class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A triangle.  Instances of this class represent 2D triangles in 3D space.  Triangles are specified by 
    ///             their verticies.  They carry all of the attributes of a standard RTShape</summary>
    ///
    /// <remarks>   Kemp, 11/7/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Triangle : Shape
    {
        /// <summary>   The vertex v0. </summary>
        protected Point v0;
        /// <summary>   The vertex v1. </summary>
        protected Point v1;
        /// <summary>   The vertex v2. </summary>
        protected Point v2;
        /// <summary>   The vector e0. </summary>
        protected Vector e0;
        /// <summary>   The vectir e1. </summary>
        protected Vector e1;
        /// <summary>   The normal. </summary>
        protected Vector normal;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets v0. </summary>
        ///
        /// <value> v0. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point V0 { get { return v0; } set { v0 = value; InitTriangle(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets v1. </summary>
        ///
        /// <value> v1. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point V1 { get { return v1; } set { v1 = value; InitTriangle(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets v2. </summary>
        ///
        /// <value> v2. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point V2 { get { return v2; } set { v2 = value; InitTriangle(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the edge vector between points v0 and v1 </summary>
        ///
        /// <value> The edge</value>
        ///-------------------------------------------------------------------------------------------------

        public Vector E0 { get { return e0; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the edge Vector between points v1 and v2 </summary>
        ///
        /// <value> The edge </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector E1 { get { return e1; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the normal of the triangle </summary>
        ///
        /// <value> The normal. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Normal { get { return normal; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. Constructs a degenerate triangle with all vertices at (0,0,0).</summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Triangle() {
            v0 = new Point();
            v1 = new Point();
            v2 = new Point();
            InitTriangle();
            BoundsCalc();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor.  Constructs a triangle with the specified vertices.</summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="cv0">  The vertex v0 of the triangle. </param>
        /// <param name="cv1">  The vertex v1 of the triangle. </param>
        /// <param name="cv2">  The vertex v2 of the triangle. </param>
        ///-------------------------------------------------------------------------------------------------

        public Triangle(Point cv0, Point cv1, Point cv2) {
            v0 = cv0.Copy();
            v1 = cv1.Copy();
            v2 = cv2.Copy();
            InitTriangle();
            BoundsCalc(); // calculate the bounds.
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape override. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   An RTTriangle as an RTShape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {
            Triangle t = new Triangle(v0, v1, v2);
            t.bounds = bounds.Copy();
            t.material = material.Copy();
            t.xform = (Matrix)xform.Clone();
            t.BoundsCalc();
            return t;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in  the local coordinate space. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Bounds LocalBounds() {
            if (v0 == null) return new Bounds();
            return new Bounds(new Point(Math.Min(Math.Min(v0.X, v1.X), v2.X), Math.Min(Math.Min(v0.Y, v1.Y), v2.Y), Math.Min(Math.Min(v0.Z, v1.Z), v2.Z)),
                              new Point(Math.Max(Math.Max(v0.X, v1.X), v2.X), Math.Max(Math.Max(v0.Y, v1.Y), v2.Y), Math.Max(Math.Max(v0.Z, v1.Z), v2.Z)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local intersect. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        /// <para>   This intersection method is an implementation of the "inside-outside" algorithm. We create 
        ///             edge vectors between the verticies and determine which side of each vector winding around
        ///             the triangle the point may be found.  In our left-handed coordinate system, if the point is 
        ///             to the left of all three vectors, then it is on the inside of the triangle.</para>
        ///
        /// <param name="ray">  The ray to intersect. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; of the intersections, if any</returns>
        ///-------------------------------------------------------------------------------------------------

        public override List<Intersection> LocalIntersect(Ray ray) {
            List<Intersection> xs = new List<Intersection>();
            Vector N = LocalNormalAt(ray.Origin);
            /// If triangle is degenerate, return empty intersect list.
            if (N.Equals(new Vector(0, 0, 0))) return xs;  // triangle degenerate

            double rayDotN = ray.Direction.Dot(N);

            /// If the ray is parallel to the plane of the triangle, return empty intersect list.
            if (Ops.Equals(rayDotN, 0)) return xs; // ray parallel to triangle

            //xs = ScratchAPixelSolution(ray);
            xs = JBSolution(ray);
            return xs;
        }

        List<Intersection> JBSolution(Ray ray) {
            List<Intersection> xs = new List<Intersection>();
            Vector dirCrossE1 = ray.Direction.Cross(e1);
            double determinant = e0.Dot(dirCrossE1);
            if (Ops.Equals(Math.Abs(determinant), 0)) return xs;

            // calculate miss on p1p3(v0v2) edge
            double f = 1.0 / determinant;
            Vector v0ToOrigin = ray.Origin - v0;
            double u = f * v0ToOrigin.Dot(dirCrossE1);
            if (u < 0 || u > 1.0) return xs;

            // calculate miss on p1p2(v0v1) edge
            Vector originCrossE0 = v0ToOrigin.Cross(e0);
            double v = f * ray.Direction.Dot(originCrossE0);
            if (v < 0 || (u+v) > 1.0) return xs;

            double t = f * e1.Dot(originCrossE0);
            xs.Add(new Intersection(t, this, u, v));
            return xs;
        }

        List<Intersection>  ScratchAPixelSolution(Ray ray) {
            List<Intersection> xs = new List<Intersection>();
            Vector N = LocalNormalAt(ray.Origin);
            /// Determine where the ray intersects the plane
            double D = N.X * v0.X + N.Y * v0.Y + N.Z * v0.Z;
            double NDotOrigin = N.X * ray.Origin.X + N.Y * ray.Origin.Y + N.Z * ray.Origin.Z;
            double t = -(NDotOrigin + D) / N.Dot(ray.Direction);
            Point p = new Point(ray.Origin + ray.Direction * t);

            /// Now see if the located point is in  the triangle by determining which side of 
            /// each edge vector the point is on.  If it is to the left of all of them, then 
            /// return the point as an intersection.  Otherwise, return an empty intersection list.

            Vector edge0 = v1 - v0;
            Vector vp0 = p - v0;
            if (N.Dot(edge0.Cross(vp0)) > 0) return xs;

            Vector edge1 = v2 - v1;
            Vector vp1 = p - v1;
            if (N.Dot(edge1.Cross(vp1)) > 0) return xs;

            Vector edge2 = v0 - v2;
            Vector vp2 = p - v2;
            if (N.Dot(edge2.Cross(vp2)) > 0) return xs;

            Intersection hit = new Intersection(t, this);
            xs.Add(hit);

            return xs;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        /// <remarks>   The precomputed normalized cross-product of two edges will be the local normal to the triangle.  Note
        ///             that we don't care about the parameter point.</remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A RTVector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint) {
            return normal;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the triangle. </summary>
        ///
        /// <remarks>   Kemp, 11/18/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected void InitTriangle() {
            e0 = v1 - v0;
            e1 = v2 - v0;
            normal = e1.Cross(e0).Normalize(); // precalculate the normal

        }
    }
}
