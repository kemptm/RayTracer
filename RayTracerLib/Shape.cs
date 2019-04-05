///-------------------------------------------------------------------------------------------------
// file:	Shape.cs
//
// summary:	Implements the ray tracer shape class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using RayTracerLib;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A Ray Tracer shape. This is the abstract shape object.  All shapes are derived from this.</summary>
    ///
    /// <remarks>   Kemp, 11/7/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public abstract class Shape
    {
        /// <summary>   The parent of this shape in the Group heirarchy. </summary>
        protected Shape parent;
        /// <summary>   The transform matrix that transforms this Shape to a superior coordinate system.. </summary>
        protected Matrix xform;
        /// <summary>   The material. This object contains information about the color, pattern, reflectivity, transparency and other parameters of the surface of the object </summary>
        protected Material material;
        /// <summary>   The bounds are the maximum and minimum coordinates of an AABB that contains the shape. </summary>
        protected Bounds bounds;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the parent reference. </summary>
        ///
        /// <value> The parent. </value>
        ///-------------------------------------------------------------------------------------------------

        public Shape Parent { get { return parent; } set { parent = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public Matrix Transform { get { return xform; } set { xform = value; BoundsCalc(); } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the material. </summary>
        ///
        /// <value> The material. </value>
        ///-------------------------------------------------------------------------------------------------

        public Material Material { get { return material; } set { material = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the bounds. </summary>
        ///
        /// <value> The bounds. </value>
        ///-------------------------------------------------------------------------------------------------

        public Bounds Bounds { get { return bounds; } set { bounds = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Shape() {
            xform = DenseMatrix.CreateIdentity(4);
            material = new Material();
            bounds = LocalBounds();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate bounds in  the local coordinate space (Abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   The Bounds. </returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract Bounds LocalBounds();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate the bounds (bounding box) of this shape. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void BoundsCalc() {
            bounds.Calc(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Local intersect (abstract). </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="ray">  The ray to intersect. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract List<Intersection> LocalIntersect(Ray ray);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the intersection of a ray with this shape. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="rayparm">  The ray to determine intersection with. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt;  containing the Intersections if any.</returns>
        ///-------------------------------------------------------------------------------------------------

        public List<Intersection> Intersect(Ray rayparm) {
            if (!bounds.Intersect(rayparm)) return new List<Intersection>(); // miss the bounding box?
            Ray ray = rayparm.Transform((Matrix)xform.Inverse());
            return LocalIntersect(ray);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copy the shape (Virtual). </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <returns>   A Shape. </returns>
        ///-------------------------------------------------------------------------------------------------
        public abstract Shape Copy(); /* {
            RTShape s = new RTShape();
            s.xform = (Matrix) xform.Clone();
            s.material = material.Copy();
            s.bounds = bounds.Copy();
            return s;
        }
        */


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="localPoint">   The local point. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public abstract Vector LocalNormalAt(Point localPoint);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate normal at a point in the local coordinate system of an RTShape. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        /// <remarks>   This should never be called.  It is used and overridden only by SmoothTriangle. It is virtual
        ///             instead of abstract so that all of the other shapes aren't required to implement it.</remarks>
        ///
        /// <param name="worldPoint">   The world point. </param>
        /// <param name="hit">          The hit. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual Vector LocalNormalAt(Point worldPoint, Intersection hit) {
            return new Vector(0, 0, 0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate the Normal Vector at the specified poin tin the world coordinate system above this shape. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="worldPoint">   The world point. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector NormalAt(Point worldPoint) {
            Point localp = WorldToObject(worldPoint);
            Vector localn = LocalNormalAt(localp);
            return NormalToWorld(localn);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Calculate the Normal Vector at the specified point in the world coordinate system above
        ///     this shape.
        /// </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///
        /// <param name="worldPoint">   The world point. </param>
        /// <param name="hit">          The hit. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector NormalAt(Point worldPoint, Intersection hit) {
            Point localp = WorldToObject(worldPoint);
            Vector localn;

            if (this is SmoothTriangle) {
                localn = LocalNormalAt(worldPoint, hit);
            }
            else {
                localn = LocalNormalAt(localp);
            }
            return NormalToWorld(localn);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transform a world point to object local point. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   A Point. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Point WorldToObject(Point point) {
            if (this.parent != null) point = parent.WorldToObject(point);
            return (Matrix)Transform.Inverse() * point;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Transforma a Normal to world coordinates. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="normal">   The local normal. </param>
        ///
        /// <returns>   A Vector. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Vector NormalToWorld(Vector normal) {
            normal = (Matrix)Transform.Transpose().Inverse() * normal;
            normal.W = 0;
            normal = normal.Normalize();

            if (parent != null) normal = parent.NormalToWorld(normal);
            return normal;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this RTShape is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 11/7/2018. </remarks>
        ///
        /// <param name="s">    The Shape to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(Shape s) {
            bool b = true;
            b = b && (this.GetType() == s.GetType());
            b = b && xform.Equals(s.xform);
            b = b && material.Equals(s.material);
            b = b && bounds.Equals(s.bounds);
            return b;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Does the complex shape Include the given parameter Shape. </summary>
        ///
        /// <remarks>   Kemp, 12/2/2018. 
        ///             This is overridden only by CSG and Group. </remarks>
        ///
        /// <param name="x">    A Shape to process. </param>
        ///
        /// <returns>   True if it is included, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual bool Includes(Shape x) {
            return false;
        }

       // public Color Lighting(LightPoint light, Point worldPosition, Vector eyev, Vector normalv, bool inShadow = false) {
       //     return new Color(0, 0, 0);
       // }
       
    }
}
