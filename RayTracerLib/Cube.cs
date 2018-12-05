///-------------------------------------------------------------------------------------------------
// file:	Cube.cs
//
// summary:	Implements the cube class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A cube. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This class algorithmically creates a cube with edges of length one, centered at the origin.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Cube : Shape
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   An RTShape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {
        
            Cube s = new Cube();
            s.xform = (Matrix) xform.Clone();
            s.material = material.Copy();
            s.bounds = bounds.Copy();
            return s;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Bounds LocalBounds() {
            return new Bounds(new Point(-1, -1, -1), new Point(1, 1, 1));
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
            List<Intersection> empty = new List<Intersection>();
            double tMinX, tMinY, tMinZ;
            double tMaxX, tMaxY, tMaxZ;
            /// if the max of the mins is greater than the min of the maxes, then it's a miss.
            CheckAxis(ray.Origin.X, ray.Direction.X, out tMinX, out tMaxX);
            CheckAxis(ray.Origin.Y, ray.Direction.Y, out tMinY, out tMaxY);
            CheckAxis(ray.Origin.Z, ray.Direction.Z, out tMinZ, out tMaxZ);

            double tmin = Math.Max(Math.Max(tMinX, tMinY), tMinZ); // max of the minimums
            double tmax = Math.Min(Math.Min(tMaxX, tMaxY), tMaxZ); // min of the maximums

            if (tmin > tmax) return new List<Intersection>();

            return Intersection.Intersections(new Intersection(tmin, this), new Intersection(tmax, this));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A RTVector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint) {
            double maxc = Math.Max(Math.Max(Math.Abs(localPoint.X), Math.Abs(localPoint.Y)), Math.Abs(localPoint.Z));

            if (maxc == Math.Abs(localPoint.X)) return new Vector(localPoint.X, 0, 0);
            if (maxc == Math.Abs(localPoint.Y)) return new Vector(0,localPoint.Y, 0);
            return new Vector(0, 0, localPoint.Z);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Check intersection of the cube with one axis of the ray. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="origin">       The origin. </param>
        /// <param name="direction">    The direction. </param>
        /// <param name="tmin">         [out] The smaller distance intersection (may be -infinity). </param>
        /// <param name="tmax">         [out] The larger distance intersection (may be infinity). </param>
        ///-------------------------------------------------------------------------------------------------

        protected void CheckAxis(double origin, double direction, out double tmin, out double tmax) {
            
            if (!Ops.Equals(Math.Abs(direction), 0)) {
                tmin = (-1 - origin) / direction;
                tmax = (1 - origin) / direction;
                if (tmin > tmax) {
                    double temp = tmin;
                    tmin = tmax;
                    tmax = temp;
                }
            }
            else {
                tmin = (-1 - origin) * double.MaxValue;
                tmax = (1 - origin) * double.MaxValue;
            }
        }
    }
}
