using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    public class Axes
    {
        public static Group Generate(double length = 25, double diameter = 1,Color cc = null) {
            Color c;
            if (cc!=null) {
                c = cc;
            }else {
                c = new Color(1, 1, 1);
            }
            Group g = new Group();
            g.Name = "Axes";
            // y axis
            Cylinder ls = new Cylinder();
            ls.MinY = 0;
            ls.MaxY = length;
            ls.Material.Color = c.Copy();
            g.AddObject(ls);
            // x Axis
            ls = new Cylinder();
            ls.MinY = 0;
            ls.MaxY = length;
            ls.Transform = MatrixOps.CreateRotationZTransform(-Math.PI / 2);
            ls.Material.Color = c.Copy();
            g.AddObject(ls);
            // z axis
            ls = new Cylinder();
            ls.MinY = 0;
            ls.MaxY = length;
            ls.Transform = MatrixOps.CreateRotationXTransform(Math.PI / 2);
            ls.Material.Color = c.Copy();
            g.AddObject(ls);
            return g;
        }
    }
}
