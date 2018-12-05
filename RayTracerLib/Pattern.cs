///-------------------------------------------------------------------------------------------------
// file:	Pattern.cs
//
// summary:	Implements the pattern class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A pattern. </summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class Pattern
    {
        protected Matrix xform;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public Matrix Transform { get { return xform; } set { xform = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Pattern() {
            xform = DenseMatrix.CreateIdentity(4);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="x">    A Matrix to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Pattern(Matrix x) {
            xform = x;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <returns>   A Pattern. </returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract Pattern Copy();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Pattern at a particular point </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="p">    A RTPoint to return pattern color. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract Color PatternAt(Point p);

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

        public abstract Color PatternAtObject(Shape s, Point worldPoint);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transfom World pattern point to Local pattern point. (Virtual)</summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="s">            A RTShape to process. </param>
        /// <param name="worldPoint">   The world point. </param>
        ///
        /// <returns>   A RTPoint. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual Point LocalPatternPoint(Shape s, Point worldPoint) {
            Point objectPoint = s.WorldToObject(worldPoint);
            Point patternPoint = (Matrix)Transform.Inverse() * objectPoint;
            return patternPoint;

        }

        public abstract bool Equals(Pattern p);
    }
}
