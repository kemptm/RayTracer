///-------------------------------------------------------------------------------------------------
// file:	ReflectionTest.cs
//
// summary:	Implements the reflection test class
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
    /// <summary>   (Unit Test Class) a reflection test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class ReflectionTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the world. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <returns>   A defaultWorld. </returns>
        ///-------------------------------------------------------------------------------------------------

        World defaultWorld = new World();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <returns>   The s. </returns>
        ///-------------------------------------------------------------------------------------------------

        Sphere s = new Sphere();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a test pattern. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected class TestPattern : Pattern
        {
            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Copies this object. </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <returns>   A Pattern. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Pattern Copy() {
                throw new NotImplementedException();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Pattern at a particular point. </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <param name="p">    A RTPoint to return pattern color. </param>
            ///
            /// <returns>   A Color. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Color PatternAt(Point p) {
                return new Color(p.X, p.Y, p.Z);
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Pattern at object. (Abstract) </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <param name="s">            A RTShape to process. </param>
            /// <param name="worldPoint">   The world point. </param>
            ///
            /// <returns>   A Color. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Color PatternAtObject(Shape s, Point worldPoint) => PatternAt(LocalPatternPoint(s, worldPoint));

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Tests if this Pattern is considered equal to another. </summary>
            ///
            /// <remarks>   Kemp, 12/4/2018. </remarks>
            ///
            /// <param name="m">    The pattern to compare to this object. </param>
            ///
            /// <returns>   True if the objects are considered equal, false if they are not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override bool Equals(Pattern m) {
                if (m is TestPattern) {
                    TestPattern c = (TestPattern)m;
                    return xform.Equals(c.xform);
                }
                return false;
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public ReflectionTest() {
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     You can use the following additional attributes as you write your tests:
        ///     
        ///     Use ClassInitialize to run code before running the first test in the class
        ///     [ClassInitialize()] public static void MyClassInitialize(TestContext testContext) { }
        ///     
        ///     Use ClassCleanup to run code after all tests in a class have run [ClassCleanup()] public
        ///     static void MyClassCleanup() { }
        ///     
        ///     Use TestInitialize to run code before running each test.
        /// </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestInitialize()]
        public void MyTestInitialize() {
            defaultWorld.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));

            Sphere s1 = new Sphere();
            s1.Material = new Material();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = 0.7;
            s1.Material.Specular = 0.2;

            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5);
            defaultWorld.AddObject(s1);
            defaultWorld.AddObject(s2);
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) reflective material attribute. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ReflectiveMaterialAttribute() {
            Material m = new Material();
            Assert.IsTrue(m.Reflective == 0.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) precompute reflection vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PrecomputeReflectionVector() {
            Shape shape = new Plane();
            Ray ray = new Ray(new Point(0, 1, -1), new RayTracerLib.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection hit = new Intersection(Math.Sqrt(2), shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Assert.IsTrue(hit.Reflectv.Equals(new RayTracerLib.Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) nonreflective material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NonreflectiveMaterial() {
            World world = defaultWorld.Copy();
            Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
            Shape s1 = new Sphere();
            s1.Material.Ambient = 1;
            Intersection hit = new Intersection(1, s1);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Color color = hit.ReflectedColor(world);
            Assert.IsTrue(color.Equals(new Color(0, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) reflective material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ReflectiveMaterial() {
            World world = defaultWorld.Copy();
            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = MatrixOps.CreateTranslationTransform(0, -1, 0);
            world.AddObject(shape);
            Ray ray = new Ray(new Point(0, 0, -3), new RayTracerLib.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection hit = new Intersection(Math.Sqrt(2), shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Color color = hit.ReflectedColor(world);
            Assert.IsTrue(color.Equals( new Color(0.19035, 0.23793, 0.14276)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shade hit with reflective material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadeHitWithReflectiveMaterial() {
            World world = defaultWorld.Copy();
            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = MatrixOps.CreateTranslationTransform(0, -1, 0);
            world.AddObject(shape);
            Ray ray = new Ray(new Point(0, 0, -3), new RayTracerLib.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            Intersection hit = new Intersection(Math.Sqrt(2), shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Color color = hit.Shade(world);
            Assert.IsTrue(color.Equals(new Color(0.87677, 0.92436, 0.82918)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) mutually re eflective surfaces. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MutuallyReEflectiveSurfaces() {
            World world = new World();

            Plane lower = new Plane();
            lower.Material.Reflective = 1;
            lower.Transform = MatrixOps.CreateTranslationTransform(0, -1, 0);
            world.AddObject(lower);

            Plane upper = new Plane();
            upper.Material.Reflective = 1;
            upper.Transform = (Matrix) (MatrixOps.CreateTranslationTransform(0, 1, 0) * MatrixOps.CreateRotationXTransform(Math.PI));

            world.AddObject(upper);

            Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));
            Color c;
            try {
                c = world.ColorAt(ray);
            }
            catch (System.StackOverflowException) {
                Assert.Fail();
                //throw;
            }
            Assert.IsTrue(true);
        }

     }
}
