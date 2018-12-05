///-------------------------------------------------------------------------------------------------
// file:	Ray.cs
//
// summary:	Implements the ray class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A ray. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   A Ray is the combination of a Point in a coordinate space and a vector defining a direction
    ///             that the ray points</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Ray
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The origin. </summary>
        ///
        /// <remarks>   The point at which the ray originates. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Point origin;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The direction. </summary>
        ///
        /// <remarks>   The direction vector of the ray.  This is usually normalized. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Vector direction;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   inverse direction. </summary>
        ///
        /// <remarks>
        ///     This is a computational convenience.  It is the ray in the opposite direction.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Vector invdir;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The sign. </summary>
        ///
        /// <remarks>
        ///     This is a computational convenience. It is a list of the signs of each of the components,
        ///     X, Y, Z of the inverse direction.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected List<bool> sign;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the origin. </summary>
        ///
        /// <value> The origin. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Origin {  get{ return origin; } set { origin = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the direction. </summary>
        ///
        /// <value> The direction. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Direction { get { return direction; } set { direction = value; InvRayInit(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the sign. </summary>
        ///
        /// <value> The sign. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<bool> Sign { get { return sign; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the invdir. </summary>
        ///
        /// <value> The invdir. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Invdir { get { return invdir; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Ray() {
            origin = new Point();
            direction = new Vector();
            invdir = new Vector();
            InvRayInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="o">    A Point to process. </param>
        /// <param name="d">    A Vector to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Ray(Point o, Vector d) {
            origin = new Point(o.X, o.Y,o.Z);
            direction = new Vector(d.X, d.Y, d.Z);
            invdir = new Vector();
            InvRayInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Inverse ray initialize. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        private void InvRayInit() {
            invdir.X = 1 / direction.X;
            invdir.Y = 1 / direction.Y;
            invdir.Z = 1 / direction.Z;
            sign = new List<bool>();
            sign.Add(invdir.X < 0);
            sign.Add(invdir.Y < 0);
            sign.Add(invdir.Z < 0);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Position. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        /// <remarks>   Calculates the point at the end of the ray.</remarks>
        ///
        /// <param name="t">    A double to process. </param>
        ///
        /// <returns>   A Point. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point Position(double t) => (direction * t) + origin;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Generate a Transformmation of this ray by Matrix m</summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    A Matrix to process. </param>
        ///
        /// <returns>   A Ray. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Ray Transform(Matrix m) => new Ray(origin.Transform(m), direction.Transform(m));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override String ToString() => "R(" + origin.ToString() + ", " + direction.ToString() + ")";
    }
}
