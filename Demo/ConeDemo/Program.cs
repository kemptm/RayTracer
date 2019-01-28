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

namespace ConeDemo
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

            /*RTPlane p = new RTPlane();
            //p.Transform = RTMatrixOps.RotationZ(Math.PI / 2);
            p.Material.Color = new Color(1, 0.9, 0.9, 0);
            //p.Material.Transparency = 1;
            w.AddObject(p);
            */
            LineSegment.LineFatness = (1000 * Ops.EPSILON);
            Cone cone = new Cone();
            cone.Material.Color = new Color(0.5, 0.3, 0);
            cone.Material.Ambient = new Color(0.5, 0.5, 0.5);
            cone.MinY = 0;
            cone.MaxY = 3;
            cone.Closed = true;
            cone.Transform = MatrixOps.CreateTranslationTransform(0, 2, 0);
            w.AddObject(cone);
            Group g = new Group();
            g = BoundingBox.Generate(cone, new Color(1, 0, 0));
            w.AddObject(g);

            Camera camera = new Camera(400, 400, Math.PI / 4);
            double cy = 0;
            double cz = -5;
            double cmult = 4;
            //double croty =0;
            double crotz = 0;
            Point cameraPoint = new Point(0, cy * cmult, cz * cmult);
            int nmin = 0;
            int nmax = 10;
            for (int n = nmin; n < nmax; n++) {
                crotz = (n * Math.PI / 2) / 10;
                Matrix camerarot =  MatrixOps.CreateRotationXTransform(crotz);
                cameraPoint = camerarot * cameraPoint;
                cameraPoint = new Point(16, 16, 16);
                double theta = ((double)n * Math.PI) / ((double)nmax * 2);
                cone.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(theta) * MatrixOps.CreateRotationZTransform(theta));
                w.RemoveObject(g);
                g = BoundingBox.Generate(cone, new Color(1, 0, 0));
                w.AddObject(g);
                //Console.WriteLine(n.ToString() + ":" + cameraPoint.ToString());

                camera.Transform = MatrixOps.CreateViewTransform(cameraPoint, new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));

                Canvas image = w.Render(camera);

                String ppm = image.ToPPM();
                /*
                String ppmname = ("ToPPM" + n.ToString() + ".ppm");
                System.IO.File.WriteAllText(@ppmname, ppm);
                */
                image.WritePNG("ToPNG" + n.ToString() + ".png");

            }
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
