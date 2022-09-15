using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputIntercept : MonoBehaviour
{
    private CinemachineBrain brain;
    public InputSystem controls;
    public CinemachineInputProvider input;
    void Awake(){
        controls = new InputSystem();
    }
    private void Start()
    {
        
        // cam = transform.GetComponent<CinemachineVirtualCamera>();
        brain = Camera.main.GetComponent<CinemachineBrain>();
        input = GetComponent<CinemachineInputProvider>();
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
            input.enabled = false;
        } else{
            input.enabled = true;
        }
    }
    private float GetAxisCustom(string axisName)
    {
            return 0.0f;
    }
}
