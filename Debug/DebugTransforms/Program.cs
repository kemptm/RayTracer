using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace DebugTransforms
{
    class Program
    {
        static void Main(string[] args) {
            Point p = new Point(0, 1, 0);
            Matrix hq = MatrixOps.CreateRotationXTransform(Math.PI / 4.0);
            Matrix inv = (Matrix)hq.Inverse();
            Console.WriteLine(hq.ToString());
            Console.WriteLine(inv.ToString());
            Console.WriteLine(p.Transform(inv).ToString());
            Matrix t = MatrixOps.CreateTranslationTransform(5, -3, 2);
            p = new Point(-3, 4, 5);
            Point pt = p.Transform(t);
            bool foo = (p.Transform(t).Equals(new Point(2, 1, 7)));

            RayTracerLib.Vector a = new RayTracerLib.Vector(1, 2, 3);
            RayTracerLib.Vector b = new RayTracerLib.Vector(2, 3, 4);
            RayTracerLib.Vector ab = a.Cross(b);
            RayTracerLib.Vector ba = b.Cross(a);
            bool foo1 = (a.Cross(b).IsEqual(new RayTracerLib.Vector(-1, 2, -1)));
            bool foo2 = (b.Cross(a).IsEqual(new RayTracerLib.Vector(1, -2, 1)));

            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }
    }
}
