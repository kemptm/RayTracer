///-------------------------------------------------------------------------------------------------
// file:	PatternsTest.cs
//
// summary:	Implements the patterns test class
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
    /// <summary>   (Unit Test Class) the patterns test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class PatternsTest {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the sphere. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <returns>   The s. </returns>
        ///-------------------------------------------------------------------------------------------------

        Sphere s = new Sphere();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Colors. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <param name="parameter1">   The first parameter. </param>
        /// <param name="parameter2">   The second parameter. </param>
        /// <param name="parameter3">   The third parameter. </param>
        ///
        /// <returns>   A white. </returns>
        ///-------------------------------------------------------------------------------------------------

        Color white = new Color(1, 1, 1);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Colors. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///
        /// <param name="parameter1">   The first parameter. </param>
        /// <param name="parameter2">   The second parameter. </param>
        /// <param name="parameter3">   The third parameter. </param>
        ///
        /// <returns>   A black. </returns>
        ///-------------------------------------------------------------------------------------------------

        Color black = new Color(0, 0, 0);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Class) a test pattern. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected class TestPattern : Pattern {

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

        public PatternsTest() {
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
        /// <summary>   (Unit Test Method) creates stripe pattern. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateStripePattern() {
            StripePattern p = new StripePattern(white, black);
            Assert.IsTrue(p.A.Equals(white));
            Assert.IsTrue(p.B.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) stripe pattern consistent in y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void StripePatternConsistentInY() {
            StripePattern p = new StripePattern(white, black);
            Assert.IsTrue(p.PatternAt(new Point(0, 0, 0)).Equals(white));
            Assert.IsTrue(p.PatternAt(new Point(0, 1, 0)).Equals(white));
            Assert.IsTrue(p.PatternAt(new Point(0, 2, 0)).Equals(white));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) stripe pattern consistent in z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void StripePatternConsistentInZ() {
            StripePattern p = new StripePattern(white, black);
            Assert.IsTrue(p.PatternAt(new Point(0, 0, 0)).Equals(white));
            Assert.IsTrue(p.PatternAt(new Point(0, 0, 1)).Equals(white));
            Assert.IsTrue(p.PatternAt(new Point(0, 0, 2)).Equals(white));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) stripe pattern alternatest in x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void StripePatternAlternatestInX() {
            StripePattern p = new StripePattern(white, black);
            Assert.IsTrue(p.PatternAt(new Point(0, 0, 0)).Equals(white));
            Assert.IsTrue(p.PatternAt(new Point(0.9, 0, 0)).Equals(white));
            Assert.IsTrue(p.PatternAt(new Point(1, 0, 0)).Equals(black));
            Assert.IsTrue(p.PatternAt(new Point(-0.1, 0, 0)).Equals(black));
            Assert.IsTrue(p.PatternAt(new Point(-1, 0, 0)).Equals(black));
            Assert.IsTrue(p.PatternAt(new Point(-1.1, 0, 0)).Equals(white));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) lighting with a pattern. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void LightingWithAPattern() {
            Material m = new Material {
                Pattern = new StripePattern(white, black),
                Ambient = 1,
                Diffuse = 0,
                Specular = 0
            };
            RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0, -1);
            RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
            LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
            Color c1 = Ops.Lighting(m, s, light, new Point(0.9, 0, 0), eyev, normalv, false);
            Color c2 = Ops.Lighting(m, s, light, new Point(1.1, 0, 0), eyev, normalv, false);
            Assert.IsTrue(c1.Equals(white));
            Assert.IsTrue(c2.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) stripes with object transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void StripesWithObjectTransformation() {
            Shape s = new Sphere {
                Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
            };
            Pattern p = new StripePattern(black, white);
            Color c = p.PatternAtObject(s, new Point(1.5, 0, 0));
            Assert.IsTrue(c.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) stripes with pattern transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void StripesWithPatternTransformation() {
            Shape s = new Sphere();
            Pattern p = new StripePattern(black, white) {
                Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
            };
            Color c = p.PatternAtObject(s, new Point(1.5, 0, 0));
            Assert.IsTrue(c.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) stripes with object and pattern transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void StripesWithObjectAndPatternTransformation() {
            Shape s = new Sphere {
                Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
            };
            Pattern p = new StripePattern(black, white) {
                Transform = MatrixOps.CreateTranslationTransform(0.5, 0, 0)
            };
            Color c = p.PatternAtObject(s, new Point(2.5, 0, 0));
            Assert.IsTrue(c.Equals(black));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) default pattern transform. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DefaultPatternXform() {
            Pattern pattern = new TestPattern();
            Assert.IsTrue(pattern.Transform.Equals(DenseMatrix.CreateIdentity(4)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) pattern assign transform. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PatternAssignXform() {
            Pattern pattern = new TestPattern {
                Transform = MatrixOps.CreateTranslationTransform(1, 2, 3)
            };
            Assert.IsTrue(pattern.Transform.Equals(MatrixOps.CreateTranslationTransform(1, 2, 3)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) pattern with object transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PatternWithObjectTransformation() {
            Shape shape = new Sphere() {
                Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
            };
            Pattern pattern = new TestPattern();
            Color c = pattern.PatternAtObject(shape, new Point(2, 3, 4));
            Assert.IsTrue(c.Equals(new Color(1, 1.5, 2)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) pattern with pattern transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PatternWithPatternTransformation() {
            Shape shape = new Sphere();
            Pattern pattern = new TestPattern {
                Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
            };
            Color c = pattern.PatternAtObject(shape, new Point(2, 3, 4));
            Assert.IsTrue(c.Equals(new Color(1, 1.5, 2)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) pattern with both transformation. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PatternWithBothTransformation() {
            Shape shape = new Sphere() {
                Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
            };
            Pattern pattern = new TestPattern {
                Transform = MatrixOps.CreateTranslationTransform(0.5, 1, 1.5)
            };
            Color c = pattern.PatternAtObject(shape, new Point(2.5, 3, 3.5));
            Assert.IsTrue(c.Equals(new Color(0.75, 0.5, 0.25)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) gradient interpolates. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GradientInterpolates() {
            Pattern pattern = new GradientPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0.25, 0, 0)).Equals(new Color(0.25,0.25,0.25)));
            Assert.IsTrue(pattern.PatternAt(new Point(0.5, 0, 0)).Equals(new Color(0.5, 0.5, 0.5)));
            Assert.IsTrue(pattern.PatternAt(new Point(0.75, 0, 0)).Equals(new Color(0.75,0.75,0.75)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ring pattern in x coordinate and z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RingPatternInXAndZ() {
            Pattern pattern = new RingPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(1, 0, 0)).Equals(white));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 1)).Equals(white));
            Assert.IsTrue(pattern.PatternAt(new Point(0.708, 0, 0.708)).Equals(white));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) checked repeats in x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CheckedRepeatsInX() {
            Pattern pattern = new CheckedPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            //Assert.IsTrue(pattern.PatternAt(new RTPoint(0.99, 0, 0)).Equals(black));
            //Assert.IsTrue(pattern.PatternAt(new RTPoint(1.01, 0, 0)).Equals(white));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) checked repeats in z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CheckedRepeatsInZ() {
            Pattern pattern = new CheckedPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0.99)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 1.01)).Equals(white));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) checked 3D repeats in x coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checked3DRepeatsInX() {
            Pattern pattern = new Checked3DPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0.99, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(1.01, 0, 0)).Equals(white));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) checked 3D repeats in y coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checked3DRepeatsInY() {
            Pattern pattern = new Checked3DPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0.99, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 1.01, 0)).Equals(white));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) checked 3D repeats in z coordinate. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checked3DRepeatsInZ() {
            Pattern pattern = new Checked3DPattern(black, white);
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 0.99)).Equals(black));
            Assert.IsTrue(pattern.PatternAt(new Point(0, 0, 1.01)).Equals(white));

        }
    }
}
