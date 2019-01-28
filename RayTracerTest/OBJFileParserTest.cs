///-------------------------------------------------------------------------------------------------
// file:	OBJFileParserTest.cs
//
// summary:	Implements the object file parser test class
///-------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   OBJ File Parser Tests. </summary>
    ///
    /// <remarks>   Kemp, 11/21/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class OBJFileParserTest
    {
        public OBJFileParserTest() {
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) Reject lines that do not conform to syntax. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Gibberish() {
            OBJFileParser p = new OBJFileParser("Gibberish.obj");
            Assert.IsTrue(p.Verticies.Count == 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) recognize triangles. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RecognizeTriangles() {
            OBJFileParser p = new OBJFileParser("RecognizeTriangles.obj");
            Group g = p.DefaultGroup;
            Triangle t1 = (Triangle)g.Children[0];
            Triangle t2 = (Triangle)g.Children[1];

            Assert.IsTrue(t1.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t1.V1.Equals(p.Verticies[1]));
            Assert.IsTrue(t1.V2.Equals(p.Verticies[2]));
            Assert.IsTrue(t2.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t2.V1.Equals(p.Verticies[2]));
            Assert.IsTrue(t2.V2.Equals(p.Verticies[3]));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) recognize polygons and triangulate. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void RecognizePolygonsAndTriangulate() {
            OBJFileParser p = new OBJFileParser("RecognizePolygonsAndTriangulate.obj");
            Group g = p.DefaultGroup;
            Triangle t1 = (Triangle)g.Children[0];
            Triangle t2 = (Triangle)g.Children[1];
            Triangle t3 = (Triangle)g.Children[2];

            Assert.IsTrue(t1.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t1.V1.Equals(p.Verticies[1]));
            Assert.IsTrue(t1.V2.Equals(p.Verticies[2]));
            Assert.IsTrue(t2.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t2.V1.Equals(p.Verticies[2]));
            Assert.IsTrue(t2.V2.Equals(p.Verticies[3]));
            Assert.IsTrue(t3.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t3.V1.Equals(p.Verticies[3]));
            Assert.IsTrue(t3.V2.Equals(p.Verticies[4]));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) named groups. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void NamedGroups() {
            OBJFileParser p = new OBJFileParser("NamedGroups.obj");
            Group g1 = p.GetGroup("FirstGroup");
            Group g2 = p.GetGroup("SecondGroup");
            Triangle t1 = (Triangle)g1.Children[0];
            Triangle t2 = (Triangle)g2.Children[0];

            Assert.IsTrue(t1.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t1.V1.Equals(p.Verticies[1]));
            Assert.IsTrue(t1.V2.Equals(p.Verticies[2]));
            Assert.IsTrue(t2.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t2.V1.Equals(p.Verticies[2]));
            Assert.IsTrue(t2.V2.Equals(p.Verticies[3]));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) gets group list. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetGroupList() {
            OBJFileParser p = new OBJFileParser("NamedGroups.obj");
            List<Group> gs = p.Groups;
            Group g1 = gs.Find(n => n.Name.Equals("FirstGroup"));
            Group g2 = gs.Find(n => n.Name.Equals("SecondGroup"));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) reads big object file. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ReadBigOBJFile() {
            OBJFileParser p = new OBJFileParser("teddy.obj");
            Assert.IsTrue(p != null);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) vertex normal records. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void VertexNormalRecords() {
            OBJFileParser p = new OBJFileParser("VertexNormalRecords.obj");
            Assert.IsTrue(p.Normals[0].Equals(new Vector(0, 0, 1)));
            Assert.IsTrue(p.Normals[1].Equals(new Vector(0.707, 0, -0.707)));
            Assert.IsTrue(p.Normals[2].Equals(new Vector(1, 2, 3)));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) faces with normals. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void FacesWithNormals() {
            OBJFileParser p = new OBJFileParser("FacesWithNormals.obj");
            Group g = p.DefaultGroup;
            SmoothTriangle t1 = (SmoothTriangle) g.Children[0];
            SmoothTriangle t2 = (SmoothTriangle)g.Children[1];
            Assert.IsTrue(t1.V0.Equals(p.Verticies[0]));
            Assert.IsTrue(t1.V1.Equals(p.Verticies[1]));
            Assert.IsTrue(t1.V2.Equals(p.Verticies[2]));
            Assert.IsTrue(t1.N0.Equals(p.Normals[2]));
            Assert.IsTrue(t1.N1.Equals(p.Normals[0]));
            Assert.IsTrue(t1.N2.Equals(p.Normals[1]));
            Assert.IsTrue(t2.Equals(t1));
        }
    }
}
