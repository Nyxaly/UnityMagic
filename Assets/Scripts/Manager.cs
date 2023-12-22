using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<DrawnPolygon> drawnPolygons = new List<DrawnPolygon>();
    public GameObject drawnPolygonPrefab;
    public Transform polygonParent;
    void Start()
    {
        //Polygon p1 = Polygon.Regular(15, Vector2.zero, 5, 0);
        //InstantiatePolygon(p1, .05f, Color.white, Color.red, Vector2.zero);
        //InstantiatePolygon(p1.mirror(p1.sides()[0]), .05f, Color.white, Color.red, Vector2.zero);
        //InstantiatePolygon(p1.mirror(p1.sides()[0]).mirror(p1.sides()[7]), .05f, Color.white, Color.red, Vector2.zero);

    } 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiatePolygon(Polygon p, float lineWidth, Color c1, Color c2, Vector2 pos)
    {
        GameObject dpg = Instantiate(drawnPolygonPrefab, polygonParent);
        DrawnPolygon dp = dpg.GetComponent<DrawnPolygon>();
        dp.polygon = p;
        dp.m = p.getMesh(lineWidth, c1, c2, pos);
    }
}
