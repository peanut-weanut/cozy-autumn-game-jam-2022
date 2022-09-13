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
}
