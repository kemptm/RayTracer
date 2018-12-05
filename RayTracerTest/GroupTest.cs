///-------------------------------------------------------------------------------------------------
// file:	GroupTest.cs
//
// summary:	Implements the group test class
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
    /// <summary>   (Unit Test Class) a group test. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class GroupTest
    {
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

        public override Bounds LocalBounds() => new Bounds(new Point(-1, -1, -1), new Point(1, 1, 1));

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

        public GroupTest() {
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
        /// <summary>   (Unit Test Method) group create. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GroupCreate() {
            Group g = new Group();
            Assert.IsTrue(g.Transform.Equals(DenseMatrix.CreateIdentity(4)));
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shape has parent. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShapeHasParent() {
            Shape s = new TestShape();
            Assert.IsTrue(s.Parent == null);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) group add child. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GroupAddChild() {
            Group g = new Group();
            TestShape s = new TestShape();
            g.AddObject(s);
            Assert.IsTrue(g.Children.Contains(s));
            Assert.IsTrue(s.Parent == g);
            g.RemoveObject(s);
            Assert.IsFalse(g.Children.Contains(s));
            Assert.IsTrue(s.Parent == null);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) group intersect empty. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GroupIntersectEmpty() {
            Group g = new Group();
            Ray r = new Ray();
            List<Intersection> xs = g.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 0);
       }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) group intersect non empty. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GroupIntersectNonEmpty() {
            Group g = new Group();
            Sphere s1 = new Sphere();
            g.AddObject(s1);

            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateTranslationTransform(0, 0, -3);
            g.AddObject(s2);

            Sphere s3 = new Sphere();
            s3.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            g.AddObject(s3);

            Ray r = new Ray(new Point(0,0,-5), new RayTracerLib.Vector(0,0,1));
            List<Intersection> xs = g.LocalIntersect(r);
            Assert.IsTrue(xs.Count == 4);
            Assert.IsTrue(xs[0].Obj == s2);
            Assert.IsTrue(xs[1].Obj == s2);
            Assert.IsTrue(xs[2].Obj == s1);
            Assert.IsTrue(xs[3].Obj == s1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) group transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GroupTransformation() {
            Group g = new Group();
            g.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            g.AddObject(s);
            Ray r = new Ray(new Point(10, 0, -10), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = g.Intersect(r);
            Assert.IsTrue(xs.Count == 2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) point world to object space. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PointWorldToObjectSpace() {
            Group g1 = new Group();
            g1.Transform = MatrixOps.CreateRotationYTransform(Math.PI / 2);
            Group g2 = new Group();
            g2.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            g1.AddObject(g2);
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            g2.AddObject(s);

            Point p = s.WorldToObject(new Point(-2, 0, -10));
            Assert.IsTrue(p.Equals(new Point(0, 0, -1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) normal object to world. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NormalObjectToWorld() {
            Group g1 = new Group();
            g1.Transform = MatrixOps.CreateRotationYTransform(Math.PI / 2);
            Group g2 = new Group();
            g2.Transform = MatrixOps.CreateScalingTransform(1, 2, 3);
            g1.AddObject(g2);
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            g2.AddObject(s);
            RayTracerLib.Vector v = new RayTracerLib.Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3);
            RayTracerLib.Vector n = s.NormalToWorld(v);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) normal on child. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NormalOnChild() {
            Group g1 = new Group();
            g1.Transform = MatrixOps.CreateRotationYTransform(Math.PI / 2);
            Group g2 = new Group();
            g2.Transform = MatrixOps.CreateScalingTransform(1, 2, 3);
            g1.AddObject(g2);

            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(5, 0, 0);
            g2.AddObject(s);

            RayTracerLib.Vector n = s.NormalAt(new Point(1.7321, 1.1547, -5.5774));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0.285704, 0.42854, -0.85716)));
        }
    }
}
