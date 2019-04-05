///-------------------------------------------------------------------------------------------------
// file:	SmoothTriangle.cs
//
// summary:	Implements the smooth triangle class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A smooth triangle. </summary>
    ///
    /// <remarks>   Kemp, 1/6/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class SmoothTriangle : Triangle {
        /// <summary>   The vector n0. </summary>
        protected Vector n0;
        /// <summary>   The vector n1. </summary>
        protected Vector n1;
        /// <summary>   The vector n2. </summary>
        protected Vector n2;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets  n0. </summary>
        ///
        /// <value> The n0. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector N0 { get { return n0; } set { n0 = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets  n1. </summary>
        ///
        /// <value> The n1. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector N1 { get { return n1; } set { n1 = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets vector n2. </summary>
        ///
        /// <value> The n2. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector N2 { get { return n2; } set { n2 = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 1/6/2019. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public SmoothTriangle() {
        }

        public SmoothTriangle(Point cv0, Point cv1, Point cv2) : base(cv0, cv1, cv2) {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 1/6/2019. </remarks>
        ///
        /// <param name="cv0">  The v0 (handled by base). </param>
        /// <param name="cv1">  The v1 (handled by base). </param>
        /// <param name="cv2">  The v2 (handled by base). </param>
        /// <param name="cn0">  The n0. </param>
        /// <param name="cn1">  The n1. </param>
        /// <param name="cn2">  The n2. </param>
        ///-------------------------------------------------------------------------------------------------

        public SmoothTriangle(Point cv0, Point cv1, Point cv2, Vector cn0, Vector cn1, Vector cn2) : base(cv0, cv1, cv2) {
            n0 = cn0;
            n1 = cn1;
            n2 = cn2;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <param name="localPoint">   The world point. </param>
        /// <param name="hit">          The hit. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint, Intersection hit) {
            return n1 * hit.U 
                 + n2 * hit.V 
                 + n0 * (1 - hit.U - hit.V);
        }

        public void AddNormals(Vector cn0, Vector cn1, Vector cn2) {
            n0 = cn0;
            n1 = cn1;
            n2 = cn2;

        }
    }
}
