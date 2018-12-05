///-------------------------------------------------------------------------------------------------
// file:	Checked3DPattern.cs
//
// summary:	Implements the checked 3D pattern class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A checked 3D pattern. </summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    /// <remarks>   This class provides methods to return the Color of a point in 3D space.  That Color, is
    ///             determined algorithmically based on the coordinates of the point in 3D space of checkerboard
    ///             blocks. This is not a texture mapper.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Checked3DPattern : Pattern
    {
        protected Color a;
        protected Color b;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the "a" Color. </summary>
        ///
        /// <value> The a Color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color A { get { return a; } set { a = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the "b" Color. </summary>
        ///
        /// <value> The b Color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color B { get { return b; } set { b = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Checked3DPattern() {
            a = new Color(0, 0, 0);
            b = new Color(1, 1, 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="a">    a. </param>
        /// <param name="b">    The b. </param>
        ///-------------------------------------------------------------------------------------------------

        public Checked3DPattern(Color a, Color b) {
            this.a = a;
            this.b = b;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="a">    One color of the checkerboard pattern. </param>
        /// <param name="b">    The other color of the checkerboard pattern. </param>
        /// <param name="x">    A Matrix to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Checked3DPattern(Color a, Color b, Matrix x) {
            this.a = a;
            this.b = b;
            xform = (Matrix)x.Clone();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Get the color from the Pattern at a point in local coordinate space. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="p">    RTPoint to process. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAt(Point p) => ((Math.Floor(p.X) + Math.Floor(p.Y) + Math.Floor(p.Z)) % 2.0) == 0 ? a : b;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at object specified in world coordinates. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="s">            RTShape to process. </param>
        /// <param name="worldPoint">   The point to get the color from in world coordinates. </param>
        ///
        /// <returns>   Color at the specified point on the object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Color PatternAtObject(Shape s, Point worldPoint) => PatternAt(LocalPatternPoint(s, worldPoint));

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <returns>   A copy of this Pattern. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Pattern Copy() => new Checked3DPattern(a, b, xform);

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
            if (m is Checked3DPattern) {
                Checked3DPattern c = (Checked3DPattern)m;
                return a.Equals(c.a) && b.Equals(c.b) && xform.Equals(c.xform);
            }
            return false;
        }

    }
}
