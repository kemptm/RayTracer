///-------------------------------------------------------------------------------------------------
// file:	StripePattern.cs
//
// summary:	Implements the stripe pattern class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A stripe pattern. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This pattern generates stripes one unit wide alternating between Color a and Color b. The stripes
    ///             run in the z direction on the plane defined by X-Z axes.  The pattern may be transformed
    ///             to any orientation, and scaling.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class StripePattern : Pattern
    {
        /// <summary>   A Color of one stripe. </summary>
        protected Color a;
        /// <summary>   A Color of the alternate stripe. </summary>
        protected Color b;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a. </summary>
        ///
        /// <value> a. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color A { get { return a; } set { a = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the b. </summary>
        ///
        /// <value> The b. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color B { get { return b; } set { b = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public StripePattern() {
            a = new Color(0, 0, 0);
            b = new Color(1, 1, 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    A Color assign to a stripe. </param>
        /// <param name="b">    A Color to assign to the alternate stripe. </param>
        ///-------------------------------------------------------------------------------------------------

        public StripePattern(Color a,Color b) {
            this.a = a;
            this.b = b;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    A Color to assign to a stripe. </param>
        /// <param name="b">    A Color to assign to the alternate stripe. </param>
        /// <param name="x">    A Matrix to transform the pattern. </param>
        ///-------------------------------------------------------------------------------------------------

        public StripePattern(Color a, Color b,Matrix x) {
            StripePattern s = new StripePattern(a, b);
            xform = (Matrix)x.Clone();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine Pattern Color at a particular local point. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="p">    A RTPoint to return pattern color. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAt(Point p) => (Math.Floor(p.X) % 2.0) == 0 ? a : b;

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
            if (m is StripePattern) {
                StripePattern c = (StripePattern)m;
                return a.Equals(c.a) && b.Equals(c.b) && xform.Equals(c.xform);
            }
            return false;
        }

    }
}
