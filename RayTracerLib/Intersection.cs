﻿///-------------------------------------------------------------------------------------------------
// file:	Intersection.cs
//
// summary:	Implements the intersection class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An intersection. 
    ///             
    ///             An intersection represents the results of determining whether or not a ray intersects with a shape.
    ///             That intersection is specified by a reference to the shape and the distance to the surface of the shape from the 
    ///             origin of the ray. Various other attributes are calculated.</summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Intersection
    {
        protected Shape obj;
        protected double t;
        protected Point point;
        protected Vector eyev;
        protected Vector normalv;
        protected Vector reflectv;
        protected bool inside;
        protected bool prepared = false;
        protected double n1;
        protected double n2;
        protected Point underPoint;
        protected double u;
        protected double v;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the object. </summary>
        ///
        /// <value> The object. </value>
        ///-------------------------------------------------------------------------------------------------

        public Shape Obj { get { return obj; }  }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the distance, t, to the shape. </summary>
        ///
        /// <value> The t. </value>
        ///-------------------------------------------------------------------------------------------------

        public double T { get { return t; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the point of intersection on the object. </summary>
        ///
        /// <value> The point. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point Point { get { return point; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the Eye Vector, eyev. </summary>
        ///
        /// <value> The eyev. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Eyev { get { return eyev; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the normal vector of the point of intersection. </summary>
        ///
        /// <value> The normal. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Normalv { get { return normalv; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the reflected vector from the light source. </summary>
        ///
        /// <value> The reflectv. </value>
        ///-------------------------------------------------------------------------------------------------

        public Vector Reflectv { get { return reflectv; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether the inside. </summary>
        ///
        /// <value> True if inside, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool Inside { get { return inside; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets n1, a computation done in the Prepare method. </summary>
        ///
        /// <value> The n 1. </value>
        ///-------------------------------------------------------------------------------------------------

        public double N1 { get { return n1; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets  n2, a computation done in the Prepare method. </summary>
        ///
        /// <value>  n2. </value>
        ///-------------------------------------------------------------------------------------------------

        public double N2 { get { return n2; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the under point. </summary>
        ///
        /// <value> The under point. </value>
        ///-------------------------------------------------------------------------------------------------

        public Point UnderPoint { get { return underPoint; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the u property. </summary>
        ///
        /// <value> The u. </value>
        ///-------------------------------------------------------------------------------------------------

        public double U { get { return u; } set { u = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the v property. </summary>
        ///
        /// <value> The v. </value>
        ///-------------------------------------------------------------------------------------------------

        public double V { get { return v; } set { v = value; } }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Intersection() {
            obj = null;
            t = 0;
            point = null;
            eyev = null;
            normalv = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="t">    The distance, t, of the object from the ray origin. </param>
        /// <param name="o">    The object intersected. </param>
        ///-------------------------------------------------------------------------------------------------

        public Intersection(double t, Shape o) {
            this.t = t;
            obj = o;
        }

        public Intersection(double t, Shape o, double u, double v) {
            this.t = t;
            obj = o;
            this.u = u;
            this.v = v;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Builds a set of Intersections into a List&lt;Intersection&gt;. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="intersects">   A variable-length parameters list containing Intersection. </param>
        ///
        /// <returns>   A List&lt;Intersection&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<Intersection> Intersections(params Intersection [] intersects) {
            List<Intersection> xs = new List<Intersection>();
            foreach(Intersection i in intersects) {
                xs.Add(i);
            }
            return xs;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Determines which Intersection in a list is the closest in the direction of the ray. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="xs">   The A list of Intersection. </param>
        ///
        /// <returns>   An Intersection. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Intersection Hit(List<Intersection> xs) {
            Intersection lowT = null;
            if (xs.Count == 0) return lowT;
            foreach (Intersection i in xs) {
                if (i.T >= 0) {
                    if ((lowT == null) || (i.T < lowT.T)) {
                        lowT = i;
                    }
                }
            }
            return lowT;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Prepares.  That is, it does common pre-computations for Intersection</summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="r">    A RTRay to consider. </param>
        /// <param name="xs">   The List of Intersection to prepare. </param>
        ///-------------------------------------------------------------------------------------------------

        public Intersection Prepare(Ray r,List<Intersection> xs) {
            const double shadowFudge = 0.0001;
            List<Shape> containers = new List<Shape>();
            // figure out refractive indexes at each point along the ray, in and out.
            foreach (Intersection i in xs) {
                if (i == this) {
                    if (containers.Count == 0) {
                        n1 = 1.0;
                    }
                    else {
                        n1 = containers.Last().Material.RefractiveIndex;
                    }
                }

                if (containers.Contains(i.obj)) {
                    containers.Remove(i.obj);
                }
                else {
                    containers.Add(i.obj);
                }

                if (i == this) {
                    if (containers.Count == 0) {
                        n2 = 1.0;
                    }
                    else {
                        n2 = containers.Last().Material.RefractiveIndex;
                    }
                    break;
                }
            }

            point = r.Position(t);
            eyev = -r.Direction;
            normalv = obj.NormalAt(point,this);

            if (normalv.Dot(eyev) < 0) {
                inside = true;
                normalv = -normalv;
            }
            else {
                inside = false;
            }
            reflectv = r.Direction.Reflect(normalv);
            Vector NormalFudge = normalv * shadowFudge;
            underPoint = point - NormalFudge;
            point = point + NormalFudge;

            prepared = true;
            return this;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate the resultant color of a pixel. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the Intersection is not prepared. <see cref="Prepare(Ray, List{Intersection})"/>. </exception>
        ///
        /// <param name="w">                    A RTWorld to process. </param>
        /// <param name="recursionRemaining">   (Optional) The recursion remaining. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Color Shade(World w, int recursionRemaining = 5) {
            if (!prepared) throw new InvalidOperationException("Intersection not Prepared");
            Color shade = new Color(0, 0, 0);
            Color reflected;
            Color refracted;
            // if no lights, then just compute ambient, etc wihtout light.
            if (w.Lights.Count == 0) {
                return Ops.Lighting(obj.Material, obj, new LightPoint(), point, eyev, normalv, false) + ReflectedColor(w, recursionRemaining);
            }
            // For each light figure out contributions of reflected, refracted, and shade to color of point
            foreach (LightPoint l in w.Lights) {

                shade = shade + Ops.Lighting(obj.Material, obj, l, point, eyev, normalv,point.IsShadowed(w,l));
                reflected = ReflectedColor(w,recursionRemaining);
                refracted = RefractedColor(w, recursionRemaining);

                Material material = obj.Material;
                if ((material.Reflective > 0) && (material.Transparency > 0)) {
                    double reflectance = this.Schlick();
                    return shade + reflected * reflectance + refracted * (1 - reflectance);
                }

                shade = shade + reflected + refracted;
            }

            return shade;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate the Reflected color component of a pixel. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="world">                The world. </param>
        /// <param name="recursionRemaining">   (Optional) The recursion remaining. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Color ReflectedColor(World world,int recursionRemaining=5) {
            if (recursionRemaining <=0 ) return new Color(0, 0, 0);
            if (obj.Material.Reflective == 0) return new Color(0, 0, 0);

            Ray reflectRay = new Ray(point, reflectv);
            Color color = world.ColorAt(reflectRay, recursionRemaining - 1);

            return color * obj.Material.Reflective;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate the Refracted color component of a pixel. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="w">                    the world. </param>
        /// <param name="recursionRemaining">   (Optional) The recursion remaining. </param>
        ///
        /// <returns>   A Color. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Color RefractedColor(World w, int recursionRemaining=5) {
            if (recursionRemaining <= 0) return new Color(0, 0, 0);
            if (obj.Material.Transparency == 0) return new Color(0, 0, 0);

            // Snell's Law
            //
            // sin(ThetaI)/sin(ThetaT) = n1/n2
            double nRatio = n1 / n2;
            double cosThetaI = eyev.Dot(normalv);
            double sin2ThetaT = (nRatio * nRatio)*(1- (cosThetaI*cosThetaI));
            if (sin2ThetaT > 1) {
                return new Color(0, 0, 0);
            }
            double cosThetaT = Math.Sqrt(1 - sin2ThetaT);
            Vector direction = normalv * (nRatio * cosThetaI - cosThetaT) - eyev * nRatio;
            Ray refractedRay = new Ray(underPoint, direction);
            Color color = w.ColorAt(refractedRay, recursionRemaining - 1) * obj.Material.Transparency;

            return color;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the fresnel reflection of a surface using the schlick algorithm. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <returns>   A double. </returns>
        ///-------------------------------------------------------------------------------------------------

        public double Schlick() {
            double cos = eyev.Dot(normalv); // cosine of eye and normal
            
            // total internal reflection can only occur if n1 > n2
            if (n1>n2) {
                double nRatio = n1 / n2;
                double sin2T = nRatio * nRatio * (1.0 - cos * cos);
                if (sin2T > 1) return 1;
                double cosT = Math.Sqrt(1.0 - sin2T);
                cos = cosT;
            }
            double r0 = (n1 - n2) / (n1 + n2);
            r0 *= r0; // r0^2
            double cospow = 1 - cos;
            cospow = cospow * cospow * cospow * cospow * cospow; // (1-cos)^5
            return r0 + (1 - r0) * cospow;
        }


    }
}
