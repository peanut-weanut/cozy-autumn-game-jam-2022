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
    private void Reset() {
        if (transform.tag != "POI")
            transform.tag = "POI";
        if (!myTrigger)
            myTrigger = GetTrigger("default_trigger", typeof(Trigger)) as Trigger;
    }
    private void Start(){
        GameManager.game.OnListUpdate += CheckTrigger;
        
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
    }
    void Update(){
        if(triggerActive){
            Debug.Log("Trigger POIs is active.");
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