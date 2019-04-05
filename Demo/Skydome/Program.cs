using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracerLib;

namespace Skydome
{
    class Program
    {
        static void Main(string[] args) {
            IcoSphereCreator dome = new IcoSphereCreator();

            OBJFileParser g = dome.Create(1);
        }
    }
}
