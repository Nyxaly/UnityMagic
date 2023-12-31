using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Manager m;
    
    private Vector3 dragOrigin;
    public Camera cam;

    private bool creatingPolygon;

    public GameObject vertexPrefab;
    public Transform tempObjParent;
    private GameObject creatingPolygonMouseObj;
    private List<GameObject> currentPolygonObjs;
    private List<Vector2> currentPolygonVerts;
    public GameObject creatingPolygonMesh;
    public CoordinateSystem CSystem;

    private Vector2 mousePos;
    float orthographicSize = 0;
    
    void Update()
    {
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            CSystem.UpdateCoordinateSystem();
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += dragOrigin - pos;
        }
        
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y, 1, 25);
        if (orthographicSize!= cam.orthographicSize) CSystem.UpdateCoordinateSystem();
        orthographicSize = cam.orthographicSize;
        

        if (Input.GetKeyDown(KeyCode.C))
        {
            CancelAllActions();
            creatingPolygon = !creatingPolygon;
            if (creatingPolygon)
                StartPolygonAction();
        }

        HandleActions();
    }

    private void CancelAllActions()
    {
        if (creatingPolygon)
            StopPolygonAction();
    }

    private void HandleActions()
    {
        if (creatingPolygon)
            CreatePolygonAction();
    }

    private void StartPolygonAction()
    {
        creatingPolygonMouseObj = Instantiate(vertexPrefab, tempObjParent);
        creatingPolygonMouseObj.GetComponent<CircleCollider2D>().enabled = false;
        creatingPolygonMouseObj.transform.position = mousePos;
        currentPolygonObjs = new List<GameObject>();
        currentPolygonVerts = new List<Vector2>();
    }

    private void StopPolygonAction()
    {
        Destroy(creatingPolygonMouseObj);
        foreach (GameObject obj in currentPolygonObjs)
            Destroy(obj);
        currentPolygonObjs.Clear();
        currentPolygonVerts.Clear();
        creatingPolygonMesh.GetComponent<MeshFilter>().mesh = null;
        creatingPolygon = false;
        Debug.Log(m.polygons[0].vertices.Count);
    }

    private void CreatePolygonAction()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("hit");
                if (currentPolygonObjs.Contains(hit.collider.gameObject))
                {
                    if (currentPolygonObjs[0] == hit.collider.gameObject)
                    {
                        Polygon p = new Polygon(new List<Vector2>(currentPolygonVerts));
                        m.InstantiatePolygon(p, .05f, Color.blue, Color.red, Vector2.zero);
                        StopPolygonAction();
                    }
                    return;
                }
            }
            currentPolygonObjs.Add(creatingPolygonMouseObj);
            currentPolygonVerts.Add(mousePos);
            creatingPolygonMouseObj.GetComponent<CircleCollider2D>().enabled = true;
            creatingPolygonMouseObj = Instantiate(vertexPrefab, tempObjParent);
            creatingPolygonMouseObj.GetComponent<CircleCollider2D>().enabled = false;
            creatingPolygonMouseObj.transform.position = mousePos;
        }
        
        creatingPolygonMouseObj.transform.position = mousePos;
        if (currentPolygonVerts.Count >= 2)
        {
            currentPolygonVerts.Add(mousePos);
            Polygon p = new Polygon(currentPolygonVerts);
            creatingPolygonMesh.GetComponent<MeshFilter>().mesh = p.getMesh2(.05f, Color.blue, Color.red,Vector2.zero);
            currentPolygonVerts.Remove(mousePos);
        }
    }
}
