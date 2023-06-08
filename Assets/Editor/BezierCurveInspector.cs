
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    private BezierCurve bezierCurve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI() {
        bezierCurve = target as BezierCurve;

        handleTransform = bezierCurve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        
        // Vector3[] points = bezierCurve.points;
        // for(int i = 0; i < points.Length-1; i++){
        //     points[i] = handleTransform.TransformPoint(bezierCurve.points[i]);
        // }
        Vector3 p0 = ShowPoint(0);
		Vector3 p1 = ShowPoint(1);
		Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);

        // Vector3[] finalPoints = new Vector3[bezierCurve.resolution];

        Handles.color = Color.green;
        Handles.DrawBezier(p0, p3, p1, p2, Color.green, null, 2f);
        // Vector3 lineStart = bezierCurve.GetPoint(0f);
        // for(int i = 1; i <= bezierCurve.resolution; i++){
        //     Vector3 lineEnd = bezierCurve.GetPoint(i/(float)bezierCurve.resolution);
        //     Handles.DrawLine(lineStart, lineEnd);
        //     lineStart = lineEnd;
        // }

        // for(int i = 0; i < points.Length-1; i++){
        //     EditorGUI.BeginChangeCheck();
        //     points[i] = Handles.DoPositionHandle(points[i], handleRotation);
        //     if(EditorGUI.EndChangeCheck()){
        //         Undo.RecordObject(bezierCurve, "Move Point");
        //         EditorUtility.SetDirty(bezierCurve);
        //         bezierCurve.points[i] = handleTransform.InverseTransformPoint(points[i]); 
        //     }
        // }
    }
    private Vector3 ShowPoint (int index){
        Vector3 point = handleTransform.TransformPoint(bezierCurve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(bezierCurve, "Move Point");
            EditorUtility.SetDirty(bezierCurve);
            bezierCurve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
    
}
#endif