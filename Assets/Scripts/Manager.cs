using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<Polygon> polygons = new List<Polygon>();
    public List<DrawnPolygon> drawnPolygons = new List<DrawnPolygon>();
    public GameObject drawnPolygonPrefab;
    public Transform polygonParent;
    void Start()
    {
        //Polygon p1 = Polygon.Regular(4, Vector2.zero, 5, 0);
        //InstantiatePolygon(p1, .05f, Color.blue, Color.red, Vector2.zero);


    } 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiatePolygon(Polygon p, float lineWidth, Color c1, Color c2, Vector2 pos)
    {
        polygons.Add(p);
        GameObject dpg = Instantiate(drawnPolygonPrefab, polygonParent);
        DrawnPolygon dp = dpg.GetComponent<DrawnPolygon>();
        dp.polygon = p;
        dp.m = p.getMesh2(lineWidth, c1, c2, pos);
    }
}
