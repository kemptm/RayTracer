﻿///-------------------------------------------------------------------------------------------------
// file:	Group.cs
//
// summary:	Implements the group class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A group. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This class implements a collection of Shapes as a single shape, applying the Group's 
    ///             Transform as necessary to all members of the group.  This is necessary for building 
    ///             shapes out of a collection of shapes.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Group : Shape {
        /// <summary>   The children; the subordinate shapes of this collection </summary>
        protected List<Shape> children;
        /// <summary>   The name of the group (usually ""). </summary>
        protected string name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the children. </summary>
        ///
        /// <value> The children. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Shape> Children { get { return children; } set { children = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name of the group. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public string Name { get { return name; } set { name = value; } }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Group() {
            children = new List<Shape>();
            name = "";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an object, or shape to the collection. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="o">    A Shape to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public void AddObject(Shape o) {
            /// Add the child.
            children.Add(o);
            o.Parent = this;
            /// Recalculate the bounds of this group.
            BoundsCalc();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the object or shape described by o. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="o">    A Shape to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public void RemoveObject(Shape o) {
            /// remove the specified child from the list
            children.Remove(o);
            if (o.Parent == this) o.Parent = null;
            /// Recalculate the bounds of this group.
            BoundsCalc();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Shape. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Shape Copy() {
            Group g = new Group();
            foreach (Shape c in children) {
                g.AddObject(c.Copy());
            }
            g.material = material.Copy();
            g.Transform = (Matrix) Transform.Clone();
            g.bounds = bounds.Copy();
            return g;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local intersect (abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="rayparm">  The ray to intersect. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public override List<Intersection> LocalIntersect(Ray rayparm) {
            List<Intersection> xs = new List<Intersection>();
            foreach(Shape s in children) {
                xs.AddRange(s.Intersect(rayparm));
            }
            xs.Sort((x, y) => x.T < y.T ? -1 : x.T > y.T ? 1 : 0);
            return xs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an Shape. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Vector LocalNormalAt(Point localPoint) {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override Bounds LocalBounds() {
            if (children == null || children.Count == 0) return new Bounds(new Point(0, 0, 0), new Point(0, 0, 0));

            Bounds b = new Bounds(new Point(double.MaxValue, double.MaxValue, double.MaxValue), new Point(-double.MaxValue, -double.MaxValue, -double.MaxValue));

            foreach(Shape c in children) {
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
        /// <summary>   Does Subtree Include the given Shape. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. </remarks>
        ///
        /// <param name="x">    A Shape to process. </param>
        ///
        /// <returns>   True if it is in subtree, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override bool Includes(Shape x) {
            foreach(Shape s in children) {
                if (s is CSG || s is Group) {
                    if (s.Includes(x)) return true;
                }
                if (s.Equals(x)) return true;
            }
            return false;
        }
    }
}
