using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace StripesPatternTest
{
    class Program
    {
        static void Main(string[] args) {
            World w = new World();
            w.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));
            // w.AddLight(new RTLightPoint(new RTPoint(0, 10, -10), new Color(0.5, 0.5, 0.5)));

            Plane p = new Plane();
            p.Material.Color = new Color(1, 0.9, 0.9, 0);
            p.Material.Pattern = new CheckedPattern(new Color(0, 0, 0), new Color(1, 0.9, 0.9));
            p.Material.Reflective = 1;
            w.AddObject(p);

            Plane p1 = new Plane();
            //p1.Material.Color = new Color(0.9, 0.5, 1);
            p1.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0,0,3)*MatrixOps.CreateRotationXTransform(Math.PI / 2));
            p1.Material.Pattern = new CheckedPattern(new Color(0, 0, 0.5), new Color(1, 0.9, 0.9));
            w.AddObject(p1);

            Point rp = p1.Transform * new Point(1, 1, 0);

            List<Intersection> rp2 = p1.Intersect(new Ray(new Point(1, 1.5, -5), new RayTracerLib.Vector(0, 0, 20)));

            Sphere middle = new Sphere();
            middle.Transform = MatrixOps.CreateTranslationTransform(-1, 1, 0.5);
            middle.Material = new Material();
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            w.AddObject(middle);

            Sphere right = new Sphere();
            right.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(1.5, 0.5, -0.5) * MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5));
            right.Material = new Material();
            right.Material.Color = new Color(0.1, 0.5, 0.9);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            w.AddObject(right);

            Sphere left = new Sphere();
            left.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(-1.5, 0.33, -0.75) * MatrixOps.CreateScalingTransform(0.33, 0.33, 0.33));
            left.Material = new Material();
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            w.AddObject(left);

            Sphere org = new Sphere();
            org.Transform = (Matrix)( MatrixOps.CreateScalingTransform(0.2, 0.2, 0.2));
            org.Material = new Material();
            org.Material.Color = new Color(1, 0, 0);
            org.Material.Diffuse = 0.7;
            org.Material.Specular = 0.3;
            w.AddObject(org);
            /**/
            Camera camera = new Camera(400, 200, Math.PI / 2);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(3, 3, -5), new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);

            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
