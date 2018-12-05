using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;

namespace RayTracerTest
{
    /// <summary>
    /// Summary description for TriangleTest
    /// </summary>
    [TestClass]
    public class TriangleTest
    {
        public TriangleTest() {
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateTriangle() {
            Point p0 = new Point(0, 1, 0);
            Point p1 = new Point(-1, 0, 0);
            Point p2 = new Point(1, 0, 0);
            Triangle t = new Triangle(p0, p1, p2);

            Assert.IsTrue(t.V0.Equals(p0));
            Assert.IsTrue(t.V1.Equals(p1));
            Assert.IsTrue(t.V2.Equals(p2));

            Assert.IsTrue(t.E0.Equals(new Vector(-1, -1, 0)));
            Assert.IsTrue(t.E1.Equals(new Vector(1, -1, 0)));
            Assert.IsTrue(t.Normal.Equals(new Vector(0, 0, -1)));
        }

        [TestMethod]
        public void NormalVector() {
            Triangle t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            Vector n1 = t.LocalNormalAt(new Point(0, 0.5, 0));
            Vector n2 = t.LocalNormalAt(new Point(-0.5, 0.75, 0));
            Vector n3 = t.LocalNormalAt(new Point(0.5, 0.25, 0));

            Assert.IsTrue(n1.Equals(t.Normal));
            Assert.IsTrue(n2.Equals(t.Normal));
            Assert.IsTrue(n3.Equals(t.Normal));
        }

        [TestMethod]
        public void DefaultTriangle() {
            Triangle t = new Triangle();
            Assert.IsTrue(t.V0.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(t.V0.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(t.V0.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(t.Bounds.Equals(new Bounds(new Point(0, 0, 0), new Point(0, 0, 0))));
        }

        [TestMethod]
        public void SpecifiedTriangle() {
            Triangle t = new Triangle(new Point(1, 1, 1), new Point(2, 2, 2), new Point(3, 3, 3));
            Assert.IsTrue(t.V0.Equals(new Point(1, 1, 1)));
            Assert.IsTrue(t.V1.Equals(new Point(2, 2, 2)));
            Assert.IsTrue(t.V2.Equals(new Point(3, 3, 3)));
            Assert.IsTrue(t.Bounds.Equals(new Bounds(new Point(1, 1, 1), new Point(3, 3, 3))));
        }

        [TestMethod]
        public void IntersectTriangle() {
            Triangle t = new Triangle(new Point(0, 0, 0), new Point(5, 0, 0), new Point(0, 5, 0));
            Ray r = new Ray(new Point(2, 2, -2), new Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 1);
            xs = t.Intersect(r);
            Assert.IsTrue(xs.Count == 1);
        }

        [TestMethod]
        public void MissTriangle() {
            Triangle t = new Triangle(new Point(0, 0, 0), new Point(5, 0, 0), new Point(0, 5, 0));
            Ray r = new Ray(new Point(6, 6, -2), new Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
            xs = t.Intersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        [TestMethod]
        public void MissP1P3() {
            Triangle t = new Triangle(new Point(0,1,0), new Point(-1,0,0), new Point(1,0,0));
            Ray r = new Ray(new Point(1, 1, -2), new Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        [TestMethod]
        public void MissP1P2() {
            Triangle t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            Ray r = new Ray(new Point(-1, 1, -2), new Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        [TestMethod]
        public void MissP2P3() {
            Triangle t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            Ray r = new Ray(new Point(0, -1, -2), new Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        [TestMethod]
        public void Strikes() {
            Triangle t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            Ray r = new Ray(new Point(0, 0.5, -2), new Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 1);
            Assert.IsTrue(xs[0].T == 2);
        }

        [TestMethod]
        public void MissTriangleParallel() {
            Triangle t = new Triangle(new Point(0, 0, 0), new Point(5, 0, 0), new Point(0, 5, 0));
            Ray r = new Ray(new Point(6, 6, -2), new Vector(1, 0, 0));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
            xs = t.Intersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        [TestMethod]
        public void MissTriangleParallel2() {
            Triangle t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            Ray r = new Ray(new Point(0, -1, -2), new Vector(0, 1, 0));
            List<Intersection> xs = t.LocalIntersect(r);

            Assert.IsTrue(xs.Count == 0);
        }

        [TestMethod]
        public void NormalTriangleCCW() {
            Triangle t = new Triangle(new Point(0, 0, 0), new Point(5, 0, 0), new Point(0, 5, 0));
            Assert.IsTrue(t.LocalNormalAt(new Point(0, 0, 0)).Equals(new Vector(0, 0, -1)));
        }

        [TestMethod]
        public void NormalTriangleCW() {
            Triangle t = new Triangle(new Point(0, 0, 0), new Point(0, 5, 0), new Point(5, 0, 0));
            Assert.IsTrue(t.LocalNormalAt(new Point(0, 0, 0)).Equals(new Vector(0, 0, 1)));
        }

        [TestMethod]
        public void DegenerateTriangle() {
            Triangle t = new Triangle(new Point(0, 0, 0), new Point(0, 10, 0), new Point(0, 5, 0));
            Ray r = new Ray(new Point(0, 1, -2), new Vector(0, 1, 1));
            List<Intersection> xs = new List<Intersection>();
            xs = t.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
            xs = t.Intersect(r);
            Assert.IsTrue(xs.Count == 0);
        }
    }
}
