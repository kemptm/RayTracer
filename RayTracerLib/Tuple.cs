///-------------------------------------------------------------------------------------------------
// file:	Tuple.cs
//
// summary:	Implements the tuple class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A tuple. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Tuple {
        /// <summary>   A Vector to process. </summary>
        protected MathNet.Numerics.LinearAlgebra.Double.Vector v  = (MathNet.Numerics.LinearAlgebra.Double.Vector)MathNet.Numerics.LinearAlgebra.Double.Vector.Build.Dense(4);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the x coordinate. </summary>
        ///
        /// <value> The x coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double X {
            get { return v[0]; }
            set { v[0] = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the y coordinate. </summary>
        ///
        /// <value> The y coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Y {
            get { return v[1]; }
            set { v[1] = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the z coordinate. </summary>
        ///
        /// <value> The z coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Z {
            get { return v[2]; }
            set { v[2] = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the w. </summary>
        ///
        /// <value> The w coordinate. </value>
        ///-------------------------------------------------------------------------------------------------

        public double W {
            get { return v[3]; }
            set { v[3] = value; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="x1">   The first x value. </param>
        /// <param name="y1">   The first y value. </param>
        /// <param name="z1">   The first z value. </param>
        /// <param name="w1">   (Optional) The first w value. </param>
        ///-------------------------------------------------------------------------------------------------

        public Tuple(double x1, double y1, double z1, double w1 = 1.0) {
            v[0] = x1;
            v[1] = y1;
            v[2] = z1;
            v[3] = w1;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Tuple() {
            v[0] = 0.0;
            v[1] = 0.0;
            v[2] = 0.0;
            v[3] = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="v1">   The Vector to create this tuple from. </param>
        ///-------------------------------------------------------------------------------------------------

        public Tuple(MathNet.Numerics.LinearAlgebra.Double.Vector v1) {
            v[0] = v1[0];
            v[1] = v1[1];
            v[2] = v1[2];
            v[3] = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a Tuple. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Tuple to add. </param>
        ///
        /// <returns>   A Tuple. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Tuple Add(Tuple a) {
            Tuple r = new Tuple();
            v.Add(a.v, r.v);
            return r;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Subtracts the given Tuple. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    Tuple to subtract. </param>
        ///
        /// <returns>   A Tuple. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Tuple Subtract(Tuple a) {
            Tuple r = new Tuple();
            v.Subtract(a.v, r.v);
            return r;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Query if 'a' is equal to this. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="a">    a Tuple to compare. </param>
        ///
        /// <returns>   True if equal, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool IsEqual(Tuple a) => Ops.Equals(X, a.X) && Ops.Equals(Y, a.Y) && Ops.Equals(Z, a.Z) ;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this Tuple is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="obj">  The tuple to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public  bool Equals(Tuple obj) => IsEqual(obj);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a string that represents the current object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A string that represents the current object. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override String ToString() => v.ToString();
    }
}
