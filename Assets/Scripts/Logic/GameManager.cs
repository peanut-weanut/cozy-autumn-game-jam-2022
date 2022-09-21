using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
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
    public DrawLines drawLines;
    public CameraControls camControls;
    public CanvasBehavior canvas;
    public DrawingUtilities drawUtils;
    public DialogueRunner dialogueRunner;


    public GameObject[] stage0Objects;
    public GameObject[] stage1Objects;
    public GameObject[] stage2Objects;
    public GameObject[] stage3Objects;
    public GameObject[] stage4Objects;
    int state = 5; // tutorial state
    private void Awake()
    {
        Application.targetFrameRate = 90;
        controls = new InputSystem();
        drawLines = GetComponent<DrawLines>();
        camControls = Camera.main.transform.GetComponent<CameraControls>();
        drawUtils = transform.GetComponent<DrawingUtilities>();
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
        POIs = FindObjectsInLayer(7);
        controls.debug.AdvanceCutscene.performed += ctx => AdvanceState();
        canvas.TransDone += AllowDrawing;
        canvas.TransDoneLooking += DisallowDrawing;
        drawUtils.OnDoneDrawing += StartTestDialogue;
        UpdateTriggers(new List<Trigger>{triggers[2]});
        OnListUpdate?.Invoke();
    }
    private static GameObject[] FindObjectsInLayer(int layer)
    {
     var ret = new List<GameObject>();
     foreach (GameObject t in GameObject.FindObjectsOfType<GameObject>())
     {
         if (t.layer == layer)
         {
             ret.Add (t);
         }
     }
     return ret.ToArray();        
    }
    void StartTestDialogue(){
        dialogueRunner.StartDialogue("PromptPlaque");
    }
    void Update(){

    }
    void AllowDrawing(){
        drawLines.allowDrawing = true;
    }
    void DisallowDrawing(){
        drawLines.allowDrawing = false;
    }
    [YarnCommand("AdvanceState")]
    void AdvanceState(){
        state++;
        switch(state){
            case 0:
                //done in start function
            break;
            case 1:
                UpdateTriggers(new List<Trigger>{triggers[5], triggers[6]});
            break;
            case 2:
                UpdateTriggers(new List<Trigger>{triggers[9],triggers[10]});
            break;
            case 3:
                UpdateTriggers(new List<Trigger>{triggers[13],triggers[14]});
            break;
            case 4:
                UpdateTriggers(new List<Trigger>{triggers[17],triggers[18]});
            break;
        }
    }

    void UpdateTriggers(List<Trigger> triggerList){
        triggersActive.Clear();
        foreach(Trigger t in triggerList){
            triggersActive.Add(t);
        }
        Debug.Log(triggersActive);
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

