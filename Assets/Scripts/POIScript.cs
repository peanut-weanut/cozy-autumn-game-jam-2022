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
    
    
    private void Start(){
        GameManager.game.OnListUpdate += CheckTrigger;
        GameManager.game.camControls.POISeen += CheckMe;
        render = GetComponent<Renderer>();
        if(!startVisible)
            render.enabled = false;
        myTag = transform.tag;
    }
    public void OnBecameVisible(){ //if it mattered, then this method would return a ray instead 
        isVisible = true;    
    }
    public void OnBecameInvisible(){ //if it mattered, then this method would return a ray instead 
        isVisible = false;  
    }
    void CheckMe(){
        if(GameManager.game.camControls.currentPOI == this.transform.gameObject){
            isDrawable = false;
            LockType();
        }
    }
    void LockType(){
        var myType = GameObject.FindGameObjectsWithTag(myTag);
        foreach(GameObject i in myType){
            i.layer = 0;
        }
    }
    void CheckTrigger(){
        int badCount = 0;
        int goodCount = 0;
        foreach(Trigger t in myTrigger){
            goodCount++;
            if (GameManager.game.triggersActive.Contains(t)){
                triggerActive = true;
            } else{
                badCount++;
            }
            if (badCount == goodCount)
                triggerActive = false;
            ExecuteTrigger(t);
            Debug.Log(t.id);
        }
        
    }
    void ExecuteTrigger(Trigger trigger){
        if(triggerActive){
            Debug.Log("Trigger is active");
            if (trigger.id.EndsWith("_Spawn")){
                render.enabled = true;
                Debug.Log("Spawned");
            } 
            else if (trigger.id.EndsWith("_MakeDrawable")){
                isDrawable = true;
                Debug.Log("Drawned");
            } 
            else if(trigger.id.EndsWith("_Despawn")){
                render.enabled = false;
                isDrawable = false;
            }
            else{
                // render.enabled = false;
            }
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