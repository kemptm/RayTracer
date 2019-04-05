///-------------------------------------------------------------------------------------------------
// file:	SmoothTriangleTest.cs
//
// summary:	Implements the smooth triangle test class
///-------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Unit Test Class) smooth triangle tests. </summary>
    ///
    /// <remarks>   Kemp, 11/21/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class SmoothTriangleTest
    {
        protected SmoothTriangle st; 

        public SmoothTriangleTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
        [TestInitialize()]
        public void MyTestInitialize() {
            st = new SmoothTriangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0),
                new Vector(0, 1, 0), new Vector(-1, 0, 0), new Vector(1, 0, 0));
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) creates smooth triangle. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateSmoothTriangle() {
            Assert.IsTrue(st.V0.Equals(new Point(0, 1, 0)));
            Assert.IsTrue(st.V1.Equals(new Point(-1, 0, 0)));
            Assert.IsTrue(st.V2.Equals(new Point(1, 0, 0)));
            Assert.IsTrue(st.N0.Equals(new Point(0, 1, 0)));
            Assert.IsTrue(st.N1.Equals(new Point(-1, 0, 0)));
            Assert.IsTrue(st.N2.Equals(new Point(1, 0, 0)));
            Assert.IsTrue(st.T0 == null);
            Assert.IsTrue(st.T1 == null);
            Assert.IsTrue(st.T2 == null);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersection with U and V. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectionWithUV() {
            Triangle s = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            Intersection i = new Intersection(3.5, s, 0.2, 0.4);
            Assert.IsTrue(i.U == 0.2);
            Assert.IsTrue(i.V == 0.4);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) populates U and V on intersection. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PopulateUVonIntersection() {
            Ray r = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            List<Intersection> xs = st.LocalIntersect(r);
            Assert.IsTrue(Ops.Equals(xs[0].U, 0.45));
            Assert.IsTrue(Ops.Equals(xs[0].V, 0.25));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) normal interpolation. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NormalInterpolation() {
            Intersection i = new Intersection(1, st, 0.45, 0.25);
            Vector n = st.NormalAt(new Point(0, 0, 0), i);
            Assert.IsTrue(n.Equals(new Vector(-0.5547, 0.83205, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) prepare normal on a smooth triangle. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PrepareNormalOnASmoothTriangle() {
            Intersection i = new Intersection(1, st, 0.45, 0.25);
            Ray r = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            List<Intersection> xs = Intersection.Intersections(i);
            Intersection hit = i.Prepare(r, xs);
            Assert.IsTrue(hit.Normalv.Equals(new Vector(-0.5547, 0.83205, 0)));
        }
    }
}
