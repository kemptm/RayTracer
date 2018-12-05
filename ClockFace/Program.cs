using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace ClockFace
{
    class Program
    {
        static void Main(string[] args) {
            uint fieldSize = 255;
            Canvas c = new Canvas(fieldSize, fieldSize);

            Matrix tr = MatrixOps.CreateTranslationTransform(fieldSize/2, fieldSize/2, 0); // the center of the field
            Matrix th = MatrixOps.CreateTranslationTransform(0,fieldSize / 3, 0); // 12 o'clock

            List<Point> points = new List<Point>();
            Point hour = new Point();
            
            for (int h = 0; h < 12; h++) {
                Matrix rot = MatrixOps.CreateRotationZTransform(h * 2 * Math.PI / 12);
                Matrix tf = (Matrix)( tr  * rot * th );
                Console.WriteLine("Hour+tf=" + hour.Transform(tf).ToString());
                Console.WriteLine("hour+seq=" + hour.Transform(th).Transform(rot).Transform(tr).ToString());
                points.Add(hour.Transform(th).Transform(rot).Transform(tr));
            }
            Color red = new Color(255, 0, 0);
            foreach(Point p in points) {
                c.WritePixel((uint)p.X, (uint)p.Y, red);
            }
            String ppm = c.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
