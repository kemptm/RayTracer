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
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace BlueMarble
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The Main Program. </summary>
    ///
    /// <remarks>   Kemp, 3/25/2019. </remarks>
    /// <remarks> This program renders two spheres, each provided with a spherical texture map.
    ///           One map goes on the outside of a small sphere that  the camera is pointed at and 
    ///           represents the earth, using NASA's Blue Marble mercator rendering.  The other sphere 
    ///           is quite large and the camera is inside it.  It represents the background of space. I used
    ///           a freely available spherical HDRI image of outer space.</remarks>
    ///-------------------------------------------------------------------------------------------------

    class Program
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <remarks>   Kemp, 3/25/2019. </remarks>
        ///
        /// <param name="args"> An array of command-line argument strings (unused). </param>
        ///-------------------------------------------------------------------------------------------------

        static void Main(string[] args) {
            World w = new World();

            w.AddLight(new LightPoint(new Point(-10, 1, -100), new Color(1, 1, 1)));
            Sphere middle = new Sphere();
            middle.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, 1, 0) * MatrixOps.CreateRotationYTransform(Math.PI));
            middle.Material = new Material();
            middle.Material.Color = new Color(0.1, 0.9, 0.5);
            middle.Material.Ambient = new Color(.5, .5, .5);
            middle.Material.Diffuse = new Color(1, 1, 1);
            middle.Material.Specular = new Color(0.3, 0.3, 0.3);
            TextureMap tm = new TextureMap("world.topo.200409.3x5400x2700.png");
            middle.Material.Map_Ka = tm;
            middle.Material.Map_Kd = tm;
            w.AddObject(middle);

            Sphere sky = new Sphere();
            sky.Transform = (Matrix)(MatrixOps.CreateScalingTransform(200000, 200000, 200000));
            TextureMap skytm = new TextureMap("hdr.png");
            sky.Material.Ambient = new Color(1, 1, 1);
            sky.Material.Diffuse = new Color(0, 0, 0);
            sky.Material.Specular = new Color(0, 0, 0);
            sky.Material.Map_Ka = skytm;
            w.AddObject(sky);

            Camera camera = new Camera(1400, 1200, Math.PI / 2);
            camera.Transform = MatrixOps.CreateViewTransform(new Point(-3, 1, 0), new Point(0, 1, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);

            string outputFile = "Blue_Marble" +
                "";
            image.WritePNG(outputFile + ".png");

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
