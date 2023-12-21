using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnPolygon : MonoBehaviour
{
    public Polygon polygon;
    public Mesh m;

    public void Start()
    {
        GetComponent<MeshFilter>().mesh = m;
    }
}
