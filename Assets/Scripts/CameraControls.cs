using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraControls : MonoBehaviour
{
    //Switches between cinemachines cameras for looking and drawing

    // public List<int> visiblePOIs;

    public GameObject[] cameras;
    // Animator anim;

    public InputSystem controls;
    public states state;
    public bool free = true;

    // Adds cursor sprites
    public Texture2D cursorSpriteDraw;
    public Image lookSprite;
    public delegate void POISeenDelegate();
    public POISeenDelegate POISeen;
    public GameObject currentPOI;

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
        //Set Cursor to look, which is the default
        Cursor.SetCursor(cursorSpriteDraw, new Vector2(0, cursorSpriteDraw.height), CursorMode.Auto);
        //cameras = GameObject.FindGameObjectsWithTag("Camera");
        controls.CameraStates.ChangeView.performed += ctx => TryDrawing();
        GameManager.game.drawUtils.OnDoneDrawing += ToggleState;
        state = states.DRAWING;
        ToggleState();

        lookSprite.enabled = true;
        
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
    void Update(){
        if(StartTimer){
            if(state == states.DRAWING)
                POITimer += Time.deltaTime;
            else{
                POITimer /= 2.0f;
                StartTimer = false;
            }

        }
        if (POITimer > POITime){
            POISeen?.Invoke();
            POITimer = 0;
            StartTimer = false;
            DebugPOISeen = true;
            currentPOI = null;
        }
    }
    public bool StartTimer = false;
    public float POITime = 15.0f;
    public float POITimer = 0.0f;
    public bool DebugPOISeen = false;
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
        lookSprite.enabled = !lookSprite.enabled;
        switch (state){
            case states.LOOKING:
                foreach (GameObject i in cameras){
                    // InheritRotation(i);
                    // MatchCameras(0);
                    i.GetComponent<CinemachineVirtualCamera>().m_Priority = i.name == "Vertical Camera" ? 1 : 0;
                }
                if(CheckPOIs())
                    StartTimer = true;
                state = states.DRAWING;
                //Cursor.SetCursor(cursorSpriteLook, Vector2.zero, CursorMode.Auto);
                Cursor.lockState = CursorLockMode.None;
            break;
            case states.DRAWING:
                foreach (GameObject i in cameras){
                    // InheritRotation(i);
                    // MatchCameras(1);
                    i.GetComponent<CinemachineVirtualCamera>().m_Priority = i.name == "Horizontal Camera" ? 1 : 0;
                }
                state = states.LOOKING;
                StartTimer = false;
                DebugPOISeen = false;
                //Cursor.SetCursor(cursorSpriteDraw, Vector2.zero, CursorMode.Auto);
                Cursor.lockState = CursorLockMode.Locked;
            break;
        }
    }
    bool CheckPOIs(){
        foreach(GameObject i in GameManager.game.POIs){
            if (i.GetComponent<POIScript>().isVisible){
                //run the script that starts selecting
                if (i.GetComponent<POIScript>().isDrawable){
                    Debug.Log(i + " is visible and drawable!");
                    currentPOI = i;
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
