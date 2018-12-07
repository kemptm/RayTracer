using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace DebugLightAndShading
{
    class Program
    {
        static void Main(string[] args) {
            {
                Sphere s = new Sphere();
                RayTracerLib.Vector n = s.NormalAt(new Point(1, 0, 0));
                bool foo = n.Equals(new RayTracerLib.Vector(1, 0, 0));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                Sphere s = new Sphere();
                Material m = s.Material;
                bool foo = m.Equals(new Material());

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                Material m = new Material();
                Point position = new Point();

                RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0, -1);
                RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
                Sphere s = new Sphere();

                LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
                Color result = Ops.Lighting(m, s, light, position, eyev, normalv);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                Material m = new Material();
                Point position = new Point();

                RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
                RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
                Sphere s = new Sphere();
                LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
                Color result = Ops.Lighting(m, s, light, position, eyev, normalv);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                //SphereNormalScaled() {
                    Sphere s = new Sphere();
                    s.Transform = MatrixOps.CreateScalingTransform(1, 1, 1);
                Point p = new Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
                RayTracerLib.Vector n = s.NormalAt(p);
                    bool foo = (n.Equals(new RayTracerLib.Vector(0, 0.97014, -0.24254)));
               

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }

            Console.Write("Press Enter to finish ... ");
            Console.Read();
        }
    }
}
