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
using System.IO;
using NDesk.Options;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace RenderTriangleMesh
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A program to demonstrate rendering a triangle mesh. </summary>
    ///
    /// <remarks>   Kemp, 11/19/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    class RenderTriangleMesh
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the camera Field of View (FOV) such that it will encompass the entire group provided. </summary>
        ///
        /// <remarks>   Kemp, 1/6/2019. </remarks>
        ///
        /// <param name="cameraPoint">      The camera point - where it is. </param>
        /// <param name="CameraPointsAt">   The camera points at - what it's pointing at. </param>
        /// <param name="g">                A Group that the camera is pointing at. </param>
        ///
        /// <returns>   The calculated camera FOV. </returns>
        ///-------------------------------------------------------------------------------------------------

        static double CalcCameraFOV(Point cameraPoint, Point CameraPointsAt, Group g) {
            RayTracerLib.Vector CameraVect = (cameraPoint - CameraPointsAt).Normalize();
            Point maxXfront = new Point(g.Bounds.MaxCorner.X, CameraPointsAt.Y, g.Bounds.MinCorner.Z);
            Point maxXrear = new Point(g.Bounds.MaxCorner.X, CameraPointsAt.Y, g.Bounds.MaxCorner.Z);
            Point maxX = ((CameraVect.Dot((cameraPoint - maxXfront).Normalize()) < CameraVect.Dot((cameraPoint - maxXrear).Normalize()))) ? maxXfront : maxXrear;

            Point minXfront = new Point(g.Bounds.MinCorner.X, CameraPointsAt.Y, g.Bounds.MinCorner.Z);
            Point minXrear = new Point(g.Bounds.MinCorner.X, CameraPointsAt.Y, g.Bounds.MaxCorner.Z);
            Point minX = ((CameraVect.Dot((cameraPoint - minXfront).Normalize()) < CameraVect.Dot((cameraPoint - minXrear).Normalize()))) ? minXfront : minXrear;

            Point maxYfront = new Point(CameraPointsAt.X, g.Bounds.MaxCorner.Y, g.Bounds.MinCorner.Z);
            Point maxYrear = new Point(CameraPointsAt.X, g.Bounds.MaxCorner.Y, g.Bounds.MaxCorner.Z);
            Point maxY = ((CameraVect.Dot((cameraPoint - maxYfront).Normalize()) < CameraVect.Dot((cameraPoint - maxYrear).Normalize()))) ? maxYfront : maxYrear;

            Point minYfront = new Point(CameraPointsAt.X, g.Bounds.MinCorner.Y, g.Bounds.MinCorner.Z);
            Point minYrear = new Point(CameraPointsAt.X, g.Bounds.MinCorner.Y, g.Bounds.MaxCorner.Z);
            Point minY = ((CameraVect.Dot((cameraPoint - minYfront).Normalize()) < CameraVect.Dot((cameraPoint - minYrear).Normalize()))) ? minYfront : minYrear;

            double cameraFOVCos = Math.Min(CameraVect.Dot((cameraPoint - maxX).Normalize()), CameraVect.Dot((cameraPoint - minX).Normalize()));
            cameraFOVCos = Math.Min(cameraFOVCos, CameraVect.Dot((cameraPoint - maxY).Normalize()));
            cameraFOVCos = Math.Min(cameraFOVCos, CameraVect.Dot((cameraPoint - minY).Normalize()));
            return Math.Acos(cameraFOVCos) * 2;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the camera point. </summary>
        ///
        /// <remarks>   Kemp, 1/6/2019. </remarks>
        ///
        /// <param name="g">    A Group to point at. </param>
        ///
        /// <returns>   The calculated camera point. </returns>
        ///-------------------------------------------------------------------------------------------------

        static Point CalcCameraPoint(Group g, double cameraY, double cameraZ) {
            /// take a first guess at camera location
            Point cameraPoint = new Point(
                g.Bounds.MinCorner.X + (g.Bounds.MaxCorner.X - g.Bounds.MinCorner.X) / 2,
                g.Bounds.MinCorner.Y + (g.Bounds.MaxCorner.Y - g.Bounds.MinCorner.Y) / 2,
                -(2 * Math.Max(g.Bounds.MaxCorner.X - g.Bounds.MinCorner.X, g.Bounds.MaxCorner.Y - g.Bounds.MinCorner.Y)));
            Matrix m1 = MatrixOps.CreateRotationYTransform(cameraY);
            Matrix m2 = MatrixOps.CreateRotationXTransform(cameraZ);
            //Matrix m3 = MatrixOps.CreateTranslationTransform(
            //    g.Bounds.MinCorner.X + (g.Bounds.MaxCorner.X - g.Bounds.MinCorner.X) / 2,
            //    g.Bounds.MinCorner.Y + (g.Bounds.MaxCorner.Y - g.Bounds.MinCorner.Y) / 2, 
            //    -(2 * Math.Max(g.Bounds.MaxCorner.X - g.Bounds.MinCorner.X, g.Bounds.MaxCorner.Y - g.Bounds.MinCorner.Y)));
            Matrix m = (Matrix)(m1 * m2);
            cameraPoint = cameraPoint.Transform(m);
            //cameraPoint = cameraPoint.Transform(m3);
            //cameraPoint = cameraPoint.Transform(m2);
            //cameraPoint = cameraPoint.Transform(m1);
            /// Point camera at center of bounding box
            //Point centerPoint = new Point(g.Bounds.MaxCorner.X / 2, g.Bounds.MaxCorner.Y / 2, g.Bounds.MaxCorner.Z/2);
            //RayTracerLib.Vector centerVect = cameraPoint - centerPoint;
            //Ray cameraRay = new Ray(cameraPoint, centerVect);
            return cameraPoint;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Color all of the shapes in a group recursively. </summary>
        ///
        /// <remarks>  Kemp, 1/5/2019. </remarks>
        ///
        /// <param name="g">   A Group to process. </param>
        /// <param name="c">   A Color to apply. </param>
        ///-------------------------------------------------------------------------------------------------

        static void ColorShapes(Group g, Color c) {
            foreach (Shape s in g.Children) {
                if (s is Group) {
                    ColorShapes((Group)s, c);
                }
                else {
                    s.Material.Color = c;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <remarks>   Kemp, 11/19/2018. </remarks>
        ///
        /// <param name="args"> An array of command-line argument strings. Not used.</param>
        ///-------------------------------------------------------------------------------------------------

        static void Main(string[] args) {
            bool show_help = false;
            bool mirror = false;
            bool center = false;
            bool bbox = false;
            bool nm = false;
            bool np = false;
            string outputFile = "";
            uint canvasX = 200;
            uint canvasY = 200;
            uint rotx = 0;
            uint roty = 0;
            uint rotz = 0;
            double cameraZ = 0;
            double cameraY = 0;
            double myFOV = 0;
            bool serial = false;
            var pa = new OptionSet() {
                { "o|output=", "the {NAME} of the output PMM file.",
                    v => outputFile = v },
                { "W|width=",
                    "The width in pixels of the output image.",
                    (uint v) => canvasX = v },
                { "H|height=",
                    "The height in pixels of the output image.",
                    (uint v) => canvasY = v},
                { "h|help",  "show this message and exit",
                    v => show_help = v != null },
                { "x|xrotation=",
                    "The rotation around the x axis in units of PI/2.",
                    (uint v) => rotx = v },
                { "y|yrotation=",
                    "The rotation around the y axis in units of PI/2.",
                    (uint v) => roty = v },
                { "z|zrotation=",
                    "The rotation around the z axis in units of PI/2.",
                    (uint v) => rotz = v },
                { "cz|cameraz=",
                "The angle of rotation of the camera in the upward direction in radians.",
                (double v) => cameraZ = v },
                { "cy|cameray=",
                "The angle of rotation of the camera in the right-ward direction in radians.",
                (double v) => cameraY = v },
                {"fov|fieldofview=",
                "The field of view of the camera (zoom).",
                (double v) => myFOV = v},
                { "s|serial",  "Use only one processor to render.",
                    v => serial = v != null },
                { "M|mirror", "Mirror image on X",
                    v => mirror = v != null },
                { "C|center", "Center group at the origin",
                    v => center = v != null },
                { "B|boundingbox","Display bounding box of object",
                v => bbox = v != null },
                { "n|nomove","Don't move object to first quadrant",
                v => nm = v != null },
                { "p|noplane","Don't show the plane",
                v => np = v != null }

            };

            List<string> extra;
            try {
                extra = pa.Parse(args);
            }
            catch (OptionException e) {
                Console.Write("RenderTriangleMesh: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `RenderTriangleMesh --help' for more information.");
                return;
            }

            if (show_help) {
                Console.WriteLine("RenderTriangleMesh [Options] inputFileName");
                Console.WriteLine("Options: ");
                pa.WriteOptionDescriptions(Console.Out);
                return;
            }

            /// The only positional parameter is the file name to process

            if (extra.Count != 1) {
                Console.Write("RenderTriangleMesh: exactly one input file name permitted.");
                return;
            }

            string ifn = extra[0];
            string ifp = Path.GetDirectoryName(ifn);
            if (outputFile == "") {
                outputFile = Path.Combine(ifp, Path.GetFileNameWithoutExtension(ifn));
            }

            Console.WriteLine("Started: " + DateTime.Now.ToString());
            World w = new World();

            /// Read the triangular mesh and color all the triangles red.
            OBJFileParser p = new OBJFileParser(ifn, OBJFileParser.TriangleType.Smooth, mirror);
            Color red = new Color(1, 0, 0);
            Color blue = new Color(0, 0, 1);
            Color gray = new Color(0.5, 0.5, 0.5);
            /// collect what was parsed into a single group
            Group g = new Group();
            //Group a = Axes.Generate(25,5,new Color(1,0,0));
            //w.AddObject(a);
            //w.AddObject(BoundingBox.Generate(a,new Color(1,1,0)));

            foreach (Group gp in p.Groups) {
                g.AddObject(gp);
            }
            Console.WriteLine("Objects in default: " + g.Children.Count().ToString());
            //ColorShapes(g, gray);

            /// Transform the group containing the triangular mesh to correspond to our world and camera view point.
            /// Move it back to origin, rotate it, and return it to where it was.
            if (center) g.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0-(g.Bounds.MinCorner.X+(g.Bounds.MaxCorner.X-g.Bounds.MinCorner.X)/2),
                    0- (g.Bounds.MinCorner.Y + (g.Bounds.MaxCorner.Y - g.Bounds.MinCorner.Y) / 2),0 - (g.Bounds.MinCorner.Z + (g.Bounds.MaxCorner.Z - g.Bounds.MinCorner.Z) / 2)) * g.Transform);
            if (rotx != 0) g.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(rotx * (Math.PI / 2)) * g.Transform);
            if (roty != 0) g.Transform = (Matrix)(MatrixOps.CreateRotationYTransform(roty * (Math.PI / 2)) * g.Transform);
            if (rotz != 0) g.Transform = (Matrix)(MatrixOps.CreateRotationZTransform(rotz * (Math.PI / 2)) * g.Transform);
            /// Move the whole group into the first quadrant
            if (!nm) {
                g.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0 - g.Bounds.MinCorner.X, 0 - g.Bounds.MinCorner.Y, 0 - g.Bounds.MinCorner.Z) * g.Transform);
                w.AddObject(g);
            }

            /// add bounding box
            if (bbox) {
                Group bb = BoundingBox.Generate(g, blue);
                w.AddObject(bb);
            }

            /// three planes make up the background.
            if (!np) {
                Plane floor = new Plane();
                floor.Material.Pattern = new Checked3DPattern(new Color(1, 1, 1), new Color(0.75, 0.75, 0.75));
                floor.Material.Pattern.Transform = MatrixOps.CreateScalingTransform(2, 2, 2);

                w.AddObject(floor);

                Plane wallz = (Plane)floor.Copy();
                wallz.Transform = (Matrix)(MatrixOps.CreateRotationXTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
                //w.AddObject(wallz);

                Plane wallx = (Plane)floor.Copy();
                wallx.Transform = (Matrix)(MatrixOps.CreateRotationZTransform(Math.PI / 2) * MatrixOps.CreateTranslationTransform(0, 0, 0));
                //w.AddObject(wallx);
            }
            Console.WriteLine("Bounds of object: " + g.Bounds.ToString());

            w.AddLight(new LightPoint(new Point(50000, 70000, -50000), new Color(0.5, 0.5, 0.5)));
            // w.AddLight(new LightPoint(new Point(-50000, 70000, -50000), new Color(0.2, 0.2, 0.2)));
            // w.AddLight(new RTLightPoint(new RTPoint(0, 10, -10), new Color(0.5, 0.5, 0.5)));

            /// Render the image
            Console.WriteLine("Now rendering ...");
            Point cameraPoint = CalcCameraPoint(g, cameraY, cameraZ);
            Point cameraPointsAt = new Point(g.Bounds.MinCorner.X + (g.Bounds.MaxCorner.X - g.Bounds.MinCorner.X) / 2,
                g.Bounds.MinCorner.Y + (g.Bounds.MaxCorner.Y - g.Bounds.MinCorner.Y) / 2,
                g.Bounds.MinCorner.Z + (g.Bounds.MaxCorner.Z - g.Bounds.MinCorner.Z) / 2);
            double cameraFOV = myFOV == 0 ? CalcCameraFOV(cameraPoint, cameraPointsAt, g) : myFOV;
            Camera camera = new Camera(canvasX, canvasY, cameraFOV);
            camera.Transform = MatrixOps.CreateViewTransform(cameraPoint, cameraPointsAt, new RayTracerLib.Vector(0, 1, 0));
            Console.WriteLine("CameraPoint = " + cameraPoint.ToString() + "CameraPointsAt = " + cameraPointsAt.ToString());
            Console.WriteLine("CameraFOV = " + cameraFOV.ToString());
            Canvas image;

            if (serial) {
                image = w.Render(camera);
            }
            else {
                image = w.ParallelRender(camera);
            }
            Console.WriteLine("Now writing output ...");
            /*
            String ppm = image.ToPPM();
            String ppmname = ("ToPPM.ppm");
            System.IO.File.WriteAllText(outputFile, ppm);
            */
            image.WritePNG(outputFile + ".png");

            Console.WriteLine("Finished: " + DateTime.Now.ToString());
            Console.Write("Press Enter to finish ... ");
            Console.Read();

        }

    }
}
