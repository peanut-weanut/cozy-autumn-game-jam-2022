using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "New Trigger", menuName = "Triggers/Continuous Trigger", order = 2)][System.Serializable]
public class ContinuousTrigger : ScriptableObject
{
    public string id; // Trigger ID, which is automattically updated based on file name
    public float waitTime; // How long to wait after the trigger has been activated to move to next trigger(-1 for checks and triggers that dont trigger other triggers)
    public bool active; // Is trigger active
    void OnValidate() {
             string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
             id = Path.GetFileNameWithoutExtension(assetPath);
    }
    
}
