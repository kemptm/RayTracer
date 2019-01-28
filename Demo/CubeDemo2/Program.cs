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

namespace CubeDemo2
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
            Group g = new Group();
            w.AddLight(new LightPoint(new Point(10, 15, -20), new Color(1, 1, 1)));

            /*RTCube room = new RTCube();
            room.Transform = (Matrix)(RTMatrixOps.Translation(15, 15, -15) * RTMatrixOps.Scaling(15, 15, 15));
            room.Material.Pattern = new Checked3DPattern(new Color(1,1,1),new Color(.5, .5, .5));
            room.Material.Pattern.Transform = RTMatrixOps.Scaling(0.05, 0.05, 0.05);
            room.Material.Ambient = .5;
            g.AddObject(room); 
            */
            /*RTSphere originBall = new RTSphere();
            originBall.Material.Color = new Color(1, 0, 0);
            g.AddObject(originBall);
            */
            Cube box1 = new Cube();
            box1.Material.Color = new Color(1, 1, 1);
            box1.Material.Ambient = new Color(0.3, 0.3, 0.3);
            //box1.Transform = (Matrix)(RTMatrixOps.Translation(9, 1, 4) * RTMatrixOps.RotationY(Math.PI / 4));
            g.AddObject(box1);
            Group gbbc = BoundingBox.Generate(box1);
            g.AddObject(gbbc);
            /*
            RTCylinder cyl1 = new RTCylinder();
            // cyl1.Closed = true;
            cyl1.MaxY = 5;
            cyl1.MinY = 1;
            cyl1.Transform = (Matrix)(RTMatrixOps.Scaling(2, 1, 2) * RTMatrixOps.Translation(2, 0, 5));
            //cyl1.Material.Reflective = 1;
            cyl1.Material.Color = new Color(0, 0, 1);
            g.AddObject(cyl1);

            RTCone cone = new RTCone();
            cone.Closed = true;
            cone.MinY = -2;
            cone.MaxY = 0;
            //cone.Closed = true;
            cone.Transform = RTMatrixOps.Translation(5, 2, 6);
            cone.Material.Color = new Color(.3, .5, .5);
            g.AddObject(cone);
            */
            w.AddObject(g);
            LineSegment.LineFatness = (1000 * Ops.EPSILON);

            Group gbb = BoundingBox.Generate(g, new Color(1,0,0));
            //w.AddObject(gbb);
            
            Camera camera = new Camera(800, 800, Math.PI / 3);
            //            camera.Transform = RTMatrixOps.ViewTransform(new RTPoint(8, 5, 8), new RTPoint(0, 0, 0), new RTVector(0, 1, 0));
            int nmin = 0;
            int nmax = 10;
            //Console.Write("Press enter to render ...");
            //Console.Read();
            /*for (int n = nmin; n < nmax; n++) {
                double theta = ((double)n * Math.PI) / ((double)nmax * 2);
                camera.Transform = RTMatrixOps.ViewTransform(new RTPoint(Math.Sin(theta)* 5, 5, -Math.Cos(theta)*5), new RTPoint(0, 0, 0), new RTVector(0, 1, 0));
                //camera.Transform = RTMatrixOps.ViewTransform(new RTPoint(5, 5, -5), new RTPoint(0, 0, 0), new RTVector(0, 1, 0));
                Console.WriteLine(n.ToString() + " " + theta.ToString() + " (" + (10*Math.Sin(theta)).ToString() + ", 10, "+ (-Math.Cos(theta)*12).ToString() + ")");
                Canvas image = w.Render(camera);

                String ppm = image.ToPPM();

                System.IO.File.WriteAllText(@"ToPPM" + n.ToString() + ".ppm", ppm);
            }*/

            for (int n = nmin; n < nmax; n++) {
                double theta = ((double)n * Math.PI) / ((double)nmax * 2);
                //camera.Transform = RTMatrixOps.ViewTransform(new RTPoint(Math.Sin(theta) * 5, 5, -Math.Cos(theta) * 5), new RTPoint(0, 0, 0), new RTVector(0, 1, 0));
                box1.Transform = (Matrix)(MatrixOps.CreateRotationYTransform(theta)* MatrixOps.CreateRotationZTransform(theta/2));
                g.RemoveObject(gbbc);
                gbbc = BoundingBox.Generate(box1, new Color(1,0,0));
                g.AddObject(gbbc);

                camera.Transform = MatrixOps.CreateViewTransform(new Point(5, 5, -5), new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));
                Console.WriteLine(n.ToString() + " " + theta.ToString() + " (" + (10 * Math.Sin(theta)).ToString() + ", 10, " + (-Math.Cos(theta) * 12).ToString() + ")");
                Canvas image = w.Render(camera);

                /*{
                    String ppm = image.ToPPM();

                    System.IO.File.WriteAllText(@"ToPPMa" + n.ToString() + ".ppm", ppm);
                }*/

                image.WritePNG("ToPNG" + n.ToString()+ ".png");
            }

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
