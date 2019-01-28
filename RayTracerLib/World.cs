///-------------------------------------------------------------------------------------------------
// file:	World.cs
//
// summary:	Implements the world class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A world. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   The world is the object that everything else, shapes, lights and cameras exist in.  It
    ///             is in the world that renderings are taken.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class World {
        /// <summary>    A List of the Lights in the world. </summary>
        protected List<LightPoint> lights;
        /// <summary>   A List of the Shapes in the world. </summary>
        protected List<Shape> objects;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the lights. </summary>
        ///
        /// <value> The lights. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<LightPoint> Lights { get { return lights; } set { lights = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the objects. </summary>
        ///
        /// <value> The objects. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Shape> Objects { get { return objects; } set { objects = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public World () {
            lights = new List<LightPoint>();
            objects = new List<Shape>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="o">    A Shape to add. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddObject(Shape o) => objects.Add(o);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the object described by o. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="o">    A Shape to remove. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveObject(Shape o) => objects.Remove(o);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a light. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="l">    A LightPoint to add. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddLight(LightPoint l) => lights.Add(l);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the light described by l. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="l">    A LightPoint to remove. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveLight(LightPoint l) => lights.Remove(l);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Intersects shapes in the world with the given Ray. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        /// <remarks>   Determines which shapes the ray intersects</remarks>
        ///
        /// <param name="r">    A Ray to process. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<Intersection> Intersect(Ray r) {
            List<Intersection> xs = new List<Intersection>();
            foreach(Shape o in objects) {
                List<Intersection> xss = o.Intersect(r);
                xs.AddRange(xss);
            }
            xs.Sort((x, y) => x.T < y.T ? -1 : x.T > y.T ? 1 : 0);
            return xs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determine Color to be rendered from the closest intersection of the Ray. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="ray">                  The ray. </param>
        /// <param name="recursionRemaining">   (Optional) The recursion remaining. This is an infinite recursion limit. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Color ColorAt(Ray ray,int recursionRemaining=5) {
            List<Intersection> xs = this.Intersect(ray);
            Color black = new Color(0, 0, 0);
            if (xs.Count == 0) {
                return black;
            }
            Intersection hit = Intersection.Hit(xs);
            if (hit == null) return black;
            hit.Prepare(ray,xs);
            return hit.Shade(this,recursionRemaining);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A World. </returns>
        ///-------------------------------------------------------------------------------------------------

        public World Copy() {
            World  n = new World();
            foreach (LightPoint l in lights) n.AddLight(l.Copy());
            foreach (Shape o in objects) n.AddObject(o.Copy());
            return n;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Renders the view that the given Camera sees. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="c">    A Camera to process. </param>
        ///
        /// <returns>   The Canvas. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Canvas Render(Camera c) {
            Canvas image = new Canvas(c.Hsize, c.Vsize);
            for (int y = 0; y < c.Vsize; y++) {
                //if (y % 10 == 0) Console.WriteLine("Rendering line " + y.ToString());
                for (int x = 0; x < c.Hsize; x++) {
                    Ray ray = c.RayForPixel((uint)x, (uint)y);
                    Color color = ColorAt(ray);
                    image.WritePixel((uint)x, (uint)y, color);
                }
            }
            return image;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parallel version of Render. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <param name="c">    A Camera to process. </param>
        ///
        /// <returns>   The Canvas. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Canvas ParallelRender(Camera c) {
            Canvas image = new Canvas(c.Hsize, c.Vsize);
            ParallelLoopResult res = Parallel.For(0, c.Vsize,y => {
                //           for (int y = 0; y < c.Vsize; y++) {
                Console.WriteLine("Rendering row: {0}", y);           
                for (int x = 0; x < c.Hsize; x++) {
                    Ray ray = c.RayForPixel((uint)x, (uint)y);
                    Color color = ColorAt(ray);
                    image.WritePixel((uint)x, (uint)y, color);
               }
                Console.WriteLine("Rendering row: {0} is done.", y);
            });
            return image;
        }
    }
}
