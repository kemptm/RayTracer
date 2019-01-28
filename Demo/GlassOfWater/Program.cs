using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace GlassOfWater
{
    class Program
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Main program for demo of a glass of water with a pencil in it. </summary>
        ///
        /// <remarks>
        ///     kemp 11/8/2018
        ///     This program defines a glass of water and a pencil in it to demonstrate the refractive
        ///     abilities of the software.  The glass of water is a cylinder of "glass" with a cylinder
        ///     of water in it, to about 80% of the height.  Just on top of that is a cylinder of air
        ///     that extends above the cylinder of water, holding the top of the glass empty.
        ///     
        ///     The pencil is two cylinders and a cone, placed in the glass so that the appropriate
        ///     refractions may be perceived.
        ///     
        ///     To provide some background, three planes are introduced with a checkered pattern to
        ///     provide a background for the refractions.
        /// </remarks>
        ///
        /// <param name="args"> Usual Main args.  Not used. </param>
        ///-------------------------------------------------------------------------------------------------

        static void Main(string[] args) {
            World w = new World();
            Group g = new Group();

            /// Here's the light on the scene
            w.AddLight(new LightPoint(new Point(10, 20, -20), new Color(1, 1, 1)));
            
            /// three planes make up the background.
            Plane floor = new Plane();
            floor.Material.Pattern = new Checked3DPattern(new Color(1,1,1), new Color(0.75, 0.75, 0.75));
            floor.Material.Pattern.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);
            w.AddObject(floor);

            Plane wallz = (Plane) floor.Copy();
            wallz.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0,0,0));
            w.AddObject(wallz);

            Plane wallx = (Plane)floor.Copy();
            wallx.Transform = (Matrix)(MatrixOps.CreateRotationZTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
            w.AddObject(wallx);

            /// we're going to collect the glass, water and air into a group, so that it can be transformed together.
            Group GlassWithWater = new Group();

            Cylinder glass = new Cylinder();
            glass.Material.Color = new Color(0, 0, 0);
            glass.Material.Ambient = new Color(0, 0, 0);
            glass.Material.Transparency = 1;
            glass.Material.Reflective = 0.5;
            glass.Material.RefractiveIndex = 1.52;
            glass.MinY = 0;
            glass.MaxY = 5;
            glass.Closed = false;
            glass.Transform = (Matrix)(MatrixOps.CreateScalingTransform(3, 1, 3));
            GlassWithWater.AddObject(glass);
            
            Cylinder air = new Cylinder();
            air.Material.Color = new Color(0, 0, 0);
            air.Material.Ambient = new Color(0, 0, 0);
            air.Material.Transparency = 1;
            air.Material.Reflective = 0.5;
            air.Material.RefractiveIndex = 1;
            air.MinY = 4;
            air.MaxY = 5;
            air.Closed = false;
            air.Transform = (Matrix)(MatrixOps.CreateScalingTransform(2.9, 1, 2.9));
            GlassWithWater.AddObject(air);
            
            Cylinder water = new Cylinder();
            water.Material.Color = new Color(0, 0, 0.2);
            water.Material.Ambient = new Color(0, 0, 0);
            water.Material.Transparency = 1;
            water.Material.Reflective = 0.5;
            water.Material.RefractiveIndex = 1.333;
            water.MinY = 0;
            water.MaxY = 4;
            water.Closed = true;
            water.Transform = (Matrix)(MatrixOps.CreateScalingTransform(2.9,1,2.9));
            GlassWithWater.AddObject(water);

            GlassWithWater.Transform = MatrixOps.CreateTranslationTransform(9, 0, -9);
            w.AddObject(GlassWithWater);

            /// now the pencil.
            Group pencil = new Group();
            pencil.Transform = (Matrix)(pencil.Transform * MatrixOps.CreateRotationXTransform(Math.PI / 6) * MatrixOps.CreateRotationYTransform(Math.PI/6)* MatrixOps.CreateRotationZTransform(-Math.PI/6));
            pencil.Transform =(Matrix)  (MatrixOps.CreateTranslationTransform(9, 1 , -10) * pencil.Transform );
            double diameterScale = 0.25;
            double length = 6;

            Cylinder barrel = new Cylinder();
            barrel.Material.Color = new Color(1, 0.8, 0);
            barrel.MinY = 0;
            barrel.MaxY = 6;
            barrel.Transform = (Matrix) (MatrixOps.CreateScalingTransform(diameterScale, 1, diameterScale));
            pencil.AddObject(barrel);

            Cylinder eraser = new Cylinder();
            eraser.Material.Color = new Color(0.8, 0.5, 0.5);
            eraser.MinY = 0;
            eraser.MaxY = 0.5;
            eraser.Closed = true;
            eraser.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, length, 0) * MatrixOps.CreateScalingTransform(diameterScale, 1, diameterScale));
            pencil.AddObject(eraser);

            Cone tip = new Cone();
            tip.MinY = 0;
            tip.MaxY = 1;
            tip.Material.Color = new Color(0.65, 0.35, 0.15);
            tip.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, -1, 0) * MatrixOps.CreateScalingTransform(diameterScale, 1, diameterScale));
            pencil.AddObject(tip);

            w.AddObject(pencil);

            Camera camera = new Camera(400, 400, Math.PI / 3);

            /// we're going to make a 10 frame movie, swinging the camera around all of the axes simultaneously, while maintaining pointing the camera at the glass.
            int nmin = 0;
            int nmax = 10;
 
            for (int n = nmin; n < nmax; n++) {
                Console.WriteLine(n.ToString());
                double theta = ((double)n * Math.PI) / ((double)nmax * 2);


                camera.Transform = MatrixOps.CreateViewTransform(new Point(12 * Math.Sin(theta) +9, 12* Math.Sin(theta) +0.5, -12* Math.Cos(theta) - 9), new Point(9, 3, -9), new RayTracerLib.Vector(0, 1, 0));

                Canvas image = w.ParallelRender(camera);
                /*
                String ppm = image.ToPPM();

                System.IO.File.WriteAllText(@"ToPPMa" + n.ToString() + ".ppm", ppm);
                */
                image.WritePNG("ToPNG" + n.ToString() + ".png");

            }

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
