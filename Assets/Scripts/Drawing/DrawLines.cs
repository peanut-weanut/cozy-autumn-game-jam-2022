using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{
// Start is called before the first frame update

    //when you click with your mouse, create a "drawing" object at ray out from screen space.
    //send a ray out from screenspace where mouse is.
    //every time the ray hits a "canvas" object, it creates a point in both the player and the drawing object.
    //the drawing object will have a line renderer between the points, similar to the targetting in oh craps
    //when the drawing is done, then it will create a bounding plane, as well as set an x position for the spine of the book
    //when the book turns, the drawings on one side will scale towards the spine to imitate a page turning
    public LayerMask canvasLayer;
    public Camera cam;
    public GameObject drawingPrefab;

    // Number of points on the line
    public int numPoints = 50;

    // distance between those points on the line
    public float accuracy = 0.1f;
    public float scaling = 1.0f;
    private List<Vector3> points;
    private Vector3 lastPoint;
    private bool isDrawing = false;
    private GameObject newDrawing = null;
    public InputSystem controls;
    public bool inputBuffer = false;

    private void Awake()
    {
        controls = new InputSystem();
    }
    
    void Start()
    {
        //cameras = GameObject.FindGameObjectsWithTag("Camera");
        controls.mouse.Click.started += ctx => StartDrawing();
        controls.mouse.Click.canceled += ctx => StopDrawing();
        // cameras = GameObject.FindGameObjectsWithTag("Camera");
        cam = Camera.main;
    }
        private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {   if (accuracy < 0.05f){ // lower bound for accuracy to prevent crashes
            accuracy = 0.05f;
        }
        // Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (inputBuffer)
            Draw();
    }
    void Draw(){
            Vector2 mousePosition = controls.mouse.MousePosition.ReadValue<Vector2>();
            Ray ray = cam.ScreenPointToRay(mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 30.0f, Color.red);
            RaycastHit hit;
            bool raycast = Physics.Raycast(ray, out hit, 100.0f, canvasLayer);
            if (raycast){
                Debug.Log("Ray hit.");
                if (isDrawing){
                    if ((hit.point-lastPoint).magnitude > accuracy * scaling){ // if the distance between the hit point and the previous point is greater than the resolution of the accuracy, place a new point
                        Vector3 newPoint = hit.point - (ray.direction); // set the new point to where it lands on the canvas
                        lastPoint = newPoint; // set the last point to the new point
                        // points.Add(newPoint);
                        // newDrawing.GetComponent<RenderLines>().AddPoints(newPoint);
                        newDrawing.GetComponent<RenderLines>().AddPointsFast(newPoint);
                    }
                } else{ // if there is no list or drawing object, then create a new list and drawing object.
                    newDrawing = Instantiate(drawingPrefab, hit.point, Quaternion.identity);
                    Debug.Log("New prefab made.");
                    // points = new List<Vector3>();
                    isDrawing = true;
                }
            } else{ // if you go off the page, then stop drawing.
                if (newDrawing !=null){
                    // newDrawing.GetComponent<RenderLines>().Stop();
                    newDrawing = null;
                }
                isDrawing = false;
            } 
        }
    void StopDrawing(){
        if (newDrawing !=null){
            // newDrawing.GetComponent<RenderLines>().Stop();
            newDrawing = null;
        }
        isDrawing = false;    
        inputBuffer = false; 
    }   
    void StartDrawing(){
        inputBuffer = true;
    }
}
