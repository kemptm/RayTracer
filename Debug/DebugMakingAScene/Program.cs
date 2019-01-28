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

namespace DebugMakingAScene
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
            World defaultWorld = new World();

            defaultWorld.AddLight(new LightPoint(new Point(-10, 10, -10), new Color(1, 1, 1)));

            Sphere s1 = new Sphere();
            s1.Material = new Material();
            s1.Material.Color = new Color(0.8, 1.0, 0.6);
            s1.Material.Diffuse = new Color(0.7, 0.7, 0.7);
            s1.Material.Specular = new Color(0.2,0.2,0.2);

            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5);
            defaultWorld.AddObject(s1);
            defaultWorld.AddObject(s2);

            {
                // ColorAtRayMisses
                World world = defaultWorld.Copy();
                Ray ray = new Ray(new Point(0, 0, -5), new RayTracerLib.Vector(0, 1, 0));
                Color c = world.ColorAt(ray);
                bool foo = c.Equals(new Color(0, 0, 0));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                // IntersectionShadingInside
                World world = defaultWorld.Copy();
                world.Lights.Clear();
                world.AddLight(new LightPoint(new Point(0, 0.25, 0), new Color(1, 1, 1)));
                Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
                Shape shape = world.Objects[1];
                Intersection hit = new Intersection(0.5, shape);
                List<Intersection> xs = new List<Intersection>();
                xs.Add(hit);
                hit.Prepare(ray, xs);
                Color c = hit.Shade(world);
                bool foo = c.Equals(new Color(0.90498, 0.90498, 0.90498));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                //ColorAtBehindRay() {
                World world = defaultWorld.Copy();
                Shape outer = defaultWorld.Objects[0];
                outer.Material.Ambient = new Color(1, 1, 1);
                Shape inner = defaultWorld.Objects[1];
                inner.Material.Ambient = new Color(1, 1, 1);
                Ray ray = new Ray(new Point(0, 0, -0.75), new RayTracerLib.Vector(0, 0, 1));
                Color imc = inner.Material.Color;
                Color c = world.ColorAt(ray);
                bool foo = world.ColorAt(ray).Equals(imc);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }

            {
                Point from = new Point(1, 3, 2);
                Point to = new Point(4, -2, 8);
                RayTracerLib.Vector up = new RayTracerLib.Vector(1, 1, 0);
                Matrix t = MatrixOps.CreateViewTransform(from, to, up);
                Matrix r = DenseMatrix.OfArray(new double[,]{
                { -0.50709, 0.50709,  0.67612,-2.36643},
                {  0.76772, 0.60609,  0.12122,-2.82843},
                { -0.35857, 0.59761, -0.71714, 0.0},
                {  0.0,     0.0,      0.0,     1.0} });
                bool foo = t.Equals(r);
                Matrix d = (Matrix)(t - r);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                Camera c1 = new Camera(200, 125, Math.PI / 2.0);
                Camera c2 = new Camera(125, 200, Math.PI / 2.0);

                bool foo1 = Ops.Equals(c1.PixelSize,0.01);
                bool foo2 = Ops.Equals(c2.PixelSize, 0.01);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                //objectBetweenLightAndPoint
                World world = defaultWorld.Copy();
                Point p = new Point(10, -10, 10);
                bool foo = p.IsShadowed(world, world.Lights[0]);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                //public void IntersectionOnInside() {
                Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
                Shape shape = new Sphere();
                Intersection hit = new Intersection(1, shape);
                List<Intersection> xs = new List<Intersection>();
                xs.Add(hit);
                hit.Prepare(ray, xs);
                bool foo1 = hit.Inside;
                bool foo2 = hit.Point.Equals(new Point(0, 0, 1.0001));
                bool foo3 = hit.Eyev.Equals(new RayTracerLib.Vector(0, 0, -1));
                bool foo4 = hit.Normalv.Equals(new RayTracerLib.Vector(0, 0, -1));

                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                //IntersectionShadingInside
                World world = defaultWorld;
                world.Lights.Clear(); // remove default light.
                world.AddLight(new LightPoint(new Point(0, 0.25, 0), new Color(1, 1, 1)));
                Ray ray = new Ray(new Point(0, 0, 0), new RayTracerLib.Vector(0, 0, 1));
                Shape shape = world.Objects[1];
                Intersection hit = new Intersection(0.5, shape);
                List<Intersection> xs = new List<Intersection>();
                xs.Add(hit);
                hit.Prepare(ray, xs);
                Color c = hit.Shade(world);
                bool foo = c.Equals(new Color(0.90498, 0.90498, 0.90498));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
}
}
