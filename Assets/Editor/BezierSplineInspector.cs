
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
    private BezierSpline bezierSpline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI() {
        bezierSpline = target as BezierSpline;

        handleTransform = bezierSpline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        
        // Vector3[] points = bezierSpline.points;
        // for(int i = 0; i < points.Length-1; i++){
        //     points[i] = handleTransform.TransformPoint(bezierSpline.points[i]);
        // }
        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < bezierSpline.ControlPointCount; i += 3) {
			Vector3 p1 = ShowPoint(i);
			Vector3 p2 = ShowPoint(i + 1);
			Vector3 p3 = ShowPoint(i + 2);
			
			Handles.color = Color.gray;
			Handles.DrawLine(p0, p1);
			Handles.DrawLine(p2, p3);
			
			Handles.DrawBezier(p0, p3, p1, p2, Color.red, null, 2f);
			p0 = p3;
		}
        // Vector3 lineStart = bezierSpline.GetPoint(0f);
        // for(int i = 1; i <= bezierSpline.resolution; i++){
        //     Vector3 lineEnd = bezierSpline.GetPoint(i/(float)bezierSpline.resolution);
        //     Handles.DrawLine(lineStart, lineEnd);
        //     lineStart = lineEnd;
        // }

        // for(int i = 0; i < points.Length-1; i++){
        //     EditorGUI.BeginChangeCheck();
        //     points[i] = Handles.DoPositionHandle(points[i], handleRotation);
        //     if(EditorGUI.EndChangeCheck()){
        //         Undo.RecordObject(bezierSpline, "Move Point");
        //         EditorUtility.SetDirty(bezierSpline);
        //         bezierSpline.points[i] = handleTransform.InverseTransformPoint(points[i]); 
        //     }
        // }
    }
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        bezierSpline = target as BezierSpline;
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", bezierSpline.Loop);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(bezierSpline, "Toggle Loop");
            EditorUtility.SetDirty(bezierSpline);
            bezierSpline.Loop = loop;
        }
        if (selectedIndex >= 0 && selectedIndex < bezierSpline.ControlPointCount) {
			DrawSelectedPointInspector();
		}

        if(GUILayout.Button("Add Curve")){
            Undo.RecordObject(bezierSpline, "Add Curve");
            bezierSpline.AddCurve();
            EditorUtility.SetDirty(bezierSpline);
        }
    }
    private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", bezierSpline.GetControlPoint(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(bezierSpline, "Move Point");
			EditorUtility.SetDirty(bezierSpline);
			bezierSpline.SetControlPoint(selectedIndex, point);
		}
        EditorGUI.BeginChangeCheck();
        BezierSplineBehaviorMode mode = (BezierSplineBehaviorMode)
            EditorGUILayout.EnumPopup("Mode", bezierSpline.GetControlPointMode(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(bezierSpline, "Change Point Mode");
			bezierSpline.SetControlPointMode(selectedIndex, mode);
			EditorUtility.SetDirty(bezierSpline);
		}
	}
    private const float handleSize = 0.06f;
    private const float pickSize = 0.1f;
    private int selectedIndex = -1;

    private Vector3 ShowPoint (int index){
        Vector3 point = handleTransform.TransformPoint(bezierSpline.GetControlPoint(index));
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0) {
			size *= 2f;
		}
        Handles.color = Color.white;
        if (Handles.Button(point, handleRotation, size * handleSize,size * pickSize, Handles.SphereHandleCap))
            {selectedIndex = index;
            Repaint();}
        if(selectedIndex == index){
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if(EditorGUI.EndChangeCheck()){
                Undo.RecordObject(bezierSpline, "Move Point");
                EditorUtility.SetDirty(bezierSpline);
                bezierSpline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }
        return point;
    }
    
}
#endif