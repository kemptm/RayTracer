///-------------------------------------------------------------------------------------------------
// file:	Plane.cs
//
// summary:	Implements the plane class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A plane. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This class defines a plane that is on the X-Z axes in local coordinate space.  Y is zero
    ///             everywhere.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Plane :Shape
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Plane() {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Material to process. </param>
        /// <param name="t">    A Matrix to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Plane(Material m, Matrix t) {
            material = m.Copy();
            xform = (Matrix)t.Clone();
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
            Plane s = new Plane(material,xform);
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

        public override Bounds LocalBounds() => new Bounds(new Point(-double.MaxValue, 0, -double.MaxValue), new Point(double.MaxValue, 0, double.MaxValue));

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
            if (Ops.Equals(ray.Direction.Y,0)) return xs;
            double t = -ray.Origin.Y / ray.Direction.Y;
            xs.Add(new Intersection(t, this));
            return xs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A Vector (always Vector(0,1,0)). </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint) => new Vector(0,1,0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this Plane is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="obj">  The plane to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(Plane obj) {
            return base.Equals(obj);
        }

    }
}
