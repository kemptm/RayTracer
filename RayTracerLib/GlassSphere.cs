///-------------------------------------------------------------------------------------------------
// file:	GlassSphere.cs
//
// summary:	Implements the glass sphere class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The glass sphere. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This subclass of Sphere is a convenience class for defining a glass sphere: transparent
    ///             and with the refractive index of glass.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GlassSphere : Sphere
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public GlassSphere() {
            material.Transparency = 1.0;
            material.RefractiveIndex = 1.5;
        }
    }
}
