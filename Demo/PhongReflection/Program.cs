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

            /// where we are looking from
            Point eyep = new Point(0, 0, -5);

            /// define the sphere
            Sphere s = new Sphere();
            s.Material = new Material();
            s.Material.Color = new Color(1, 0.2, 1);
            //s.Transform = RTMatrixOps.Scaling(1, 0.5, 1);

            /// define the light
            Point lightPos = new Point(-10, 10, -10);
            Color lightColor = new Color(1, 1, 1);
            LightPoint light = new LightPoint(lightPos, lightColor);

            /// Calculate canvas size and resolution
            const uint canvasResolution = 256;
            const uint canvasZ = 10;
            RayTracerLib.Vector tangentRay = (new Point(0, 1, 0) - eyep).Normalize();
            double canvasSize = ((tangentRay * (canvasZ - eyep.Z)).Y * 1.1) * 2;
            Point canvasOrigin = new Point(-canvasSize / 2.0, -canvasSize / 2.0, canvasZ);
            Canvas c = new Canvas(canvasResolution, canvasResolution);

            //now loop collecting canvas points.
            Point canvaspoint = new Point(0, 0, 10);
            Color red = new Color(255, 0, 0);
            for (int iy = 0; iy < canvasResolution; iy++) {
                for (int ix = 0; ix < canvasResolution; ix++) {
                    canvaspoint.X = (double)ix * canvasSize / canvasResolution + canvasOrigin.X;
                    canvaspoint.Y = (double)iy * canvasSize / canvasResolution + canvasOrigin.Y;
                    RayTracerLib.Vector rayv = (canvaspoint - eyep).Normalize();
                    Ray r = new Ray(eyep, rayv);
                    List<Intersection> xs = s.Intersect(r);
                    if (xs.Count != 0) {
                        Intersection hit = xs[0];
                        Point hitpoint = r.Position(hit.T);
                        RayTracerLib.Vector normal = ((Sphere)hit.Obj).NormalAt(hitpoint);
                        Color phong = Ops.Lighting(((Sphere)hit.Obj).Material, hit.Obj, light,hitpoint,-rayv,normal);
                        c.WritePixel((uint)ix, (uint)iy, phong);
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
