using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracerLib;

public class IcoSphereCreator
{
    private struct Tri
    {
        public int v1;
        public int v2;
        public int v3;

        public Tri(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    private OBJFileParser geometry;
    private int index;
    private Dictionary<Int64, int> middlePointIndexCache;

    // add vertex to mesh, fix position to be on unit sphere, return index
    private int addVertex(Point p)
    {
        double length = Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
        geometry.Verticies.Add(new Point(p.X/length, p.Y/length, p.Z/length));
        return index++;
    }

    // return index of point in the middle of p1 and p2
    private int getMiddlePoint(int p1, int p2)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        Int64 smallerIndex = firstIsSmaller ? p1 : p2;
        Int64 greaterIndex = firstIsSmaller ? p2 : p1;
        Int64 key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if (this.middlePointIndexCache.TryGetValue(key, out ret))
        {
            return ret;
        }

        // not in cache, calculate it
        Point point1 = this.geometry.Verticies[p1]; // Positions
        Point point2 = this.geometry.Verticies[p2]; // Positions
        Point middle = new Point(
            (point1.X + point2.X) / 2.0, 
            (point1.Y + point2.Y) / 2.0, 
            (point1.Z + point2.Z) / 2.0);

        // add vertex makes sure point is on unit sphere
        int i = addVertex(middle); 

        // store it, return index
        this.middlePointIndexCache.Add(key, i);
        return i;
    }

    public OBJFileParser Create(int recursionLevel)
    {
        this.geometry = new OBJFileParser();
        this.middlePointIndexCache = new Dictionary<long, int>();
        this.index = 0;

        // create 12 vertices of a icosahedron
        var t = (1.0 + Math.Sqrt(5.0)) / 2.0;

        addVertex(new Point(-1,  t,  0));
        addVertex(new Point( 1,  t,  0));
        addVertex(new Point(-1, -t,  0));
        addVertex(new Point( 1, -t,  0));

        addVertex(new Point( 0, -1,  t));
        addVertex(new Point( 0,  1,  t));
        addVertex(new Point( 0, -1, -t));
        addVertex(new Point( 0,  1, -t));

        addVertex(new Point( t,  0, -1));
        addVertex(new Point( t,  0,  1));
        addVertex(new Point(-t,  0, -1));
        addVertex(new Point(-t,  0,  1));


        // create 20 triangles of the icosahedron
        var faces = new List<Tri>();

        // 5 faces around point 0
        faces.Add(new Tri(0, 11, 5));
        faces.Add(new Tri(0, 5, 1));
        faces.Add(new Tri(0, 1, 7));
        faces.Add(new Tri(0, 7, 10));
        faces.Add(new Tri(0, 10, 11));

        // 5 adjacent faces 
        faces.Add(new Tri(1, 5, 9));
        faces.Add(new Tri(5, 11, 4));
        faces.Add(new Tri(11, 10, 2));
        faces.Add(new Tri(10, 7, 6));
        faces.Add(new Tri(7, 1, 8));

        // 5 faces around point 3
        faces.Add(new Tri(3, 9, 4));
        faces.Add(new Tri(3, 4, 2));
        faces.Add(new Tri(3, 2, 6));
        faces.Add(new Tri(3, 6, 8));
        faces.Add(new Tri(3, 8, 9));

        // 5 adjacent faces 
        faces.Add(new Tri(4, 9, 5));
        faces.Add(new Tri(2, 4, 11));
        faces.Add(new Tri(6, 2, 10));
        faces.Add(new Tri(8, 6, 7));
        faces.Add(new Tri(9, 8, 1));


        // refine triangles
        for (int i = 0; i < recursionLevel; i++)
        {
            var faces2 = new List<Tri>();
            foreach (var tri in faces)
            {
                // replace triangle by 4 triangles
                int a = getMiddlePoint(tri.v1, tri.v2);
                int b = getMiddlePoint(tri.v2, tri.v3);
                int c = getMiddlePoint(tri.v3, tri.v1);

                faces2.Add(new Tri(tri.v1, a, c));
                faces2.Add(new Tri(tri.v2, b, a));
                faces2.Add(new Tri(tri.v3, c, b));
                faces2.Add(new Tri(a, b, c));
            }
            faces = faces2;
        }

        // done, now add triangles to mesh
        Point origin = new Point(0, 0, 0);
        foreach (var tri in faces)
        {
            // Create the point normals for the smoothing
            Vector n0 = (geometry.Verticies[tri.v1] - origin).Normalize();
            Vector n1 = (geometry.Verticies[tri.v2] - origin).Normalize();
            Vector n2 = (geometry.Verticies[tri.v3] - origin).Normalize();
            // texture map coordinates
            Point t0 = new Point((geometry.Verticies[tri.v1].X + 1) / 2, (geometry.Verticies[tri.v1].Y + 1) / 2, 0);
            Point t1 = new Point((geometry.Verticies[tri.v2].X + 1) / 2, (geometry.Verticies[tri.v2].Y + 1) / 2, 0);
            Point t2 = new Point((geometry.Verticies[tri.v3].X + 1) / 2, (geometry.Verticies[tri.v3].Y + 1) / 2, 0);

            // Create the texture maps for each vertex
            SmoothTriangle st = new SmoothTriangle(geometry.Verticies[tri.v1], geometry.Verticies[tri.v2], geometry.Verticies[tri.v3]);
            st.AddNormals(n0, n1, n2);
            st.AddTexture(t0, t1, t2);
            this.geometry.AddToCurrentGroup(st);
        }

        return this.geometry;        
    }
}