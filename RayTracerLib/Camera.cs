///-------------------------------------------------------------------------------------------------
// file:	RTCamera.cs
//
// summary:	Implements the right camera class
///-------------------------------------------------------------------------------------------------

using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A Camera is a virtual point-of-view through a Canvas into the ray-traced world.
    /// </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   It may be translated and rotated to point at any particular aspect of the scene.
    ///             At one world-unit in in front of the camera is the Canvas, a 2 dimensional array
    ///             of pixels, which will represent the 2D transformation of the camera's view into
    ///             the scene. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Camera {
        /// <summary>   The horizontal size (in pixels) of the canvas that the picture will be rendered to. </summary>
        protected uint hsize;
        /// <summary>   The canvas' vertical size (in pixels). </summary>
        protected uint vsize;
        /// <summary>   The field of view is an angle, in radians, that describes how much the camera can see.  When the 
        ///             field of view is small, the view will be "zoomed in," magnifying a smaller area of the 
        ///             scene. </summary>
        protected double fieldOfView;
        /// <summary>   The transform is a matrix describing how the world should be oriented relative to the camera.
        ///             This is usually a view transformation. </summary>
        protected Matrix transform;
        /// <summary>   Size of the pixel on the canvas in front of the camera in world space units. </summary>
        protected double pixelSize;
        /// <summary>   Width of half of the field-of-view. </summary>
        protected double halfWidth;
        /// <summary>   Height of half of the field-of-view. </summary>
        protected double halfHeight;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Horizontal Size. </summary>
        ///
        /// <value> The hsize. </value>
        ///-------------------------------------------------------------------------------------------------

        public uint Hsize { get { return hsize; } set { hsize = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the Vertical Size. </summary>
        ///
        /// <value> The vsize. </value>
        ///-------------------------------------------------------------------------------------------------

        public uint  Vsize { get { return vsize; } set { vsize = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the field of view. </summary>
        ///
        /// <value> The field of view. </value>
        ///-------------------------------------------------------------------------------------------------

        public double FieldOfView { get { return fieldOfView; } set { fieldOfView = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transform. </summary>
        ///
        /// <value> The transform. </value>
        ///-------------------------------------------------------------------------------------------------

        public Matrix Transform { get { return transform; } set { transform = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the pixel size. </summary>
        ///
        /// <value> The size of the pixel. </value>
        ///-------------------------------------------------------------------------------------------------

        public double PixelSize { get { return pixelSize; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Camera() {
            hsize = 0;
            vsize = 0;
            fieldOfView = 0.0;
            transform = DenseMatrix.CreateIdentity(4);
            pixelSize = CalcPixelSize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="h">    canvas horizontal size in pixels. </param>
        /// <param name="v">    canvas vertical size in pixels. </param>
        /// <param name="fov">    Field-of-View in radians. </param>
        ///-------------------------------------------------------------------------------------------------

        public Camera(uint h, uint v, double fov) {
            hsize = h;
            vsize = v;
            fieldOfView = fov;
            transform = DenseMatrix.CreateIdentity(4);
            pixelSize = CalcPixelSize();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates the pixel size. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   The calculated pixel size. </returns>
        ///-------------------------------------------------------------------------------------------------

        double CalcPixelSize() {
            double halfView = Math.Tan(fieldOfView/2.0);
            double aspect = (double)hsize / (double)vsize;
                        
            if (aspect >= 1) {
                halfWidth = halfView;
                halfHeight = halfView / aspect;
            }
            else {
                halfWidth = halfView * aspect;
                halfHeight = halfView;
            }
            return (halfWidth * 2.0) / hsize;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Construct a RTRay from camera through specified canvas pixel. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="x">    X coordinate of pixel on canvas. </param>
        /// <param name="y">    Y coordinate of pixel on canvas. </param>
        ///
        /// <returns>   A RTRay. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Ray RayForPixel(uint x, uint y) {
            double xOffset = (x + 0.5) * pixelSize;
            double yOffset = (y + 0.5) * pixelSize;

            double worldX = halfWidth - xOffset;
            double worldY = halfHeight - yOffset;

            Point pixel = (Matrix)transform.Inverse() * new Point(worldX, worldY, -1);
            Point origin = (Matrix)transform.Inverse() * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new Ray(origin,direction);
        }
    }
}