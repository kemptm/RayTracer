///-------------------------------------------------------------------------------------------------
// file:	LightAndShadingTest.cs
//
// summary:	Implements the light and shading test class
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
    /// <summary>   LightAndShading test cases. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class LightAndShadingTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public LightAndShadingTest() {
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
        /// <summary>   (Unit Test Method) sphere normal x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalX() {
            Sphere s = new Sphere();
            RayTracerLib.Vector n = s.NormalAt(new Point(1, 0, 0));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(1, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalY() {
            Sphere s = new Sphere();
            RayTracerLib.Vector n = s.NormalAt(new Point(0, 1, 0));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0, 1, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalZ() {
            Sphere s = new Sphere();
            RayTracerLib.Vector n = s.NormalAt(new Point(0, 0, 1));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0, 0, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal other. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalOther() {
            Sphere s = new Sphere();
            RayTracerLib.Vector n = s.NormalAt(new Point(Math.Sqrt(3)/3, Math.Sqrt(3) / 3, (Math.Sqrt(3) / 3)));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, (Math.Sqrt(3) / 3))));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal is normalized. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalIsNormalized() {
            Sphere s = new Sphere();
            RayTracerLib.Vector n = s.NormalAt(new Point(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, (Math.Sqrt(3) / 3)));
            Assert.IsTrue(n.Normalize().Equals(n));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal scaled. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalScaled() {
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateScalingTransform(1, 0.5, 1);
            RayTracerLib.Vector n = s.NormalAt(new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0, 0.97014, -0.24254)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere normal translated. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereNormalTranslated() {
            Sphere s = new Sphere();
            s.Transform = MatrixOps.CreateTranslationTransform(0, 1, 0);
            RayTracerLib.Vector n = s.NormalAt(new Point(0, 1.70711,-0.70711));
            Assert.IsTrue(n.Equals(new RayTracerLib.Vector(0, 0.70711, -0.70711)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) reflect 45 vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Reflect45Vector() {
            RayTracerLib.Vector v = new RayTracerLib.Vector(1, -1, 0);
            RayTracerLib.Vector n = new RayTracerLib.Vector(0, 1, 0);
            RayTracerLib.Vector r = v.Reflect(n);
            Assert.IsTrue(r.Equals(new RayTracerLib.Vector(1, 1, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) reflect slanted surface. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ReflectSlantedSurface() {
            RayTracerLib.Vector v = new RayTracerLib.Vector(0, -1, 0);
            RayTracerLib.Vector n = new RayTracerLib.Vector(Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0);
            Assert.IsTrue(v.Reflect(n).Equals(new RayTracerLib.Vector(1, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) point light. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PointLight() {
            Color intensity = new Color(1, 1, 1);
            Point pos = new Point(0, 0, 0);
            LightPoint light = new LightPoint(pos, intensity);
            Assert.IsTrue(light.Position.Equals(pos));
            Assert.IsTrue(light.Intensity.Equals(intensity));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) material default. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MaterialDefault() {
            Material m = new Material();
            Assert.IsTrue(m.Color.Equals(new Color(1, 1, 1)));
            Assert.IsTrue(m.Ambient == 0.1);
            Assert.IsTrue(m.Diffuse == 0.9);
            Assert.IsTrue(m.Specular == 0.9);
            Assert.IsTrue(m.Shininess == 200);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere default material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod] 
        public void SphereDefaultMaterial() {
            Sphere s = new Sphere();
            Material m = s.Material;
            Assert.IsTrue(m.Equals(new Material()));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) sphere assign material. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SphereAssignMaterial() {
            Sphere s = new Sphere();
            Material m = new Material();
            s.Material = m;
            Assert.IsTrue(s.Material.Equals(m));
        }
    }
}
