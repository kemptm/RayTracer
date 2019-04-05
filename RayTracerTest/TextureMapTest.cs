///-------------------------------------------------------------------------------------------------
// file:	TextureMapTest.cs
//
// summary:	Implements the texture map test class
///-------------------------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracerLib;
using System.Drawing;

namespace RayTracerTest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Unit Test Class) a texture map test. </summary>
    ///
    /// <remarks>   Kemp, 3/8/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [TestClass]
    public class TextureMapTest
    {
        [TestMethod]
        public void CreateTextureMap() {
            TextureMap tm = new TextureMap();
            Assert.IsTrue(tm != null);
        }

        [TestMethod]
        public void LoadTextureMap() {
            TextureMap tm = null;
            try {
                tm = new TextureMap("Sting_Base_Color.png");
                Assert.IsNotNull(tm);
                Assert.IsNotNull(tm.Tm);
            }
            catch (Exception e) {
                Assert.IsTrue(false);
            }
            Assert.IsTrue(tm.Tm.Height == 4096);
            Assert.IsTrue(tm.Tm.Width == 4096);
            System.Drawing.Color c = tm.Tm.GetPixel(2047, 2047);
            Assert.IsTrue(c == System.Drawing.Color.FromArgb(156, 175, 178));
        }

        [TestMethod]
        public void GetTextureColor() {
            TextureMap tm = null;
            try {
                tm = new TextureMap("Sting_Base_Color.png");
                Assert.IsNotNull(tm);
                Assert.IsNotNull(tm.Tm);
            }
            catch (Exception e) {
                Assert.IsTrue(false);
            }
            SmoothTriangle t = new SmoothTriangle(new RayTracerLib.Point(1, 0, 0), new RayTracerLib.Point(0, 1, 0), new RayTracerLib.Point(0, 0, 0));
            t.AddNormals(new Vector(0.5, 0, 0), new Vector(0, 0.5, 0), new Vector(0, 0, 0.5));
            t.AddTexture(new RayTracerLib.Point(1, 0, 0), new RayTracerLib.Point(0, 1, 0), new RayTracerLib.Point(0, 0, 0));
            Intersection i = new Intersection(5, t, 0.350, 0.350);
            RayTracerLib.Color c = tm.MapTexture(i);
            Assert.IsTrue(c.Equals(new RayTracerLib.Color(0.59607, 0.6745, 0.70588)));
        }
    }
}
