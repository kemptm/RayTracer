using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace GlassSphereWithAirBubble
{
    class Program
    {
        static void Main(string[] args) {
            World w = new World();
            w.AddLight(new LightPoint(new Point(10, 10, 10), new Color(1, 1, 1)));

            Plane floor = new Plane();
            floor.Material.Pattern = new CheckedPattern();
            w.AddObject(floor);

            GlassSphere a = new GlassSphere();
            a.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 15, 0) * MatrixOps.CreateScalingTransform(2, 2, 2));
            a.Material.Ambient = 0;
            a.Material.Color = new Color(0, 0, 0);
            a.Material.Reflective = 0.5;
            w.AddObject(a);

            Sphere b = new Sphere();
            b.Transform = MatrixOps.CreateTranslationTransform(3, 3, 0);
            b.Material.Color = new Color(1, 0, 0);
            w.AddObject(b);

            GlassSphere ainner = new GlassSphere();
            ainner.Transform = MatrixOps.CreateTranslationTransform(0, 15, 0);
            ainner.Material.Ambient = 0;
            ainner.Material.Color = new Color(0, 0, 0);
            ainner.Material.RefractiveIndex = 1;
            ainner.Material.Reflective = 0.5;
            w.AddObject(ainner);

            Camera camera = new Camera(400, 400, Math.PI / 3);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(0, 20, 0), new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));

            Canvas image = w.Render(camera);

            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
