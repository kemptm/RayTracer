///-------------------------------------------------------------------------------------------------
// file:	Program.cs
//
// summary:	Implements the program class
///-------------------------------------------------------------------------------------------------

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
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A program. </summary>
    ///
    /// <remarks>   Kemp, 1/18/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    class Program
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <remarks>   Kemp, 1/18/2019. </remarks>
        ///
        /// <param name="args"> An array of command-line argument strings. </param>
        ///-------------------------------------------------------------------------------------------------

        static void Main(string[] args) {
            World w = new World();
            w.AddLight(new LightPoint(new Point(10, 10, -10), new Color(1, 1, 1)));
            /*
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
            */
            Cube box = new Cube();
            box.Material.Pattern = new Checked3DPattern(new Color(1,1,1),new Color(0.5, 0.5, 0.5));
            box.Material.Pattern.Transform = MatrixOps.CreateScalingTransform(0.05, 0.05, 0.05);
            box.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(18, 20, -20) * MatrixOps.CreateScalingTransform(20, 20, 20));
            w.AddObject(box);
            Console.WriteLine(box.Bounds.ToString());

            Sphere middle = new Sphere();
            middle.Transform = MatrixOps.CreateTranslationTransform(-0.5, 1, 0.5);
            middle.Material = new Material();
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            middle.Material.Specular = new Color(0.3, 0.3, 0.3);
            middle.Material.Transparency = 0.5;
            //middle.Material.RefractiveIndex = 1.02;
            w.AddObject(middle);

            Sphere right = new Sphere();
            right.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(1.5, 0.5, -0.5) *MatrixOps.CreateScalingTransform(0.5,0.5,0.5));
            right.Material = new Material();
            right.Material.Color = new Color(0.1, 0.5, 0.9);
            right.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            right.Material.Specular = new Color(0.3, 0.3, 0.3);
            w.AddObject(right);

            Sphere left = new Sphere();
            left.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(-1.5, 0.33, -0.75) * MatrixOps.CreateScalingTransform(0.33, 0.33, 0.33));
            left.Material = new Material();
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            left.Material.Specular = new Color(0.3, 0.3, 0.3);
            w.AddObject(left);

            Sphere under = new Sphere();
            under.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(-1.5, -1, -0.75) * MatrixOps.CreateScalingTransform(0.33, 0.33, 0.33));
            under.Material = new Material();
            under.Material.Color = new Color(1, 0.8, 0.6);
            under.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            under.Material.Specular = new Color(0.3, 0.3, 0.3);
            w.AddObject(under);

            Camera camera = new Camera(400, 200, Math.PI / 3);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(1, 2.5, -10), new Point(0, 1, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);
            /*
            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);
            */
            image.WritePNG("MakeAScene.png");

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
