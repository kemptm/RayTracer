///-------------------------------------------------------------------------------------------------
// file:	Drawing_on_CanvasTest.cs
//
// summary:	Implements the drawing on canvas test class
///-------------------------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Unit Test Class) a drawing on canvas test. </summary>
    ///
    /// <remarks>   Kemp, 12/2/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class Drawing_on_CanvasTest {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) creates the color. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateColor() {
            Color c = new RayTracerLib.Color(-0.5, 0.4, 1.7);
            Assert.IsTrue((c.Red == -0.5) && (c.Green == 0.4) && (c.Blue == 1.7));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) adds colors. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void AddColors() {
            Color c1 = new Color(0.9, 0.6, 0.75);
            Color c2 = new Color(0.7, 0.1, 0.25);
            Assert.IsTrue((c1 + c2).IsEqual(new Color(1.6, 0.7, 1.0)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) subtract color. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SubtractColor() {
            Color c1 = new Color(0.9, 0.6, 0.75);
            Color c2 = new Color(0.7, 0.1, 0.25);
            Assert.IsTrue((c1 - c2).IsEqual(new Color(0.2, 0.5, 0.5)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply color by scalar. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyColorByScalar() {
            Color c1 = new Color(0.2, 0.3, 0.4);
            Assert.IsTrue((c1 * 2.0).IsEqual(new Color(0.4, 0.6, 0.8)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) multiply colors. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void MultiplyColors() {
            Color c1 = new Color(1.0, 0.2,0.4);
            Color c2 = new Color(0.9,1.0,0.1);
            Assert.IsTrue((c1 * c2).IsEqual(new Color(0.9, 0.2, 0.04)));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) creates the canvas. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CreateCanvas() {
            Canvas c = new Canvas(10, 20);
            Color black = new Color(0, 0, 0);
            Assert.IsTrue(c.Width == 10);
            Assert.IsTrue(c.Height == 20);
            for (uint i = 0;i < c.Width; i++) {
                for (uint j = 0;j < c.Height; j++) {
                   Assert.IsTrue(c.PixelAt(i, j).IsEqual(black));
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) writes the pixel to canvas. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void WritePixelToCanvas() {
            Canvas c = new Canvas(10, 20);
            Color Red = new Color(1.0, 0, 0);
            c.WritePixel(2, 3, Red);
            Assert.IsTrue(c.PixelAt(2, 3).IsEqual(Red));

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) ppm header. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void PPMHeader() {
            Canvas c = new Canvas(5, 3);
            Color Red = new Color(1.0, 0, 0);
            String nl = Environment.NewLine;
            String ppm = c.ToPPM();
            Assert.IsTrue(ppm.Substring(0,14).Equals("P3" + nl + "5 3" + nl + "255" + nl));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   (Unit Test Method) full ppm. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void FullPPM() {
            String nl = Environment.NewLine;
            Canvas c = new Canvas(5, 3);
            Color c1 = new Color(1.5, 0, 0);
            Color c2 = new Color(0, 0.5, 0);
            Color c3 = new Color(-0.5, 0, 1);
            c.WritePixel(0, 0, c1);
            c.WritePixel(2, 1, c2);
            c.WritePixel(4, 2, c3);
            String ppm = c.ToPPM();
            Assert.IsTrue(ppm == ("P3" + nl 
                + "5 3" + nl 
                + "255" + nl 
                + "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0 " + nl 
                + "0 0 0 0 0 0 0 127 0 0 0 0 0 0 0 " + nl 
                + "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255 " + nl 
                + nl));
        }

    }
}
