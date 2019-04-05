///-------------------------------------------------------------------------------------------------
// file:	OBJFileParser.cs
//
// summary:	Implements the object file parser class
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
    /// <summary>   An object file (OBJ) parser. </summary>
    ///
    /// <remarks>   Kemp, 11/21/2018. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class OBJFileParser
    {
        /// <summary>   Filename of the file. </summary>
        protected string filename;
        /// <summary>   The vertices. </summary>
        protected List<Point> vertices;
        /// <summary>   The groups. </summary>
        protected List<Group> groups;
        /// <summary>   The current group. </summary>
        protected Group currentGroup;
        /// <summary>   The default group. </summary>
        protected Group defaultGroup;
        /// <summary>   The normals. </summary>
        protected List<Vector> normals;
        protected List<Point> textures;
        protected MTLFileParser materials;
        protected Material currentMaterial;
        protected bool mirror;
        protected int lineNo;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Values that represent triangle types. </summary>
        ///
        /// <remarks>   Kemp, 1/14/2019. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public enum TriangleType { Regular, Smooth };
        protected TriangleType tt;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the verticies. </summary>
        ///
        /// <value> The verticies. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Point> Verticies { get { return vertices; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the groups. </summary>
        ///
        /// <value> The groups. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Group> Groups {  get { return groups; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the default group. </summary>
        ///
        /// <value> The default group. </value>
        ///-------------------------------------------------------------------------------------------------

        public Group DefaultGroup { get { return defaultGroup; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the vector normals. </summary>
        ///
        /// <value> The normals. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Vector> Normals { get { return normals; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the vertex textures. </summary>
        ///
        /// <value> The textures. </value>
        ///-------------------------------------------------------------------------------------------------

        public List<Point> Textures { get { return textures; } }

        public Group CurrentGroup { get { return currentGroup; } set { currentGroup = value; } }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public OBJFileParser() {
            ParserInit();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///
        /// <param name="file"> The file. </param>
        ///-------------------------------------------------------------------------------------------------

        public OBJFileParser(String file,TriangleType t = TriangleType.Smooth,bool m=false) {
            ParserInit();
            filename = file;
            tt = t;
            mirror = m;
            ParseFile(file);

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parser initialize. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///-------------------------------------------------------------------------------------------------

        protected void ParserInit() {
            vertices = new List<Point>();
            groups = new List<Group>();
            currentGroup = new Group();
            groups.Add(currentGroup);
            defaultGroup = currentGroup;
            normals = new List<Vector>();
            textures = new List<Point>();
            mirror = false;
            currentMaterial = new Material();
            lineNo = 0;


        }

        protected void ParseFaceNode(string node, ref int vertex, ref int texture, ref int normal) {
            vertex = 0;
            texture = 0;
            normal = 0;

            string[] v = node.Split('/');
            try {
                vertex = Int32.Parse(v[0]);
                if (v.Count() > 1) {
                    if (v[1] != "") {
                        texture = Int32.Parse(v[1]);
                    }
                }
                if (v.Count() > 2) {
                    if(v[2] != "") {
                        normal = Int32.Parse(v[2]);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex reference: " + e.Message);
                throw (e);
            }

            return;
        }

        public void AddToCurrentGroup(Shape s) {
            currentGroup.AddObject(s);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse file. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///
        /// <param name="file"> The file. </param>
        ///-------------------------------------------------------------------------------------------------

        public void ParseFile(String file) {

            //FileStream objFile = File.OpenRead(file);
            var lines = File.ReadLines(file);
            

            foreach (var line in lines) {
                lineNo++;
                string[] blank = { " " };
                string[] words = line.Split(blank,StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 0) continue;

                switch (words[0]) {
                    case "v": // vertex
                        /// Vertex is simple case.  A simple point with exactly 3 coordinnates
                        double p0, p1, p2;
                        if (words.Count() >= 4) { // may be a junk null string on the end.
                            try {
                                p0 = double.Parse(words[1]);
                                p1 = double.Parse(words[2]);
                                p2 = double.Parse(words[3]);
                            }
                            catch (FormatException e) {
                                Console.WriteLine("line " + lineNo.ToString() + ": " + e.Message);
                                break;
                            }
                            if (mirror) p0 = -p0;
                            vertices.Add(new Point(p0, p1, p2));
                        }
                        else {
                            Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex");
                        }
                        break;
                    case "f": // face
                        /// Face must have at least 3 vertices, but as many as necessary. They will be
                        /// parsed into triangles.
                        int v0 = 0, v1 = 0, v2 = 0;
                        int wvn0 = 0, wvn1 = 0, wvn2 = 0;
                        int wtn0 = 0, wtn1 = 0, wtn2 = 0;
                        if (words.Count() >= 4) {
                            SmoothTriangle st;
                            Triangle rt;
                            try {

                                ParseFaceNode(words[1], ref v0, ref wtn0, ref wvn0);
                                
                                if (v0 < 0) v0 = normals.Count + v0 + 1;
                                if (wtn0 < 0) wtn0 = textures.Count + wtn0 + 1;
                                if (wvn0 < 0) wvn0 = normals.Count + wvn0 + 1;

                                for (int vi = 2; vi < words.Count() - 1; vi += 1) {
                                    ParseFaceNode(words[vi], ref v1, ref wtn1, ref wvn1);
                                       
                                    if (v1 < 0) v1 = vertices.Count + v1 + 1;
                                    if (wtn1 < 0) wtn1 = textures.Count + wtn1 + 1;
                                    if (wvn1 < 0) wvn1 = normals.Count + wvn1 + 1;

                                    ParseFaceNode(words[vi + 1], ref v2, ref wtn2, ref wvn2);

                                    if (v2 < 0) v2 = vertices.Count + v2 + 1;
                                    if (wtn2 < 0) wtn2 = textures.Count + wtn2 + 1;
                                    if (wvn2 < 0) wvn2 = normals.Count + wvn2 + 1;

                                    if (tt == TriangleType.Smooth && wvn0 != 0) {
                                        st = new SmoothTriangle(vertices[v0 - 1].Copy(), vertices[v1 - 1].Copy(), vertices[v2 - 1].Copy());
                                        if (wvn0 != 0) st.AddNormals(normals[wvn0 - 1].Copy(), normals[wvn1 - 1].Copy(), normals[wvn2 - 1].Copy());
                                        if (wtn0 != 0) st.AddTexture(textures[wtn0 - 1].Copy(), textures[wtn1 - 1].Copy(), textures[wtn2 - 1].Copy());
                                        st.Material = currentMaterial.Copy();
                                        currentGroup.AddObject(st);
                                    } else {
                                        rt = new Triangle(vertices[v0 - 1].Copy(), vertices[v1 - 1].Copy(), vertices[v2 - 1].Copy());
                                        if (wtn0 != 0) rt.AddTexture(textures[wtn0 - 1].Copy(), textures[wtn1 - 1].Copy(), textures[wtn2 - 1].Copy());
                                        rt.Material = currentMaterial.Copy();
                                        currentGroup.AddObject(rt);
                                    }
                                }
                            }
                            catch (Exception e) {
                                Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex reference: " + e.Message);
                            }
                        }
                        else {
                            /*                            v0 = Int32.Parse(faceV0[0]); // face indicies are 1 based
                                                        for (int vi = 2; vi < words.Count() - 1; vi += 1) {
                                                            faceV0 = words[vi].Split('/');
                                                            v1 = Int32.Parse(faceV0[0]);
                                                            faceV0 = words[vi + 1].Split('/');
                                                            v2 = Int32.Parse(faceV0[0]);
                                                            {
                                                                rt = new Triangle(vertices[v0 - 1].Copy(), vertices[v1 - 1].Copy(), vertices[v2 - 1].Copy());
                                                                rt.Material = currentMaterial.Copy();
                                                                currentGroup.AddObject(rt);
                                                            }
                                                        } */
                            Console.WriteLine("line " + lineNo.ToString() + ":  Face must have at least 3 verticies. " );
                        }
                        break;
                    case "o": // object; same as group
                    case "g": // group
                        if (words.Count() >= 2) {
                            Group g =GetGroup(words[1]);
                            if (g == null) {
                                g = new Group();
                                g.Name = words[1];
                                groups.Add(g);
                                Console.WriteLine("Group: " + g.Name);
                            }
                            currentGroup = g;
                        }
                        else {
                            Console.WriteLine("line " + lineNo.ToString() + ":  Group command malformed.");
                        }
                        break;
                    case "vn": // vertex normal
                        /// Vertex Normals is a simple case.  A simple point with exactly 3 coordinnates
                        double vn0, vn1, vn2;
                        if (words.Count() >= 4) { // may be a junk null string on the end.
                            try {
                                vn0 = double.Parse(words[1]);
                                vn1 = double.Parse(words[2]);
                                vn2 = double.Parse(words[3]);
                            }
                            catch (FormatException e) {
                                Console.WriteLine("line " + lineNo.ToString() + ": " + e.Message);
                                break;
                            }
                            normals.Add(new Vector(vn0, vn1, vn2));
                        }
                        else {
                            Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex normal");
                        }
                        break;
                    case "mtllib":
                        //File mtlFile = File.OpenRead();
                        string myPath = Path.GetDirectoryName(Path.GetFullPath(file));
                        string mtlFilePath = myPath + "\\" + words[1];
                        mtlFilePath = mtlFilePath.Replace(@"\",@"/");
                        materials = new MTLFileParser(mtlFilePath);
                        break;
                    case "usemtl":
                        if (materials == null)
                            currentMaterial = new Material();
                        else {
                            Material getm = materials.GetMaterial(words[1]);
                            if (getm == null) currentMaterial = new Material();
                            else currentMaterial = getm;
                        }
                            break;
                    case "vt": // texture vertex
                               /// Texture Vertex is simple case.  A simple point with exactly 2 coordinnates
                        double t0, t1;
                        if (words.Count() >= 3) { // may be a junk null string on the end.
                            try {
                                t0 = double.Parse(words[1]);
                                t1 = double.Parse(words[2]);
                            }
                            catch (FormatException e) {
                                Console.WriteLine("line " + lineNo.ToString() + ": " + e.Message);
                                break;
                            }
                            textures.Add(new Point(t0, t1, 0));
                        }
                        else {
                            Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed texture");
                        }
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a group from the list of groups found. </summary>
        ///
        /// <remarks>   Kemp, 11/17/2018. </remarks>
        ///
        /// <param name="g">    The name of the group requested. </param>
        ///
        /// <returns>   The group. </returns>
        ///-------------------------------------------------------------------------------------------------

        public Group GetGroup(string g) => groups.Find(n => n.Name.Equals(g));
    }
}
