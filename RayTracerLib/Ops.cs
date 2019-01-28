///-------------------------------------------------------------------------------------------------
// file:	Ops.cs
//
// summary:	Implements the ops class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An abstract class of operations. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This class contains methods for operations on numbers, matricies and colors. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class Ops
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The epsilon. </summary>
        ///
        /// <value> The 00001. </value>
        ///-------------------------------------------------------------------------------------------------

        public const double EPSILON = 0.00001;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if objects are considered equal within a specified Epsilon. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">        Double to be compared. </param>
        /// <param name="b">        Double to be compared. </param>
        /// <param name="epsilon">  (Optional) The epsilon. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool Equals(double a, double b, double epsilon = EPSILON) => Math.Abs(a - b) < epsilon ? true : false;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if matricies are considered equal within a specified Epsilon. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">        Matrix to be compared. </param>
        /// <param name="b">        Matrix to be compared. </param>
        /// <param name="epsilon">  (Optional) The epsilon, the grace. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool Equals(Matrix a, Matrix b, double epsilon = EPSILON) {
            /// If the matricies are not the same shape, they can't be equal.
            if (!((a.RowCount == b.RowCount) && (a.ColumnCount == b.ColumnCount))) return false;
            /// Element by element compare
            for (int i = 0; i < a.RowCount; i++) {
                for (int j = 0; j < a.ColumnCount; j++) {
                    if (!Equals(a[i, j], b[i, j], epsilon)) return false;
                }
            }
            return true;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Swaps two generic items in a list. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="list">     The list. </param>
        /// <param name="indexA">   The index of item a. </param>
        /// <param name="indexB">   The index of item b. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void Swap<T>(IList<T> list, int indexA, int indexB) {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Lighting. Calculate the color of a point on an object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="material">         The material. </param>
        /// <param name="obj">              The object. </param>
        /// <param name="light">            The light. </param>
        /// <param name="worldPosition">    The world position. </param>
        /// <param name="eyev">             The eyev. </param>
        /// <param name="normalv">          The normalv. </param>
        /// <param name="inShadow">         (Optional) True to in shadow. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Color Lighting(Material material, Shape obj, LightPoint light, Point worldPosition, Vector eyev, Vector normalv, bool inShadow = false) {
            Color ambient;
            Color diffuse;
            Color specular;
            Color c;

            /// If there is a pattern, start with its contribution.  This can include textures too.
            if (material.Pattern != null) c = material.Pattern.PatternAtObject(obj,worldPosition);
            else c = material.Color;
            Color effectiveColor = c * light.Intensity;
            
            /// Calculate the ambient contribution
            ambient = effectiveColor * material.Ambient;
            /// if we're shadowed from the light, ambient is all there is, so return it.
            if (inShadow) return ambient;

            Color black = new Color(0, 0, 0);
            // find the direction to the light source.
            Vector lightv = (light.Position - worldPosition).Normalize();

            /// lightDotNormal represents the cosine of the angle between 
            /// the light vector and the normal vector.  A negative number means the
            /// light is on the other side of the surface. Ergo, no diffuse or specular contributions.
            double lightDotNormal = lightv.Dot(normalv);
            if (lightDotNormal < 0) {
                diffuse = black;
                specular = black;
            }
            else {
                /// Compute the diffuse contribution
                diffuse = effectiveColor * material.Diffuse * lightDotNormal;
                
                /// reflectDotEye represents the cosine of the angle between the
                /// reflection vector and the eye vector.  a negative number means the
                /// light reflects away from the eye.  Ergo, no specular contribution.
                Vector reflectv = (-lightv).Reflect(normalv);
                double reflectDotEye = Math.Pow(reflectv.Dot(eyev), material.Shininess);
                if (reflectDotEye <= 0) {
                    specular = black;
                }
                else {
                    /// Compute the specular contribution
                    specular = light.Intensity * (material.Specular * reflectDotEye);
                }
            }

            /// Combine the contributions to the color and return it.
            return ambient + diffuse + specular;
        }

    }
}
