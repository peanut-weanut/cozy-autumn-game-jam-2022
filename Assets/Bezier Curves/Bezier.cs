using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
    public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector2 p3, float t) {
		t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        
        return 
        oneMinusT * oneMinusT * p0 + 
        2f * oneMinusT * t * p1 + 
        t * t *p2;
	}
    public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		return
			2f * (1f - t) * (p1 - p0) +
			2f * t * (p2 - p1);
	}
}