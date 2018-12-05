///-------------------------------------------------------------------------------------------------
// file:	CylinderTest.cs
//
// summary:	Implements the cylinder test class
///-------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Unit Test Class) a cylinder test. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class CylinderTest {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a teststruct. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        class teststruct {
            /// <summary>   The origin. </summary>
            public Point Origin;
            /// <summary>   The direction. </summary>
            public Vector Direction;
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

            public teststruct(Point o, Vector d, double c, double t1 = 0, double t2 = 0) {
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

        public CylinderTest() {
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
        /// <summary>   (Unit Test Method) cylinder transformed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderTransformed() {
            Cylinder cyl = new Cylinder();
            cyl.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            Ray r = new Ray(new Point(5, 0, -5), new Vector(0, 0, 1).Normalize());
            List<Intersection> xs = cyl.Intersect(r);
            Assert.IsTrue(xs.Count == 2);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder missed by ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderMissedByRay() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(1,0,0), new Vector(0,1,0),0),
                new teststruct(new Point(0,0,0),new Vector(0,1,0),0),
                new teststruct(new Point(0,0,-5),new Vector(1,1,1),0)
            };

            Cylinder cyl = new Cylinder();

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray r = new Ray(tx.Origin, dir);
                List<Intersection> xs = cyl.LocalIntersect(r);
                Assert.IsTrue(xs.Count == 0);
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder intersected by ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderIntersectedByRay() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(1, 0, -5), new Vector(0, 0, 1),2, 5, 5),
                new teststruct(new Point(0, 0, -5), new Vector(0, 0, 1), 2, 4, 6),
                new teststruct(new Point(0.5, 0, -5), new Vector(0.1, 1, 1), 2, 6.80798, 7.08872)
            };

            Cylinder cyl = new Cylinder();

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray r = new Ray(tx.Origin, dir);
                List<Intersection> xs = cyl.LocalIntersect(r);
                Assert.IsTrue(xs.Count == 2);
                Assert.IsTrue(Ops.Equals(xs[0].T, tx.T1));
                Assert.IsTrue(Ops.Equals(xs[1].T, tx.T2));
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder normal. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderNormal() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(1, 0, 0), new Vector(1, 0, 0),0),
                new teststruct(new Point(0, 5, -1), new Vector(0, 0, -1),0),
                new teststruct(new Point(0, -2, 1), new Vector(0, 0, 1),0),
                new teststruct(new Point(-1, 1, 0), new Vector(-1, 0, 0),0)
            };

            Cylinder cyl = new Cylinder();

            foreach (teststruct tx in tests) {
                Vector n = cyl.LocalNormalAt(tx.Origin);
                Assert.IsTrue(n.Equals(tx.Direction));
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder default y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderDefaultY() {
            Cylinder cyl = new Cylinder();
            Assert.IsTrue(cyl.MinY == -double.MaxValue);
            Assert.IsTrue(cyl.MaxY == double.MaxValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder truncated. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderTruncated() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point( 0, 2.5,  0), new Vector( 0.1, 1, 0), 0),
                new teststruct(new Point( 0, 4,   -5), new Vector( 0,   0, 1), 0),
                new teststruct(new Point( 0, 1,   -5), new Vector( 0,   0, 1), 0),
                new teststruct(new Point( 0, 3,   -5), new Vector( 0,   0, 1), 0),
                new teststruct(new Point( 0, 1,   -5), new Vector( 0,   0, 1), 0),
                new teststruct(new Point( 0, 1.5, -2), new Vector( 0,   0, 1), 2),
                new teststruct(new Point( 0, 5,   -2),new Vector(0,   -1,  1), 1),
           };

            Cylinder cyl = new Cylinder();
            cyl.MinY = 1;
            cyl.MaxY = 3;

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray ray = new Ray(tx.Origin, dir);
                List<Intersection> xs = cyl.LocalIntersect(ray);
                Assert.IsTrue(xs.Count == tx.Count);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder default not cclosed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderDefaultNotCclosed() {
            Cylinder cyl = new Cylinder();
            Assert.IsFalse(cyl.Closed);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder intersect capabilities. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderIntersectCaps() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 3, 0), new Vector(0, -1, 0), 0),
                new teststruct(new Point(0, 3, -2), new Vector(0, -1, 2), 2),
                new teststruct(new Point(0, 4, -2), new Vector(0, -1, 1), 2), // corner case
                new teststruct(new Point(0, 0, -2), new Vector(0, 1, 2), 2),
                new teststruct(new Point(0, -1, -2), new Vector(0, 1, 1), 2), // corner case
           };

            Cylinder cyl = new Cylinder();
            cyl.MinY = 1;
            cyl.MaxY = 2;
            cyl.Closed = true;

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray ray = new Ray(tx.Origin, dir);
                List<Intersection> xs = cyl.LocalIntersect(ray);
                Assert.IsTrue(xs.Count == tx.Count);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder capability normal. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderCapNormal() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0,   1,  0),   new Vector(0, -1, 0),0),
                new teststruct(new Point(0.5, 1,  0),   new Vector(0, -1, 0),0),
                new teststruct(new Point(0,   1, -0.5), new Vector(0, -1, 0),0),
                new teststruct(new Point(0,   2,  0),   new Vector(0, 1,  0),0),
                new teststruct(new Point(0.5, 2,  0),   new Vector(0, 1,  0),0),
                new teststruct(new Point(0,   2,  0.5), new Vector(0, 1,  0),0),
                new teststruct(new Point(1, 2.5,  0.5), new Vector(1, 0,  0.5),0),
           };

            Cylinder cyl = new Cylinder();
            cyl.MinY = 1;
            cyl.MaxY = 2;
            cyl.Closed = true;

            foreach (teststruct tx in tests) {
                Vector n = cyl.LocalNormalAt(tx.Origin);
                Assert.IsTrue(n.Equals(tx.Direction));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder open bounds. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderOpenBounds() {
            Cylinder c = new Cylinder();
            Bounds b = c.LocalBounds();
            Assert.IsTrue(b.MinCorner.Equals(new Point(-1, -double.MaxValue, -1)));
            Assert.IsTrue(b.MaxCorner.Equals(new Point(1, double.MaxValue, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cylinder closed bounds. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CylinderClosedBounds() {
            Cylinder c = new Cylinder();
            c.MinY = -1;
            c.MaxY = 1;
            c.Closed = true;
            Bounds b = c.LocalBounds();
            Assert.IsTrue(b.MinCorner.Equals(new Point(-1, -1, -1)));
            Assert.IsTrue(b.MaxCorner.Equals(new Point(1, 1, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone missed by ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeMissedByRay() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(1, 0, 0), new Vector(0, 0, 1), 0),
                // new teststruct(new RTPoint(1, 0, 1), new RTVector(0, 1, 1), 0),
                new teststruct(new Point(0, 0,-5), new Vector(1, 0, 1), 0)
            };

            Cone cone = new Cone();

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray r = new Ray(tx.Origin, dir);
                List<Intersection> xs = cone.LocalIntersect(r);
                Assert.IsTrue(xs.Count == 0);
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone intersected by ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeIntersectedByRay() {
            Point p1 = MatrixOps.CreateRotationYTransform(Math.PI / 3) * new Point(2, 0, 0);
            Vector r1 = MatrixOps.CreateRotationYTransform(Math.PI / 3) * new Vector(-1, -1, 0);
            List<teststruct> tests = new List<teststruct> {
            /* 6 */    new teststruct(p1, r1, 0),
            /* 1 */    new teststruct(new Point(2, 0,  0), new Vector(-1, -1, 0), 0),
            /* 3 */    new teststruct(new Point(0, 1, -5), new Vector(0, 0, 1), 2, 4, 6),
            /* 4 */    new teststruct(new Point(1, 0,  1), new Vector(1, 1, 1), 2, -5.91359, -1.01461),
            /* 5 */    new teststruct(new Point(2, 1,  2), new Vector(0, 1, 1), 0),
            /* 7 */    new teststruct(new Point(0, 0,  1), new Vector(0, 1, 1), 0),
            /* 8 */    new teststruct(new Point(0, 0,  0), new Vector(1, 1, 0), 0),
            /* 9 */    new teststruct(new Point(0, 0,  0), new Vector(1, 0, 1), 2, 0, 0),
            /* 10 */   new teststruct(new Point(0, 0, -2), new Vector(0, 1, 1), 0)
            };

            Cone cone = new Cone();


            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray r = new Ray(tx.Origin, dir);
                List<Intersection> xs = cone.LocalIntersect(r);
                Assert.IsTrue(xs.Count == tx.Count);
                if (xs.Count >= 1) Assert.IsTrue( Ops.Equals(xs[0].T, tx.T1));
                if (xs.Count == 2) Assert.IsTrue( Ops.Equals(xs[1].T, tx.T2));
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone normal. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeNormal() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(1, 1.41421, 1), new Vector(1, -1.41421, 1),0),
                new teststruct(new Point(0, 5, 5), new Vector(0, -5, 5),0),
                new teststruct(new Point(2, -2, 0), new Vector(2, 2, 0),0),
                new teststruct(new Point(-1, 1, 0), new Vector(-1, -1, 0),0)
            };

            Cone cone = new Cone();

            foreach (teststruct tx in tests) {
                Vector n = cone.LocalNormalAt(tx.Origin);
                Assert.IsTrue(n.Equals(tx.Direction));
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone default y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeDefaultY() {
            Cone cyl = new Cone();
            Assert.IsTrue(cyl.MinY == -double.MaxValue);
            Assert.IsTrue(cyl.MaxY == double.MaxValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone truncated. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeTruncated() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 1.5, 0), new Vector(0.1, 1, 0), 0),
                new teststruct(new Point(0, 3, -5), new Vector(0, 0, 1), 0),
                new teststruct(new Point(0, 0, -5), new Vector(0, 0, 1), 0),
                new teststruct(new Point(0, 2, -5), new Vector(0, 0, 1), 0),
                new teststruct(new Point(0, 1, -5), new Vector(0, 0, 1), 0),
                new teststruct(new Point(0, 1.5, -2), new Vector(0, 0, 1), 2),
           };

            Cone cone = new Cone();
            cone.MinY = 1;
            cone.MaxY = 2;

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray ray = new Ray(tx.Origin, dir);
                List<Intersection> xs = cone.LocalIntersect(ray);
                Assert.IsTrue(xs.Count == tx.Count);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cpme default not cclosed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CpmeDefaultNotCclosed() {
            Cone cyl = new Cone();
            Assert.IsFalse(cyl.Closed);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone intersect capabilities. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeIntersectCaps() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 3, 0), new Vector(0, -1, 0), 0),
                new teststruct(new Point(0, 3, -2), new Vector(0, -1, 2), 2),
                new teststruct(new Point(0, 4, -2), new Vector(0, -1, 1), 0), // corner case
                new teststruct(new Point(0, 0, -2), new Vector(0, 1, 2), 2),
                new teststruct(new Point(0, -1, -2), new Vector(0, 1, 1), 0), // corner case
           };

            Cone cone = new Cone();
            cone.MinY = 1;
            cone.MaxY = 2;
            cone.Closed = true;

            foreach (teststruct tx in tests) {
                Vector dir = tx.Direction.Normalize();
                Ray ray = new Ray(tx.Origin, dir);
                List<Intersection> xs = cone.LocalIntersect(ray);
                Assert.IsTrue(xs.Count == tx.Count);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone capability normal. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeCapNormal() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0,   1,  0),   new Vector(0, -1, 0),0),
                new teststruct(new Point(0.5, 1,  0),   new Vector(0, -1, 0),0),
                new teststruct(new Point(0,   1, -0.5), new Vector(0, -1, 0),0),
                new teststruct(new Point(0,   2,  0),   new Vector(0, 1,  0),0),
                new teststruct(new Point(0.5, 2,  0),   new Vector(0, 1,  0),0),
                new teststruct(new Point(0,   2,  0.5), new Vector(0, 1,  0),0),
                new teststruct(new Point(1, 2.5,  0.5), new Vector(0, 1,  0),0),
           };

            Cone cone = new Cone();
            cone.MinY = 1;
            cone.MaxY = 2;
            cone.Closed = true;

            foreach (teststruct tx in tests) {
                Vector n = cone.LocalNormalAt(tx.Origin);
                Assert.IsTrue(n.Equals(tx.Direction));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone transformed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeTransformed() {
            Cone cone = new Cone();
            cone.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            Ray r = new Ray(new Point(5, 1, -5), new Vector(0, 0, 1).Normalize());
            List<Intersection> xs = cone.Intersect(r);
            Assert.IsTrue(xs.Count == 2);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cone closed bounds. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ConeClosedBounds() {
            Cone c = new Cone();
            c.MinY = -1;
            c.MaxY = 1;
            c.Closed = true;
            Bounds b = c.LocalBounds();
            Assert.IsTrue(b.MinCorner.Equals(new Point(-1, -1, -1)));
            Assert.IsTrue(b.MaxCorner.Equals(new Point(1, 1, 1)));
        }

    }
}
