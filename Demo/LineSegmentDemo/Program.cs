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

namespace LineSegmentDemo
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
            w.AddLight(new LightPoint(new Point(-10, 15, -10), new Color(1, 1, 1)));
            // w.AddLight(new RTLightPoint(new RTPoint(0, 10, -10), new Color(0.5, 0.5, 0.5)));

            Plane p = new Plane();
            //p.Transform = RTMatrixOps.RotationZ(Math.PI / 2);
            p.Material.Color = new Color(1, 0.9, 0.9, 0);
            //p.Material.Transparency = 1;
            w.AddObject(p);
            

            LineSegment l1 = new LineSegment();
            l1.PLo = -2;
            l1.PHi = 2;
            l1.Material.Color = new Color(1, 1, 1);
            w.AddObject(l1);

            LineSegment l2 = (LineSegment)l1.Copy();
           // l2.Transform = (Matrix)(RTMatrixOps.RotationZ(Math.PI / 2)*RTMatrixOps.Translation(2,2,0));
            l2.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(2, 2, 0) * MatrixOps.CreateRotationZTransform(Math.PI / 2) );
            l2.Material.Color = new Color(1, 1, 0);
            w.AddObject(l2);

            LineSegment l3 = (LineSegment)l1.Copy();
            // l2.Transform = (Matrix)(RTMatrixOps.RotationZ(Math.PI / 2)*RTMatrixOps.Translation(2,2,0));
            l3.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(2, -2, 0) * MatrixOps.CreateRotationZTransform(Math.PI / 2));
            l3.Material.Color = new Color(0, 1, 1);
            w.AddObject(l3);

            LineSegment l4 = (LineSegment)l1.Copy();
            // l2.Transform = (Matrix)(RTMatrixOps.RotationZ(Math.PI / 2)*RTMatrixOps.Translation(2,2,0));
            l4.Transform = MatrixOps.CreateTranslationTransform(4, 0, 0);
            l4.Material.Color = new Color(1, 0, 1);
            w.AddObject(l4); 

            Sphere s = new Sphere();
            s.Material.Color = new Color(1, 0, 0);
            w.AddObject(s);

            Group g = BoundingBox.Generate(s);
            w.AddObject(g);

            Camera camera = new Camera(400, 400, Math.PI / 4);
            double cy = 0;
            double cz = -5;
            double cmult = 4;
            //double croty = 0;
            double crotz = 0;
            Point cameraPoint = new Point(0, cy * cmult, cz * cmult);
            for (int n = 3; n < 4; n++) {
                crotz = (n * Math.PI / 2) / 10;
                Matrix camerarot = MatrixOps.CreateRotationXTransform(crotz);
                cameraPoint = camerarot * cameraPoint;
                cameraPoint = new Point(3, 5, -3);
                //Console.WriteLine(n.ToString() + ":" + cameraPoint.ToString());

                camera.Transform = MatrixOps.CreateViewTransform(cameraPoint, new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));

                Canvas image = w.Render(camera);

                String ppm = image.ToPPM();
                String ppmname = ("ToPPM" + n.ToString() + ".ppm");
                System.IO.File.WriteAllText(@ppmname, ppm);
            }
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
