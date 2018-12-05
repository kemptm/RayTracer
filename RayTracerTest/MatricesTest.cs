///-------------------------------------------------------------------------------------------------
// file:	MatricesTest.cs
//
// summary:	Implements the matrices test class
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
    /// <summary>   (Unit Test Class) the matrices test. </summary>
    ///
    /// <remarks>   Kemp, 12/4/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class MatricesTest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public MatricesTest() {
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
        /// <summary>   (Unit Test Method) construct 4x 4 matrix. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Construct4x4Matrix() {
            Matrix<double> m = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });

            Assert.IsTrue(m[0, 0] == 1);
            Assert.IsTrue(m[0, 1] == 2);
            Assert.IsTrue(m[0, 2] == 3);
            Assert.IsTrue(m[0, 3] == 4);
            Assert.IsTrue(m[1, 0] == 5.5);
            Assert.IsTrue(m[1, 1] == 6.5);
            Assert.IsTrue(m[1, 2] == 7.5);
            Assert.IsTrue(m[1, 3] == 8.5);
            Assert.IsTrue(m[2, 0] == 9);
            Assert.IsTrue(m[2, 1] == 10);
            Assert.IsTrue(m[2, 2] == 11);
            Assert.IsTrue(m[2, 3] == 12);
            Assert.IsTrue(m[3, 0] == 13.5);
            Assert.IsTrue(m[3, 1] == 14.5);
            Assert.IsTrue(m[3, 2] == 15.5);
            Assert.IsTrue(m[3, 3] == 16.5);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) construct 2x 2 matrix. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Construct2x2Matrix() {
            Matrix<double> m = DenseMatrix.OfArray(new double[,] { { -3, 5 }, { 1, -2 } });

            Assert.IsTrue(m[0, 0] == -3);
            Assert.IsTrue(m[0, 1] == 5);
            Assert.IsTrue(m[1, 0] == 1);
            Assert.IsTrue(m[1, 1] == -2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) construct 3x3 matrix. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Construct3x3Matrix() {
            Matrix<double> m = DenseMatrix.OfArray(new double[,] {
                { -3,5,0},
                { 1,-2,7},
                { 0,1,1}, });

            Assert.IsTrue(m[0, 0] == -3);
            Assert.IsTrue(m[0, 1] == 5);
            Assert.IsTrue(m[0, 2] == 0);
            Assert.IsTrue(m[1, 0] == 1);
            Assert.IsTrue(m[1, 1] == -2);
            Assert.IsTrue(m[1, 2] == 7);
            Assert.IsTrue(m[2, 0] == 0);
            Assert.IsTrue(m[2, 1] == 1);
            Assert.IsTrue(m[2, 2] == 1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) compare matrices equal. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CompareMatricesEqual() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });

            Matrix b = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });

            Assert.IsTrue(a.Equals(b));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) compare matrices close equal. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CompareMatricesCloseEqual() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 0.9999999990,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });

            Matrix b = DenseMatrix.OfArray(new double[,] {
                { 1.0,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });
            //Assert.IsTrue(DoubleCompare.Equals(1.0, 0.9999999990));
            Assert.IsTrue(Ops.Equals(a, b));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) compare matrices not equal. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CompareMatricesNotEqual() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 0,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });

            Matrix b = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 5.5,6.5,7.5,8.5},
                { 9,10,11,12},
                { 13.5,14.5,15.5,16.5 } });

            Assert.IsFalse(a.Equals(b));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply matrices. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyMatrices() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 2,3,4,5},
                { 3,4,5,6},
                { 4,5,6,7 } });

            Matrix b = DenseMatrix.OfArray(new double[,] {
                { 0,1,2,4},
                { 1,2,4,8 },
                { 2,4,8,16},
                { 4,8,16,32 } });

            Matrix ab = DenseMatrix.OfArray(new double[,] {
                { 24,49,98,196},
                { 31,64,128,256 },
                { 38,79,158,316},
                { 45,94,188,376 } });

            Assert.IsTrue(Ops.Equals(a * b, ab));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply matri x coordinate tuple. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyMatriXTuple() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 2,4,4,2},
                { 8,6,4,1},
                { 0,0,0,1 } });
            RayTracerLib.Tuple t = new RayTracerLib.Tuple(1, 2, 3);
            RayTracerLib.Tuple r = new RayTracerLib.Tuple(18, 24, 33);

            Assert.IsTrue(MatrixOps.MatrixXTuple(a, t).Equals(r));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) identity matrix multiply. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void IdentityMatrixMultiply() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                { 1,2,3,4},
                { 2,4,4,2},
                { 8,6,4,1},
                { 0,0,0,1 } });
            Matrix i = DiagonalMatrix.CreateIdentity(4);

            Assert.IsTrue(a.Multiply(i).Equals(a));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) transposes this object. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Transpose() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {0,9,3,0},
                {9,8,0,8},
                {1,8,5,3},
                {0,0,5,8}
            });
            Matrix at = DenseMatrix.OfArray(new double[,] {
                {0,9,1,0},
                {9,8,8,0},
                {3,0,5,5},
                {0,8,3,8}
            });

            Assert.IsTrue(a.Transpose().Equals(at));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) transpose identity. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void TransposeIdentity() {
            Matrix i = DiagonalMatrix.CreateIdentity(4);
            Assert.IsTrue(i.Transpose().Equals(i));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) determinant of 2x 2 matrix. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DeterminantOf2x2Matrix() {
            Matrix a = DenseMatrix.OfArray(new Double[,] { { 1, 5 }, { -3, 2 } });
            Assert.IsTrue(a.Determinant() == 17);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) submatrix 3x 3. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Submatrix3x3() { 
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {1,5,0},
                {-3,2,7},
                {0,6,-3}
            });
            Matrix a2 = DenseMatrix.OfArray(new double[,] { { -3, 2 }, { 0, 6 } });

            Assert.IsTrue(MatrixOps.SubMatrix(a, 0, 2).Equals(a2));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) submatrix 4x 4. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Submatrix4x4() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {0,9,3,0},
                {9,8,0,8},
                {1,8,5,3},
                {0,0,5,8}
            });
            Matrix a2 = DenseMatrix.OfArray(new double[,] {
                {0,9,0},
                {9,8,8},
                {0,0,8}
            });
            Assert.IsTrue(MatrixOps.SubMatrix(a, 2, 2).Equals(a2));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) minor of 3x 3. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MinorOf3x3() {
            Matrix a2 = DenseMatrix.OfArray(new double[,] {
                {3,5,0},
                {2,-1,-7},
                {6,-1,5}
            });

            Assert.IsTrue(MatrixOps.Minor(a2, 1, 0) == 25);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) cofactor of 3x 3. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CofactorOf3x3() {
            Matrix a2 = DenseMatrix.OfArray(new double[,] {
                {3,5,0},
                {2,-1,-7},
                {6,-1,5}
            });

            Assert.IsTrue(MatrixOps.Minor(a2, 0, 0) == -12);
            Assert.IsTrue(MatrixOps.Cofactor(a2, 0, 0) == -12);
            Assert.IsTrue(MatrixOps.Minor(a2, 1, 0) == 25);
            Assert.IsTrue(MatrixOps.Cofactor(a2, 1, 0) == -25);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) determinant of 3x 3. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DeterminantOf3x3() {
            Matrix a2 = DenseMatrix.OfArray(new double[,] {
                {1,2,6},
                {-5,8,-4},
                {2,6,4}
            });
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a2, 0, 0), 56));
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a2, 0, 1), 12));
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a2, 0, 2), -46));
            Assert.IsTrue(Ops.Equals(a2.Determinant(), -196.0));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) determinant of 4x 4. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void DeterminantOf4x4() { 
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {-2,-8,3,5},
                {-3,1,7,3},
                {1,2,-9,6},
                {-6,7,7,-9}
            });
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a, 0, 0), 690));
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a, 0, 1), 447));
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a, 0, 2), 210));
            Assert.IsTrue(Ops.Equals(MatrixOps.Cofactor(a, 0, 3), 51));
            Assert.IsTrue(Ops.Equals(a.Determinant(), -4071));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) inverse of 4x 4 1. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void InverseOf4x4_1() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {-5,2,6,-8},
                {1,-5,1,8},
                {7,7,-6,-7},
                {1,-3,7,4}
            });
            Matrix ia = DenseMatrix.OfArray(new double[,] {
                {0.21805,0.45113,0.24060,-0.04511},
                {-0.80827,-1.45677,-0.44361,0.52068},
                {-0.07895,-0.22368,-0.05263, 0.19737},
                {-0.52256,-0.81391,-0.30075,0.30639}
            });
            Assert.IsTrue(Ops.Equals((Matrix)a.Inverse(), ia));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) inverse of 4x 4 2. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void InverseOf4x4_2() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {9,3,0,9},
                {-5,-2,-6,-3},
                {-4,9,6,4},
                {-7,6,6,2}
            });
            Matrix ia = DenseMatrix.OfArray(new double[,] {
                {-0.04074,-0.07778,0.14444,-0.22222},
                {-0.07778,0.03333,0.36667,-0.33333},
                {-0.02901,-0.14630,-0.10926,0.12963},
                {0.17778,0.06667,-0.26667,0.33333}
            });
            Assert.IsTrue(Ops.Equals((Matrix)a.Inverse(), ia));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply product by inverse. </summary>
        ///
        /// <remarks>   Kemp, 12/4/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyProductByInverse() {
            Matrix a = DenseMatrix.OfArray(new double[,] {
                {3,-9,7,3 },
                {3,-8,2,-9},
                {-4,4,4,1},
                {-6,5,-1,1}
            });
            Matrix b = DenseMatrix.OfArray(new double[,] {
                {8,2,2,2},
                {3,-1,7,0},
                {7,0,5,4},
                {6,-2,0,5}
            });
            Matrix c = (Matrix)(a * b);

            Assert.IsTrue(Ops.Equals((Matrix)(c * b.Inverse()), a));
        }
    }
}
