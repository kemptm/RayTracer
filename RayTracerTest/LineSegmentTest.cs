///-------------------------------------------------------------------------------------------------
// file:	LineSegmentTest.cs
//
// summary:	Implements the line segment test class
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
    /// <summary>   Summary description for LineSegments. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class LineSegmentTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a teststruct. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        class teststruct
        {
            /// <summary>   The origin. </summary>
            public Point Origin;
            /// <summary>   The direction. </summary>
            public RayTracerLib.Vector Direction;
            /// <summary>   Number of. </summary>
            public double Count;
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
                Count = 0;
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
            /// <param name="c">    A double to process. </param>
            /// <param name="t1">   (Optional) The first double. </param>
            /// <param name="t2">   (Optional) The second double. </param>
            ///-------------------------------------------------------------------------------------------------

            public teststruct(Point o, RayTracerLib.Vector d, double c, double t1 = 0, double t2 = 0) {
                Origin = o;
                Direction = d;
                Count = c;
                T1 = t1;
                T2 = t2;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public LineSegmentTest() {
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
        /// <summary>   (Unit Test Method) line segment instance. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void LSInstance() {
            LineSegment l = new LineSegment();
            Assert.IsTrue(l.PLo == -double.MaxValue);
            Assert.IsTrue(l.PHi == double.MaxValue);
            Assert.IsTrue(l.Origin.Equals(new Point(0, -1, 0)));
            Assert.IsTrue(l.Direction.Equals(new RayTracerLib.Vector(0, 1, 0)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) line segment ray intersects. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void LSRayIntersects() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0,0,-1), new RayTracerLib.Vector(0,0,1),1),
                new teststruct(new Point(0,0,0),new RayTracerLib.Vector(0,0,1),1),
                new teststruct(new Point(0.5,0,0.5),new RayTracerLib.Vector(-1,0.5,-1),1)
            };
            LineSegment l = new LineSegment();
            List<Intersection> xs;
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                xs = l.LocalIntersect(r);
                Assert.IsTrue(xs.Count == tx.Count);
                xs = l.Intersect(r);
                Assert.IsTrue(xs.Count == tx.Count);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) line segment ray misses. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void LSRayMisses() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0,0,-1), new RayTracerLib.Vector(0,0,-1),0),
                new teststruct(new Point(0,0,0),new RayTracerLib.Vector(0,1,0),0),
                new teststruct(new Point(0.5,0,0.5),new RayTracerLib.Vector(1,0.5,-1),0)
            };
            LineSegment l = new LineSegment();
            List<Intersection> xs;
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                xs = l.LocalIntersect(r);
                Assert.IsTrue(xs.Count == 0);
                xs = l.Intersect(r);
                Assert.IsTrue(xs.Count == 0);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) line segment ray capped intersects. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void LSRayCappedIntersects() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0,0,-1), new RayTracerLib.Vector(0,0,1),1),
                new teststruct(new Point(0,0,0),new RayTracerLib.Vector(0,0,1),1),
                new teststruct(new Point(0.5,0,0.5),new RayTracerLib.Vector(-1,0.5,-1),1)
            };
            LineSegment l = new LineSegment();
            l.PLo = -1;
            l.PHi = 1;
            List<Intersection> xs;
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                xs = l.LocalIntersect(r);
                Assert.IsTrue(xs.Count != 0);
                xs = l.Intersect(r);
                Assert.IsTrue(xs.Count != 0);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) line segment ray capped misses. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void LSRayCappedMisses() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0,0,-1), new RayTracerLib.Vector(0,0,-1),0),
                new teststruct(new Point(0,2,0),new RayTracerLib.Vector(1,1,0),0),
                new teststruct(new Point(0,0,0),new RayTracerLib.Vector(0,1,0),0),
                new teststruct(new Point(0.5,0,0.5),new RayTracerLib.Vector(1,0.5,-1),0)
            };
            LineSegment l = new LineSegment();
            l.PLo = -1;
            l.PHi = 1;
            List<Intersection> xs;
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                xs = l.LocalIntersect(r);
                Assert.IsTrue(xs.Count == 0);
                xs = l.Intersect(r);
                Assert.IsTrue(xs.Count == 0);
            }
        }
    }
}
