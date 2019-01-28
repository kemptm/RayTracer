////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Color.cs
//
// summary:	Implements the color class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerLib
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A color. </summary>
    ///
    /// <remarks>   Kemp, 11/7/2018. </remarks>
    /// <remarks> This class is derived from RTTuple and so inherits matrix operations from it.</remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Color : Tuple 
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Red Component. </summary>
        ///
        /// <value> The red component of the color.  Note that this component is mapped to the X component of the underlying RTTuple . </value>
        ///-------------------------------------------------------------------------------------------------

        public double Red { get { return X; } set { X = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Green component. </summary>
        ///
        /// <value> The green component of the color.  Note that this component is mapped to the Y component of the underlying RTTuple . </value>
        ///-------------------------------------------------------------------------------------------------

        public double Green { get { return Y; } set { Y = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Blue component. </summary>
        ///
        /// <value> The blue component of the color.  Note that this component is mapped to the Z component of the underlying RTTuple </value>
        ///-------------------------------------------------------------------------------------------------

        public double Blue { get { return Z; } set { Z = value; } }

        const double colorMin = 0.0;
        const double colorMax = 1.0;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color() {
            X = 0;
            Y = 0;
            Z = 0;
            W = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="red">      The red component of the color. </param>
        /// <param name="green">    The green component of the color. </param>
        /// <param name="blue">     The blue component of the color. </param>
        /// <param name="Wa">       (Optional) The W component of the <code>RTTuple</code>. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color(double red, double green, double blue, double Wa = 0) {
            X = red;
            Y = green;
            Z = blue;
            W = Wa;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   A Color. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color Copy() => new Color(Red, Green, Blue, W);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public override string ToString() {
            return "C(" + v.ToString() + ")";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a color to this color. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="a">    Color to add to this one. </param>
        ///
        /// <returns>   A new Color object representing the sum of this color and the parameter. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color Add(Color a) {
            return new Color(X + a.X, Y + a.Y, Z + a.Z, W + a.W);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Multiply color by a scalar. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="a">    a scalar to multiply each of the components of the Color. </param>
        ///
        /// <returns>   A new Color. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color MultiplyScalar(double a) {
            return new Color(X * a, Y * a, Z * a, W * a);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Returns a new color of this color subtracted by the parameter.</summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="a">    Color to subtract. </param>
        ///
        /// <returns>   A new Color. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color Subtract(Color a) {
            return new Color(X - a.X, Y - a.Y, Z - a.Z, W = a.W);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Computes Hadamard Product. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        /// <remarks>   The Hadamard Product (or Schur Product) is a method for blending colors. The red, green, and blue components of
        ///             the current <code>Color</code> are multiplied by the corresponding components of the parameter 
        ///             <code>Color</code></remarks>
        ///
        /// <param name="c">   The Color to blend. </param>
        ///
        /// <returns>   A Color. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color HadamardProduct( Color c) {
            return new Color(Red * c.Red, Green * c.Green, Blue * c.Blue, W * c.W);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clamp Red. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        /// <remarks> If the value of the red color is out of bounds [colorMin, colorMax] then
        ///           it is brought to the closest limit, either high or low.</remarks>
        ///
        /// <returns>   this. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color ClampR() {
           if (X < colorMin) X = colorMin;
           else if (X > colorMax) X = colorMax;
           return this;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clamp Green. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        /// <remarks> If the value of the green color is out of bounds [colorMin, colorMax] then
        ///           it is brought to the closest limit, either high or low.</remarks>
        ///
        /// <returns>  this. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color ClampG() {
            if (Y < colorMin) Y = colorMin;
            else if (Y > colorMax) Y = colorMax;
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clamp Blue. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        /// <remarks> If the value of the blue color is out of bounds [colorMin, colorMax] then
        ///           it is brought to the closest limit, either high or low.</remarks>
        ///
        /// <returns>   this. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color ClampB() {
            if (Z < colorMin) Z = colorMin;
            else if (Z > colorMax) Z = colorMax;
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clamps all Color Components. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   this. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Color Clamp() {
            ClampR();
            ClampG();
            ClampB();
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Addition operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="v">    A Color to add. </param>
        /// <param name="a">    A Color to add. </param>
        ///
        /// <returns>   The sum of the colors. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Color operator +(Color v, Color a) {
            return v.Add(a);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Subtraction operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="v">    A Color to process. </param>
        /// <param name="a">    A Color to subtract. </param>
        ///
        /// <returns>   The result of the subtraction. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Color operator -(Color v, Color a) {
            return v.Subtract(a);
        }

         ////////////////////////////////////////////////////////////////////////////////////////////////////
         /// <summary>  Multiplication operator. </summary>
         ///
         /// <remarks>  Kemp, 11/7/2018. </remarks>
         ///
         /// <param name="c">   A Color to multiply. </param>
         /// <param name="a">   A double to multiply. </param>
         ///
         /// <returns>  The result of the operation. </returns>
         ////////////////////////////////////////////////////////////////////////////////////////////////////

         public static Color operator *(Color c, double a) {
            return c.MultiplyScalar(a);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Multiplication operator. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="c1">   The first Color. </param>
        /// <param name="c2">   The second Color. </param>
        ///
        /// <returns>   The result of the operation, the Hadamard Product. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Color operator *(Color c1, Color c2) {
            return c1.HadamardProduct(c2);
        }




    }
}
