using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    [HideInInspector]
    public GameObject[] POIs;
    public List<Trigger> triggers; // triggers that happen exactly once
    public List<ContinuousTrigger> continuousTriggers; //triggers that loop until deactivated
    private List<Trigger> triggersActive;
    private List<ContinuousTrigger> cTriggersActive; 
    // there are a list of triggers and continuous triggers. 
    // basically when you want to set a trigger, you use the SetTriggers function to say which triggers should be active at any given moment
    // the triggers list corresponds to a

    // public DialogueRunner runner;
    void Start(){
        POIs = GameObject.FindGameObjectsWithTag("POI");
    }

}

