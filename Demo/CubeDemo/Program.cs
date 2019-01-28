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

namespace CubeDemo
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
            /*
                        Cube room = new Cube();
                        room.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(15, 15, 15) * MatrixOps.CreateScalingTransform(15, 15, 15));
                        room.Material.Pattern = new Checked3DPattern(new Color(1,1,1),new Color(.5, .5, .5));
                        room.Material.Pattern.Transform = MatrixOps.CreateScalingTransform(0.05, 0.05, 0.05);
                        g.AddObject(room);
                        */
            LineSegment.LineFatness = (1000 * Ops.EPSILON);
            Sphere originBall = new Sphere();
            originBall.Material.Color = new Color(1, 0, 0);
            g.AddObject(originBall);

            Cube box1 = new Cube();
            box1.Material.Color = new Color(0, 1, 0);
            box1.Material.Ambient = new Color(0.3, 0.3, 0.3);
            box1.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(9, 1, 4)*MatrixOps.CreateRotationYTransform(Math.PI/4));
            g.AddObject(box1);
            Group gbbc = BoundingBox.Generate(box1, new Color(1, 0, 0));
            g.AddObject(gbbc);

            Cylinder cyl1 = new Cylinder();
            // cyl1.Closed = true;
            cyl1.MaxY = 5;
            cyl1.MinY = 1;
            cyl1.Transform = (Matrix)(MatrixOps.CreateScalingTransform(2,1,2) * MatrixOps.CreateTranslationTransform(2, 0, 5));
            //cyl1.Material.Reflective = 1;
            cyl1.Material.Color = new Color(0, 0, 1);
            g.AddObject(cyl1);

            Cone cone = new Cone();
            cone.Closed = true;
            cone.MinY = -2;
            cone.MaxY = 0;
            //cone.Closed = true;
            cone.Transform = MatrixOps.CreateTranslationTransform(5, 2, 6);
            cone.Material.Color = new Color(.3, .5, .5);
            g.AddObject(cone);
            gbbc = BoundingBox.Generate(cone, new Color(1, 0, 0));
            g.AddObject(gbbc);

            w.AddObject(g);
            LineSegment.LineFatness = (1000 * Ops.EPSILON);
            Group gbb = BoundingBox.Generate(g, new Color(1,0,0));
            w.AddObject(gbb);
                       
            Camera camera = new Camera(400, 400, Math.PI / 3);
//            camera.Transform = RTMatrixOps.ViewTransform(new RTPoint(8, 5, 8), new RTPoint(0, 0, 0), new RTVector(0, 1, 0));
            camera.Transform = MatrixOps.CreateViewTransform(new Point(10, 6, -8), new Point(5, 0, 0), new RayTracerLib.Vector(0, 1, 0));

            Canvas image = w.Render(camera);
            /*{
                String ppm = image.ToPPM();

                System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);
            }*/
            image.WritePNG("ToPNG.png");

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
