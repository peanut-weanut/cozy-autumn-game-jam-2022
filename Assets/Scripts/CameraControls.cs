using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class CameraControls : MonoBehaviour
{
    //Switches between cinemachines cameras for looking and drawing

    // public List<int> visiblePOIs;
    public string browserVersion;

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
    public List<GameObject> currentPOI = new List<GameObject>();
    public GameObject realCurrentPOI;

    // Start is called before the first frame update
    public enum states {
        LOOKING,
        DRAWING
    };
    private void Awake()
    {
        controls = new InputSystem();
        Cursor.lockState = CursorLockMode.Locked;
        #if UNITY_WEBGL && !UNITY_EDITOR
            browserVersion = GetBrowserVersion();
        #else
            browserVersion = "N/A";
        #endif
        Debug.Log("Browser version: " + browserVersion);
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
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string GetBrowserVersion();
    #endif

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
            currentPOI.Clear();
        }
        if (state == states.LOOKING){
            if (POISeenCancelBuffer > 0.0f){
                POISeenCancelBuffer -= Time.deltaTime;
            } else if (POISeenCancelBuffer <= 0.0f){
                DebugPOISeen = false;
            }
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
    public float POISeenCancelBufferTime, POISeenCancelBuffer = 2.0f;
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
                // DebugPOISeen = false;
                POISeenCancelBuffer = POISeenCancelBufferTime;
                //Cursor.SetCursor(cursorSpriteDraw, Vector2.zero, CursorMode.Auto);
                Cursor.lockState = CursorLockMode.Locked;
            break;
        }
    }
    bool CheckPOIs(){
        currentPOI.Clear();
        realCurrentPOI = null;
        for(int i = 0; i < GameManager.game.POIs.Length-1; i++){
            var newCPOI = GameManager.game.POIs[i];
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            bool POIVisible = GeometryUtility.TestPlanesAABB(frustumPlanes,GameManager.game.POIs[i].GetComponent<Renderer>().bounds);
            if (POIVisible){
                //run the script that starts selecting
                if (GameManager.game.POIs[i].GetComponent<POIScript>().isDrawable){
                    currentPOI.Add(GameManager.game.POIs[i]);
                    PrioritizePOI();
                    Debug.Log(newCPOI.name + " is visible and drawable! \n" + realCurrentPOI.name + " is your current POI.");
                    
                }
                else{
                    // Debug.Log(newCPOI.name + " is visible!");
                    
                }
            } else {
                // Debug.Log(newCPOI.name + " is not visible!");
                
            }                     
        }
        if (currentPOI.Count > 0)
            return true;
        return false;
    }
    
    void PrioritizePOI(){
        int i = 0;
        float camAngle = Camera.main.transform.forward.y;
        float minAngle = 180.0f;
        int min = i;
        foreach(GameObject POI in currentPOI){
            if(Mathf.Abs(Camera.main.transform.position.y - POI.transform.position.y) < minAngle){
                minAngle = Mathf.Abs(Camera.main.transform.position.y - POI.transform.position.y);
                min = i;
            }  
            i++;  
            
        }
        realCurrentPOI = currentPOI[min];       
    }

    //coroutine
    IEnumerator RandomBullshit(){
        //a += 1
        yield return new WaitUntil(IsMusicStopped);
        // a += 1
        // a = 2
    }
    bool IsMusicStopped() => finalSongStopped;
    bool finalSongStopped;

    // void InheritRotation(GameObject cam){
    //     cam.GetComponent<CinemachineVirtualCamera>().transform.rotation = Camera.main.transform.rotation;
    // }
    // public void MatchCameras(int hackyIndex)
    // {
    //     //Match Values : follow offset, position & rotation
    //         cameras[hackyIndex].GetComponent<CinemachineVirtualCamera>().ForceCameraPosition(transform.position, Camera.main.transform.rotation);
    // }

}
