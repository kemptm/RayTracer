///-------------------------------------------------------------------------------------------------
// file:	TextureMap.cs
//
// summary:	Implements the texture map class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Map of textures. </summary>
    ///
    /// <remarks>   Kemp, 3/8/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class TextureMap
    {
        protected Bitmap bm;
        protected String fileName;

        public String FileName { get { return fileName; } set { fileName = value; } }
        public Bitmap Tm { get { return bm; } }
        private readonly object _locker = new object();

        public TextureMap() {
            ;
        }

        public TextureMap(String fn) {
            fileName = fn;
            LoadMap(fn);
        }

        public void LoadMap(String fn) {
            try {
                bm = (Bitmap)Image.FromFile(fn);
            }
            catch (Exception e) {
                Console.WriteLine("Texture File Exception: " + e.Message);
            }
        }

        public Color MapTexture(Intersection i) {
            /// if no texture verticies, then return black
            Color c;
            int x = 0, y = 0;
            lock (_locker) {
                double w = 1 - (i.U + i.V);
                if (i.Obj is Triangle) {
                    Triangle t = (Triangle)i.Obj;
                    x = Math.Min((int)((t.T0.X * w + t.T1.X * i.U + t.T2.X * i.V) * bm.Width), bm.Width - 1);
                    y = Math.Min((int)(bm.Height - Math.Min((t.T0.Y * w + t.T1.Y * i.U + t.T2.Y * i.V), 1.0) * bm.Height), bm.Height - 1);
                    if (t.T0 != null && t.T0.X != 0) {
                        c = new Color(bm.GetPixel(x, y));
                    }
                    else {
                        c = new Color(0, 0, 0);
                    }
                }
                else if (i.Obj is Sphere) {
                    //Sphere s = (Sphere)i.Obj;
                    //x = ((int)(i.Point.X>=0?(i.Point.Z<0?i.Point.X/4:i.Point.X/4+0.25):(i.Point.Z<0?-i.Point.X/4+0.75:-i.Point.X/4+0.50)) * tm.Width) % tm.Width;
                    Point localPoint = i.Point.Transform((Matrix)i.Obj.Transform.Inverse());
                    
                    double u = 0.5 + Math.Atan2(localPoint.Z, localPoint.X) / (2 * Math.PI);
                    double v = 0.5 - Math.Asin(localPoint.Y) / Math.PI;
                    x = Math.Min((int)(u * bm.Width),bm.Width - 1);
                    y = Math.Min((int)(v * bm.Height), bm.Height - 1);
                    c = new Color(bm.GetPixel(x, y));
                }
                else c = new Color(0, 0, 0);
            }
            //Console.WriteLine("( {0},{1}) = [{2}, {3}, {4}]", x, y, c.Red, c.Green, c.Blue);
            return c;
        }
    }
}
