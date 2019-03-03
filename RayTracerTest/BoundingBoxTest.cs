///-------------------------------------------------------------------------------------------------
// file:	BoundingBoxTest.cs
//
// summary:	Implements the bounding box test class
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
    /// <summary>   (Unit Test Class) a bounding box test. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class BoundingBoxTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a test structure class. </summary>
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
            /// <param name="o">    A Point to initialize. </param>
            /// <param name="d">    A Vector to initialize. </param>
            /// <param name="c">    A double to initialize. </param>
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
        /// <summary>   (Unit Test Class) a test shape. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public class TestShape : Shape
        {
            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Copy the shape (Virtual). </summary>
            ///
            /// <remarks>   Kemp, 12/2/2018. </remarks>
            ///
            /// <returns>   A Shape. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Shape Copy() {
                throw new NotImplementedException();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
            ///
            /// <remarks>   Kemp, 12/2/2018. </remarks>
            ///
            /// <returns>   The Bounds. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Bounds LocalBounds() {
                return new Bounds(new Point(0, 0, 0), new Point(0, 0, 0));
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Local intersect (abstract). </summary>
            ///
            /// <remarks>   Kemp, 12/2/2018. </remarks>
            ///
            /// <param name="rayparm">  The ray to intersect. </param>
            ///
            /// <returns>   A List&lt;Intersection&gt; </returns>
            ///-------------------------------------------------------------------------------------------------

            public override List<Intersection> LocalIntersect(Ray rayparm) {
                throw new NotImplementedException();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
            ///
            /// <remarks>   Kemp, 12/2/2018. </remarks>
            ///
            /// <param name="localPoint">   The local point. </param>
            ///
            /// <returns>   A Vector. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override RayTracerLib.Vector LocalNormalAt(Point localPoint) {
                throw new NotImplementedException();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public BoundingBoxTest() {
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
        /// <summary>   (Unit Test Method) bounds null. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsNull() {
            Shape s = new TestShape();
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(0, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds of sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsOfSphere() {
            Sphere s = new Sphere();
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(1, 1, 1)));
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(-1, -1, -1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds of transformed sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsOfTransformedSphere() {
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(2, 2, 2)));
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(-2, -2, -2)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) group transformation bounds. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GroupTransformationBounds() {
            Group g = new Group();
            g.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            g.AddObject(s);

            Sphere s1 = new Sphere();
            s1.Transform = MatrixOps.CreateTranslationTransform(0, 0, 5);
            g.AddObject(s1);

            Assert.IsTrue(g.Bounds.MinCorner.Equals(new Point(-2, -2, -2)));
            Assert.IsTrue(g.Bounds.MaxCorner.Equals(new Point(12, 2, 12)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds intersect. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsIntersect() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 0, -1), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(2, 2, 2), new RayTracerLib.Vector(-1, -1, -1), 2),
                new teststruct(new Point(2,2,1), new RayTracerLib.Vector(-1,-1,-0.5),2),
            };
            Sphere s = new Sphere();
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsTrue(s.Bounds.Intersect(r));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds misses. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsMisses() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 0, -2), new RayTracerLib.Vector(0, 0, -1), 0),
                new teststruct(new Point(2, 2, 2), new RayTracerLib.Vector(1, 1, 1), 0),
                new teststruct(new Point(2, 2, 1), new RayTracerLib.Vector(1, 1, 0.5), 0)
            };
            Sphere s = new Sphere();
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsFalse(s.Bounds.Intersect(r));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds degenerate. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsDegenerate() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 0, -1), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(2, 2, 2), new RayTracerLib.Vector(-1, -1, -1), 2),
            };
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateScalingTransform(0, 1, 0);
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsTrue(s.Bounds.Intersect(r));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds cube. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsCube() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 0, -1), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(2, 2, 2), new RayTracerLib.Vector(-1, -1, -1), 2),
            };
            Cube s = new Cube();
            //s.Transform = RTMatrixOps.Scaling(0, 1, 0);
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsTrue(s.Bounds.Intersect(r));
            }
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(-1, -1, -1)));
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(1, 1, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds cube rotated. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsCubeRotated() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(0, 0, -2), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(2, 2, 2), new RayTracerLib.Vector(-1, -1, -1), 2),
            };
            Cube s = new Cube();
            s.Transform = MatrixOps.CreateRotationYTransform(Math.PI / 4);
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsTrue(s.Bounds.Intersect(r));
                Assert.IsTrue(s.Intersect(r).Count != 0);
            }
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(-Math.Sqrt(2), -1, -Math.Sqrt(2))));
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(Math.Sqrt(2), 1, Math.Sqrt(2))));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds cube translated. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsCubeTranslated() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(5, 5, -3), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(7, 7, 7), new RayTracerLib.Vector(-1, -1, -1), 2),
            };
            Cube s = new Cube();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 5, 5);
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsTrue(s.Bounds.Intersect(r));
                Assert.IsTrue(s.Intersect(r).Count != 0);
            }
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(4, 4, 4)));
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(6, 6, 6)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds cube translated rotated. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsCubeTranslatedRotated() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(5, 5, -3), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(7, 7, 7), new RayTracerLib.Vector(-1, -1, -1), 2),
            };
            Cube s = new Cube();
            s.Transform = (Matrix)(MatrixOps.CreateRotationYTransform(Math.PI / 4) * s.Transform);
            s.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(5, 5, 5) * s.Transform);
            //Console.WriteLine(s.Transform.ToMatrixString());
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                Assert.IsTrue(s.Bounds.Intersect(r));
                Assert.IsTrue(s.Intersect(r).Count != 0);
            }
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(3.58578, 4, 3.58578)));
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(6.41421, 6, 6.41421)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) bounds cube rotated translated. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void BoundsCubeRotatedTranslated() {
            List<teststruct> tests = new List<teststruct> {
                new teststruct(new Point(7, 5, -3), new RayTracerLib.Vector(0, 0, 1), 2),
                new teststruct(new Point(8, 7, 2), new RayTracerLib.Vector(-1, -1, -1), 2),
            };
            Cube s = new Cube();
            s.Transform = (Matrix)(MatrixOps.CreateRotationYTransform(Math.PI / 4) * MatrixOps.CreateTranslationTransform(5, 5, 5));
            Console.WriteLine(s.Transform.ToMatrixString());
            foreach (teststruct tx in tests) {
                Ray r = new Ray(tx.Origin, tx.Direction);
                List<Intersection> inter = s.Intersect(r);
                Assert.IsTrue(s.Bounds.Intersect(r));
                Assert.IsTrue(s.Intersect(r).Count != 0);
            }
            Assert.IsTrue(s.Bounds.MinCorner.Equals(new Point(5.65685, 4, -Math.Sqrt(2))));
            Assert.IsTrue(s.Bounds.MaxCorner.Equals(new Point(8.48528, 6, Math.Sqrt(2))));
        }
        [TestMethod]
        public void BoundsGroupRotatedTranslated() {
            Point p0 = new Point(0, 0, -1);
            Point p1 = new Point(0, 0, 1);
            Point p2 = new Point(0, -1, 0);
            Point p3 = new Point(0, 1, 0);
            Point p4 = new Point(-1, 0, 0);
            Point p5 = new Point(1, 0, 0);
            Triangle t0 = new Triangle(p0, p2, p4);
            Triangle t1 = new Triangle(p2, p0, p5);
            Triangle t2 = new Triangle(p0, p3, p4);
            Triangle t3 = new Triangle(p3, p0, p5);
            Triangle t4 = new Triangle(p1, p2, p4);
            Triangle t5 = new Triangle(p2, p1, p5);
            Triangle t6 = new Triangle(p1, p3, p4);
            Triangle t7 = new Triangle(p3, p1, p5);
            Group g = new Group();
            g.AddObject(t0);
            g.AddObject(t1);
            g.AddObject(t2);
            g.AddObject(t3);
            g.AddObject(t4);
            g.AddObject(t5);
            g.AddObject(t6);
            g.AddObject(t7);
            g.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateRotationYTransform(Math.PI / 2)  *  MatrixOps.CreateRotationZTransform(Math.PI / 2) );
            g.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(1, 1, 1) * g.Transform);
            Group g0 = new Group();
            g0.AddObject(g);
            Group bb =  BoundingBox.Generate(g);
            Group bb0 = BoundingBox.Generate(g0);
            Assert.IsTrue(g.Bounds.MinCorner.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(g.Bounds.MaxCorner.Equals(new Point(2, 2, 2)));
            Assert.IsTrue(bb.Bounds.MinCorner.Equals(new Point(-0.02, -0.02, -0.02)));
            Assert.IsTrue(bb.Bounds.MaxCorner.Equals(new Point(2.02, 2.02, 2.02)));
            Assert.IsTrue(bb0.Bounds.MinCorner.Equals(new Point(-0.02, -0.02, -0.02)));
            Assert.IsTrue(bb0.Bounds.MaxCorner.Equals(new Point(2.02, 2.02, 2.02)));
        }
    }
}
