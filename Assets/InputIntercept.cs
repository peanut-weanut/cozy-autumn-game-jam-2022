using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputIntercept : MonoBehaviour
{
    private CinemachineBrain brain;
    public InputSystem controls;
    void Awake(){
        controls = new InputSystem();
    }
    private void Start()
    {
        
        // cam = transform.GetComponent<CinemachineVirtualCamera>();
        brain = Camera.main.GetComponent<CinemachineBrain>();
        // CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    void Update(){
        if (brain.IsBlending){
            Debug.Log("BLENDING");
            
        }
    }
    // private float GetAxisCustom(string axisName)
    // {
    //     if (brain.IsBlending){
    //         Debug.Log("BLENDING");
    //         return 0.0f;
    //     }
    //     return Input.GetAxis(axisName);
    // }
}
