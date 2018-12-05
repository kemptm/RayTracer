////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	BoundingBox.cs
//
// summary:	Implements the bounding box class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace RayTracerLib
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Generate a bounding box for a shape. </summary>
    ///
    /// <remarks>   This class exists to define a static BoundingBox.Generate method. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class BoundingBox
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generate a Bounding Box. </summary>
        ///
        /// <remarks>
        ///     This method generates a new RTGroup that coontains 12 RTLineSegment
        ///           objects defining the bounds of the passed shape. The intent is to make
        ///           visible the bounding box around the passed shape.  The passed shape may also be an
        ///           RTGroup containing a number of shapes.
        /// </remarks>
        ///
        /// <param name="s">    The shape or group of shapes to create the bonding box around. </param>
        /// <param name="cc">   (Optional) The color to assign to the created line segments. </param>
        ///
        /// <returns>   An RTGroup. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Group Generate(Shape s, Color cc = null) {

            /// The group to contain the created line segments 
            Group g = new Group();

            /// If color isn't specified, use white.
            Color c;
            if (cc == null) c = new Color(1, 1, 1);
            else c = cc;

            Point minbb = s.Bounds.MinCorner;
            Point maxbb = s.Bounds.MaxCorner;

             LineSegment ls = new LineSegment();
            /// Create 4 segments in y direction from x and z min maxes
            {
                ls.PLo = minbb.Y;
                ls.PHi = maxbb.Y;
                ls.Transform = MatrixOps.CreateTranslationTransform(minbb.X, 0, minbb.Z);
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.Y;
                ls.PHi = maxbb.Y;
                ls.Transform = MatrixOps.CreateTranslationTransform(minbb.X, 0, maxbb.Z);
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.Y;
                ls.PHi = maxbb.Y;
                ls.Transform = MatrixOps.CreateTranslationTransform(maxbb.X, 0, minbb.Z);
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.Y;
                ls.PHi = maxbb.Y;
                ls.Transform = MatrixOps.CreateTranslationTransform(maxbb.X, 0, maxbb.Z);
                ls.Material.Color = c.Copy();
                g.AddObject(ls);
            }
            /// Create 4 segments in x direction from y and z min maxes
            {
                ls = new LineSegment();
                ls.PLo = minbb.X;
                ls.PHi = maxbb.X;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, minbb.Y, minbb.Z) * MatrixOps.CreateRotationZTransform(-Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.X;
                ls.PHi = maxbb.X;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, minbb.Y, maxbb.Z) * MatrixOps.CreateRotationZTransform(-Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.X;
                ls.PHi = maxbb.X;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, maxbb.Y, minbb.Z) * MatrixOps.CreateRotationZTransform(-Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.X;
                ls.PHi = maxbb.X;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(0, maxbb.Y, maxbb.Z) * MatrixOps.CreateRotationZTransform(-Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);
            }
            /// Create 4 segments in the z direction and run from min to max in x and y
            {
                ls = new LineSegment();
                ls.PLo = minbb.Z;
                ls.PHi = maxbb.Z;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(minbb.X, minbb.Y, 0) * MatrixOps.CreateRotationXTransform(Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.Z;
                ls.PHi = maxbb.Z;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(minbb.X, maxbb.Y, 0) * MatrixOps.CreateRotationXTransform(Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.Z;
                ls.PHi = maxbb.Z;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(maxbb.X, minbb.Y, 0) * MatrixOps.CreateRotationXTransform(Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);

                ls = new LineSegment();
                ls.PLo = minbb.Z;
                ls.PHi = maxbb.Z;
                ls.Transform = (Matrix)(MatrixOps.CreateTranslationTransform(maxbb.X, maxbb.Y, 0) * MatrixOps.CreateRotationXTransform(Math.PI / 2));
                ls.Material.Color = c.Copy();
                g.AddObject(ls);
            }
            /// Return the created group.
            return g;
        }
    }
}
