///-------------------------------------------------------------------------------------------------
// file:	CheckedPattern.cs
//
// summary:	Implements the checked pattern class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A checked pattern. </summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    /// <remarks>   This pattern algorithmically produces, based on an input point, a Color.  That color is
    ///             based on the 2D X-Z plane.  The pattern does not change with variations in Y.  This
    ///             is not a texture map.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CheckedPattern : Pattern
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
        /// <summary>   Gets or sets the b Color. </summary>
        ///
        /// <value> The b Color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color B { get { return b; } set { b = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        /// <remarks>   The a and b Colors are set to a = black and b = white.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public CheckedPattern() {
            a = new Color(0, 0, 0);
            b = new Color(1, 1, 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. Colors Specified</summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="a">    The a Color. </param>
        /// <param name="b">    The b Color. </param>
        ///-------------------------------------------------------------------------------------------------

        public CheckedPattern(Color a, Color b) {
            this.a = a;
            this.b = b;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="a">    The a Color. </param>
        /// <param name="b">    The b Color. </param>
        /// <param name="x">    A Transform Matrix apply to the pattern. </param>
        ///-------------------------------------------------------------------------------------------------

        public CheckedPattern(Color a, Color b, Matrix x) {
            this.a = a;
            this.b = b;
            xform = (Matrix)x.Clone();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at specified RTpoint in local coordinate space. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="p">    A RTPoint location for which we want the Color. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAt(Point p) => (((Math.Floor(p.X) + Math.Floor(p.Z)) % 2.0) == 0) ? a : b;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at object in world coordinate space. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="s">            A RTShape to get the pattern from. </param>
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
        /// <returns>   A copy of this Pattern. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Pattern Copy() => new CheckedPattern(a, b, xform);

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
            if (m is CheckedPattern) {
                CheckedPattern c = (CheckedPattern)m;
                return a.Equals(c.a) && b.Equals(c.b) && xform.Equals(c.xform);
            }
            return false;
        }


    }
}
