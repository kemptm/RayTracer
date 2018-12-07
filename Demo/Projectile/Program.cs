using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracerLib;

namespace Projectile
{
    public class Projectile
    {
        protected Point position;
        protected Vector velocity;
        public Point Position { get { return position; } set { position = value; } }
        public Vector Velocity { get { return velocity; } set { velocity = value; } }

        public Projectile() { }

        public Projectile(Point p, Vector v) {
            this.position = p;
            this.velocity = v;
        }
    }

    public class World
    {
        protected Vector gravity;
        protected Vector wind;
        public Vector Gravity { get { return gravity; } set { Gravity = value; } }
        public Vector Wind { get { return wind; } set { Wind = value; } }
        public World() { }
        public World(Vector g, Vector w) {
            this.gravity = g;
            this.wind = w;
        }
    }
    class Program
    {
        static Projectile tick(World w, Projectile p) {
            return new Projectile(p.Position + p.Velocity, p.Velocity + w.Gravity + w.Wind);
        }

        static void Main(string[] args) {

            Projectile p = new Projectile(new Point(0, 1, 0), new Vector(1, 1, 0).Normalize());
            World w = new World(new Vector(0, -0.1, 0), new Vector(-0.01, 0, 0));
            List < Point > pl = new List<Point>();

            double  scale = 128.0;
            Canvas c = new Canvas((uint)(scale), (uint)(scale));
            Color red = new Color(1.0, 0, 0);


            while (p.Position.Y > 0) {
                pl.Add(new Point(p.Position.X, p.Position.Y, 0));
                System.Console.Write("Position: " + p.Position.ToString());
                System.Console.WriteLine(" Velocity: " + p.Velocity.ToString());
                
                p = tick(w, p);
            }
            double maxX = 0;
            double maxY = 0;
            foreach(Point pp in pl) {
                maxX = pp.X > maxX ? pp.X : maxX;
                maxY = pp.Y > maxY ? pp.Y : maxY;
            }
            double scaleX = (scale-1) / maxX;
            double scaleY = (scale-1) / maxY;
            foreach(Point pp in pl) {
                double xpos = pp.X * scaleX;
                double ypos = pp.Y * scaleY;
                //c.WritePixel((uint)xpos, (uint)((scale-1) - (ypos)), red);
                c.WritePixel((uint)xpos, (uint)(ypos), red);
                System.Console.WriteLine(pp.ToString());


            }
            String ppm = c.ToPPM();
            System.IO.File.WriteAllText(@"ToPPM.ppm", ppm);
            System.Console.WriteLine("press any key to exit ...");
            System.Console.ReadKey();
        }
    }
}
