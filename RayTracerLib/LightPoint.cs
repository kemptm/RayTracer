///-------------------------------------------------------------------------------------------------
// file:	LightPoint.cs
//
// summary:	Implements the light point class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A light point. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This class defines a point light source, that is a light source where the light
    ///             comes from a single point. It is defined by position and intensity.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class LightPoint {
        /// <summary>   The position in the local space of the light. </summary>
        protected Point position;
        /// <summary>   The intensity/Color of the light. </summary>
        protected Color intensity;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the position. </summary>
        ///
        /// <value> The position. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Position { get { return position; } set { position = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the intensity. </summary>
        ///
        /// <value> The intensity. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Intensity { get { return intensity; } set { intensity = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public LightPoint() {
            position = new Point();
            intensity = new Color();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="pos">      The position. </param>
        /// <param name="inten">    The inten. </param>
        ///-------------------------------------------------------------------------------------------------

        public LightPoint(Point pos, Color inten) {
            position = pos.Copy();
            intensity = inten.Copy();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A LightPoint. </returns>
        ///-------------------------------------------------------------------------------------------------

        public LightPoint Copy() => new LightPoint(position, intensity);
    }
}
