using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracerLib;

namespace RayTracer
{
    class Program
    {
        static void Main(string[] args) {
            /* uint size = 255;
             Canvas c = new Canvas(size, size);
             Color Pix = new Color(0, 0, 0);
             String nl = Environment.NewLine;
             for (uint i = 0; i < c.Width; i++) {
                 for (uint j = 0; j < c.Height; j++) {
                     Pix.Red = ((double)i) / 255.0;
                     Pix.Blue = 1.0 -(((double)j) / (double) size);
                     // System.Console.WriteLine(Pix.ToString());
                     c.WritePixel(i, j, Pix);
                 }
             }
             */
            Canvas c = new Canvas(5, 3);
            Color c1 = new Color(1.5, 0, 0);
            Color c2 = new Color(0, 0.5, 0);
            Color c3 = new Color(-0.5, 0, 1);
            c.WritePixel(0, 0, c1);
            c.WritePixel(2, 1, c2);
            c.WritePixel(4, 2, c3);

            String ppm = c.ToPPM();
            //System.Console.Write(ppm);

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);
            Console.Write("Press Enter to finish ... ");
            Console.Read();
        }
    }
}
