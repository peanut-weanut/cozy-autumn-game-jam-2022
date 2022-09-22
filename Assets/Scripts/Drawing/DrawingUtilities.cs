using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawingUtilities : MonoBehaviour
{
    public List<GameObject> drawings; // we prefer lists here because we wont be updating the list very often, nor will we be creating or removing in a single frame. 
    public List<Texture2D> screenshots;
    public Vector3[][] points;
    public InputSystem controls;
    public Transform canvas;
    private Vector3 canvasPos;
    private List<Vector3[]> canvasOffset;
    private float angleDiff;
    private float initialAngle, newAngle;
    public Transform camRig;
    public delegate void OnDoneDrawingDelegate();
    public OnDoneDrawingDelegate OnDoneDrawing;
    private void Awake()
    {
        controls = new InputSystem();
    }
    private void Start(){
        drawings = new List<GameObject>();
        canvasOffset = new List<Vector3[]>();
        controls.inputs.Undo.performed += ctx => UndoDrawing();
        controls.inputs.Submit.performed += ctx => SubmitDrawing();
        // GameManager.game.drawLines.OnDrawEnded += BeginGetOffset;
        canvasPos = canvas.position;
        // controls.CameraStates.ChangeView.performed += ctx => HideDrawings();
        canvas.GetComponent<CanvasBehavior>().TransDone += ShowDrawings;
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    void Update(){
        // if(Input.GetButtonDown("Fire2"))
        //     SaveAllToArray();
        // if(Input.GetButtonDown("Submit"))
        //     UndoDrawing();

        // if (canvasPos != canvas.position){
        //     for(var i = drawings.Count - 1; i > -1; i--){
        //         SetOffset(i, canvasOffset[i]);
        //     }
        // }
        
        
    }
    void SaveAllToArray(){
        RefreshDrawings();
        Debug.Log("Recieved input 'Save'.");
        int index = 0;
        points = new Vector3[drawings.Count][]; // create an array of arrays to store the points and seperate them by object
        foreach(GameObject i in drawings){
            points[index] = new Vector3[i.GetComponent<LineRenderer>().positionCount]; //when you access anything from iPoints[i], you are pulling an entire array out
            i.GetComponent<LineRenderer>().GetPositions(points[index]); // saves an array to the index, so if you want to access any drawings you can simply pull from here

            index++;
        } 
    }
    void LoadPoints(){
        for(var x = 0; x < points.Length; x++){
            for(var y = 0; y < points[x].Length; y++){

            }
        }
    }
    void RefreshDrawings(){
            drawings = new List<GameObject>(); // clears list to make sure there arent any stragglers
            var drawingsTemp = GameObject.FindGameObjectsWithTag("Line"); // we have to do this since you cant convert an array into a list
            for(var i = 0; i < drawingsTemp.Length; i++){
                if (drawingsTemp[i] != null)
                    drawings.Add(drawingsTemp[i]);
            }
            
    }
    void ClearDrawings(){
        RefreshDrawings();
        if(drawings.Count > 0)
            for(var i = drawings.Count - 1; i > -1; i--){
                var dead = drawings[i];
                drawings.Remove(dead);
                Destroy(dead);
            }
        drawings = new List<GameObject>();
        // RefreshDrawings();
    }
    void ShowDrawings(){
        for(var i = 0; i < drawings.Count; i++){
            var hide = drawings[i];
            var lRenderer = hide.GetComponent<LineRenderer>();
            // Debug.Log("Tried to show 2.");
            lRenderer.enabled = true;
        }
    }
    Vector3 debugCanvasPos;
    public Transform pointDrawing;
    void HideDrawings(){
        Debug.Log("Hiding drawings!");
        RefreshDrawings();
        Debug.Log("Tried to hide 1.");
        for(var i = 0; i < drawings.Count; i++){
            var hide = drawings[i];
            var lRenderer = hide.GetComponent<LineRenderer>();
            Debug.Log("Tried to hide 2.");
            lRenderer.enabled = false;
            // Vector3[] pointsRaw = new Vector3[lRenderer.positionCount];
            // lRenderer.GetPositions(pointsRaw);
            // Vector3[] offset = new Vector3[lRenderer.positionCount];
            // for(var x = 0; x < pointsRaw.Length - 1; x++){
            //     offset[x] = pointsRaw[x] - pointDrawing.position;
            // }
            // canvasOffset.Add(offset);
            // if(GameManager.game.camControls.state == CameraControls.states.LOOKING){
            //     debugCanvasPos = pointDrawing.position;
            //     initialAngle = camRig.rotation.eulerAngles.y;
            // }
        }
        
    }
    
    // void Readjust(){
    //     // newAngle = camRig.rotation.eulerAngles.y;
    //     Vector3 pointDir = pointDrawing.position - camRig.position;
    //     angleDiff = Mathf.DeltaAngle(initialAngle, camRig.rotation.eulerAngles.y);
    //      Debug.DrawLine(camRig.position, canvas.position, Color.green);
    //      Debug.DrawLine(camRig.position, debugCanvasPos, Color.blue);
    //     //Debug.DrawLine(camRig.position,initialAngle-pointDir, Color.red);
    //     pointDir *= angleDiff;
    //     // pointDir.Normalize();
    //     for(var i = 0; i < drawings.Count; i++){
    //         drawings[i].transform.rotation = canvas.rotation;
    //         for(var x = 0; x < drawings[i].GetComponent<LineRenderer>().positionCount; x++)
    //             drawings[i].GetComponent<LineRenderer>().SetPosition(x, (canvas.transform.position + (Quaternion.Euler(0, angleDiff, 0)*(canvasOffset[i][x]))));
    //             //basically here you should be using the above line of code but doing (x, (canvas.transform.position + offset) * Quaternion.Euler(0, angleDiff, 0))
    //     }
    //     ShowDrawings();
    // }
    // void BeginGetOffset(){
    //     RefreshDrawings();
    //     for(var i = drawings.Count - 1; i > -1; i--){
    //         canvasOffset[i] = GetOffset(i);
    //     }
    // }

    // private Vector3[] GetOffset(int i){
    //     var lRenderer = drawings[i].GetComponent<LineRenderer>();
    //     Vector3[] pointsRaw = new Vector3[lRenderer.positionCount];
    //     lRenderer.GetPositions(pointsRaw);
    //     Vector3[] offset = new Vector3[lRenderer.positionCount];
    //     for(var x = pointsRaw.Length - 1; x > -1; x--){
    //         offset[x] = pointsRaw[x] - canvas.position;
    //     }
    //     return offset;
    // }

    // void SetOffset(int i, Vector3[] offset){
    //     var lRenderer = drawings[i].GetComponent<LineRenderer>();
    //     Vector3[] pointsRaw = new Vector3[lRenderer.positionCount];
    //     // lRenderer.GetPositions(pointsRaw);
    //     for(var x = pointsRaw.Length - 1; x > -1; x--){
    //         Vector3 pointNormalized = canvas.position + offset[x];
    //         lRenderer.SetPosition(x, pointNormalized);
    //     }
    // }
    public Camera[] screenshottingCameras;
    public Texture2D textPic;
    void SubmitDrawing(){ //VERY IMPORTANT TO NOT CALL CLEARDRAWINGS ANYWHERE ELSE THAN HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (GameManager.game.camControls.state == CameraControls.states.DRAWING){
            SaveAllToArray();
            screenshots.Add(GetScreenshot(screenshottingCameras[0])); // for final gallery at end of game
            textPic = GetScreenshot(screenshottingCameras[1]);
            //send textpic to dialogue helper
            // SendToDialogueHelper();
            ClearDrawings();
            OnDoneDrawing();
        }
    }
    private Texture2D GetScreenshot(Camera cam ){
        var rTex = cam.targetTexture;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        int height = cam.pixelHeight;
        int width = cam.pixelWidth;
        Texture2D screenshotTexture = new Texture2D(width, height);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();
        RenderTexture.active = null;
        Debug.Log("Screenshot from" + cam.name + " recorded.");
        return screenshotTexture;
    }
    void UndoDrawing(){
        RefreshDrawings();
        Debug.Log("Recieved input 'Undo'.");
        int deadIndex = drawings.Count;
        GameObject dead = drawings[deadIndex-1]; //takes the newest drawing
        Debug.Log(deadIndex + "/" + drawings.Count + dead.name);
        drawings.Remove(dead); // remove the last entry from drawings
        Destroy(dead); 
        RefreshDrawings();
    }
}
