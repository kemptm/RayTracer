///-------------------------------------------------------------------------------------------------
// file:	MTLFileParser.cs
//
// summary:	Implements the material file parser class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A  Wavefront material (MTL) file parser. </summary>
    ///
    /// <remarks>   Kemp, 3/9/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class MTLFileParser
    {
        /// <summary>   The materials. </summary>
        protected List<Material> materials;
        /// <summary>   Filename of the material file. </summary>
        protected String filename;
        /// <summary>   The current material. </summary>
        protected Material currentMaterial;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 3/9/2019. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public MTLFileParser() {
            materials = new List<Material>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 3/9/2019. </remarks>
        ///
        /// <param name="fn">   The filename. </param>
        ///-------------------------------------------------------------------------------------------------

        public MTLFileParser(String fn) {
            filename = fn;
            materials = new List<Material>();
            ParseFile();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse file. </summary>
        ///
        /// <remarks>   Kemp, 3/9/2019. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void ParseFile() {
            try {
                string myPath = Path.GetDirectoryName(Path.GetFullPath(filename));
                var lines = File.ReadLines(filename);
                int lineNo = 0;
                double w1 = 0;
                double w2 = 0;
                double w3 = 0;
                HashSet<String> cmds = new HashSet<string>() { "Ns", "Ka", "Kd", "Ks", "illum", "Ni", "d", "Tr", "Kc" };

                foreach (var lineRaw in lines) {
                    string line;
                    line = lineRaw.Trim(' ', '\t');
                    lineNo++;
                    string[] blank = { " " };
                    string[] words = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Count() == 0) continue;
                    if (cmds.Contains(words[0])) {
                        try {
                            w1 = double.Parse(words[1]);
                            if (words.Count() == 4) {
                                w2 = double.Parse(words[2]);
                                w3 = double.Parse(words[3]);
                            }
                            else {
                                w2 = w1;
                                w3 = w1;
                            }
                        }
                        catch (FormatException e) {
                            Console.WriteLine("line " + lineNo.ToString() + ": " + e.Message);
                            continue;
                        }
                    }

                    switch (words[0]) {
                        case "newmtl":
                            currentMaterial = new Material(words[1]);
                            materials.Add(currentMaterial);
                            break;
                        case "Ns": // shininess
                            currentMaterial.Shininess = w1;
                            break;
                        case "Ka": // ambient color
                            currentMaterial.Ambient = new Color(w1, w2, w3);
                            break;
                        case "Kd": // diffuse color
                            currentMaterial.Diffuse = new Color(w1, w2, w3);
                            break;
                        case "Ks": // specular color
                            currentMaterial.Specular = new Color(w1, w2, w3);
                            break;
                        case "Kc": // my own extension for specifying the color
                            currentMaterial.Color = new Color(w1, w2, w3);
                            break;
                        case "illum": // illumination model
                            currentMaterial.IlluminationMode = (uint)w1;
                            break;
                        case "Ni": // refractive index
                            currentMaterial.RefractiveIndex = w1;
                            break;
                        case "d": // opacity
                            currentMaterial.Transparency = 1 - w1;
                            break;
                        case "Tr": // transparency (1-d)
                            currentMaterial.Transparency = w1;
                            break;
                        case "map_Ka": // Ambient texture map
                            currentMaterial.Map_Ka = new TextureMap(myPath + "\\" + words[1]);
                            break;
                        case "map_Kd": // Diffuse texture map
                            currentMaterial.Map_Kd = new TextureMap(myPath + "\\" + words[1]);
                            break;
                        case "map_Ks": // Specular texture map
                            currentMaterial.Map_Ks = new TextureMap(myPath + "\\" + words[1]);
                            break;
                        case "#": // comment
                                  // ignore line.
                            break;
                        default:
                            System.Console.WriteLine("gibberish found:" + String.Join(" ", words, 0, words.Count()));
                            break;
                    }

                }

            }
            catch (Exception e) {
                Console.WriteLine("Materials file not found.  Ignoring");
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a material. </summary>
        ///
        /// <remarks>   Kemp, 3/9/2019. </remarks>
        ///
        /// <param name="m">    The message text. </param>
        ///
        /// <returns>   The material. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Material GetMaterial(string m) => materials.Find(n => n.Name.Equals(m));
    }
}
