using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControls : MonoBehaviour
{
    //Switches between cinemachines cameras for looking and drawing
    public GameObject[] POIs;
    // public List<int> visiblePOIs;

    public GameObject[] cameras;
    // Animator anim;

    public InputSystem controls;
    public states state;
    public bool free = true;

    // Start is called before the first frame update
    public enum states {
        LOOKING,
        DRAWING
    };
    private void Awake()
    {
        controls = new InputSystem();
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Start()
    {
        //cameras = GameObject.FindGameObjectsWithTag("Camera");
        controls.CameraStates.ChangeView.performed += ctx => TryDrawing();
        state = states.LOOKING;
        POIs = GameObject.FindGameObjectsWithTag("POI");
        // cameras = GameObject.FindGameObjectsWithTag("Camera");
    }
        private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // void Update(){
    //     switch(state){
    //         case states.LOOKING:
    //             MatchCameras(0);
    //         break;
    //         case states.DRAWING:
    //             MatchCameras(1);
    //         break;
    //     }
    // }
    void TryDrawing(){
        Debug.Log("Tried to draw.");
        if (!free){ // if you are not free to draw whatever,
            if(CheckPOIs()) // check if youre looking at something correct
                ToggleState(); // and toggle the state
        }else{
            ToggleState(); // otherwise just toggle the state
        }
    }
    void ToggleState(){
        Debug.Log("Tried to toggle.");
        switch (state){
            case states.LOOKING:
                foreach (GameObject i in cameras){
                    // InheritRotation(i);
                    // MatchCameras(0);
                    i.GetComponent<CinemachineVirtualCamera>().m_Priority = i.name == "Horizontal Camera" ? 1 : 0;
                }
                state = states.DRAWING;
                Cursor.lockState = CursorLockMode.Locked;
            break;
            case states.DRAWING:
                foreach (GameObject i in cameras){
                    // InheritRotation(i);
                    // MatchCameras(1);
                    i.GetComponent<CinemachineVirtualCamera>().m_Priority = i.name == "Vertical Camera" ? 1 : 0;
                }
                state = states.LOOKING;
                Cursor.lockState = CursorLockMode.None;
            break;
        }
    }
    bool CheckPOIs(){
        foreach(GameObject i in POIs){
            if (i.GetComponent<Renderer>().isVisible){
                //run the script that starts selecting
                if (i.GetComponent<POIScript>().isDrawable){
                    Debug.Log(i + " is visible and drawable!");
                    return true;
                }
                else{
                    Debug.Log(i + " is visible!");
                    return false;
                }
            } else {
                Debug.Log(i + " is not visible!");
                return false;
            }                     
        }
        return false;
    }
    void InheritRotation(GameObject cam){
        cam.GetComponent<CinemachineVirtualCamera>().transform.rotation = Camera.main.transform.rotation;
    }
    public void MatchCameras(int hackyIndex)
    {
        //Match Values : follow offset, position & rotation
            cameras[hackyIndex].GetComponent<CinemachineVirtualCamera>().ForceCameraPosition(transform.position, Camera.main.transform.rotation);


    }

}
