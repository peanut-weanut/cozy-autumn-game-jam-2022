using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLines : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int pointCount;
    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.forceRenderingOff = false; 
    }
    public void AddPointsFast(Vector3 point){
        pointCount++;
        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPosition(pointCount-1, point); // pretty much just adds "positions" to the attached LineRenderer's "positions" list
    }
    
    // public Vector3 beginPos = new Vector3(-1.0f, -1.0f, 0);
    // public Vector3 endPos = new Vector3(1.0f, 1.0f, 0);

    // Vector3 beginPosOffset;
    // Vector3 endPosOffset;

    // LineRenderer diagLine;

    // void Start()
    // {

    //     diagLine = gameObject.AddComponent<LineRenderer>();
    //     diagLine.material = new Material(Shader.Find("Sprites/Default"));
    //     diagLine.startColor = diagLine.endColor = Color.green;
    //     diagLine.startWidth = diagLine.endWidth = 0.15f;

    //     diagLine.SetPosition(0, beginPos);
    //     diagLine.SetPosition(1, endPos);

    //     //Get offset
    //     beginPosOffset = transform.position - diagLine.GetPosition(0);
    //     endPosOffset = transform.position - diagLine.GetPosition(1);
    // }

    // void Update()
    // {
    //     //Calculate new postion with offset
    //     Vector3 newBeginPos = transform.position + beginPosOffset;
    //     Vector3 newEndPos = transform.position + endPosOffset;

    //     //Apply new position with offset
    //     diagLine.SetPosition(0, newBeginPos);
    //     diagLine.SetPosition(1, newEndPos);
    // }
}
