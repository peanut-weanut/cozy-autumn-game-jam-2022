using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour
{
    public Transform lookingPoint, drawingPoint;
    [Range(0.0f, 1.0f)]
    public float offset;
    [Range(0.0f, 0.1f)]
    public float speed;
    public delegate void FinishedTransformDelegate();
    public FinishedTransformDelegate TransDone;
    public FinishedTransformDelegate TransDoneLooking;
    void FixedUpdate(){
        switch (GameManager.game.camControls.state){
            case CameraControls.states.LOOKING:
                if (Vector3.Distance(transform.position, lookingPoint.localPosition) > offset){
                    transform.localPosition = Vector3.Lerp(transform.localPosition, lookingPoint.localPosition, speed);
                } else{
                    TransDoneLooking();
                }
            break;
            case CameraControls.states.DRAWING:
                if (Vector3.Distance(transform.localPosition, drawingPoint.localPosition) > offset){
                    transform.localPosition = Vector3.Lerp(transform.localPosition, drawingPoint.localPosition, speed);
                } else{
                    TransDone();
                }
            break;
        }
    }
}
