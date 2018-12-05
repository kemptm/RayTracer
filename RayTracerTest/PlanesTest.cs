///-------------------------------------------------------------------------------------------------
// file:	PlanesTest.cs
//
// summary:	Implements the planes test class
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
    /// <summary>   (Unit Test Class) the planes test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class PlanesTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a test shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected class TestShape : Shape
        {
            /// <summary>   The test ray. </summary>
            public Ray testRay;

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Local intersect (abstract). </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <param name="rayparm">  The ray to intersect. </param>
            ///
            /// <returns>   A List&lt;Intersection&gt; </returns>
            ///-------------------------------------------------------------------------------------------------

            public override List<Intersection> LocalIntersect(Ray rayparm) {
                testRay = rayparm;
                return new List<Intersection>();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <param name="worldPoint">   The local point. </param>
            ///
            /// <returns>   A Vector. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override RayTracerLib.Vector LocalNormalAt(Point worldPoint) => new RayTracerLib.Vector(worldPoint.X, worldPoint.Y, worldPoint.Z);

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Copy the shape (Virtual). </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <returns>   A Shape. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Shape Copy() {
                throw new NotImplementedException();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <returns>   The Bounds. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Bounds LocalBounds() {
                return new Bounds(new Point(-double.MaxValue, 0, -double.MaxValue), new Point(double.MaxValue, 0, double.MaxValue));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public PlanesTest() {
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
        /// <summary>   (Unit Test Method) default transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DefaultTransformation() {
            Shape s = new TestShape();
            Assert.IsTrue(s.Transform.Equals(DenseMatrix.CreateIdentity(4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) assign transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AssignTransformation() {
            Shape s = new TestShape();
            s.Transform = MatrixOps.CreateTranslationTransform(2, 3, 4);
            Assert.IsTrue(s.Transform.Equals(MatrixOps.CreateTranslationTransform(2, 3, 4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) default material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DefaultMaterial() {
            Shape s = new TestShape();
            Material m = new Material();
            s.Material = m;
            Assert.IsTrue(s.Material.Equals(m));
         }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) assigning a material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AssigningAMaterial() {
            Shape s = new TestShape();
            Material m = new Material();
            m.Ambient = 1;
            s.Material = m;
            Assert.IsTrue(s.Material.Equals(m));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersecting scaled shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectingScaledShape() {
            Ray r = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            TestShape s = new TestShape();
            s.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(s.testRay.Origin.Equals(new Point(0, 0, -2.5)));
            Assert.IsTrue(s.testRay.Direction.Equals(new RayTracerLib.Vector(0, 0, 0.5)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersecting translated shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectingTranslatedShape() {
            Ray r = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            TestShape s = new TestShape();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            List<Intersection> xs = s.Intersect(r);
            Assert.IsTrue(s.testRay.Origin.Equals(new Point(-5, 0, -5)));
            Assert.IsTrue(s.testRay.Direction.Equals(new RayTracerLib.Vector(0, 0, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) computing normal on translated shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ComputingNormalOnTranslatedShape() {
            TestShape s = new TestShape();
            s.Transform = MatrixOps.CreateTranslationTransform(0, 1, 0);
            RayTracerLib.Vector n = s.NormalAt(new Point(0, 1.70711, -0.70711));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0, 0.70711, -0.70711)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) computing normal on scaled shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ComputingNormalOnScaledShape() {
            TestShape s = new TestShape();
            s.Transform = MatrixOps.CreateScalingTransform(1, 0.5, 1);
            RayTracerLib.Vector n = s.NormalAt(new Point(0, Math.Sqrt(2)/2.0, -Math.Sqrt(2) / 2.0));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0, 0.97014, -0.24254)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane intersect with parallel ray. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneIntersectWithParallelRay() {
            Plane p = new RayTracerLib.Plane();
            Ray r = new Ray(new Point(0, 10, 0), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane intersect with coplanar ray. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneIntersectWithCoplanarRay() {
            Plane p = new RayTracerLib.Plane();
            Ray r = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane intersect with ray above. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneIntersectWithRayAbove() {
            Plane p = new RayTracerLib.Plane();
            Ray r = new Ray(new Point(0, 1, 0), new RayTracerLib.Vector(0, -1, 0));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 1);
            Assert.IsTrue(xs[0].T == 1);
            Assert.IsTrue(xs[0].Obj.Equals(p));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane intersect with ray below. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneIntersectWithRayBelow() {
            Plane p = new RayTracerLib.Plane();
            Ray r = new Ray(new Point(0, -1, 0), new RayTracerLib.Vector(0, 1, 0));
            List<Intersection> xs = p.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 1);
            Assert.IsTrue(xs[0].T == 1);
            Assert.IsTrue(xs[0].Obj.Equals(p));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane normal same everywhere. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneNormalSameEverywhere() {
            Plane p = new Plane();
            RayTracerLib.Vector n1 = p.LocalNormalAt(new Point(0, 0, 0));
            RayTracerLib.Vector n2 = p.LocalNormalAt(new Point(10, 0, -10));
            RayTracerLib.Vector n3 = p.LocalNormalAt(new Point(-5, 0, 150));
            Assert.IsTrue(n1.Equals(new RayTracerLib.Vector(0, 1, 0)));
            Assert.IsTrue(n2.Equals(new RayTracerLib.Vector(0, 1, 0)));
            Assert.IsTrue(n3.Equals(new RayTracerLib.Vector(0, 1, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane transformed. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneTransformed() {
            Plane p = new Plane();
            p.Transform = MatrixOps.CreateTranslationTransform(0, 5, 0);
            Ray r = new Ray(new Point(0, 10, 0), new RayTracerLib.Vector(0, -1, 0).Normalize());
            List<Intersection> xs = p.Intersect(r);
            Assert.IsTrue(xs.Count == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) plane transformed copied. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PlaneTransformedCopied() {
            Plane p = new Plane();
            p.Transform = MatrixOps.CreateTranslationTransform(0, 5, 0);
            p.Material.Pattern = new Checked3DPattern();
            Plane p2 = (Plane) p.Copy();
            bool foo = p.Equals(p2);
            Assert.IsTrue(foo);
        }
    }
}
