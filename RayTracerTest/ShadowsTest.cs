using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace RayTracerTest
{
    /// <summary>
    /// Summary description for ShadowsTest
    /// </summary>
    [TestClass]
    public class ShadowsTest
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
        public ShadowsTest() {
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
            defaultWorld.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));

            Sphere s1 = new Sphere();
            s1.Material = new Material();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            s1.Material.Specular = new Color(0.2, 0.2, 0.2);

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

        [TestMethod]
        public void SurfaceInShadow() {
            Intersection i = new Intersection();
            i.Obj = new Sphere();
            i.Point = new Point(0, 0, 0);
            i.Eyev = new RayTracerLib.Vector(0, 0, -1);
            i.Normalv = new RayTracerLib.Vector(0, 0, -1);

            LightPoint l = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
            bool inShadow = true;
            Color result = i.Lighting(l, i.Point, inShadow);

            Assert.IsTrue(result.Equals( new Color(0.1, 0.1, 0.1)));
        }

        [TestMethod]
        public void NothingCollinear() {
            Intersection i = new Intersection();
            i.Obj = s;
            i.Point = new Point(0, 10, 0);
            //i.Eyev = new RayTracerLib.Vector(0, 0, -1);
            //i.Normalv = new RayTracerLib.Vector(0, 0, -1);

            Assert.IsFalse(i.Point.IsShadowed(defaultWorld, defaultWorld.Lights[0] ));
        }

        [TestMethod]
        public void ObjectBetweenPointAndLight() {
            Intersection i = new Intersection();
            i.Obj = s;
            i.Point = new Point(10, -10, 10);

            Assert.IsTrue(i.Point.IsShadowed(defaultWorld, defaultWorld.Lights[0]));
        }

        [TestMethod]
        public void ObjectBehindLight() {
            Intersection i = new Intersection();
            i.Obj = s;
            i.Point = new Point(-20, 20, -20);

            Assert.IsFalse(i.Point.IsShadowed(defaultWorld, defaultWorld.Lights[0]));
        }

        [TestMethod]
        public void ObjectBehindPoint() {
            Intersection i = new Intersection();
            i.Obj = s;
            i.Point = new Point(-2, 2, -2);

            Assert.IsFalse(i.Point.IsShadowed(defaultWorld, defaultWorld.Lights[0]));
        }

        [TestMethod]
        public void ShadeHitGetsIntersectionInShadow() {
            World w = new World();
            w.AddLight(new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1)));
            Sphere s1 = new Sphere();
            w.AddObject(s1);
            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateTranslationTransform(0, 0, 10);
            w.AddObject(s2);
            Ray r = new Ray(new Point(0, 0, 5), new RayTracerLib.Vector(0, 0, 1));
            Intersection i = new Intersection(4, s2);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(i);
            i.Prepare(r,xs);
            Color c = i.Shade(w, 5);
            Assert.IsTrue(c.Equals(new Color(0.1, 0.1, 0.1)));
        }

        [TestMethod]
        public void HitShouldOffsetPoint() {
            Ray r = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(0, 0, 1);
            Intersection i = new Intersection(5, s);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(i);
            i.Prepare(r, xs);

            Assert.IsTrue( i.OverPoint.Z < -Ops.EPSILON / 2);
            Assert.IsTrue(i.Point.Z > i.OverPoint.Z);
        }
    }
}
