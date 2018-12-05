///-------------------------------------------------------------------------------------------------
// file:	Sphere.cs
//
// summary:	Implements the sphere class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A sphere. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Sphere : Shape
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Sphere() : base() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Material to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Sphere(Material m) {
            material = m;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Material to process. </param>
        /// <param name="t">    A Matrix to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Sphere(Material m, Matrix t) {
            material = m;
            xform = (Matrix)t.Clone();
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
            //RTRay ray = rayparm.Transform((Matrix)xform.Inverse());
            Vector sphereToRay = ray.Origin - new Point(0,0,0);
            List<Intersection> rl = new List<Intersection>();
            double a = ray.Direction.Dot(ray.Direction);
            double b = 2.0 * ray.Direction.Dot(sphereToRay);
            double c = sphereToRay.Dot(sphereToRay) - 1;
            double discrim = b * b - 4 * a * c;

            if(discrim < 0) return rl;

            rl.Add(new Intersection((-b - Math.Sqrt(discrim)) / (2 * a),this));
            rl.Add(new Intersection((-b + Math.Sqrt(discrim)) / (2 * a),this));
            if (rl[0].T > rl[1].T) Ops.Swap(rl, 0, 1);
            return rl;
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

        public override Vector LocalNormalAt(Point localPoint) => (localPoint - new Point(0, 0, 0)).Normalize();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override String ToString() => Transform.ToString();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Shape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {
            Sphere s =  new Sphere(material, xform);
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

        public override Bounds LocalBounds() => new Bounds(new Point(-1, -1, -1), new Point(1, 1, 1));
    }
}
