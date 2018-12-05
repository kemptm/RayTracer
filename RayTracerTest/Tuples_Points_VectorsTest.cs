///-------------------------------------------------------------------------------------------------
// file:	Tuples_Points_VectorsTest.cs
//
// summary:	Implements the tuples points vectors test class
///-------------------------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;


namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Unit Test Class) tuples points and vectors tests. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class Tuples_Points_VectorsTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) instance tuple as a point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void InstanceTupleAsAPoint() {
            RayTracerLib.Tuple a = new RayTracerLib.Tuple(4.3, -4.2, 3.1, 1.0);
            Assert.IsFalse(a is Point);
            Assert.IsFalse(a is Vector);
            Vector v = new Vector(a);
            Assert.IsTrue(v is Vector);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) instance tuple as a vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void InstanceTupleAsAVector() {
            RayTracerLib.Tuple a = new RayTracerLib.Tuple(4.3, -4.2, 3.1, 0.0);
            Assert.IsFalse(a is Point);
            Assert.IsFalse(a is Vector);
            Point p = new Point(a);
            Assert.IsTrue(p is Point);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) instance vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void InstanceVector() {
            RayTracerLib.Tuple a = new Vector(4.3, -4.2, 3.1);
            Assert.IsFalse(a is Point);
            Assert.IsTrue(a is Vector);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) instance point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void InstancePoint() {
            RayTracerLib.Tuple a = new Point(4.3, -4.2, 3.1);
            Assert.IsTrue(a is Point);
            Assert.IsFalse(a is Vector);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) adds tuples. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddTuples() {
            RayTracerLib.Tuple a1 = new RayTracerLib.Tuple(3, -2, 5);
            RayTracerLib.Tuple a2 = new RayTracerLib.Tuple(-2, 3, 1);
            RayTracerLib.Tuple a1a2 = a1.Add(a2);
            Assert.IsTrue(Ops.Equals(a1a2.X, 1.0) && Ops.Equals(a1a2.Y, 1.0) && Ops.Equals(a1a2.Z, 6.0) );
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) adds vector to point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddVectorToPoint() {
            Point a1 = new Point(3, -2, 5);
            Vector a2 = new Vector(-2, 3, 1);
            Point a1a2 = a1.Add(a2);
            Assert.IsTrue(a1a2.IsEqual(new Point(1, 1, 6)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) adds point to vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddPointToVector() {
            Vector a1 = new Vector(3, -2, 5);
            Point a2 = new Point(-2, 3, 1);
            Point a1a2 = a1.Add(a2);
            Assert.IsTrue(a1a2.IsEqual(new Point(1, 1, 6)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) subtract points. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SubtractPoints() {
            Point a1 = new Point(3, 2, 1);
            Point a2 = new Point(5, 6, 7);
            RayTracerLib.Tuple a1a2 = a1.Subtract(a2);
            Assert.IsTrue(a1a2.IsEqual(new Vector(-2.0, -4.0, -6.0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) subtract vector from point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SubtractVectorFromPoint() {
            Point a1 = new Point(3, 2, 1);
            Vector a2 = new Vector(5, 6, 7);
            RayTracerLib.Tuple a1a2 = a1.Subtract(a2);
            Assert.IsTrue(a1a2.IsEqual(new Point(-2.0, -4.0, -6.0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) subtract vectors. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SubtractVectors() {
            Vector a1 = new Vector(3, 2, 1);
            Vector a2 = new Vector(5, 6, 7);
            RayTracerLib.Tuple a1a2 = a1.Subtract(a2);
            Assert.IsTrue(a1a2.IsEqual(new Vector(-2.0, -4.0, -6.0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) negate tuple. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NegateTuple() {
            Vector zero = new Vector(0, 0, 0);
            Vector v = new Vector(1, -2, 3);
            RayTracerLib.Tuple v1 = zero.Subtract(v);
            Assert.IsTrue(v1.IsEqual(new Vector(-1, 2, -3)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) negate vector operator. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NegateVectorOperator() {
            Vector v = new Vector(1, -2, 3);
            Assert.IsTrue((-v).IsEqual(new Vector(-1, 2, -3)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector multiply by scalar. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMultiplyByScalar() {
            Vector v = new Vector(1, -2, 3);
            Assert.IsTrue((v * 3.5).IsEqual(new Vector(3.5, -7, 10.5)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector multiply by fractionr. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMultiplyByFractionr() {
            Vector v = new Vector(1, -2, 3);
            Assert.IsTrue((v * 0.5).IsEqual(new Vector(0.5, -1, 1.5)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector divide by fractionr. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorDivideByFractionr() {
            Vector v = new Vector(1, -2, 3);
            Assert.IsTrue((v / 2.0).IsEqual(new Vector(0.5, -1, 1.5)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector magnitude in x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMagnitudeInX() {
            Vector v = new Vector(1, 0, 0);
            Assert.IsTrue(v.Magnitude() == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector magnitude in y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMagnitudeInY() {
            Vector v = new Vector(0, 1, 0);
            Assert.IsTrue(v.Magnitude() == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector magnitude in z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMagnitudeInZ() {
            Vector v = new Vector(0, 0, 1);
            Assert.IsTrue(v.Magnitude() == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector magnitude positive. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMagnitudePositive() {
            Vector v = new Vector(1, 2, 3);
            Assert.IsTrue(v.Magnitude() == Math.Sqrt(14.0));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vector magnitude negative. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VectorMagnitudeNegative() {
            Vector v = new Vector(-1, -2, -3);
            Assert.IsTrue(v.Magnitude() == Math.Sqrt(14.0));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) normalize 1. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Normalize1() {
            Vector v = new Vector(4, 0, 0);
            Assert.IsTrue(v.Normalize().IsEqual( new Vector(1, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) normalize 2. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Normalize2() {
            Vector v = new Vector(1, 2, 3);
            Assert.IsTrue(v.Normalize().IsEqual(new Vector(0.26726, 0.53452, 0.80178)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) normalized magnitude. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NormalizedMagnitude() {
            Vector v = new Vector(1, 2, 3);
            Assert.IsTrue(v.Normalize().Magnitude() == 1.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) dot product. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DotProduct() {
            Vector a = new Vector(1, 2, 3);
            Vector b = new Vector(2, 3, 4);
            Assert.IsTrue(a.Dot(b) == 20.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cross product. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CrossProduct() {
            Vector a = new Vector(1, 2, 3);
            Vector b = new Vector(2, 3, 4);
            Assert.IsTrue(a.Cross(b).IsEqual(new Vector(-1, 2, -1)));
            Assert.IsTrue(b.Cross(a).IsEqual(new Vector(1, -2, 1)));

        }
    }
}
