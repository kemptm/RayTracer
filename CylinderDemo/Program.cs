using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace CylinderDEmo
{
    class Program
    {
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
            Cylinder cyl = new Cylinder();
            cyl.Material.Color = new Color(1, 1, 1);
            cyl.Material.Ambient = 0.3;
            cyl.MinY = -1;
            cyl.MaxY = 1;
            //box1.Transform = (Matrix)(RTMatrixOps.Translation(9, 1, 4) * RTMatrixOps.RotationY(Math.PI / 4));
            g.AddObject(cyl);
            Group gbbc = BoundingBox.Generate(cyl);
            g.AddObject(gbbc);
            
            Cylinder cyl1 = new Cylinder();
            cyl1.Closed = true;
            cyl1.MaxY = 5;
            cyl1.MinY = 1;
            cyl1.Transform = (Matrix)(MatrixOps.CreateScalingTransform(2, 1, 2) * MatrixOps.CreateTranslationTransform(2, 0, 5));
            //cyl1.Material.Reflective = 1;
            cyl1.Material.Color = new Color(0, 1, 1);
            g.AddObject(cyl1);
            Group gbbc2 = BoundingBox.Generate(cyl1);
            g.AddObject(gbbc2);
            /*
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
            Group gbb = BoundingBox.Generate(g);
            w.AddObject(gbb);

            Camera camera = new Camera(400, 400, Math.PI / 3);
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
                cyl.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(theta) * MatrixOps.CreateRotationZTransform(theta ));
                g.RemoveObject(gbbc);
                gbbc = BoundingBox.Generate(cyl);
                g.AddObject(gbbc);
                w.RemoveObject(gbb);
                gbb = BoundingBox.Generate(g);
                w.AddObject(gbb);

                camera.Transform = MatrixOps.CreateViewTransform(new Point(10, 10, -10), new Point(2, 0, 0), new RayTracerLib.Vector(0, 1, 0));
                //Console.WriteLine(n.ToString() + " " + theta.ToString() + " (" + (10 * Math.Sin(theta)).ToString() + ", 10, " + (-Math.Cos(theta) * 12).ToString() + ")");
                Canvas image = w.Render(camera);

                String ppm = image.ToPPM();

                System.IO.File.WriteAllText(@"ToPPMa" + n.ToString() + ".ppm", ppm);
            }

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
