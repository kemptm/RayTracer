using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace SphereShadow
{
    class Program
    {
        static void Main(string[] args) {
            
            Point light = new Point(0, 0, -5);
            Sphere s = new Sphere();
            const uint canvasResolution = 128;
            const uint canvasZ = 10;
            RayTracerLib.Vector tangentRay = (new Point(0, 1, 0) - light).Normalize();
            double canvasSize = ((tangentRay * (canvasZ - light.Z)).Y * 1.1) * 2;
            Point canvasOrigin = new Point(-canvasSize / 2.0, -canvasSize / 2.0, canvasZ);            

            Canvas c = new Canvas(canvasResolution, canvasResolution);

            Point canvaspoint = new Point(0,0,10);
            Color red = new Color(255, 0, 0);
            for (int iy = 0; iy < canvasResolution; iy++) {
                for (int ix = 0; ix < canvasResolution; ix++) {
                    canvaspoint.X = (double)ix * canvasSize / canvasResolution + canvasOrigin.X;
                    canvaspoint.Y = (double)iy * canvasSize / canvasResolution + canvasOrigin.Y;
                    RayTracerLib.Vector rayv = (canvaspoint - light).Normalize();
                    Ray r = new Ray(light, rayv);
                    List<Intersection> xs = s.Intersect(r);
                    if (xs.Count != 0) {
                        c.WritePixel((uint)ix, (uint)iy, red);
                     }
                }
            }
            String ppm = c.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();
        }
    }
}
