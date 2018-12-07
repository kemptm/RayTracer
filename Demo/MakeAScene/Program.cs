using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace MakeAScene
{
    class Program
    {
        static void Main(string[] args) {
            World w = new World();
            w.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));
            Sphere floor = new Sphere();
            floor.Transform = MatrixOps.CreateScalingTransform(10, 0.01, 10);
            floor.Material = new Material();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;
            floor.Material.Transparency = 1;
            floor.Material.RefractiveIndex = 1;
            w.AddObject(floor);
            
            Sphere leftWall = new Sphere();
            leftWall.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 0, 5) * MatrixOps.CreateRotationYTransform(-Math.PI / 4) * MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateScalingTransform(10, 0.01, 10));
            leftWall.Material = floor.Material;
            w.AddObject(leftWall);

            Sphere rightWall = new Sphere();
            rightWall.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 0, 5) * MatrixOps.CreateRotationYTransform(Math.PI / 4) * MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateScalingTransform(10, 0.01, 10));
            rightWall.Material = floor.Material;
            w.AddObject(rightWall);

            Sphere middle = new Sphere();
            middle.Transform = MatrixOps.CreateTranslationTransform(-0.5, 1, 0.5);
            middle.Material = new Material();
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            middle.Material.Transparency = 0.5;
            //middle.Material.RefractiveIndex = 1.02;
            w.AddObject(middle);

            Sphere right = new Sphere();
            right.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(1.5, 0.5, -0.5) *MatrixOps.CreateScalingTransform(0.5,0.5,0.5));
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

            Sphere under = new Sphere();
            under.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(-1.5, -1, -0.75) * MatrixOps.CreateScalingTransform(0.33, 0.33, 0.33));
            under.Material = new Material();
            under.Material.Color = new Color(1, 0.8, 0.6);
            under.Material.Diffuse = 0.7;
            under.Material.Specular = 0.3;
            w.AddObject(under);

            Camera camera = new Camera(400, 200, Math.PI / 3);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(0, 2.5, -10), new Point(0, 1, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);

            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
