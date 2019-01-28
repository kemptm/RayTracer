///-------------------------------------------------------------------------------------------------
// file:	MakingASceneTest.cs
//
// summary:	Implements the making a scene test class
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
    /// <summary>   (Unit Test Class) a making scene test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class MakingASceneTest {

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
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public MakingASceneTest() {
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) creating a world. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreatingAWorld() {
            World w = new World();
            Assert.IsTrue(w.Objects.Count == 0);
            Assert.IsTrue(w.Lights.Count == 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) check default construction. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CheckDefault() {
            LightPoint light = new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1));
            Sphere s1 = new Sphere();
            s1.Material = new Material();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            s1.Material.Specular = new Color(0.2, 0.2, 0.2);

            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5);

            Assert.IsTrue(defaultWorld.Lights[0].Position.Equals(light.Position));
            Assert.IsTrue(defaultWorld.Lights[0].Intensity.Equals(light.Intensity));

            Sphere s1w = (Sphere)defaultWorld.Objects[0];
            Assert.IsTrue((s1w.Material.Diffuse.Equals(s1.Material.Diffuse)) && (s1w.Material.Specular.Equals(s1.Material.Specular)) && s1w.Material.Color.Equals(s1.Material.Color));

            Sphere s2w = (Sphere)defaultWorld.Objects[1];
            Assert.IsTrue(s2w.Transform.Equals(s2.Transform));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) world ray intersections. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void WorldRayIntersections() {
            World w = defaultWorld;
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = w.Intersect(ray);
            Assert.IsTrue(xs.Count == 4);
            Assert.IsTrue(xs[0].T == 4);
            Assert.IsTrue(xs[1].T == 4.5);
            Assert.IsTrue(xs[2].T == 5.5);
            Assert.IsTrue(xs[3].T == 6);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) precompute intersection. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PrecomputeIntersection() {
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(4, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray,xs);
            Assert.IsTrue(hit.Point.Equals(new Point(0, 0, -1.0001)));
            Assert.IsTrue(hit.Eyev.Equals(new RayTracerLib.Vector(0, 0, -1)));
            Assert.IsTrue(hit.Normalv.Equals(new RayTracerLib.Vector(0, 0, -1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersection on outside of shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectionOnOutside() {
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(4, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Assert.IsFalse(hit.Inside);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersection on the inside of a shape. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectionOnInside() {
            Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(1, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Assert.IsTrue(hit.Inside);
            Assert.IsTrue(hit.Point.Equals(new Point(0, 0, 0.9999)));
            Assert.IsTrue(hit.Eyev.Equals(new RayTracerLib.Vector(0, 0, -1)));
            Assert.IsTrue(hit.Normalv.Equals(new RayTracerLib.Vector(0, 0, -1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersection shading. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectionShading() {
            World world = defaultWorld;
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Shape shape = world.Objects[0];
            Intersection hit = new Intersection(4, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Color c = hit.Shade(world);
            Assert.IsTrue(c.Equals(new Color(0.38066, 0.47583, 0.2855)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) intersection shading inside. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectionShadingInside() {
            World world = defaultWorld;
            world.Lights.Clear(); // remove default light.
            world.AddLight(new LightPoint(new Point(0, 0.25, 0), new Color(1, 1, 1)));
            Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
            Shape shape = world.Objects[1].Copy();
            Intersection hit = new Intersection(0.5, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Color c = hit.Shade(world);
            Assert.IsTrue(c.Equals(new Color(0.90495, 0.90495, 0.90495)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) color at ray misses. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ColorAtRayMisses() {
            World world = defaultWorld;
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 1, 0));
            Assert.IsTrue(world.ColorAt(ray).Equals(new Color(0, 0, 0)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) color at ray hits. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ColorAtRayHits() {
            World world = defaultWorld;
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Assert.IsTrue(world.ColorAt(ray).Equals(new Color(0.38066, 0.47583, 0.2855)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) color at behind ray. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ColorAtBehindRay() {
            World world = defaultWorld;
            Shape outer = defaultWorld.Objects[0];
            outer.Material.Ambient = new Color(1, 1, 1);
            Shape inner = defaultWorld.Objects[1];
            inner.Material.Ambient = new Color(1, 1, 1);
            Ray ray = new Ray(new Point(0, 0, -0.75), new RayTracerLib.Vector(0, 0, 1));
            Assert.IsTrue(world.ColorAt(ray).Equals(inner.Material.Color));
            //Assert.IsTrue(world.ColorAt(ray).Clamp().Equals(inner.Material.Color));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) view transformation default. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ViewTransformationDefault() {
            Point from = new Point(0, 0, 0);
            Point to = new Point(0, 0, -1);
            RayTracerLib.Vector up = new RayTracerLib.Vector(0, 1, 0);
            Matrix t = MatrixOps.CreateViewTransform(from, to, up);
            Assert.IsTrue(t.Equals(DenseMatrix.CreateIdentity(4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) view transformation positive z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ViewTransformationPositiveZ() {
            Point from = new Point(0, 0, 0);
            Point to = new Point(0, 0, 1);
            RayTracerLib.Vector up = new RayTracerLib.Vector(0, 1, 0);
            Matrix t = MatrixOps.CreateViewTransform(from, to, up);
            Assert.IsTrue(t.Equals(MatrixOps.CreateScalingTransform(-1,1,-1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) view transformation moves world. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ViewTransformationMovesWorld() {
            Point from = new Point(0, 0, 8);
            Point to = new Point(0, 0, 0);
            RayTracerLib.Vector up = new RayTracerLib.Vector(0, 1, 0);
            Matrix t = MatrixOps.CreateViewTransform(from, to, up);
            Assert.IsTrue(t.Equals(MatrixOps.CreateTranslationTransform(0, 0, -8)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) view transformation arbitrary. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ViewTransformationArbitrary() {
            Point from = new Point(1, 3, 2);
            Point to = new Point(4, -2, 8);
            RayTracerLib.Vector up = new RayTracerLib.Vector(1, 1, 0);
            Matrix t = MatrixOps.CreateViewTransform(from,to,up);
            Matrix r = DenseMatrix.OfArray(new double[,]{ 
                { -0.50709, 0.50709,  0.67612,-2.36643},
                {  0.76772, 0.60609,  0.12122,-2.82843},
                { -0.35857, 0.59761, -0.71714, 0.0},
                {  0.0,     0.0,      0.0,     1.0} });
            Assert.IsTrue(Ops.Equals(t,r));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera construct. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraConstruct() {
            uint hsize = 160;
            uint vsize = 120;
            double fieldOfView = Math.PI / 2.0;
            Camera c = new Camera(hsize, vsize, fieldOfView);
            Assert.IsTrue(c.Hsize == hsize);
            Assert.IsTrue(c.Vsize == vsize);
            Assert.IsTrue(c.FieldOfView == fieldOfView);
            Assert.IsTrue(c.Transform.Equals(DenseMatrix.CreateIdentity(4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera pixel size horizontal canvas. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraPixelSizeHorizontalCanvas() {
            Camera c = new Camera(200, 125, Math.PI / 2.0);
            Assert.IsTrue(Ops.Equals(c.PixelSize,0.01));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera pixel size vertical canvas. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraPixelSizeVerticalCanvas() {
            Camera c = new Camera(125, 200, Math.PI / 2.0);
            Assert.IsTrue(Ops.Equals(c.PixelSize, 0.01));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera ray thru center of canvas. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraRayThruCenterOfCanvas() {
            Camera c = new Camera(201, 101, Math.PI / 2);
            Ray r = c.RayForPixel(100, 50);
            Assert.IsTrue(r.Origin.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(r.Direction.Equals(new RayTracerLib.Vector(0, 0, -1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera ray thru corner of canvas. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraRayThruCornerOfCanvas() {
            Camera c = new Camera(201, 101, Math.PI / 2);
            Ray r = c.RayForPixel(0, 0);
            Assert.IsTrue(r.Origin.Equals(new Point(0, 0, 0)));
            Assert.IsTrue(r.Direction.Equals(new RayTracerLib.Vector(0.66519, 0.33259, -0.66851)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera ray when camera transformed. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraRayWhenCameraTransformed() {
            Camera c = new Camera(201, 101, Math.PI / 2);
            c.Transform = (Matrix)(MatrixOps.CreateRotationYTransform(Math.PI / 4) * MatrixOps.CreateTranslationTransform(0, -2, 5));
            Ray r = c.RayForPixel(100, 50);
            Assert.IsTrue(r.Origin.Equals(new Point(0, 2, -5)));
            Assert.IsTrue(r.Direction.Equals(new RayTracerLib.Vector(Math.Sqrt(2)/2, 0, -Math.Sqrt(2)/2)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) camera render world. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CameraRenderWorld() {
            World w = defaultWorld.Copy();
            Camera c = new Camera(11, 11, Math.PI / 2);
            Point from = new Point(0, 0, -5);
            Point to = new Point(0, 0, 0);
            RayTracerLib.Vector up = new RayTracerLib.Vector(0, 1, 0);
            c.Transform = MatrixOps.CreateViewTransform(from, to, up);
            Canvas image = w.Render(c);
            Assert.IsTrue(image.PixelAt(5, 5).Equals(new Color(0.38066, 0.47583, 0.2855)));
            
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow light with shadow. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowLightWithShadow() {
            Material m = new Material();
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0, -1);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            Point position = new Point(0, 0, 0);
            LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
            bool inShadow = true;
            Assert.IsTrue(Ops.Lighting(m, s, light, position, eyev, normalv, inShadow).Equals(new Color(0.1, 0.1, 0.1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow nothing between light and point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowNothingBetweenLightAndPoint() {
            World world = defaultWorld.Copy();
            Point p = new Point(0, 10, 0);
            Assert.IsFalse(p.IsShadowed(world, world.Lights[0]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow object between light and point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowObjectBetweenLightAndPoint() {
            World world = defaultWorld.Copy();
            Point p = new Point(10, -10, 10);
            Assert.IsTrue(p.IsShadowed(world, world.Lights[0]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow light between object and point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowLightBetweenObjectAndPoint() {
            World world = defaultWorld.Copy();
            Point p = new Point(-20, 20, -20);
            Assert.IsFalse(p.IsShadowed(world, world.Lights[0]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow point between light and object. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowPointBetweenLightAndObject() {
            World world = defaultWorld.Copy();
            Point p = new Point(-2, 2, -2);
            Assert.IsFalse(p.IsShadowed(world, world.Lights[0]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow point inside object. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowPointInsideObject() {
            World world = defaultWorld.Copy();
            Point p = new Point(0, 0, 0);
            Assert.IsTrue(p.IsShadowed(world, world.Lights[0]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow shade gets intersection in shadow. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowShadeGetsIntersectionInShadow() {
            World w = new World();
            w.AddLight(new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1)));
            Sphere s1 = new Sphere();
            w.AddObject(s1);
            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateTranslationTransform(0, 0, 10);
            w.AddObject(s2);
            Ray ray = new Ray(new Point(0, 0, 5), new RayTracerLib.Vector(0, 0, 1));
            Intersection h = new Intersection(4, s2);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(h);
            h.Prepare(ray, xs);
            Color c = h.Shade(w);
            Assert.IsTrue(c.Equals(new Color(0.1, 0.1, 0.1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow offset point. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowOffsetPoint() {
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(4, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Assert.IsTrue((hit.Point.Z > -1.1) && (hit.Point.Z < -1));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) shadow offset point negative. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ShadowOffsetPointNegative() {
            Ray ray = new Ray(new Point(0, 0, 5), new RayTracerLib.Vector(0, 0, -1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(4, shape);
            List<Intersection> xs = new List<Intersection>();
            xs.Add(hit);
            hit.Prepare(ray, xs);
            Assert.IsTrue((hit.Point.Z > 1) && (hit.Point.Z < 1.1));
        }
    }
}
