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

namespace RenderTriangleMesh
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A program to demonstrate rendering a triangle mesh. </summary>
    ///
    /// <remarks>   Kemp, 11/19/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    class RenderTriangleMesh
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <remarks>   Kemp, 11/19/2018. </remarks>
        ///
        /// <param name="args"> An array of command-line argument strings. Not used.</param>
        ///-------------------------------------------------------------------------------------------------

        static void Main(string[] args) {
            Console.WriteLine("Started: " + DateTime.Now.ToString());
            World w = new World();

            /// Read the triangular mesh and color all the triangles red.
            OBJFileParser p = new OBJFileParser("Dog.obj");
            Color red = new Color(1, 0, 0);
            Group g = p.Groups[1];
            Console.WriteLine("triangles in default: " + g.Children.Count().ToString());
            foreach(Shape s in g.Children) {
                s.Material.Color = red;
            }
            /// Transform the group containing the triangular mesh to correspond to our world and camera view point.
            g.Transform = (Matrix)(p.DefaultGroup.Transform  * MatrixOps.CreateTranslationTransform(10, 15, -10) * 
                            MatrixOps.CreateRotationZTransform(Math.PI/2) * MatrixOps.CreateRotationYTransform(Math.PI/2));
            w.AddObject(g);

            /// three planes make up the background.
            Plane floor = new Plane();
            floor.Material.Pattern = new Checked3DPattern(new Color(1, 1, 1), new Color(0.75, 0.75, 0.75));
            floor.Material.Pattern.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            w.AddObject(floor);

            Plane wallz = (Plane)floor.Copy();
            wallz.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
            //w.AddObject(wallz);

            Plane wallx = (Plane)floor.Copy();
            wallx.Transform = (Matrix)(MatrixOps.CreateRotationZTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
            //w.AddObject(wallx);

            Point minCorner = g.Bounds.MinCorner;
            Point maxCorner = g.Bounds.MaxCorner;
            Console.WriteLine("minCorner = " + minCorner.ToString() + ", maxCorner = " + maxCorner.ToString());

            w.AddLight(new LightPoint(new Point(50, 70, -50), new Color(1, 1, 1)));
            // w.AddLight(new RTLightPoint(new RTPoint(0, 10, -10), new Color(0.5, 0.5, 0.5)));

            /// Render the image
            Console.WriteLine("Now rendering ...");
            Camera camera = new Camera(400, 400, Math.PI / 4);
            double cy = 35;
            double cz = -50;
            double cmult = 1.5;
            Point cameraPoint = new Point(25, cy * cmult, cz * cmult);
            camera.Transform = MatrixOps.CreateViewTransform(cameraPoint, new Point(10, 15, 5), new RayTracerLib.Vector(0, 1, 0));

            //Canvas image = w.Render(camera);
            Canvas image = w.ParallelRender(camera);
            Console.WriteLine("Now writing output ...");

            String ppm = image.ToPPM();
            String ppmname = ("ToPPM.ppm");
            System.IO.File.WriteAllText(@ppmname, ppm);

            Console.WriteLine("Finished: " + DateTime.Now.ToString());
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
