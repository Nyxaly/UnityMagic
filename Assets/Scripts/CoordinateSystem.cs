
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem : MonoBehaviour
{
    public Camera cam;

    private Mesh _m;


    private void Start()
    {
        _m = new Mesh();
        GetComponent<MeshFilter>().mesh = _m;
    }

    private void Update()
    {
        float linewidth = (cam.ViewportToWorldPoint(new(1,0)) - cam.ViewportToWorldPoint(new(0,0))).x/2000;

        List<Vector3> mverts = new List<Vector3>();//{new(linewidth, -100), new(-linewidth, -100), new(linewidth, 100), new(linewidth, 100)};
        List<int> triangles = new List<int>();//{0,2, 1, 2, 3, 1};
        int t = 0;
        float ln = 0.1f;
        for (int i =-124; i<=124; i++)
        {
            if (i%125!=0 || i==0) {ln=linewidth*3f;}
            if (i%25!=0) {ln=linewidth*2f;}
            if (i%5!=0) {ln=linewidth*1.5f;}
            if (cam.orthographicSize>20 && i%25!=0) continue;
            if (cam.orthographicSize>5 && i%5!=0) continue;
            
            mverts.Add(new Vector3(-100, i+ln));
            mverts.Add(new(-100, i-ln));
            mverts.Add(new(100, i+ln));
            mverts.Add(new(100, i-ln));
            mverts.Add(new(i+ln, -100));
            mverts.Add(new(i-ln, -100));
            mverts.Add(new(i+ln, 100));
            mverts.Add(new(i-ln, 100));
            triangles.Add(t+1);
            triangles.Add(t);
            triangles.Add(t+2);
            
            triangles.Add(t+2);
            triangles.Add(t+3);
            triangles.Add(t+1);
            
            triangles.Add(t+5);
            triangles.Add(t+4);
            triangles.Add(t+6);
            
            triangles.Add(t+6);
            triangles.Add(t+7);
            triangles.Add(t+5);
            t += 8;
            ln = linewidth;
        }
        
        _m.SetVertices(mverts);
        _m.triangles = triangles.ToArray();
        
    }
}
