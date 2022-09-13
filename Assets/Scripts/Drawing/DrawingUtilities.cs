using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingUtilities : MonoBehaviour
{
    public List<GameObject> drawings; // we prefer lists here because we wont be updating the list very often, nor will we be creating or removing in a single frame. 
                                      // its more performant than resizing an array(which in c# is deleting and recreating it) every time we undo, from what i can tell
    private GameObject[] drawingsTemp;
    public Vector3[][] points;
    
    
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
            drawingsTemp = GameObject.FindGameObjectsWithTag("Line"); // we have to do this since you cant convert an array into a list
            foreach(GameObject drawing in drawingsTemp){
                drawings.Add(drawing);
            }
    }
    void ClearDrawings(){
        RefreshDrawings();
        foreach(GameObject i in drawings){
            drawings.Remove(i);
            Destroy(i);
        }
    }
    void UndoDrawing(){
        RefreshDrawings();
        Debug.Log("Recieved input 'Undo'.");
        int deadIndex = drawings.Count;
        GameObject dead = drawings[deadIndex-1]; // since we did this we can also do things like highlight the last drawn line, or 
        Debug.Log(deadIndex + "/" + drawings.Count + dead.name);
        drawings.Remove(dead); // remove the last entry from drawings
        Destroy(dead); 
    }
}
