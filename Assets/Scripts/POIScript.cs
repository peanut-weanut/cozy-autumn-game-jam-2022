using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class POIScript : MonoBehaviour
{
    public bool isVisible;
    public bool isDrawable;
    public Trigger myTrigger;
    public bool triggerActive;
    private Renderer render;
    private void Reset() {
        if (transform.tag != "POI")
            transform.tag = "POI";
        if (!myTrigger)
            myTrigger = GetTrigger("default_trigger", typeof(Trigger)) as Trigger;
    }
    private void Start(){
        GameManager.game.OnListUpdate += CheckTrigger;
        render = GetComponent<Renderer>();
        render.enabled = false;
    }
    public void OnBecameVisible(){ //if it mattered, then this method would return a ray instead 
        isVisible = true;    
    }
    public void OnBecameInvisible(){ //if it mattered, then this method would return a ray instead 
        isVisible = false;  
    }
    void CheckTrigger(){
        if (GameManager.game.triggersActive.Contains(myTrigger)){
            triggerActive = true;
        } else{
            triggerActive = false;
        }
        ExecuteTrigger();
    }
    void ExecuteTrigger(){
        if(triggerActive){
            if (myTrigger.id.EndsWith("_Spawn")){
                render.enabled = true;
            } 
            else if (myTrigger.id.EndsWith("_MakeDrawable")){
                isDrawable = true;
            } 
            else if(myTrigger.id.EndsWith("_Despawn")){
                render.enabled = false;
                isDrawable = false;
            }
            else{
                render.enabled = false;
            }
        }
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
}