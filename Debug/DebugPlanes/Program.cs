using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace DebugPlanes
{
    class Program
    {
        protected class TestShape : Shape
        {
            public Ray testRay;
            public override List<Intersection> LocalIntersect(Ray rayparm) {
                testRay = rayparm;
                return new List<Intersection>();
            }
            public override RayTracerLib.Vector LocalNormalAt(Point worldPoint) => new RayTracerLib.Vector(worldPoint.X, worldPoint.Y, worldPoint.Z);
            public override Shape Copy() {
                throw new NotImplementedException();
            }

            public override Bounds LocalBounds() {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args) {

            World defaultWorld = new World();

            defaultWorld.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));

            Sphere s1 = new Sphere();
            s1.Material = new Material();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = 0.7;
            s1.Material.Specular = 0.2;

            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5);
            defaultWorld.AddObject(s1);
            defaultWorld.AddObject(s2);

            {
                //ComputingNormalOnTranslatedShape
                TestShape s = new TestShape();
                s.Transform = MatrixOps.CreateTranslationTransform(0, 1, 0);
                RayTracerLib.Vector n = s.NormalAt(new Point(0, 1.70711, -0.70711));
                bool foo = (n.Equals(new RayTracerLib.Vector(0, 0.70711, -0.70711)));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            
            {
                World w = new World();
                w.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));

                Sphere s3 = new Sphere();
                s3.Material = new Material();
                s3.Material.Color = new Color(0.8, 1.0, 0.6);
                s3.Material.Diffuse = 0.7;
                s3.Material.Specular = 0.2;
                w.AddObject(s3);

                Plane p = new Plane();
                p.Transform = (Matrix)( MatrixOps.CreateTranslationTransform(0,0,2)* MatrixOps.CreateRotationXTransform(Math.PI / 2));
                p.Material.Pattern = new CheckedPattern(new Color(0, 0, 1), new Color(1, 0.9, 0.9));
                w.AddObject(p);

                Plane p1 = new Plane();
                p1.Transform = MatrixOps.CreateTranslationTransform(1,0,0);
                p1.Material.Pattern = new GradientPattern(new Color(1, 0, 0), new Color(1, 0.9, 0.9));
                w.AddObject(p1);


                Ray r = new Ray(new Point(1, 1, -5), new RayTracerLib.Vector(0, -0.5, 0));
                List<Intersection> xs = p.Intersect(r);
                bool foo1, foo2, foo3;
                if (xs.Count > 0) {
                     foo1 = (xs.Count == 1);
                     foo2 = (xs[0].T >= 1);
                     foo3 = (xs[0].Obj.Equals(p));
                }

                Camera camera = new Camera(400, 200, Math.PI / 2);
                camera.Transform = MatrixOps.CreateViewTransform(new Point(3, 3, -5), new Point(0, 0, 0), new RayTracerLib.Vector(0, 1, 0));

                Canvas image = w.Render(camera);

                String ppm = image.ToPPM();

                System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
