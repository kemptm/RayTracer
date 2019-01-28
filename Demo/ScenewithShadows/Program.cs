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

namespace SceneWithShadows
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
            w.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(0.5, 0.5, 0.5)));
            w.AddLight(new LightPoint(new Point( 0, 10, -10), new Color(0.5, 0.5, 0.5)));

            Sphere floor = new Sphere();
            floor.Transform = MatrixOps.CreateScalingTransform(10, 0.01, 10);
            floor.Material = new Material();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = new Color(0, 0, 0);
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
            middle.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            middle.Material.Specular = new Color(0.3, 0.3, 0.3);
            w.AddObject(middle);

            Sphere right = new Sphere();
            right.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(1.5, 0.5, -0.5) * MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5));
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

            Camera camera = new Camera(400, 200, Math.PI / 3);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(0, 1.5, -5), new Point(0, 1, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);

            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
