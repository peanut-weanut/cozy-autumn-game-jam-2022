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
    public AudioManager audioManager;


    public GameObject[] stage0Objects;
    public GameObject[] stage1Objects;
    public GameObject[] stage2Objects;
    public GameObject[] stage3Objects;
    public GameObject[] stage4Objects;
    public string nextPrompt;
    public delegate void OnStoryToldDelegate();
    public OnStoryToldDelegate OnStoryTold;
    int state = -2; // -1 is the tutorial state
    private void Awake()
    {
        Application.targetFrameRate = 90;
        controls = new InputSystem();
        drawLines = GetComponent<DrawLines>();
        camControls = Camera.main.transform.GetComponent<CameraControls>();
        drawUtils = transform.GetComponent<DrawingUtilities>();
        dialogueRunner.AddCommandHandler<int>("SetState", SetState);
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
        drawUtils.OnDoneDrawing += PlayPrompt;
        
        AdvanceState();
    }
    void PlayPrompt(){
        if(camControls.DebugPOISeen){
            switch(camControls.realCurrentPOI.tag){
                case "Plaque":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptPlaqueSuccess");
                    break;
                case "Tree":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptSomewhereToSitSuccess_Tree");
                    break;
                case "Lake":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptSomewhereToSwimSuccess_Lake");
                    break;
                case "Picnic Table":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptSomewhereToSitSuccess_PicnicTable");
                    break;
                case "Stump":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptSomewhereToSitSuccess_Stump");
                    break;
                case "Waterfall":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptSomewhereToSwimSuccess_Waterfall");
                    break;
                case "Lakehouse":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptLakehouseSuccess");
                    break;
                case "Frog":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptAnimalsSuccess_Frog");
                    break;
                case "Bird":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptAnimalsSuccess_Bird");
                    break;
                case "Bee":
                    Debug.Log("Ran " + camControls.realCurrentPOI.tag);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("PromptAnimalsSuccess_Bee");
                    break;   
                default:
                    Debug.Log("Couldn't run dialogue.");
                break;
            }
            OnStoryTold();
        }
        else if (state == -1){
            AdvanceState();
            Debug.Log("Since this is the tutorial, we advance the state for free.");
        }
        else
        {
            Debug.Log("Could not get poi seen");
        }
    }
    void SendPicture(){
        dialogueRunner.StartDialogue("PostPic");
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
    [YarnCommand("NextPrompt")]
    void StartNewDialogue(){
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue(nextPrompt);
    }
    [YarnCommand("Stop")] // for the ui animation
    void StopDialogue(){
        dialogueRunner.Stop();
    }
    void Update(){

    }
    void AllowDrawing(){
        drawLines.allowDrawing = true;
    }
    void DisallowDrawing(){
        drawLines.allowDrawing = false;
    }
   
    void AdvanceState(){
        foreach(GameObject POI in POIs){
            POI.GetComponent<POIScript>().isDrawable = false;
        }
        state++;
        Debug.Log("State is " + state);
        switch(state){
            case -1: // tutorial dialogue
                UpdateTriggers(new List<Trigger>{});
                nextPrompt = "PromptPlaque";
                audioManager.currentSongPack = audioManager.songPack0;
            break;
            case 0:
                StartNewDialogue();
                UpdateTriggers(new List<Trigger>{triggers[2]});
                nextPrompt = "PromptSomewhereToSwim";
                audioManager.currentSongPackIndex = 0;
            break;
            case 1:
                StartNewDialogue();
                UpdateTriggers(new List<Trigger>{triggers[5], triggers[6]});
                nextPrompt = "PromptAnimals";
                audioManager.currentSongPackIndex = 1;
            break;
            case 2:
                StartNewDialogue();
                UpdateTriggers(new List<Trigger>{triggers[9],triggers[10]});
                nextPrompt = "PromptSomewhereToSit";
                audioManager.currentSongPackIndex = 2;
            break;
            case 3:
                StartNewDialogue();
                UpdateTriggers(new List<Trigger>{triggers[13],triggers[14]});
                nextPrompt = "PromptLakehouse";
                audioManager.currentSongPackIndex = 3;
            break;
            case 4:
                StartNewDialogue();
                UpdateTriggers(new List<Trigger>{triggers[17],triggers[18]});
                audioManager.currentSongPackIndex = 4;
                // nextPrompt = "PromptPlaque";
            break;
        }
    }

    void SetState(int newState){
        state = newState-1;
        AdvanceState();
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

