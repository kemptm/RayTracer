///-------------------------------------------------------------------------------------------------
// file:	Material.cs
//
// summary:	Implements the material class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A material. </summary>
    ///
    /// <remarks>   Kemp, 11/9/2018. </remarks>
    /// <remarks>   This class encapsulates all of the attributes that contribut to the perceived color
    ///             of a surface rendered.</remarks>
    ///-------------------------------------------------------------------------------------------------

    public class Material
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The name. </summary>
        /// 
        /// <remarks>
        ///     This is the name of the material as defined by an entry in the MTL file.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------
        protected string name;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The color. </summary>
        ///
        /// <remarks>
        ///     This is the intrinsic color of the Shape. Any reflected, ambient or refracted rays will
        ///     be blended with this color.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Color color;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The ambient light. </summary>
        ///
        /// <remarks>
        ///     This is a factor for adding the notion of an "ambient" light contribution.  It is a hack
        ///     to simulate light reflected many times so that it seems to come from everywhere.  For
        ///     very pure scenes, make it zero.  For a self-lit Shape, make it 1.0.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Color ambient;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The diffuse. </summary>
        ///
        /// <remarks>
        ///     This is a factor used by the Phong model.  It indicates how wide an angle that light
        ///     reflected from a surface may be seen.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Color diffuse;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The specular. </summary>
        ///
        /// <remarks>
        ///     This is a factor that determines whether and how much a specular highlihght spot will be
        ///     on a surface.  That is how large an angle will the specular light be distributed around.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Color specular;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The shininess. </summary>
        ///
        /// <remarks>   This factor controls how wide the specular highlight will be. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected double shininess;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The reflective. </summary>
        ///
        /// <remarks>
        ///     This attribute controls the contribution of the reflective algorithm that calculates how
        ///     well items in the vicinity are seen on the reflective surface.  Zero will not show
        ///     surrounding Shapes at all.  1.0 will make the surfact completely mirrorlike.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected double reflective;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The transparency. </summary>
        ///
        /// <remarks>
        ///     This is a value between 0.0 and 1.0 that describes the transparency of the Shape. At 0.0,
        ///     the Shape is opaque.  At 1.0, the shape is completely transparent.  This attribute works
        ///     in harmony with the refractiveIndex attribute.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected double transparency;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Zero-based refractive index of the material. </summary>
        ///
        /// <remarks>
        ///     This is the standard refractive index which computes how much a ray crossing a surface
        ///     between two different materials bends.  Air is 1.0.  Water is 1.33.  Glass is 1.5.  It is this
        ///     refractiveIndex that controls the magnification that is seen through the shape.  The world
        ///     is presumed to have refractiveIndes = 1.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected double refractiveIndex;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Specifies the pattern. </summary>
        ///
        /// <remarks>
        ///     This specifies an object that modifies the resultant color according to a pattern
        ///     algorighm.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected Pattern pattern;

        protected uint illuminationMode;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name. </value>
        ///-------------------------------------------------------------------------------------------------

        public String Name { get { return name; } set { name = value; } }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the color attribute. </summary>
        ///
        /// <value> The color. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Color { get { return color; } set { color = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the ambient attribute. </summary>
        ///
        /// <value> The ambient. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Ambient { get { return ambient; } set { ambient = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the diffuse attribute. </summary>
        ///
        /// <value> The diffuse. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Diffuse { get { return diffuse; } set { diffuse = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the specular attribute. </summary>
        ///
        /// <value> The specular. </value>
        ///-------------------------------------------------------------------------------------------------

        public Color Specular { get { return specular; } set { specular = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the shininess attribute. </summary>
        ///
        /// <value> The shininess. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Shininess { get { return shininess; } set { shininess = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the reflective attribute. </summary>
        ///
        /// <value> The reflective. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Reflective { get { return reflective; } set { reflective = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the pattern attribute. </summary>
        ///
        /// <value> The pattern. </value>
        ///-------------------------------------------------------------------------------------------------

        public Pattern Pattern { get { return pattern; } set { pattern = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the transparency attribute. </summary>
        ///
        /// <value> The transparency. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Transparency { get { return transparency; } set { transparency = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the zero-based index of the refractive. </summary>
        ///
        /// <value> The refractive index. </value>
        ///-------------------------------------------------------------------------------------------------

        public double RefractiveIndex { get { return refractiveIndex; } set { refractiveIndex = value; } }

        public uint IlluminationMode { get { return illuminationMode; } set { illuminationMode = value; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public Material() {
            InitializeMaterial();
        }

        protected void InitializeMaterial() { 
            name = "";
            color = new Color(1, 1, 1);
            //color = new Color(0, 0, 0);
            ambient = new Color(0.1, 0.1, 0.1);
            diffuse = new Color(0.9, 0.9, 0.9);
            specular = new Color(0.9, 0.9, 0.9);
            shininess = 200;
            reflective = 0;
            transparency = 0;
            refractiveIndex = 1;
            illuminationMode = 2;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 1/17/2019. </remarks>
        ///
        /// <param name="n">    A string to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public Material(string n) {
            InitializeMaterial();
            name = n;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="c">    A Color. </param>
        /// <param name="a">    Ambient attribute. </param>
        /// <param name="d">    Diffuse attribute. </param>
        /// <param name="sp">   The specular attribute. </param>
        /// <param name="sc">   The shininess attribute. </param>
        ///-------------------------------------------------------------------------------------------------

        public Material(Color c, Color a, Color d, Color sp, double sc) {
            InitializeMaterial();
            color = new Color(c.Red,c.Green,c.Blue);
            ambient = a;
            diffuse = d;
            specular = sp;
            shininess = sc;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="p">    Pattern attribute. </param>
        /// <param name="c">    Color attribute. </param>
        /// <param name="a">    Ambient attribute. </param>
        /// <param name="d">    Diffuse attribute. </param>
        /// <param name="sp">   The specular attribute. </param>
        /// <param name="sc">   The shininess attribute. </param>
        ///-------------------------------------------------------------------------------------------------

        public Material(Pattern p,Color c, Color a, Color d, Color sp, double sc) {
            InitializeMaterial();
            if (pattern != null) pattern = p.Copy();
            color = new Color(c.Red, c.Green, c.Blue);
            ambient = a;
            diffuse = d;
            specular = sp;
            shininess = sc;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="p">    Pattern attribute. </param>
        /// <param name="c">    Color attribute. </param>
        /// <param name="a">    Ambient attribute. </param>
        /// <param name="d">    Difuse attribute. </param>
        /// <param name="sp">   The specular attribute. </param>
        /// <param name="sc">   The shininess attribute. </param>
        /// <param name="re">   The reflective attribute. </param>
        /// <param name="tr">   The transparency attribute. </param>
        /// <param name="ri">   The refractiveIndex attribute. </param>
        ///-------------------------------------------------------------------------------------------------

        public Material(string n, Pattern p, Color c, Color a, Color d, Color sp, double sc, double re, double tr, double ri) {
            InitializeMaterial();
            name = n;
            if (p != null) pattern = p.Copy();
            color = new Color(c.Red, c.Green, c.Blue);
            ambient = a;
            diffuse = d;
            specular = sp;
            shininess = sc;
            reflective = re;
            transparency = tr;
            refractiveIndex = ri;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Tests if this Material is considered equal to another. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <param name="m">    The material to compare to this object. </param>
        ///
        /// <returns>   True if the objects are considered equal, false if they are not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool Equals(Material m) {
            return (name == m.name) && (ambient.Equals(m.ambient)) && (diffuse.Equals(m.diffuse)) && (specular.Equals(m.specular)) && (shininess == m.shininess) &&
            color.Equals(m.color) && ((pattern == null && m.pattern == null) || (pattern != null && m.pattern != null && pattern.Equals(m.pattern))) && 
            (transparency == m.transparency) && (refractiveIndex == m.refractiveIndex) && (illuminationMode==m.illuminationMode);
           
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Copies this object. </summary>
        ///
        /// <remarks>   Kemp, 11/9/2018. </remarks>
        ///
        /// <returns>   A Material. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Material Copy() => new Material(name, pattern, color, ambient, diffuse, specular, shininess, reflective, transparency, refractiveIndex);
      

     }
}
