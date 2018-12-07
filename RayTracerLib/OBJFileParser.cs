﻿///-------------------------------------------------------------------------------------------------
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

        public OBJFileParser(String file) {
            ParserInit();
            filename = file;
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

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse file. </summary>
        ///
        /// <remarks>   Kemp, 11/21/2018. </remarks>
        ///
        /// <param name="file"> The file. </param>
        ///-------------------------------------------------------------------------------------------------

        public void ParseFile(String file) {
            var lines = File.ReadLines(file);
            int lineNo = 0;

            foreach (var line in lines) {
                lineNo++;
                string[] words = line.Split(' ');
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
                            vertices.Add(new Point(p0, p1, p2));
                        }
                        else {
                            Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex");
                        }
                        break;
                    case "f": // face
                        /// Face must have at least 3 vertices, but as many as necessary. They will be
                        /// parsed into triangles.
                        int v0, v1, v2;
                        int wvn0, wvn1, wvn2;
                        if (words.Count() >= 4) {
                            try {
                                if (words[1].Contains('/')) {
                                    string[] faceV0 = words[1].Split('/');
                                    v0 = Int32.Parse(faceV0[0]);
                                    v1 = 0;
                                    v2 = 0;
                                    wvn0 = Int32.Parse(faceV0[2]);
                                    wvn1 = 0;
                                    wvn2 = 0;

                                    for (int vi = 2; vi < words.Count() - 1; vi += 1) {
                                        if (words[vi].Contains("/")) {
                                            string[] faceV = words[vi].Split('/');
                                            if (faceV.Count() == 3) {
                                                v1 = Int32.Parse(faceV[0]);
                                                wvn1 = Int32.Parse(faceV[2]);
                                            }
                                            faceV = words[vi + 1].Split('/');
                                            if (faceV.Count() == 3) {
                                                v2 = Int32.Parse(faceV[0]);
                                                wvn2 = Int32.Parse(faceV[2]);
                                            }
                                            SmoothTriangle st = new SmoothTriangle(vertices[v0 - 1].Copy(), vertices[v1 - 1].Copy(), vertices[v2 - 1].Copy(),
                                                                                    normals[v0 - 1].Copy(),  normals[v1 - 1].Copy(),  normals[v2 - 1].Copy());
                                            currentGroup.AddObject(st);
                                        }

                                        else {
                                            Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex reference");

                                        }
                                        
                                    }
                                }
                                else {
                                    v0 = Int32.Parse(words[1]); // face indicies are 1 based
                                    for (int vi = 2; vi < words.Count() - 1; vi += 1) {
                                        v1 = Int32.Parse(words[vi]);
                                        v2 = Int32.Parse(words[vi + 1]);
                                        Triangle t = new Triangle(vertices[v0 - 1].Copy(), vertices[v1 - 1].Copy(), vertices[v2 - 1].Copy()); // face indicies are 1 based.
                                        currentGroup.AddObject(t);
                                    }
                                } 
                            }
                            catch {
                                Console.WriteLine("line " + lineNo.ToString() + ":  Ill formed vertex reference");
                            }
                        }
                        else {
                           Console.WriteLine("line " + lineNo.ToString() + ":  Face must have at least 3 vertices");
                        }
                        break;
                    case "g": // group
                        if (words.Count() >= 2) {
                            Group g = new Group();
                            g.Name = words[1];
                            groups.Add(g);
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
                    case "vt": // texture vertex
                        // ignore this
                        // break;
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