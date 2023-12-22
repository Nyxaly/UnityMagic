using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSource : MonoBehaviour
{
    public Manager manager;
    public List<Polygon> polygons;

    public List<RayHit> path;
    public float laserWidth = 0.01f;
    public Color laserColor;


    public void Start()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log(manager.polygons[0].vertices.Count);
        if (Input.GetKeyDown(KeyCode.W))
            StartRay(new Vector2(1, 1.523f), 1);
        if (Input.GetKeyDown(KeyCode.E))
        {
            stepForward();
            drawLaser();
        }
    }

    public void StartRay(Vector2 rayDir, int steps)
    {
        Debug.Log(manager.polygons[0].vertices.Count);
        polygons = manager.polygons;
        Debug.Log(manager.polygons[0].vertices.Count);
        path = new List<RayHit>();
        path.Add(new RayHit(transform.position, Vector2.zero, rayDir, null, null, 0, 0));
        for (int i = 0; i < steps; i++)
        {
            stepForward();
        }
        drawLaser();
    }

    public void stepForward()
    {
        path.Add(nextHit(path[path.Count - 1]));
    }

    public void drawLaser()
    {

        if (path == null || path.Count < 2)
            return; 
        
        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        vertices.Add(path[0].pos - rotate90((path[1].pos - path[0].pos).normalized) * laserWidth * 0.5f);
        vertices.Add(path[0].pos + rotate90((path[1].pos - path[0].pos).normalized) * laserWidth * 0.5f);
        List<int> triangles = new List<int>();
        int t = 0;
        colors.Add(laserColor);
        colors.Add(laserColor);

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 p1 = path[i - 1].pos;
            Vector2 p2 = path[i].pos;
            Vector2 s = path[i].side.sideDir;
            float angle = Vector2.Angle(p2 - p1, s)*Mathf.Deg2Rad ;
            float offsetDst = (laserWidth/2) / Mathf.Sin(angle);
            Vector2 v1 = path[i].pos + s.normalized * offsetDst;
            Vector2 v2 = path[i].pos - s.normalized * offsetDst;
            if (Vector2.Dot(s, p2-p1) < 0)
            {
                vertices.Add(v1);
                vertices.Add(v2);
            }
            else
            {
                vertices.Add(v2);
                vertices.Add(v1);
            }

            colors.Add(laserColor);
            colors.Add(laserColor);

            triangles.AddRange(new int[3] {t, t+2, t+1});
            triangles.AddRange(new int[3] {t+1, t+2, t+3});
            t += 2;

        }
        Mesh m = new Mesh();
        m.SetVertices(vertices);
        m.triangles = triangles.ToArray();
        GetComponent<MeshFilter>().mesh = m;
    }

    private Vector2 rotate90(Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }

    public RayHit nextHit(RayHit previous)
    {
        RayHit r = new RayHit();
        Intersection closestIntersection = null;

        
        foreach (Polygon p in polygons)
        {
            Debug.Log(p.vertices.Count);
            foreach (Side s in p.sides())
            {
                if (s.Equals(previous.side))
                {
                    continue;
                }
                var hit = Polygon.IntersectsSide(previous.pos, previous.outDir, s);
                if (hit != null)
                {
                    Debug.Log(hit.p + " at n= " + previous.n);
                    if (closestIntersection == null || closestIntersection.dst > hit.dst)
                    {
                        closestIntersection = hit;
                        r.inDir = previous.outDir;
                        r.pos = closestIntersection.p;
                        r.side = s;
                        r.polygon = p;
                        r.dst = closestIntersection.dst;
                        r.n = previous.n + 1;
                    }
                }
            }
        }
        if (closestIntersection != null)
        {
            Vector2 reflection = Polygon.MirrorPoint(closestIntersection.p + previous.outDir, r.side.p1, r.side.sideDir) - closestIntersection.p;
            r.outDir = reflection;
        } else
        {
            Debug.Log("no intersection???: " + previous.pos + ", " + previous.outDir);
        }
        return r;

    }
}

public class RayHit
{
    public Vector2 pos;
    public Side side;
    public Polygon polygon;
    public float dst;
    public int n;
    public Vector2 inDir;
    public Vector2 outDir;


    public RayHit(Vector2 pos, Vector2 inDir, Vector2 outDir, Side side, Polygon polygon, float dst, int n)
    {
        this.pos = pos;
        this.side = side;
        this.polygon = polygon;
        this.dst = dst;
        this.n = n;
        this.inDir = inDir;
        this.outDir = outDir;
    }

    public RayHit()
    {

    } 
    
    public override string ToString()
    {
        return pos.ToString();
    }
}
