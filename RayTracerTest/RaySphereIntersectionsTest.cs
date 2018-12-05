///-------------------------------------------------------------------------------------------------
// file:	RaySphereIntersectionsTest.cs
//
// summary:	Implements the ray sphere intersections test class
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
    /// <summary>   (Unit Test Class) a ray sphere intersections test. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class RaySphereIntersectionsTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public RaySphereIntersectionsTest() {
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
        /// <summary>   (Unit Test Method) creates the ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateRay() {
            Point origin = new Point(1, 2, 3);
            RayTracerLib.Vector direction = new RayTracerLib.Vector(4, 5, 6);
            Ray r = new Ray(origin, direction);

            Assert.IsTrue(r.Origin.Equals(origin));
            Assert.IsTrue(r.Direction.Equals(direction));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray position. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayPosition() {
            Ray r = new Ray(new Point(2, 3, 4), new RayTracerLib.Vector(1, 0, 0));
            Assert.IsTrue(r.Position(0).Equals(new Point(2, 3, 4)));
            Assert.IsTrue(r.Position(1).Equals(new Point(3, 3, 4)));
            Assert.IsTrue(r.Position(-1).Equals(new Point(1, 3, 4)));
            Assert.IsTrue(r.Position(2.5).Equals(new Point(4.5, 3, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray intersects at 2 points. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayIntersectsAt2Points() {
            Ray r = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
            Assert.IsTrue(xs[0].T == 4);
            Assert.IsTrue(xs[1].T == 6);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray intersets at tangent. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayIntersetsAtTangent() {
            Ray r = new Ray(new Point(0, 1, -5), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
            Assert.IsTrue(xs[0].T == 5);
            Assert.IsTrue(xs[1].T == 5);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray misses. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayMisses() {
            Ray r = new Ray(new Point(0, 2, -5), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray inside sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayInsideSphere() {
            Ray r = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
            Assert.IsTrue(xs[0].T == -1);
            Assert.IsTrue(xs[1].T == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray in front of sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayInFrontOfSphere() {
            Ray r = new Ray(new Point(0, 0, 5), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 2|| xs.Count == 0); // either have negative hits or was eliminated by bounding box
            if (xs.Count == 2) {
                Assert.IsTrue(xs[0].T == -6);
                Assert.IsTrue(xs[1].T == -4);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) creates the intersection. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateIntersection() {
            Sphere s = new Sphere();
            Intersection i = new Intersection(3.5, s);
            Assert.IsTrue(i.T == 3.5);
            Assert.IsTrue(i.Obj == s);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersections this object. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Intersections() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(1, s);
            Intersection i2 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2);
            Assert.IsTrue(xs.Count == 2);
            Assert.IsTrue(xs[0].T == 1);
            Assert.IsTrue(xs[1].T == 2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) hit with all positive. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void HitWithAllPositive() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(1, s);
            Intersection i2 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2);
            Intersection h = Intersection.Hit(xs);
            Assert.IsTrue(h == i1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) hit with some positive. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void HitWithSomePositive() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(-1, s);
            Intersection i2 = new Intersection(1, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2);
            Intersection h = Intersection.Hit(xs);
            Assert.IsTrue(h == i2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) hit with no positive. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void HitWithNoPositive() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(-2, s);
            Intersection i2 = new Intersection(-1, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2);
            Intersection h = Intersection.Hit(xs);
            Assert.IsTrue(h == null);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) hit with lowest positive. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void HitWithLowestPositive() {
            Sphere s = new Sphere();
            Intersection i1 = new Intersection(5, s);
            Intersection i2 = new Intersection(7, s);
            Intersection i3 = new Intersection(-3, s);
            Intersection i4 = new Intersection(2, s);
            List<Intersection> xs = Intersection.Intersections(i1, i2, i3, i4);
            Intersection h = Intersection.Hit(xs);
            Assert.IsTrue(h == i4);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray translate. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayTranslate() {
            Ray r = new Ray(new Point(1, 2, 3), new RayTracerLib.Vector(0, 1, 0));
            Matrix m = MatrixOps.CreateTranslationTransform(3, 4, 5);
            Ray r2 = r.Transform(m);
            Assert.IsTrue(r2.Origin.Equals(new Point(4, 6, 8)));
            Assert.IsTrue(r2.Direction.Equals(new RayTracerLib.Vector(0, 1, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray scale. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayScale() {
            Ray r = new Ray(new Point(1, 2, 3), new RayTracerLib.Vector(0, 1, 0));
            Matrix m = MatrixOps.CreateScalingTransform(2, 3, 4);
            Ray r2 = r.Transform(m);
            Assert.IsTrue(r2.Origin.Equals(new Point(2, 6, 12)));
            Assert.IsTrue(r2.Direction.Equals(new RayTracerLib.Vector(0, 3, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere default transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereDefaultTransformation() {
            Sphere s = new Sphere();
            Assert.IsTrue(s.Transform.Equals(DenseMatrix.CreateIdentity(4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere set transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereSetTransformation() {
            Sphere s = new Sphere();
            Matrix m = MatrixOps.CreateTranslationTransform(2, 3, 4);
            s.Transform = m;
            Assert.IsTrue(s.Transform == m);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere transformed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereTransformed() {
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            Ray r = new Ray(new Point(5, 1, -5), new RayTracerLib.Vector(0, 0, 1).Normalize());
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere translated intersect ray. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereTranslatedIntersectRay() {
            Ray r = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal at non axial point. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalAtNonAxialPoint() {
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateRotationYTransform(Math.PI / 4.0);
            double root3over3 = Math.Sqrt(3.0) / 3.0;
            RayTracerLib.Vector n = s.NormalAt(new Point(root3over3, root3over3, root3over3));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(root3over3, root3over3, root3over3)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere bounds. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereBounds() {
            Sphere c = new Sphere();
            c.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            Bounds b = c.LocalBounds();
            Assert.IsTrue(b.MinCorner.Equals(new Point(-1, -1, -1)));
            Assert.IsTrue(b.MaxCorner.Equals(new Point(1, 1, 1)));
            Assert.IsTrue(c.Bounds.MinCorner.Equals(new Point(-2, -2, -2)));
            Assert.IsTrue(c.Bounds.MaxCorner.Equals(new Point(2, 2, 2)));
        }
    }
}
