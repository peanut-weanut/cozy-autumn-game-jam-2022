using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingUtilities : MonoBehaviour
{
    public List<GameObject> drawings; // we prefer lists here because we wont be updating the list very often, nor will we be creating or removing in a single frame. 
                                      // its more performant than resizing an array(which in c# is deleting and recreating it) every time we undo, from what i can tell
    
    public Vector3[][] points;
    public InputSystem controls;
    private void Awake()
    {
        controls = new InputSystem();
    }
    private void Start(){
        controls.inputs.Undo.performed += ctx => UndoDrawing();
        controls.inputs.Submit.performed += ctx => SubmitDrawing();
        controls.CameraStates.ChangeView.performed += ctx => ClearDrawings();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    // !!!ALL OF THIS IS UNTESTED!!!

    void Update(){
        // if(Input.GetButtonDown("Fire2"))
        //     SaveAllToArray();
        // if(Input.GetButtonDown("Submit"))
        //     UndoDrawing();
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
    void RefreshDrawings(){
            drawings.Clear();
            var drawingsTemp = GameObject.FindGameObjectsWithTag("Line"); // we have to do this since you cant convert an array into a list
            for(var i = 0; i < drawingsTemp.Length; i++){
                if (drawingsTemp[i] != null)
                    drawings.Add(drawingsTemp[i]);
            }
            
    }
    void ClearDrawings(){
        RefreshDrawings();
        for(var i = drawings.Count - 1; i > -1; i--){
            var dead = drawings[i];
            drawings.Remove(dead);
            Destroy(dead);
        }
        RefreshDrawings();
    }
    void SubmitDrawing(){ //VERY IMPORTANT TO NOT CALL CLEARDRAWINGS ANYWHERE ELSE THAN HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        SaveAllToArray();
        // SaveBufferToPNG()/Screenshot();
        ClearDrawings();
    }
    void UndoDrawing(){
        RefreshDrawings();
        Debug.Log("Recieved input 'Undo'.");
        int deadIndex = drawings.Count;
        GameObject dead = drawings[deadIndex-1]; // since we did this we can also do things like highlight the last drawn line, or 
        Debug.Log(deadIndex + "/" + drawings.Count + dead.name);
        drawings.Remove(dead); // remove the last entry from drawings
        Destroy(dead); 
        RefreshDrawings();
    }
}
