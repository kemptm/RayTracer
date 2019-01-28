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

namespace GlassSphereOnChecks
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
            w.AddLight(new LightPoint(new Point(10, 10, 10), new Color(1, 1, 1)));

            Plane floor = new Plane();
            floor.Material.Pattern = new CheckedPattern();
            w.AddObject(floor);

            GlassSphere a = new GlassSphere();
            a.Transform =(Matrix)(MatrixOps.CreateTranslationTransform(0, 15, 0)*MatrixOps.CreateScalingTransform(2, 2, 2));
            a.Material.Ambient = new Color(0, 0, 0);
            a.Material.Color = new Color(0, 0, 0);
            w.AddObject(a);

            Sphere b = new Sphere();
            b.Transform = MatrixOps.CreateTranslationTransform(3, 3, 0);
            b.Material.Color = new Color(1, 0, 0);
            w.AddObject(b);

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
