using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;
namespace TriangleDemo
{
    class Program
    {
        static void Main(string[] args) {
            World w = new World();
            Group g = new Group();
            w.AddLight(new LightPoint(new Point(60, 45, -60), new Color(1, 1, 1)));
            w.AddLight(new LightPoint(new Point(2, 20, -20), new Color(.25, 0, .5)));
 
            /// three planes make up the background.
            Plane floor = new Plane();
            floor.Material.Pattern = new Checked3DPattern(new Color(1, 1, 1), new Color(0.75, 0.75, 0.75));
            floor.Material.Pattern.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            w.AddObject(floor);

            Plane wallz = (Plane)floor.Copy();
            wallz.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
            w.AddObject(wallz);
           
            Plane wallx = (Plane)floor.Copy();
            wallx.Transform = (Matrix)(MatrixOps.CreateRotationZTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
            w.AddObject(wallx);

            Triangle t = new Triangle(new Point(4, 0, 0),new Point(0,4,0), new Point(0,0,4));
            t.Material.Color = new Color(1, 0, 0);
            g.AddObject(t);
            t = new Triangle(new Point(4, 0, 0), new Point(0, 4, 0), new Point(0, 0, -4));
            t.Material.Color = new Color(0, 1, 0);
            g.AddObject(t);
            t = new Triangle(new Point(-4, 0, 0), new Point(0, 4, 0), new Point(0, 0, 4));
            t.Material.Color = new Color(0, 0, 1);
            g.AddObject(t);
            t = new Triangle(new Point(-4, 0, 0), new Point(0, 4, 0), new Point(0, 0, -4));
            t.Material.Color = new Color(1, 1, 0);
            g.AddObject(t);

            g.Transform = MatrixOps.CreateTranslationTransform(6, 3, -12);

            w.AddObject(g);

            Camera camera = new Camera(400, 400, Math.PI / 3);
            //            camera.Transform = RTMatrixOps.ViewTransform(new RTPoint(8, 5, 8), new RTPoint(0, 0, 0), new RTVector(0, 1, 0));
            camera.Transform = MatrixOps.CreateViewTransform(new Point(12, 12, -36), new Point(6, 1, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);

            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
