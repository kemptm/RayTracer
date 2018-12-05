///-------------------------------------------------------------------------------------------------
// file:	CSGTests.cs
//
// summary:	Implements the CSG tests class
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
    /// <summary>   Summary description for CSGTests. </summary>
    ///
    /// <remarks>   Kemp, 11/26/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class CSGTest
    {
        protected class CSGOpTests
        {
            public CSG.Ops op;
            public bool lhit;
            public bool inl;
            public bool inr;
            public bool result;

            public CSGOpTests() {

            }

            public CSGOpTests(CSG.Ops o,bool lh, bool il, bool ir, bool res) {
                op = o;
                lhit = lh;
                inl = il;
                inr = ir;
                result = res;
            }
        }

        protected class OpTest
        {
            public CSG.Ops operation;
            public int x0;
            public int x1;

            public OpTest() { }
            public OpTest(CSG.Ops o, int a, int b) {
                operation = o;
                x0 = a;
                x1 = b;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CSGTest() {
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
        /// <summary>   (Unit Test Method) Create a CSG. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateCSG() {
            Sphere s1 = new Sphere();
            Cube s2 = new Cube();
            CSG c = new CSG(CSG.Ops.Union, s1, s2);

            Assert.IsTrue(c.Operation == CSG.Ops.Union);
            Assert.IsTrue(c.Left == s1);
            Assert.IsTrue(c.Right == s2);
            Assert.IsTrue(s1.Parent == c);
            Assert.IsTrue(s2.Parent == c);
            Assert.IsTrue(c.Bounds.MinCorner.Equals(new Point(-1, -1, -1)));
            Assert.IsTrue(c.Bounds.MaxCorner.Equals(new Point(1, 1, 1)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) test the union rule of IntersectionAllowed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void UnionRule() {
            List<CSGOpTests> ts = new List<CSGOpTests>();
            ts.Add(new CSGOpTests(CSG.Ops.Union, true,  true,  true,  false));        
            ts.Add(new CSGOpTests(CSG.Ops.Union, true,  true,  false, true));
            ts.Add(new CSGOpTests(CSG.Ops.Union, true,  false, true,  false));
            ts.Add(new CSGOpTests(CSG.Ops.Union, true,  false, false, true));
            ts.Add(new CSGOpTests(CSG.Ops.Union, false, true,  true,  false));
            ts.Add(new CSGOpTests(CSG.Ops.Union, false, true,  false, false));
            ts.Add(new CSGOpTests(CSG.Ops.Union, false, false, true,  true));
            ts.Add(new CSGOpTests(CSG.Ops.Union, false, false, false, true));

            foreach (CSGOpTests t in ts) {
                Assert.IsTrue(CSG.IntersectionAllowed(t.op, t.lhit, t.inl, t.inr) == t.result);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) test the intersection rule of IntersectionAllowed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IntersectionRule() {
            List<CSGOpTests> ts = new List<CSGOpTests>();
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, true,  true,  true, true));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, true,  true,  false, false));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, true,  false, true,  true));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, true,  false, false, false));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, false, true,  true,  true));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, false, true,  false, true));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, false, false, true,  false));
            ts.Add(new CSGOpTests(CSG.Ops.Intersection, false, false, false, false));

            foreach (CSGOpTests t in ts) {
                Assert.IsTrue(CSG.IntersectionAllowed(t.op, t.lhit, t.inl, t.inr) == t.result);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) test the difference rule of IntersectionAllowed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DifferenceRule() {
            List<CSGOpTests> ts = new List<CSGOpTests>();
            ts.Add(new CSGOpTests(CSG.Ops.Difference, true,  true,  true,  false));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, true,  true,  false, true));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, true,  false, true,  false));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, true,  false, false, true));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, false, true,  true,  true));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, false, true,  false, true));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, false, false, true,  false));
            ts.Add(new CSGOpTests(CSG.Ops.Difference, false, false, false, false));

            foreach (CSGOpTests t in ts) {
                Assert.IsTrue(CSG.IntersectionAllowed(t.op, t.lhit, t.inl, t.inr) == t.result);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) filter list of intersections. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

       [TestMethod]
        public void FilterListOfIntersections() {
            List<OpTest> ts = new List<OpTest>();
            ts.Add(new OpTest(CSG.Ops.Union, 0, 3));
            ts.Add(new OpTest(CSG.Ops.Intersection, 1, 2));
            ts.Add(new OpTest(CSG.Ops.Difference, 0, 1));

            Sphere s1 = new Sphere();
            Cube s2 = new Cube();
            List<Intersection> xs = Intersection.Intersections(new Intersection(1, s1), new Intersection(2, s2), new Intersection(3, s1), new Intersection(4, s2));
            foreach(OpTest t in ts) {
                CSG c = new CSG(t.operation, s1, s2);
                List<Intersection> fs = c.FilterIntersections(xs);
                Assert.IsTrue(fs.Count == 2);
                Assert.IsTrue(fs[0] == xs[t.x0]);
                Assert.IsTrue(fs[1] == xs[t.x1]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray misses CSG. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayMissesCSG() {
            Sphere s1 = new Sphere();
            Cube s2 = new Cube();
            CSG c = new CSG(CSG.Ops.Union, s1, s2);
            Ray r = new Ray(new Point(0, 2, -5), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();

            xs = c.LocalIntersect(r);

            Assert.IsTrue(xs.Count == 0);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ray hits CSG. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RayHitsCSG() {
            Sphere s1 = new Sphere();
            Cube s2 = new Cube();
            s2.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 0, 0.5));
            CSG c = new CSG(CSG.Ops.Union, s1, s2);
            Ray r = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 0, 1));
            List<Intersection> xs = new List<Intersection>();

            xs = c.LocalIntersect(r);

            Assert.IsTrue(xs.Count == 2);
            Assert.IsTrue(xs[0].T == 4);
            Assert.IsTrue(xs[0].Obj.Equals(s1));
            Assert.IsTrue(xs[1].T == 6.5);
            Assert.IsTrue(xs[1].Obj.Equals(s2));

        }
    }
}
