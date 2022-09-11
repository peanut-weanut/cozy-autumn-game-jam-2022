using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    //Switches between cinemachines cameras for looking and drawing
    public GameObject[] POIs;
    // public List<int> visiblePOIs;

    public GameObject[] cameras;
    private int currentCamera = 0;
    public enum states {
        LOOKING,
        DRAWING
    };
    public states state;
    void Start()
    {
        //cameras = GameObject.FindGameObjectsWithTag("Camera");

        state = states.LOOKING;
        POIs = GameObject.FindGameObjectsWithTag("POI");
    }
    void Update(){
        if (Input.GetButtonDown("Fire3")){
            foreach(GameObject i in POIs){
            if (i.GetComponent<Renderer>().isVisible){
                //run the script that starts selecting
                if (i.GetComponent<POIScript>().isDrawable){
                    Debug.Log(i + " is visible and drawable!");
                }
                else{
                    Debug.Log(i + " is visible!");
                }
            } else {
                Debug.Log(i + " is not visible!");
            }                     
        }
        }
        // int index = 0;
        // foreach(GameObject i in POIs){
        //     index ++;
        //     if (i.GetComponent<Renderer>().isVisible == true){
        //         visiblePOIs.Add(index);
        //     } else {
        //         visiblePOIs.Remove(index);
        //     }                     
        // }
        // visiblePOIs.Clear();
        // index = 0;
    }

}
