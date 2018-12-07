using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracerLib;

namespace DebugIntersect
{
    class Program
    {
        static void Main(string[] args) {
            // intersects at 2 points
            {
                Ray r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
                Sphere s = new Sphere();
                List<Intersection> xs = s.Intersect(r);
            }
            {
                Sphere s = new Sphere();
                Intersection i1 = new Intersection(-1, s);
                Intersection i2 = new Intersection(1, s);
                List<Intersection> xs = Intersection.Intersections(i1, i2);
                Intersection h = Intersection.Hit(xs);

            }
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
