///-------------------------------------------------------------------------------------------------
// file:	Canvas.cs
//
// summary:	Implements the canvas class
///-------------------------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A Canvas. </summary>
    ///
    /// <remarks>   Kemp, 11/8/2018. </remarks>
    /// <remarks>   A Canvas is a two dimensional matrix of Color objects. It is often used by the
    ///             Camera class.  This class defines the matrix in the desired height and width and
    ///             then permits reading and writing it.  Additionally, a method is provided that
    ///             will write this matrix to a ppm-format file.  The file is returned as a String.
    ///             I plan to do a png emitter as well. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Canvas
    {
        private Color[,] workspace;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the workspace. </summary>
        ///
        /// <value> The workspace. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color[,] Workspace { get { return workspace; } set { workspace = value; } }
        private  uint width;
        private  uint height;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the width. </summary>
        ///
        /// <value> The width. </value>
        ///-------------------------------------------------------------------------------------------------

        public  uint Width { get { return width; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the height. </summary>
        ///
        /// <value> The height. </value>
        ///-------------------------------------------------------------------------------------------------

        public  uint Height { get { return height; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="w">    An uint to process. </param>
        /// <param name="h">    An uint to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Canvas(uint w, uint h) {
            width = w;
            height = h;
            workspace = new Color[w, h];
            for (uint i = 0; i < width; i++) {
                for (uint j = 0; j < height; j++) {
                    workspace[i, j] = new Color(0, 0, 0);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Get Pixel. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="x">    An uint to process. </param>
        /// <param name="y">    An uint to process. </param>
        ///
        /// <returns>   Color of the selected pixel. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Color PixelAt(uint x, uint y) {
            return workspace[x, y].Copy();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Writes a pixel. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <param name="x">    X coordinate of pixel to write. </param>
        /// <param name="y">    Y coordinate of pixel to write. </param>
        /// <param name="c">    Color to copy into the selected pixel. </param>
        ///-------------------------------------------------------------------------------------------------

        public void WritePixel(uint x, uint y, Color c) {
            workspace[x, y].Red = c.Red;
            workspace[x, y].Green = c.Green;
            workspace[x, y].Blue = c.Blue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts this object to a ppm. </summary>
        ///
        /// <remarks>   Kemp, 11/8/2018. </remarks>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid. </exception>
        ///
        /// <returns>   This object as a String in ppm file format. </returns>
        ///-------------------------------------------------------------------------------------------------

        public String ToPPM() {
            double scale = 255.0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder((int)(width * height * 4 + 30));
            System.Text.StringBuilder sbl = new System.Text.StringBuilder((int)(70*4+10));
            String nl = Environment.NewLine;
            sb.Append("P3" + nl); // Magic character
            sb.Append(width.ToString() + " " + height.ToString() + nl); // canvas dimensions
            sb.Append("255" + nl); // max value for a color
            for (uint yi = 0; yi < height; yi++) {
                for (uint xi = 0; xi < width; xi++) {
                    if (yi > height) throw new InvalidOperationException();
                    uint r = (uint)Math.Max(Math.Min(workspace[xi, yi].X * scale, scale), 0.0);
                    uint g = (uint)Math.Max(Math.Min(workspace[xi, yi].Y * scale, scale), 0.0);
                    uint b = (uint)Math.Max(Math.Min(workspace[xi, yi].Z * scale, scale), 0.0);
                    sbl.Append(r.ToString() + " " + g.ToString() + " " + b.ToString() + " " );
                    if (sbl.Length > 60) {
                        sb.Append(sbl + nl);
                        sbl.Clear();
                    }
                }
                sb.Append(sbl + nl);
                sbl.Clear();
            }
            sb.Append(nl);
            return sb.ToString();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Writes a PNG file of the canvas </summary>
        ///
        /// <remarks>   Kemp, 1/3/2019. </remarks>
        ///
        /// <param name="filename"> Filename of the file. </param>
        ///-------------------------------------------------------------------------------------------------

        public void WritePNG(string filename) {
            double scale = 255.0;
            Bitmap bm = new Bitmap((int) width,(int) height);
            /// Convert the canvas to a System.Drawing.Bitmap.
            for (int h = 0; h < height; h++) {
                for (int w = 0; w < width; w++) {
                    int r, g, b;
                    if (Double.IsNaN(workspace[w, h].X)) {
                        r = g = b = 0;
                    }
                    else {
                        r = (int)Math.Max(Math.Min(workspace[w, h].X * scale, scale), 0.0);
                        g = (int)Math.Max(Math.Min(workspace[w, h].Y * scale, scale), 0.0);
                        b = (int)Math.Max(Math.Min(workspace[w, h].Z * scale, scale), 0.0);
                    }
                    bm.SetPixel(w, h, System.Drawing.Color.FromArgb(r,g,b));
                }
            }
            /// Write the bitmap.
            bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
