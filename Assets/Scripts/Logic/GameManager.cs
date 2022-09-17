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
    public List<Trigger> triggersActive;
    public List<ContinuousTrigger> cTriggersActive; 
    public delegate void OnUpdateDelegate();
    public event OnUpdateDelegate OnListUpdate;
    public event OnUpdateDelegate OnCListUpdate;
    // there are a list of triggers and continuous triggers. 
    // basically when you want to set a trigger, you use the SetTriggers function to say which triggers should be active at any given moment
    // the triggers list corresponds to a

    //new implementation -- there are just "types" of events, and each trigger corresponds to an event id
    //eg "dialogue(start_event)" will do the starting dialogue
    //or "spawn(start_event)" will instantiate everything that needs to be instantiated
    // and these "commands" will just extend functions contained within game manager

    // public DialogueRunner runner;
    public InputSystem controls;
    int state = 0;
    private void Awake()
    {
        controls = new InputSystem();
        game = this;
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    void Start(){
        POIs = GameObject.FindGameObjectsWithTag("POI");
        controls.debug.AdvanceCutscene.performed += ctx => AdvanceState();
    }
    void Update(){

    }
    void AdvanceState(){
        state++;
        switch(state){
            case 0:
                //initialize everything
            break;
            case 1:
                UpdateTriggers(new List<Trigger>(){triggers[0]});
                UpdateCTriggers(new List<ContinuousTrigger>(){continuousTriggers[0]});
            break;
            case 2:
                UpdateTriggers(new List<Trigger>(){triggers[1], triggers[4]});
            break;
            case 3:
                UpdateTriggers(new List<Trigger>(){triggers[2], triggers[3]});
            break;
        }
    }

    void UpdateTriggers(List<Trigger> triggerList){
        triggersActive.Clear();
        foreach(Trigger t in triggerList){
            triggersActive.Add(t);
        }
        OnListUpdate?.Invoke();
    }
        void UpdateCTriggers(List<ContinuousTrigger> triggerList){
        cTriggersActive.Clear();
        foreach(ContinuousTrigger t in triggerList){
            cTriggersActive.Add(t);
        }
        OnCListUpdate?.Invoke();
    }

}

