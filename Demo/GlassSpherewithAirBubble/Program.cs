///-------------------------------------------------------------------------------------------------
// file:	Program.cs
//
// summary:	Implements the program class
///-------------------------------------------------------------------------------------------------

using System;
/// <summary>   The system. collections. generic. </summary>
using System.Collections.Generic;
/// <summary>   The system. linq. </summary>
using System.Linq;
/// <summary>   The system. text. </summary>
using System.Text;
/// <summary>   The system. threading. tasks. </summary>
using System.Threading.Tasks;
/// <summary>   The mathematics net. numerics. linear algebra. </summary>
using MathNet.Numerics.LinearAlgebra;
/// <summary>   The mathematics net. numerics. linear algebra. double. </summary>
using MathNet.Numerics.LinearAlgebra.Double;
/// <summary>   The ray tracer library. </summary>
using RayTracerLib;


///-------------------------------------------------------------------------------------------------
// namespace: GlassSphereWithAirBubble
//
// summary:	.
///-------------------------------------------------------------------------------------------------

namespace GlassSphereWithAirBubble
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A program to generate a ray traced rendering of a glass sphere with a large bubble in it
    ///             over a checked background. </summary>
    ///
    /// <remarks>   Kemp, 1/3/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    class Program
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <remarks>   Kemp, 1/3/2019. </remarks>
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
            a.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 15, 0) * MatrixOps.CreateScalingTransform(2, 2, 2));
            a.Material.Ambient = new Color(0, 0, 0);
            a.Material.Color = new Color(0, 0, 0);
            a.Material.Reflective = 0.5;
            w.AddObject(a);

            Sphere b = new Sphere();
            b.Transform = MatrixOps.CreateTranslationTransform(3, 3, 0);
            b.Material.Color = new Color(1, 0, 0);
            w.AddObject(b);

            GlassSphere ainner = new GlassSphere();
            ainner.Transform = MatrixOps.CreateTranslationTransform(0, 15, 0);
            ainner.Material.Ambient = new Color(0, 0, 0);
            ainner.Material.Color = new Color(0, 0, 0);
            ainner.Material.RefractiveIndex = 1;
            ainner.Material.Reflective = 0.5;
            w.AddObject(ainner);

            Camera camera = new Camera(400, 400, Math.PI / 3);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(0, 20, 0), new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));

            Canvas image = w.Render(camera);

            /*
            String ppm = image.ToPPM();

            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);
            */
            image.WritePNG("ToPNG.png");
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
