using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class POIScript : MonoBehaviour
{
    public bool startVisible = true;
    public bool isVisible;
    public bool isDrawable;
    public Trigger[] myTrigger;
    public bool triggerActive;
    private Renderer render;
    private string myTag;
    List<int> activeIndices = new List<int>();
    public Outline outline;
    private void Start(){
        GameManager.game.OnListUpdate += CheckTrigger;
        GameManager.game.camControls.POISeen += CheckMe;
        render = GetComponent<Renderer>();
        if(!startVisible)
            render.enabled = false;
        myTag = transform.tag;
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public void OnBecameVisible(){ //if it mattered, then this method would return a ray instead 
        isVisible = true; 
        SetOutline();
    }
    public void OnBecameInvisible(){ //if it mattered, then this method would return a ray instead 
        isVisible = false;  
        SetOutline();
    }
    void SetOutline(){
        if(outline){
            if(isDrawable){
                outline.enabled = true;
            } else{
                outline.enabled = false;
            }
        }
    }
    void CheckMe(){
        if(GameManager.game.camControls.realCurrentPOI == this.transform.gameObject){
            isDrawable = false;
            LockType();
            GameManager.game.audioManager.playPOI = true;
            //toggle visual indicator here.
            Debug.Log(this.transform.name + " has just checked itself.");
        }
    }
    void LockType(){
        var myType = GameObject.FindGameObjectsWithTag(myTag);
        foreach(GameObject i in myType){
            i.layer = 0;
        }
    }
    void CheckTrigger(){
        activeIndices.Clear();
        for(int i  = 0; i < myTrigger.Length-1; i++){
            
            if (GameManager.game.triggersActive.Contains(myTrigger[i])){
                triggerActive = true;
                activeIndices.Add(i);
            } else{
            }
            
            // Debug.Log(this.transform.name + " checked " + t.id);
        }
        foreach (int i in activeIndices)
            ExecuteTrigger(myTrigger[i]);
        
    }
    void ExecuteTrigger(Trigger trigger){
        if(triggerActive){
            Debug.Log("Trigger is active");
            if (trigger.id.EndsWith("_Spawn")){
                render.enabled = true;
            } 
            else if (trigger.id.EndsWith("_MakeDrawable")){
                isDrawable = true;
                SetOutline();
            } 
            else if(trigger.id.EndsWith("_Despawn")){
                render.enabled = false;
                isDrawable = false;
            }
            else if(trigger.id.EndsWith("_Despawn")){
                render.enabled = false;
                isDrawable = false;
            }
            else{
                // render.enabled = false;
            }
            Debug.Log(this.transform.name + " executed " + trigger.id);
        }
    }
    #if UNITY_EDITOR
    private void Reset() {
        if (gameObject.layer != 7)
            gameObject.layer = 7;
        if (myTrigger.Length == 0)
            myTrigger[0] = GetTrigger("default_trigger", typeof(Trigger)) as Trigger;
    }
    public static object GetTrigger(string SOName, System.Type type)
    {
        string[] matchingAssets = AssetDatabase.FindAssets(SOName, new string[]{"Assets/Triggers"});

        foreach (var asset in matchingAssets)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(asset);
            var SO = AssetDatabase.LoadAssetAtPath(SOpath, type);
            return SO;
        }

        return null;    
    }
    #endif
}