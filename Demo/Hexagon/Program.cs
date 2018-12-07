using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace Hexagon
{
    class Program
    {
        static void Main(string[] args) {
            World w = new World();
            w.AddLight(new LightPoint(new Point(10, 15, -10), new Color(1, 1, 1)));
            // w.AddLight(new RTLightPoint(new RTPoint(0, 10, -10), new Color(0.5, 0.5, 0.5)));

            Group g1 = new Group();

            Sphere s = new Sphere();

            Cylinder c = new Cylinder();
            c.MinY = 0;
            c.MaxY = 3;
            c.Transform = MatrixOps.CreateRotationZTransform(-Math.PI / 2);

            Cube cube = new Cube();
            cube.Transform = (Matrix)(MatrixOps.CreateScalingTransform(0.25, 0.25, 0.25) *MatrixOps.CreateTranslationTransform(12, 8, 0)* MatrixOps.CreateRotationYTransform(Math.PI/6));
            cube.Material.Color = new Color(0, 1, 1);


            g1.AddObject(cube);
            g1.AddObject(s);
            g1.AddObject(c);
            Group sb = BoundingBox.Generate(s);
            Group cb = BoundingBox.Generate(c);
            Group cubb = BoundingBox.Generate(cube);
            //w.AddObject(sb);
            //w.AddObject(cb);
            //w.AddObject(cubb);
            Group g = new Group();
            g.AddObject(g1);
            //g1.AddObject(BoundingBox.Generate(g1));
            
            Group g2 = (Group) g1.Copy();
            g2.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(3, 0, 0) * MatrixOps.CreateRotationYTransform(Math.PI / 3.0));
            g.AddObject(g2);
            //g2.AddObject(BoundingBox.Generate(g2.Children[2]));
            //w.AddObject(BoundingBox.Generate(g2));

            Group g3 = (Group)g2.Copy();
            g3.Transform = (Matrix) (g3.Transform * MatrixOps.CreateTranslationTransform(3, 0, 0) * MatrixOps.CreateRotationYTransform(Math.PI / 3.0));
            g.AddObject(g3);
            //w.AddObject(BoundingBox.Generate(g3));

            Group g4 = (Group)g3.Copy();
            g4.Transform = (Matrix)(g4.Transform * MatrixOps.CreateTranslationTransform(3, 0, 0) * MatrixOps.CreateRotationYTransform(Math.PI / 3.0));
            g.AddObject(g4);
            //w.AddObject(BoundingBox.Generate(g4));

            Group g5 = (Group)g4.Copy();
            g5.Transform = (Matrix)(g5.Transform * MatrixOps.CreateTranslationTransform(3, 0, 0) * MatrixOps.CreateRotationYTransform(Math.PI / 3.0));
            g.AddObject(g5);
            //w.AddObject(BoundingBox.Generate(g5));

            Group g6 = (Group)g5.Copy();
            g6.Transform = (Matrix)(g6.Transform * MatrixOps.CreateTranslationTransform(3, 0, 0) * MatrixOps.CreateRotationYTransform(Math.PI / 3.0));
            g.AddObject(g6);
            //w.AddObject(BoundingBox.Generate(g6));
          
            w.AddObject(g);

            Console.WriteLine("Now rendering ...");
            Camera camera = new Camera(400, 400, Math.PI / 4);
            double cy = 5;
            double cz = -5;
            double cmult = 1.5;
            Point cameraPoint = new Point(0, cy * cmult, cz * cmult);
            camera.Transform = MatrixOps.CreateViewTransform(cameraPoint, new Point(0.5, 0, -4), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);
            Console.WriteLine("Now writing output ...");

            String ppm = image.ToPPM();
            String ppmname = ("ToPPM.ppm");
            System.IO.File.WriteAllText(@ppmname, ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
