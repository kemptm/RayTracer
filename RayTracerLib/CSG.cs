///-------------------------------------------------------------------------------------------------
// file:	CSG.cs
//
// summary:	Implements the CSG class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A CSG: Constructive Solid Geometry . </summary>
    ///
    /// <remarks>   Kemp, 11/26/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CSG : Shape {
        /// <summary>   The CSG operation. </summary>
        protected Ops operation;
        /// <summary>   The left shape operand. </summary>
        protected Shape left;
        /// <summary>   The right shape operand. </summary>
        protected Shape right;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the left. </summary>
        ///
        /// <value> The left. </value>
        ///-------------------------------------------------------------------------------------------------

        public Shape Left { get { return left; } set { SetShape(ref left, value); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the right. </summary>
        ///
        /// <value> The right. </value>
        ///-------------------------------------------------------------------------------------------------

        public Shape Right { get { return right; } set { SetShape(ref right, value); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the operation. </summary>
        ///
        /// <value> The operation. </value>
        ///-------------------------------------------------------------------------------------------------

        public Ops Operation { get { return operation; } set { operation = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Values that represent operations. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public enum Ops  {None, Union, Intersection, Difference };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CSG() {
            bounds = LocalBounds();
            operation = Ops.None;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <param name="op">   The operation. </param>
        /// <param name="l">    A Shape to process. </param>
        /// <param name="r">    A Shape to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public CSG(Ops op, Shape l, Shape r) {
            operation = op;
            SetShape(ref left, l);
            SetShape(ref right, r);
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy this shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <returns>   A Shape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {
            return (Shape)new CSG(operation, left, right);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Bounds LocalBounds() {
            if (left == null || right == null) return new Bounds(new Point(0, 0, 0), new Point(0, 0, 0));

            Bounds b = new Bounds(new Point(double.MaxValue, double.MaxValue, double.MaxValue), new Point(-double.MaxValue, -double.MaxValue, -double.MaxValue));
            List<Shape> lr = new List<Shape>();
            lr.Add(left);
            lr.Add(right);
            foreach (Shape c in lr) {
                if (c.Bounds.MinCorner.X < b.MinCorner.X) b.MinCorner.X = c.Bounds.MinCorner.X;
                if (c.Bounds.MinCorner.Y < b.MinCorner.Y) b.MinCorner.Y = c.Bounds.MinCorner.Y;
                if (c.Bounds.MinCorner.Z < b.MinCorner.Z) b.MinCorner.Z = c.Bounds.MinCorner.Z;
                if (c.Bounds.MaxCorner.X > b.MaxCorner.X) b.MaxCorner.X = c.Bounds.MaxCorner.X;
                if (c.Bounds.MaxCorner.Y > b.MaxCorner.Y) b.MaxCorner.Y = c.Bounds.MaxCorner.Y;
                if (c.Bounds.MaxCorner.Z > b.MaxCorner.Z) b.MaxCorner.Z = c.Bounds.MaxCorner.Z;
            }
            return b;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local intersect (abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <param name="ray">  The ray to intersect. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public override List<Intersection> LocalIntersect(Ray ray) {
            List<Intersection> leftxs = left.Intersect(ray);
            List<Intersection> rightxs = right.Intersect(ray);
            leftxs.AddRange(rightxs);
            leftxs.Sort((x, y) => x.T < y.T ? -1 : x.T > y.T ? 1 : 0);
            return FilterIntersections(leftxs);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of a Shape. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint) {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets a shape on either left or right. </summary>
        ///
        /// <remarks>   Kemp, 11/26/2018. </remarks>
        ///
        /// <param name="lr">       [in,out] The shape to set. </param>
        /// <param name="newShape"> The new shape. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void SetShape(ref Shape lr, Shape newShape) {
            if (lr != null) {
                lr.Parent = null;
            }
            lr = newShape;
            lr.Parent = this;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Intersection allowed. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        /// <remarks> Check to see whether this intersection is visible under the conditions of the named CSG
        ///           operation.</remarks>
        ///
        /// <param name="o">    The Operation to process. </param>
        /// <param name="lh">   true == Left Shape Hit. false == Right Shape Hit. </param>
        /// <param name="il">   true == Hit was inside Left Shape. </param>
        /// <param name="ir">   true == Hit was inside Right Shape. </param>
        ///
        /// <returns>   True if the intersection should be visible. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool IntersectionAllowed(CSG.Ops o, bool lh, bool il, bool ir) {
            switch (o) {
                case CSG.Ops.Union:
                    return (lh && !ir) || (!lh && !il);
                case CSG.Ops.Intersection:
                    return (lh && ir) || (!lh && il);
                case CSG.Ops.Difference:
                    return (lh && !ir) || (!lh && il);
                default:
                    return false;
            }
            //return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Does the complex shape Include the given parameter Shape. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///
        /// <param name="x">    A Shape to process. </param>
        ///
        /// <returns>   True if it is included, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool Includes(Shape x) {
            if (left is CSG || left is Group) {
                if (left.Includes(x)) return true;
            }

            if (left.Equals(x)) return true;

            if (right is CSG || right is Group) {
                if (right.Includes(x)) return true;
            }
            if (right.Equals(x)) return true;

            return false;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Filter intersections. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///
        /// <param name="xs">   The list of intersections to filter. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<Intersection> FilterIntersections(List<Intersection> xs) {
            List<Intersection> result = new List<Intersection>();

            bool leftHit = false;
            bool insideLeft = false;
            bool insideRight = false;

            foreach(Intersection x in xs) {
                leftHit = false;
                if (left is CSG || left is Group) {
                    leftHit = left.Includes(x.Obj);
                }
                else {
                    leftHit = left.Equals(x.Obj);
                }

                if (IntersectionAllowed(operation, leftHit, insideLeft, insideRight)) result.Add(x);

                if (leftHit) insideLeft = !insideLeft;
                else insideRight = !insideRight;

            }

            return result;
        }
    }
}
