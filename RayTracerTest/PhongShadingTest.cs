///-------------------------------------------------------------------------------------------------
// file:	PhongShadingTest.cs
//
// summary:	Implements the phong shading test class
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
    /// <summary>   (Unit Test Class) a phong shading test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class PhongShadingTest
    {
        /// <summary>   A Material to process. </summary>
        private Material m;
        /// <summary>   The position. </summary>
        private Point position;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <returns>   The s. </returns>
        ///-------------------------------------------------------------------------------------------------

        Sphere s = new Sphere();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public PhongShadingTest() {
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
            m = new Material();
            position = new Point();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) eye between lignt and surface. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void EyeBetweenLigntAndSurface() {
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0, -1);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
            Color result = Ops.Lighting(m, s, light, position, eyev, normalv);
            Assert.IsTrue(result.Equals(new Color(1.9, 1.9, 1.9)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) eye between light and surface offset 45. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void EyeBetweenLightAndSurfaceOffset45() {
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
            Color result = Ops.Lighting(m, s, light, position, eyev, normalv);
            Assert.IsTrue(result.Equals(new Color(1.0, 1.0, 1.0)));
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) eye opposite surface light offset 45. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void EyeOppositeSurfaceLightOffset45() {
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0, -1);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            LightPoint light = new LightPoint(new Point(0, 10, -10), new Color(1, 1, 1));
            Color result = Ops.Lighting(m, s, light, position, eyev, normalv);
            Assert.IsTrue(result.Equals(new Color(0.7364, 0.7364, 0.7364)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) eye in path of reflection vector. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void EyeInPathOfReflectionVector() {
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            LightPoint light = new LightPoint(new Point(0, 10, -10), new Color(1, 1, 1));
            Color result = Ops.Lighting(m, s, light, position, eyev, normalv);
            Assert.IsTrue(result.Equals(new Color(1.6364, 1.6364, 1.6364)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) eye on normal light behind surface. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void EyeOnNormalLightBehindSurface() {
            Sphere s = new Sphere();
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0,-1);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            LightPoint light = new LightPoint(new Point(0, 0, 10), new Color(1, 1, 1));
            Color result = Ops.Lighting(m, s, light, position, eyev, normalv);
            Assert.IsTrue(result.Equals(new Color(0.1, 0.1, 0.1)));

        }
    }
}
