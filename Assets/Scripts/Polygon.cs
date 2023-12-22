using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<Vector2> vertices;



    
    public Polygon(List<Vector2> verts)
    {
        vertices = verts;
    }
    
    public List<Side> sides()
    {
        List<Side> sides = new List<Side>();
        for (int i = 0; i < vertices.Count - 1; i++)
            sides.Add(new Side(vertices[i], vertices[i + 1]));
        sides.Add(new Side(vertices[vertices.Count - 1], vertices[0]));
        Debug.Log(vertices.Count);
        return sides;

    }

    public Mesh getMesh(float lineWidth, Color c1, Color c2, Vector2 relPosition)
    {
        Mesh m = new Mesh();
        List<Vector3> mverts = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();
        int t = 0;
        for (int i = 0; i < vertices.Count; i++)
        {
            colors.Add(Color.Lerp(c1, c2, (float)i / (float)vertices.Count));
            colors.Add(Color.Lerp(c1, c2, (float)i / (float)vertices.Count));
            int j = i != 0 ? i - 1 : vertices.Count - 1;
            int k = i != vertices.Count - 1 ? i + 1 : 0;
            Vector2 offset = findPointOffsetClamped(vertices[j], vertices[i], vertices[k], lineWidth);
            mverts.Add(vertices[i] + relPosition - offset);
            mverts.Add(vertices[i] + relPosition + offset);
            triangles.Add(t);
            triangles.Add((t + 2) % (2 * vertices.Count));
            triangles.Add(t + 1);
            triangles.Add(t + 1);
            triangles.Add((t + 2) % (2 * vertices.Count));
            triangles.Add((t + 3) % (2 * vertices.Count));
            t += 2;

        }

        m.SetVertices(mverts);
        m.SetColors(colors);
        m.triangles = triangles.ToArray();
        return m;
    }

    public Mesh getMesh2(float lineWidth, Color c1, Color c2, Vector2 relPosition)
    {
        Mesh m = new Mesh();
        List<Vector3> mverts = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();
        int t = 0;
        for (int i = 0; i < vertices.Count; i++)
        {
            colors.Add(Color.Lerp(c1, c2, (float)i / (float)vertices.Count));
            colors.Add(Color.Lerp(c1, c2, (float)i / (float)vertices.Count));
            colors.Add(Color.Lerp(c1, c2, (float)i / (float)vertices.Count));
            colors.Add(Color.Lerp(c1, c2, (float)i / (float)vertices.Count));
            int j = i != 0 ? i - 1 : vertices.Count - 1;
            int k = i != vertices.Count - 1 ? i + 1 : 0;
            Vector2 a = vertices[i] - vertices[j];
            Vector2 b = vertices[k] - vertices[i];
            Vector2 offset1 = new Vector2(-a.y, a.x).normalized * lineWidth;
            Vector2 offset2 = new Vector2(-b.y, b.x).normalized * lineWidth;

            mverts.Add(vertices[i] + relPosition - offset1);
            mverts.Add(vertices[i] + relPosition + offset1);
            mverts.Add(vertices[i] + relPosition - offset2);
            mverts.Add(vertices[i] + relPosition + offset2);
            triangles.Add(t);
            triangles.Add(t+2);
            triangles.Add(t+1);
            triangles.Add(t+1);
            triangles.Add(t+2);
            triangles.Add(t+3);

            triangles.Add(t+2);
            triangles.Add((t + 4) % (4 * vertices.Count));
            triangles.Add(t + 3);
            triangles.Add(t + 3);
            triangles.Add((t + 4) % (4 * vertices.Count));
            triangles.Add((t + 5) % (4 * vertices.Count));
            t += 4;

        }

        m.SetVertices(mverts);
        m.SetColors(colors);
        m.triangles = triangles.ToArray();
        return m;
    }


    public static Vector2 findPointOffset(Vector2 p1, Vector2 p2, Vector2 p3, float h)
    {
        Vector2 a = p1 - p2;
        Vector2 b = p3 - p2;
        if ((a.x / b.x == a.y / b.y) || (a.x == 0 && b.x == 0) || (b.y == 0 && a.y == 0))
            return (new Vector2(b.y, -b.x)).normalized * h;
        Vector2 dir = (a.normalized + b.normalized).normalized;
        float dst = Mathf.Abs(h / Mathf.Sin(Vector2.Angle(dir, a) * Mathf.Deg2Rad));

        return dir * dst;
    }

    public static Vector2 findPointOffsetClamped(Vector2 p1, Vector2 p2, Vector2 p3, float h)
    {
        Vector2 a = p1 - p2;
        Vector2 b = p3 - p2;
        if ((a.x / b.x == a.y / b.y) || (a.x == 0 && b.x == 0) || (b.y == 0 && a.y == 0))
            return (new Vector2(b.y, -b.x)).normalized * h;
        Vector2 dir = (a.normalized + b.normalized).normalized;
        float dst = Mathf.Clamp(Mathf.Abs(h / Mathf.Sin(Vector2.Angle(dir, a) * Mathf.Deg2Rad)), 0.0001f, .1f);

        return dir * dst;
    }
    public static Intersection IntersectsPolygon(Vector2 rayOrigin, Vector2 rayDirection, Polygon p)
    {
        foreach (Side s in p.sides())
        {
            Intersection i = IntersectsSide(rayOrigin, rayDirection, s);
            if (i != null)
            {
                Debug.Log(s.p1);
                Debug.Log(s.p2);
                return i;
            }
        }
        return null;
    }
    public static Intersection IntersectsSide(Vector2 rayOrigin, Vector2 rayDirection, Side side)
    {
        //clean lmfao
        float a;
        float s;
        float r;
        if (rayDirection.x == 0)
        {
            if (side.sideDir.x == 0)
                return null;

            s = (rayOrigin.x - side.p1.x) / side.sideDir.x;
            r = (side.p1.y + s * side.sideDir.y - rayOrigin.y) / rayDirection.y;
        }
        else if (rayDirection.y == 0)
        {
            s = (rayOrigin.y - side.p1.y) / side.sideDir.y;
            r = (side.p1.x + s * side.sideDir.x - rayOrigin.x) / rayDirection.x;
        }
        else
        {
            a = rayDirection.y / rayDirection.x;
            s = (a * (rayOrigin.x - side.p1.x) + side.p1.y - rayOrigin.y) / (a * side.sideDir.x - side.sideDir.y);
            r = (side.p1.y + s * side.sideDir.y - rayOrigin.y) / rayDirection.y;
        }
        if (s <= 1 && s >= 0 && r > 0)
        {
            return new Intersection(rayOrigin + rayDirection * r, r);
        }
        return null;
    }

    public static Vector2 MirrorPoint(Vector2 p, Vector2 a, Vector2 dir)
    {
        Vector2 w = new Vector2(-dir.y, dir.x);
        return p + w * 2 * Vector2.Dot(a - p, w) / (w.magnitude * w.magnitude);
    }

    public Polygon mirror(Side s)
    {
        List<Vector2> nverts = new List<Vector2>();
        foreach(Vector2 v in vertices)
        {
            nverts.Add(MirrorPoint(v, s.p1, s.sideDir));
        }
        return new Polygon(nverts);
    }


    public static Polygon Regular(int vertCount, Vector2 center, float radius, float rotation)
    {
        List<Vector2> verts = new List<Vector2>();
        if (vertCount <= 2)
            throw new System.Exception("Cannot generate Polygon with less than 3 vertices");
        float angleOffset = Mathf.PI * 2 / (float) vertCount;
        for (int i = 0; i < vertCount; i++)
        {
            verts.Add(center + radius * fromAngle(i * angleOffset + rotation));
        }
        return new Polygon(verts);
    }

    private static Vector2 fromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    
}

public class Side
{
    public Vector2 p1;
    public Vector2 p2;
    public Vector2 sideDir;

    public Side(Vector2 p1, Vector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
        sideDir = p2 - p1;
    }

    public override bool Equals(object obj)
    {
        return obj is Side side &&
               p1.Equals(side.p1) &&
               p2.Equals(side.p2) &&
               sideDir.Equals(side.sideDir);
    }

    public override int GetHashCode()
    {
        int hashCode = 1840839150;
        hashCode = hashCode * -1521134295 + p1.GetHashCode();
        hashCode = hashCode * -1521134295 + p2.GetHashCode();
        hashCode = hashCode * -1521134295 + sideDir.GetHashCode();
        return hashCode;
    }
}


public class Intersection
{
    public Vector2 p;
    public float dst;

    public Intersection(Vector2 p, float dst)
    {
        this.p = p;
        this.dst = dst;
    }

    public override string ToString()
    {
        return p.ToString() + " with dst: " + dst;
    }
}
