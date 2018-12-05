///-------------------------------------------------------------------------------------------------
// file:	MatrixTransformationsTest.cs
//
// summary:	Implements the matrix transformations test class
///-------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   ffffffffffffffffffffffffffffffffffffffffffffffMatrixTransformations tests. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class MatrixTransformationsTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public MatrixTransformationsTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>   The test context instance. </summary>
        private TestContext testContextInstance;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the test context which provides information about and functionality for the
        ///     current test run.
        /// </summary>
        ///
        /// <value> The test context. </value>
        ///-------------------------------------------------------------------------------------------------

        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply by translation matrix. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyByTranslationMatrix() {
            Matrix t = MatrixOps.CreateTranslationTransform(5, -3, 2);
            Point p = new Point(-3, 4, 5);
            Assert.IsTrue(p.Transform(t).Equals(new Point(2, 1, 7)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply by inverse translation matrix. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyByInverseTranslationMatrix() {
            Matrix t = MatrixOps.CreateTranslationTransform(5, -3, 2);
            Point p = new Point(-3, 4, 5);
            Assert.IsTrue(p.Transform((Matrix)t.Inverse()).Equals(new Point(-8, 7, 3)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) no translate vectors. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NoTranslateVectors() {
            Matrix t = MatrixOps.CreateTranslationTransform(5, -3, 2);
            RayTracerLib.Vector v = new RayTracerLib.Vector(-3, 4, 5);
            Assert.IsTrue(v.Transform(t).Equals(v));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) scale a point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScaleAPoint() {
            Matrix t = MatrixOps.CreateScalingTransform(2, 3, 4);
            Point p = new Point(-4, 6, 8);
            Assert.IsTrue(p.Transform(t).Equals(new Point(-8, 18, 32)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) scale a vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScaleAVector() {
            Matrix t = MatrixOps.CreateScalingTransform(2, 3, 4);
            RayTracerLib.Vector v = new RayTracerLib.Vector(-4, 6, 8);
            Assert.IsTrue(v.Transform(t).Equals(new RayTracerLib.Vector(-8, 18, 32)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) scale a vector by inverse. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ScaleAVectorByInverse() {
            Matrix t = MatrixOps.CreateScalingTransform(2, 3, 4);
            RayTracerLib.Vector v = new RayTracerLib.Vector(-4, 6, 8);
            Assert.IsTrue(v.Transform((Matrix)t.Inverse()).Equals(new RayTracerLib.Vector(-2, 2, 2)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) reflect a point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ReflectAPoint() {
            Matrix t = MatrixOps.CreateScalingTransform(-1, 1, 1);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(-2, 3, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) rotate around x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RotateAroundX() {
            Point p = new Point(0, 1, 0);
            Matrix hq = MatrixOps.CreateRotationXTransform(Math.PI / 4.0);
            Matrix fq = MatrixOps.CreateRotationXTransform(Math.PI / 2.0);
            Assert.IsTrue(p.Transform(hq).Equals(new Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2)));
            Assert.IsTrue(p.Transform(fq).Equals(new Point(0, 0, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) rotate around x coordinate inverse. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RotateAroundXInverse() {
            Point p = new Point(0, 1, 0);
            Matrix hq = MatrixOps.CreateRotationXTransform(Math.PI / 4.0);
            Matrix inv = (Matrix)hq.Inverse();
            Assert.IsTrue(p.Transform(inv).Equals(new Point(0, Math.Sqrt(2) / 2, -(Math.Sqrt(2) / 2))));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) rotate around y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RotateAroundY() {
            Point p = new Point(0, 0, 1);
            Matrix hq = MatrixOps.CreateRotationYTransform(Math.PI / 4.0);
            Matrix fq = MatrixOps.CreateRotationYTransform(Math.PI / 2.0);
            Assert.IsTrue(p.Transform(hq).Equals(new Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2)));
            Assert.IsTrue(p.Transform(fq).Equals(new Point(1, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) rotate around y coordinate negative point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RotateAroundYNegativePoint() {
            Point p = new Point(0, 0, -1);
            Matrix hq = MatrixOps.CreateRotationYTransform(Math.PI / 4.0);
            Matrix fq = MatrixOps.CreateRotationYTransform(Math.PI / 2.0);
            Point phq = p.Transform(hq);
            Point pfq = p.Transform(fq);
            Assert.IsTrue(p.Transform(hq).Equals(new Point(-Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2)));
            Assert.IsTrue(p.Transform(fq).Equals(new Point(-1, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) rotate around z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RotateAroundZ() {
            Point p = new Point(0, 1, 0);
            Matrix hq = MatrixOps.CreateRotationZTransform(Math.PI / 4.0);
            Matrix fq = MatrixOps.CreateRotationZTransform(Math.PI / 2.0);
            Assert.IsTrue(p.Transform(hq).Equals(new Point(-(Math.Sqrt(2) / 2), Math.Sqrt(2) / 2, 0)));
            Assert.IsTrue(p.Transform(fq).Equals(new Point(-1, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shearing x coordinate 2 y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShearingX2Y() {
            Matrix t = MatrixOps.CreateShearingTransform(1, 0, 0, 0, 0, 0);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(5, 3, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shearing x coordinate 2 z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShearingX2Z() {
            Matrix t = MatrixOps.CreateShearingTransform(0, 1, 0, 0, 0, 0);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(6, 3, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shearing y coordinate 2 x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShearingY2X() {
            Matrix t = MatrixOps.CreateShearingTransform(0, 0, 1, 0, 0, 0);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(2, 5, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shearing y coordinate 2 z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShearingY2Z() {
            Matrix t = MatrixOps.CreateShearingTransform(0, 0, 0, 1, 0, 0);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(2, 7, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shearing z coordinate 2 x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShearingZ2X() {
            Matrix t = MatrixOps.CreateShearingTransform(0, 0, 0, 0, 1, 0);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(2, 3, 6)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shearing z coordinate 2 y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShearingZ2Y() {
            Matrix t = MatrixOps.CreateShearingTransform(0, 0, 0, 0, 0, 1);
            Point p = new Point(2, 3, 4);
            Assert.IsTrue(p.Transform(t).Equals(new Point(2, 3, 7)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) individual transformations. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IndividualTransformations() {
            Point p = new Point(1, 0, 1);
            Matrix a = MatrixOps.CreateRotationXTransform(Math.PI / 2.0);
            Matrix b = MatrixOps.CreateScalingTransform(5, 5, 5);
            Matrix c = MatrixOps.CreateTranslationTransform(10, 5, 7);

            Point p2 = p.Transform(a);
            Assert.IsTrue(p2.Equals(new Point(1, -1, 0)));

            Point p3 = p2.Transform(b);
            Assert.IsTrue(p3.Equals(new Point(5, -5, 0)));

            Point p4 = p3.Transform(c);
            Assert.IsTrue(p4.Equals(new Point(15, 0, 7)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) chained transformations. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ChainedTransformations() {
            Point p = new Point(1, 0, 1);
            Matrix a = MatrixOps.CreateRotationXTransform(Math.PI / 2.0);
            Matrix b = MatrixOps.CreateScalingTransform(5, 5, 5);
            Matrix c = MatrixOps.CreateTranslationTransform(10, 5, 7);

            Matrix t = (Matrix)(c * b * a);
            Assert.IsTrue(p.Transform(t).Equals(new Point(15, 0, 7)));

        }
    }
}