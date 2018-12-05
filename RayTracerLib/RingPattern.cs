///-------------------------------------------------------------------------------------------------
// file:	RingPattern.cs
//
// summary:	Implements the ring pattern class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A ring pattern.   </summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    /// <remarks>   Rings of alternate (a, b) Color from the origin in the X,Z plane. It will
    ///             algorighmically create a ring pattern in 2D that extrudes through the third
    ///             dimension. Per the superclass, Pattern, this pattern may be Transformed. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class RingPattern : Pattern
    {
        protected Color a;
        protected Color b;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a Color. </summary>
        ///
        /// <value> a Color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color A { get { return a; } set { a = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the b Color </summary>
        ///
        /// <value> The b Color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color B { get { return b; } set { b = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public RingPattern() {
            a = new Color(0, 0, 0);
            b = new Color(1, 1, 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    The a Color. </param>
        /// <param name="b">    The b Color. </param>
        ///-------------------------------------------------------------------------------------------------

        public RingPattern(Color a, Color b) {
            this.a = a;
            this.b = b;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    The a Color. </param>
        /// <param name="b">    The b Color. </param>
        /// <param name="x">    Transform Matrix. </param>
        ///-------------------------------------------------------------------------------------------------

        public RingPattern(Color a, Color b, Matrix x) {
            this.a = a;
            this.b = b;
            xform = (Matrix)x.Clone();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at a particular point. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="p">    A RTPoint to return pattern color. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAt(Point p) => (Math.Floor(Math.Sqrt(p.X*p.X +p.Z*p.Z)) % 2.0) == 0?a:b;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at object. (Abstract) </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="s">            A RTShape to process. </param>
        /// <param name="worldPoint">   The world point. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAtObject(Shape s, Point worldPoint) => PatternAt(LocalPatternPoint(s, worldPoint));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Pattern. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Pattern Copy() => new StripePattern(a, b, xform);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this Pattern is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 11/15/2018. </remarks>
        ///
        /// <param name="m">    The pattern to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool Equals(Pattern m) {
            if (m is RingPattern) {
                RingPattern c = (RingPattern)m;
                return a.Equals(c.a) && b.Equals(c.b) && xform.Equals(c.xform);
            }
            return false;
        }

    }
}
