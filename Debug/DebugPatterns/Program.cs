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

namespace DebugPatterns
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A program. </summary>
    ///
    /// <remarks>   Kemp, 1/18/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    class Program
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A test shape. </summary>
        ///
        /// <remarks>   Kemp, 1/18/2019. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected class TestShape : Shape
        {
            /// <summary>   The test ray. </summary>
            public Ray testRay;

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Local intersect (abstract). </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <param name="rayparm">  The ray to intersect. </param>
            ///
            /// <returns>   A List&lt;Intersection&gt; </returns>
            ///-------------------------------------------------------------------------------------------------

            public override List<Intersection> LocalIntersect(Ray rayparm) {
                testRay = rayparm;
                return new List<Intersection>();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <param name="worldPoint">   The local point. </param>
            ///
            /// <returns>   A Vector. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override RayTracerLib.Vector LocalNormalAt(Point worldPoint) => new RayTracerLib.Vector(worldPoint.X, worldPoint.Y, worldPoint.Z);

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Copy the shape (Virtual). </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <returns>   A Shape. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Shape Copy() {
                throw new NotImplementedException();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <returns>   The Bounds. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Bounds LocalBounds() {
                throw new NotImplementedException();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   A test pattern. </summary>
        ///
        /// <remarks>   Kemp, 1/18/2019. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected class TestPattern : Pattern
        {
            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Copies this object. </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <returns>   A Pattern. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Pattern Copy() {
                throw new NotImplementedException();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Pattern at a particular point. </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <param name="p">    A RTPoint to return pattern color. </param>
            ///
            /// <returns>   A Color. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Color PatternAt(Point p) {
                return new Color(p.X, p.Y, p.Z);
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Pattern at object. (Abstract) </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <param name="s">            A RTShape to process. </param>
            /// <param name="worldPoint">   The world point. </param>
            ///
            /// <returns>   A Color. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override Color PatternAtObject(Shape s, Point worldPoint) => PatternAt(LocalPatternPoint(s, worldPoint));

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Tests if this Pattern is considered equal to another. </summary>
            ///
            /// <remarks>   Kemp, 1/18/2019. </remarks>
            ///
            /// <param name="m">    The pattern to compare to this object. </param>
            ///
            /// <returns>   True if the objects are considered equal, false if they are not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public override bool Equals(Pattern m) {
                if (m is TestPattern) {
                    TestPattern c = (TestPattern)m;
                    return  xform.Equals(c.xform);
                }
                return false;
            }

        }

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
            s1.Material.Specular = new Color(0.2, 0.2, 0.2);

            Sphere s2 = new Sphere();
            s2.Transform = MatrixOps.CreateScalingTransform(0.5, 0.5, 0.5);
            defaultWorld.AddObject(s1);
            defaultWorld.AddObject(s2);
            Color white = new Color(1, 1, 1);
            Color black = new Color(0, 0, 0);
            Sphere s = new Sphere();


            {
                Material m = new Material();
                m.Pattern = new StripePattern(white, black);
                m.Ambient = new Color(1, 1, 1);
                m.Diffuse = new Color(0, 0, 0);
                m.Specular = new Color(0, 0, 0);
                m.Shininess = 0;
                RayTracerLib.Vector eyev = new RayTracerLib.Vector(0, 0, -1);
                RayTracerLib.Vector normalv = new RayTracerLib.Vector(0, 0, -1);
                Point p1 = new Point(0.9, 0, 0);
                Point p2 = new Point(1.1, 0, 0);
                LightPoint light = new LightPoint(new Point(0, 0, -10), new Color(1, 1, 1));
                Color c1 = Ops.Lighting(m, s, light, p1, eyev, normalv, false);
                Color c2 = Ops.Lighting(m, s, light, p2, eyev, normalv, false);
                bool foo1 = (c1.Equals(white));
                bool foo2 = (c2.Equals(black));
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            //public void PatternWithObjectTransformation() 

            {
                Shape shape = new Sphere() {
                    Transform = MatrixOps.CreateScalingTransform(2, 2, 2)
                };
                Pattern pattern = new TestPattern {
                    Transform = MatrixOps.CreateTranslationTransform(0.5, 1, 1.5)
                };
                Color c = pattern.PatternAtObject(shape, new Point(2.5, 3, 3.5));
                bool foo1 = (c.Equals(new Color(0.75, 0.5, 0.25)));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }

            //public void GradientInterpolates() 
            {
                Pattern pattern = new GradientPattern(black, white);
                bool foo1 = (pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
                bool foo2 = (pattern.PatternAt(new Point(0.25, 0, 0)).Equals(new Color(0.25, 0.25, 0.25)));
                bool foo3 = (pattern.PatternAt(new Point(0.5, 0, 0)).Equals(new Color(0.5, 0.5, 0.5)));
                bool foo4 = (pattern.PatternAt(new Point(0.75, 0, 0)).Equals(new Color(0.75, 0.75, 0.75)));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            {
                Pattern pattern = new CheckedPattern(black, white);
                bool foo1 = (pattern.PatternAt(new Point(0, 0, 0)).Equals(black));
                bool foo2 = (pattern.PatternAt(new Point(0.99, 0, 0)).Equals(black));
                bool foo3 = (pattern.PatternAt(new Point(1.01, 0, 0)).Equals(white));

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
            }
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
