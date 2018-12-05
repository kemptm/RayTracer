///-------------------------------------------------------------------------------------------------
// file:	GradientPattern.cs
//
// summary:	Implements the gradient pattern class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A gradient pattern. </summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    /// <remarks>   This is an algorithmically generated pattern where a gradient from Color a to Color b
    ///             in the X direction is returned.  The gradient repeats every integer value of X.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GradientPattern : Pattern
    {
        protected Color a;
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
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public GradientPattern() {
            a = new Color(0, 0, 0);
            b = new Color(0, 0, 0);
            xform = DenseMatrix.CreateIdentity(4);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="a">    a. </param>
        /// <param name="b">    The b. </param>
        ///-------------------------------------------------------------------------------------------------

        public GradientPattern(Color a, Color b, Matrix x = null) {
            this.a = a;
            this.b = b;
            if (x != null) xform = (Matrix)x.Clone();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at a particular point. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="p">    A RTPoint to return pattern color. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAt(Point p) {
            Color distance = b - a;
            double fraction = p.X - Math.Floor(p.X);
            return a + distance * fraction;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at object. (Abstract) </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
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
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <returns>   A Pattern. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Pattern Copy() => new GradientPattern(a, b, xform);

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
            if (m is GradientPattern) {
                GradientPattern c = (GradientPattern)m;
                return a.Equals(c.a) && b.Equals(c.b) && xform.Equals(c.xform);
            }
            return false;
        }

    }
}
