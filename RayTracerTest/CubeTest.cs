///-------------------------------------------------------------------------------------------------
// file:	CubeTest.cs
//
// summary:	Implements the cube test class
///-------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Unit Test Class) a cube test. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class CubeTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a tests tructure. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        class teststruct
        {
            /// <summary>   The origin. </summary>
            public Point Origin;
            /// <summary>   The direction. </summary>
            public Vector Direction;
            /// <summary>   The first t. </summary>
            public double T1;
            /// <summary>   The second t. </summary>
            public double T2;

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Default constructor. </summary>
            ///
            /// <remarks>   Kemp, 12/2/2018. </remarks>
            ///-------------------------------------------------------------------------------------------------

            public teststruct() {
                Origin = null;
                Direction = null;
                T1 = 0;
                T2 = 0;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Constructor. </summary>
            ///
            /// <remarks>   Kemp, 12/2/2018. </remarks>
            ///
            /// <param name="o">    A Point to process. </param>
            /// <param name="d">    A Vector to process. </param>
            /// <param name="t1">   (Optional) The first double. </param>
            /// <param name="t2">   (Optional) The second double. </param>
            ///-------------------------------------------------------------------------------------------------

            public teststruct(Point o, Vector d, double t1=0, double t2=0) {
                Origin = o;
                Direction = d;
                T1 = t1;
                T2 = t2;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CubeTest() {
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
        /// <summary>   (Unit Test Method) cube intersected by ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CubeIntersectedByRay() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct( new Point(5, 0.5, 0), new Vector(-1, 0, 0), 4, 6),
                new teststruct( new Point(-5, 0.5, 0), new Vector(1, 0, 0), 4, 6),
                new teststruct( new Point(0.5, 5, 0), new Vector(0, -1, 0), 4, 6),
                new teststruct( new Point(0.5, -5, 0), new Vector(0, 1, 0), 4, 6),
                new teststruct( new Point(0.5, 0, 5), new Vector(0, 0, -1), 4, 6),
                new teststruct( new Point(0.5, 0, -5), new Vector(0, 0, 1), 4, 6),
                new teststruct( new Point(0, 0.5, 0), new Vector(0, 0, 1), -1, 1)
            };
            Cube cube = new Cube();

            foreach (teststruct ts in tests) {
                Ray ray = new Ray(ts.Origin, ts.Direction);
                List<Intersection> xs = cube.LocalIntersect(ray);
                Assert.IsTrue((xs[0].T == ts.T1) && (xs[1].T == ts.T2));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cube missed by ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CubeMissedByRay() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct( new Point(-2, 0, 0), new Vector(0.2673, 0.5345, 0.0018)),
                new teststruct( new Point(0, -2, 0), new Vector(0.0018, 0.2673, 0.5345)),
                new teststruct( new Point(0, 0, -2), new Vector(0.5345, 0.0018, 0.2673)),
                new teststruct( new Point(2, 0, 2), new Vector(0, 0, -1)),
                new teststruct( new Point(0, 2, 2), new Vector(0, -1, 0)),
                new teststruct( new Point(2, 2, 0), new Vector(-1, 0, 0))
            };
            Cube cube = new Cube();

            foreach (teststruct ts in tests) {
                Ray ray = new Ray(ts.Origin, ts.Direction);
                List<Intersection> xs = cube.LocalIntersect(ray);
                Assert.IsTrue(xs.Count == 0);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cube normal. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CubeNormal() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct( new Point(1, 0.5, -0.8), new Vector(1, 0, 0)),
                new teststruct( new Point(-1, -0.2, 0.9), new Vector(-1, 0, 0)),
                new teststruct( new Point(-0.4, 1, -0.1), new Vector(0, 1, 0)),
                new teststruct( new Point(0.3, -1, -0.7), new Vector(0, -1, 0)),
                new teststruct( new Point(-0.6, 0.3, 1), new Vector(0, 0, 1)),
                new teststruct( new Point(0.4, 0.4, -1), new Vector(0, 0, -1)),
                new teststruct( new Point(1, 1, 1), new Vector(1, 0, 0)),
                new teststruct( new Point(-1, -1, -1), new Vector(-1, 0, 0)),
           };
            Cube cube = new Cube();

            foreach (teststruct ts in tests) {
                Vector norm = cube.LocalNormalAt(ts.Origin);
                Assert.IsTrue(norm.Equals(ts.Direction));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cube transformed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CubeTransformed() {
            Cube cube = new Cube();
            cube.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            Ray r = new Ray(new Point(5, 0, -5), new Vector(0, 0, 1).Normalize());
            List<Intersection> xs = cube.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cube rotated (y coordinate). </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CubeRotatedY() {
            Cube cube = new Cube();
            cube.Transform = MatrixOps.CreateRotationYTransform(Math.Sqrt(2));
            Ray r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1).Normalize());
            List<Intersection> xs = cube.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
        }
    }
}
