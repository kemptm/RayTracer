///-------------------------------------------------------------------------------------------------
// file:	RefractionTest.cs
//
// summary:	Implements the refraction test class
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
    /// <summary>   (Unit Test Class) a refraction test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class RefractionTest
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
        /// <summary>   Gets the world. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <returns>   A refractTest. </returns>
        ///-------------------------------------------------------------------------------------------------

        World refractTest = new World();

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

        public RefractionTest() {
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


            refractTest.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));

            Plane floor = new Plane();
            floor.Material.Pattern = new CheckedPattern(new Color(1,0,0),new Color(0,1,0));
            refractTest.AddObject(floor);

            GlassSphere s3 = new GlassSphere();
            s3.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 3, 0) *  MatrixOps.CreateScalingTransform(2, 2, 2));
            s3.Material.Ambient = new Color(0, 0, 0);
            s3.Material.Color = new Color(0, 0, 0);
            s3.Material.Specular = new Color(0, 0, 0);
            refractTest.AddObject(s3);
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) glass sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GlassSphere() {
            GlassSphere s = new GlassSphere();
            Assert.IsTrue(s.Transform.Equals(DenseMatrix.CreateIdentity(4)));
            Assert.IsTrue(s.Material.Transparency == 1.0);
            Assert.IsTrue(s.Material.RefractiveIndex == 1.5);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) transparency and reactive defaults. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void TransparencyAndReactiveDefaults() {
            Material m = new Material();
            Assert.IsTrue(m.Transparency == 0);
            Assert.IsTrue(m.RefractiveIndex == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) N1 and N2 at various intersections. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void N1AndN2AtVariousIntersections() {
            GlassSphere a = new GlassSphere();
            a.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            a.Material.RefractiveIndex = 1.5;

            GlassSphere b = new GlassSphere();
            b.Transform = MatrixOps.CreateTranslationTransform(0, 0, -0.25);
            b.Material.RefractiveIndex = 2.0;

            GlassSphere c = new GlassSphere();
            c.Transform = MatrixOps.CreateTranslationTransform(0, 0, -0.25);
            c.Material.RefractiveIndex = 2.5;

            Ray ray = new Ray(new Point(0, 0, -4), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = Intersection.Intersections(
                new Intersection(2, a),
                new Intersection(2.75, b),
                new Intersection(3.25, c),
                new Intersection(4.75, b),
                new Intersection(5.25, c),
                new Intersection(6, a));

            foreach (Intersection x in xs) {
                x.Prepare(ray, xs);
            }
            Assert.IsTrue(xs[0].N1 == 1.0);
            Assert.IsTrue(xs[0].N2 == 1.5);
            Assert.IsTrue(xs[1].N1 == 1.5);
            Assert.IsTrue(xs[1].N2 == 2.0);
            Assert.IsTrue(xs[2].N1 == 2.0);
            Assert.IsTrue(xs[2].N2 == 2.5);
            Assert.IsTrue(xs[3].N1 == 2.5);
            Assert.IsTrue(xs[3].N2 == 2.5);
            Assert.IsTrue(xs[4].N1 == 2.5);
            Assert.IsTrue(xs[4].N2 == 1.5);
            Assert.IsTrue(xs[5].N1 == 1.5);
            Assert.IsTrue(xs[5].N2 == 1.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) under point under surface. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void UnderPointUnderSurface() {
            GlassSphere s = new GlassSphere();
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            Intersection hit = new Intersection(4, s);
            List<Intersection> xs = new List<Intersection>();
            xs = Intersection.Intersections(hit);
            hit.Prepare(ray, xs);
            Assert.IsTrue((Math.Abs(hit.UnderPoint.Z) < 1) && (Math.Abs(hit.UnderPoint.Z) > 0.9));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) refracted color opaque surface. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RefractedColorOpaqueSurface() {
            World world = defaultWorld.Copy();
            Shape shape = world.Objects[0];
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = Intersection.Intersections(new Intersection(4, shape), new Intersection(6, shape));
            xs[0].Prepare(ray, xs);
            Color black = new Color(0, 0, 0);
            Color c = xs[0].RefractedColor(world, 5);
            Assert.IsTrue(c.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) refracted color maximum recursion. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RefractedColorMaxRecursion() {
            World world = defaultWorld.Copy();
            Shape shape = world.Objects[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = Intersection.Intersections(new Intersection(4, shape), new Intersection(6, shape));
            xs[0].Prepare(ray, xs);
            Color black = new Color(0, 0, 0);
            Color c = xs[0].RefractedColor(world, 0);
            Assert.IsTrue(c.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) refracted color total internal reflection. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RefractedColorTotalInternalReflection() {
            World world = defaultWorld.Copy();
            Shape shape = world.Objects[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            Ray ray = new Ray(new Point(0, 0, Math.Sqrt(2) / 2), new RayTracerLib.Vector(0, 1, 0));
            List<Intersection> xs = Intersection.Intersections(new Intersection(-Math.Sqrt(2) / 2, shape), new Intersection(Math.Sqrt(2) / 2, shape));
            xs[1].Prepare(ray, xs);
            Color black = new Color(0, 0, 0);
            Color c = xs[1].RefractedColor(world, 5);
            Assert.IsTrue(c.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) refracted color. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RefractedColor() {
            World world = defaultWorld.Copy();
            Shape a = world.Objects[0];
            a.Material.Ambient = new Color(1, 1, 1);
            a.Material.Pattern = new TestPattern();
            Shape b = world.Objects[1];
            b.Material.Transparency = 1;
            b.Material.RefractiveIndex = 1.5;
            Ray ray = new Ray(new Point(0, 0, 0.1), new RayTracerLib.Vector(0, 1, 0));
            List<Intersection> xs = Intersection.Intersections(
                new Intersection(-0.9899, a),
                new Intersection(-0.4899, b),
                new Intersection(0.4899, b),
                new Intersection(0.9899, a));
            List<Intersection> xs2 = world.Intersect(ray);
            xs[2].Prepare(ray, xs);
            xs2[2].Prepare(ray, xs2);
            Color c = xs[2].RefractedColor(world, 5);
            Color c2 = xs2[2].RefractedColor(world, 5);
            Assert.IsTrue(c.Equals(new Color(0, 0.99888, 0.04725)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) refraction in shade hit. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RefractionInShadeHit() {
            World world = defaultWorld.Copy();

            Plane floor = new Plane();
            floor.Transform = MatrixOps.CreateTranslationTransform(0, -1, 0);
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            world.AddObject(floor);

            Sphere ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = new Color(0.5, 0.5, 0.5);
            ball.Transform = MatrixOps.CreateTranslationTransform(0, -3.5, -0.5);
            world.AddObject(ball);

            Ray ray = new Ray(new Point(0, 0, -3), new RayTracerLib.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));

            List<Intersection> xs = Intersection.Intersections(new Intersection(Math.Sqrt(2), floor));

            xs[0].Prepare(ray, xs);
            Color color = xs[0].Shade(world);
            Assert.IsTrue(color.Equals(new Color(0.93642, 0.68642, 0.68642)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) glass sphere at normal. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GlassSphereAtNormal() {
            List<Intersection> xs;
            World w = refractTest;
            Ray r = new Ray(new Point(0, 10, 0), new RayTracerLib.Vector(0, -1, 0));

            xs = w.Intersect(r);
            Assert.IsTrue(xs.Count != 0);
            Intersection hit = xs[0];
            hit.Prepare(r, xs);
            Color c1 = hit.Shade(w);
            Assert.IsTrue(c1.Equals(new Color(.61962, 0, 0)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) glass sphere intersect at 45. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GlassSphereIntersectAt45() {
            double Sqrt2over2 = Math.Sqrt(2) / 2;
            List<Intersection> xs;
            World w = refractTest;
            Ray r = new Ray(new Point(0, 10, -Sqrt2over2), new RayTracerLib.Vector(0, -1, 0));

            xs = w.Intersect(r);
            Assert.IsTrue(xs.Count != 0);
            Intersection hit = xs[0];
            hit.Prepare(r, xs);
            Color c1 = hit.Shade(w);
            Assert.IsTrue(c1.Equals(new Color(.61915, 0, 0)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) schlick total internal reflection. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SchlickTotalInternalReflection() {
            double Sqrt2over2 = Math.Sqrt(2) / 2;
            GlassSphere s = new GlassSphere();
            Ray ray = new Ray(new Point(0, 0, Sqrt2over2),new RayTracerLib.Vector(0,1,0));
            List<Intersection> xs = Intersection.Intersections(new Intersection(-Sqrt2over2, s), new Intersection(Sqrt2over2, s));
            xs[1].Prepare(ray, xs);
            double reflectance = xs[1].Schlick();
            Assert.IsTrue(reflectance == 1.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) schlick perpendicular ray. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SchlickPerpendicularRay() {
            //double Sqrt2over2 = Math.Sqrt(2) / 2;
            GlassSphere s = new GlassSphere();
            Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));
            List<Intersection> xs = Intersection.Intersections(new Intersection(-1, s), new Intersection(1, s));
            xs[1].Prepare(ray, xs);
            double reflectance = xs[1].Schlick();
            Assert.IsTrue(Ops.Equals(reflectance, 0.04));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) schlick n1 > n2. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SchlickN1GTN2() {
            //double Sqrt2over2 = Math.Sqrt(2) / 2;
            GlassSphere s = new GlassSphere();
            Ray ray = new Ray(new Point(0, 0.99, -2), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = Intersection.Intersections(new Intersection(1.8589, s));
            xs[0].Prepare(ray, xs);
            double reflectance = xs[0].Schlick();
            Assert.IsTrue(Ops.Equals(reflectance,0.48873));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) schlick combine. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SchlickCombine() {
            double Sqrt2over2 = Math.Sqrt(2) / 2;
            World w = defaultWorld;

            Ray r = new Ray(new Point(0, 0, -3), new RayTracerLib.Vector(0, -Sqrt2over2, Sqrt2over2));

            Plane floor = new Plane();
            floor.Transform = MatrixOps.CreateTranslationTransform(0, -1, 0);
            floor.Material.Reflective = 0.5;
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            w.AddObject(floor);

            Sphere ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = new Color(0.5, 0.5, 0.5);
            ball.Transform = MatrixOps.CreateTranslationTransform(0, -3.5, -0.5);
            w.AddObject(ball);

            List<Intersection> xs = Intersection.Intersections(new Intersection( Math.Sqrt(2), floor));

            xs[0].Prepare(r, xs);
            Color color = xs[0].Shade(w);
            Assert.IsTrue(color.Equals(new Color(0.93391, 0.69643, 0.69243)));

        }
    }
}
