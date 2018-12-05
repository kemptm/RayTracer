using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerLib
{
    public class SmoothTriangle : Triangle {
        protected Vector n0;
        protected Vector n1;
        protected Vector n2;

        public Vector N0 { get { return n0; } set { n0 = value; } }
        public Vector N1 { get { return n1; } set { n1 = value; } }
        public Vector N2 { get { return n2; } set { n2 = value; } }

        public SmoothTriangle() {
        }

        public SmoothTriangle(Point cv0, Point cv1, Point cv2, Vector cn0, Vector cn1, Vector cn2) : base(cv0, cv1, cv2) {
            n0 = cn0;
            n1 = cn1;
            n2 = cn2;
        }

        public override Vector LocalNormalAt(Point localPoint, Intersection hit) {
            return n1 * hit.U 
                 + n2 * hit.V 
                 + n0 * (1 - hit.U - hit.V);
        }
    }
}
